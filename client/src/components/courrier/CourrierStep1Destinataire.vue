<template>
  <section class="w-full">

    <!-- En-tête -->
    <div class="mb-6 flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
      <div>
        <div class="mb-2 inline-flex items-center gap-2 rounded-full bg-brand-amberLight px-3 py-1 text-xs font-semibold text-brand-text">
          <span class="flex h-5 w-5 items-center justify-center rounded-full bg-brand-amber text-[10px] text-brand-amberDark font-bold">1</span>
          Destinataire
        </div>

        <h2 class="text-2xl font-bold tracking-tight text-gray-900 sm:text-3xl">Choisissez votre destinataire</h2>

        <p class="mt-2 max-w-2xl text-sm leading-6 text-gray-500">
          Sélectionnez le service auquel vous souhaitez adresser votre demande.
          Si vous ne savez pas lequel choisir, notre assistant peut vous aider.
        </p>
      </div>

      <div v-if="!loading" class="flex shrink-0 items-center gap-2 rounded-xl border border-gray-200 bg-white px-3 py-2 shadow-sm">
        <span class="flex h-8 w-8 items-center justify-center rounded-lg bg-brand-amberLight text-brand-text">
          <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
            <path d="M3 21h18" /><path d="M5 21V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v16" />
            <path d="M9 7h2M9 11h2M13 7h2M13 11h2" />
          </svg>
        </span>
        <div>
          <p class="text-xs font-medium text-gray-400">Services disponibles</p>
          <p class="text-sm font-bold text-gray-800">{{ services.length }}</p>
        </div>
      </div>
    </div>

    <!-- Assistant IA -->
    <div class="mb-7">
      <AssistantDestinataire :services="services" @select="selectService" />
    </div>

    <!-- Recherche / filtres -->
    <div class="mb-5 rounded-2xl border border-gray-200 bg-white p-4 shadow-sm sm:p-5">
      <div class="flex flex-col gap-4 lg:flex-row lg:items-center">

        <div class="relative flex-1">
          <span class="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-gray-400">
            <svg class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <circle cx="11" cy="11" r="7" /><path d="m20 20-4-4" />
            </svg>
          </span>
          <input
            v-model="search"
            type="text"
            placeholder="Rechercher un organisme ou un service..."
            class="h-12 w-full rounded-xl border border-gray-200 bg-gray-50 pl-12 pr-10 text-sm text-gray-800 outline-none transition placeholder:text-gray-400 focus:border-brand-amber focus:bg-white focus:ring-4 focus:ring-brand-amberLight"
          />
          <button
            v-if="search"
            type="button"
            @click="search = ''"
            class="absolute right-3 top-1/2 flex h-7 w-7 -translate-y-1/2 items-center justify-center rounded-lg text-gray-400 transition hover:bg-gray-200 hover:text-gray-700"
            aria-label="Effacer la recherche"
          >×</button>
        </div>

        <div class="relative lg:w-56">
          <select
            v-model="selectedCity"
            class="h-12 w-full appearance-none rounded-xl border border-gray-200 bg-gray-50 px-4 pr-10 text-sm font-medium text-gray-700 outline-none transition focus:border-brand-amber focus:bg-white focus:ring-4 focus:ring-brand-amberLight"
          >
            <option value="">Toutes les villes</option>
            <option v-for="city in cities" :key="city" :value="city">{{ city }}</option>
          </select>
          <svg class="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="m6 9 6 6 6-6" />
          </svg>
        </div>
      </div>

      <div class="mt-4 flex flex-wrap items-center gap-2">
        <span class="text-xs font-medium text-gray-400">Affichage :</span>
        <span class="rounded-lg bg-brand-amberLight px-2.5 py-1 text-xs font-semibold text-brand-text">
          {{ filteredServices.length }} résultat{{ filteredServices.length > 1 ? 's' : '' }}
        </span>
        <button
          v-if="search || selectedCity"
          type="button"
          @click="clearFilters"
          class="text-xs font-semibold text-gray-500 underline decoration-gray-300 underline-offset-2 transition hover:text-gray-900"
        >Réinitialiser les filtres</button>
      </div>
    </div>

    <!-- Erreur -->
    <div v-if="error" class="mb-5 flex items-start gap-3 rounded-2xl border border-red-200 bg-red-50 p-4 text-sm text-red-700">
      <div class="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-red-100">
        <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="12" cy="12" r="9" /><path d="M12 8v4M12 16h.01" />
        </svg>
      </div>
      <div>
        <p class="font-semibold">Impossible de charger les services</p>
        <p class="mt-1 text-xs text-red-600">{{ error }}</p>
      </div>
    </div>

    <!-- Chargement -->
    <div v-if="loading" class="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3">
      <div v-for="i in 6" :key="i" class="animate-pulse rounded-2xl border border-gray-200 bg-white p-5">
        <div class="flex items-center justify-between">
          <div class="h-12 w-12 rounded-xl bg-gray-200"></div>
          <div class="h-6 w-20 rounded-lg bg-gray-100"></div>
        </div>
        <div class="mt-5 h-3 w-20 rounded bg-gray-100"></div>
        <div class="mt-2 h-5 w-4/5 rounded bg-gray-200"></div>
        <div class="my-4 h-px bg-gray-100"></div>
        <div class="h-3 w-16 rounded bg-gray-100"></div>
        <div class="mt-2 h-5 w-3/5 rounded bg-gray-200"></div>
        <div class="mt-5 h-8 w-24 rounded-lg bg-gray-100"></div>
      </div>
    </div>

    <!-- Liste -->
    <div v-else-if="filteredServices.length" class="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-3">
      <ServiceCard
        v-for="service in filteredServices"
        :key="service.id"
        :service="service"
        :selected="selectedService?.id === service.id"
        @select="selectService"
      />
    </div>

    <!-- Aucun résultat -->
    <div v-else class="rounded-2xl border border-dashed border-gray-300 bg-white px-6 py-14 text-center">
      <div class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-gray-100 text-gray-400">
        <svg class="h-7 w-7" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6">
          <circle cx="11" cy="11" r="7" /><path d="m20 20-4-4" />
        </svg>
      </div>
      <h3 class="mt-4 text-sm font-bold text-gray-800">Aucun service trouvé</h3>
      <p class="mx-auto mt-2 max-w-md text-sm leading-6 text-gray-500">
        Aucun service ne correspond à votre recherche. Essayez un autre mot-clé ou réinitialisez les filtres.
      </p>
      <button type="button" @click="clearFilters" class="mt-5 rounded-xl bg-brand-amber px-4 py-2.5 text-xs font-semibold text-brand-amberDark transition hover:brightness-95">
        Réinitialiser
      </button>
    </div>

    <!-- Footer de sélection -->
    <div class="mt-6 rounded-2xl border border-gray-200 bg-white p-4 shadow-sm sm:p-5">
      <div class="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div class="flex items-center gap-3">
          <div
            class="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl"
            :class="selectedService ? 'bg-brand-amberLight text-brand-text' : 'bg-gray-100 text-gray-400'"
          >
            <svg class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <path d="M5 21V5a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v16" /><path d="M3 21h18" />
              <path d="M9 7h2M13 7h2M9 11h2M13 11h2" />
            </svg>
          </div>
          <div class="min-w-0">
            <p class="text-xs font-medium text-gray-400">Destinataire</p>
            <p v-if="selectedService" class="truncate text-sm font-bold text-gray-800">
              {{ selectedService.nomService || selectedService.nomOrganisme }}
            </p>
            <p v-else class="text-sm font-medium text-gray-500">Aucun service sélectionné</p>
          </div>
        </div>

        <button
          type="button"
          @click="continueStep"
          :disabled="!selectedService"
          class="inline-flex h-11 items-center justify-center gap-2 rounded-xl px-5 text-sm font-bold transition-all"
          :class="selectedService
            ? 'bg-brand-amber text-brand-amberDark shadow-sm hover:-translate-y-0.5 hover:brightness-95 hover:shadow-md'
            : 'cursor-not-allowed bg-gray-100 text-gray-400'"
        >
          Continuer
          <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M5 12h14" /><path d="m13 6 6 6-6 6" />
          </svg>
        </button>
      </div>
    </div>

  </section>
</template>

<script setup>
import { ref, computed } from 'vue'
import AssistantDestinataire from './AssistantDestinataire.vue'
import ServiceCard from './ServiceCard.vue'

const props = defineProps({
  services: { type: Array, default: () => [] },
  loading: { type: Boolean, default: false },
  error: { type: String, default: '' },
  selectedService: { type: Object, default: null }
})

const emit = defineEmits(['select', 'continue'])

const search = ref('')
const selectedCity = ref('')

const cities = computed(() => {
  const values = props.services.map(service => service.ville).filter(Boolean)
  return [...new Set(values)].sort((a, b) => String(a).localeCompare(String(b)))
})

const filteredServices = computed(() => {
  const keyword = search.value.trim().toLowerCase()
  return props.services.filter(service => {
    const matchesSearch =
      !keyword ||
      String(service.nomOrganisme || '').toLowerCase().includes(keyword) ||
      String(service.nomService || '').toLowerCase().includes(keyword) ||
      String(service.typeOrganisme || '').toLowerCase().includes(keyword) ||
      String(service.ville || '').toLowerCase().includes(keyword)

    const matchesCity = !selectedCity.value || String(service.ville || '') === selectedCity.value
    return matchesSearch && matchesCity
  })
})

function selectService(service) {
  emit('select', service)
}

function continueStep() {
  if (!props.selectedService) return
  emit('continue')
}

function clearFilters() {
  search.value = ''
  selectedCity.value = ''
}
</script>