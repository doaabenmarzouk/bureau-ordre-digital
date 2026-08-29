<template>
  <section class="bg-white border border-gray-200 rounded-2xl shadow-sm overflow-hidden">

    <!-- Header -->
    <div class="px-6 py-5 border-b border-gray-100">
      <div class="flex items-start gap-3">
        <div class="w-10 h-10 rounded-xl bg-brand-amberLight flex items-center justify-center shrink-0">
          <span class="text-lg">🏛️</span>
        </div>

        <div>
          <h2 class="text-lg font-semibold text-gray-900">
            Choisir le destinataire
          </h2>

          <p class="text-sm text-gray-500 mt-1">
            Sélectionnez l'organisme et le service auxquels vous souhaitez
            adresser votre courrier.
          </p>
        </div>
      </div>
    </div>

    <div class="p-6">

      <!-- Recherche -->
      <div class="relative mb-5">
        <span class="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
          🔍
        </span>

        <input
          v-model="search"
          type="text"
          placeholder="Rechercher un organisme ou un service..."
          class="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-xl text-sm
                 focus:outline-none focus:ring-2 focus:ring-brand-amber/30
                 focus:border-brand-amber transition"
          @input="searchServices"
        />
      </div>

      <!-- Filtres -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-3 mb-6">

        <div>
          <label class="block text-xs font-medium text-gray-600 mb-1.5">
            Ville
          </label>

          <select
            v-model="selectedCity"
            @change="loadServices"
            class="w-full px-3 py-2.5 border border-gray-300 rounded-lg text-sm
                   focus:outline-none focus:border-brand-amber"
          >
            <option value="">Toutes les villes</option>

            <option
              v-for="city in cities"
              :key="city"
              :value="city"
            >
              {{ city }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-xs font-medium text-gray-600 mb-1.5">
            Type d'organisme
          </label>

          <select
            v-model="selectedType"
            @change="loadServices"
            class="w-full px-3 py-2.5 border border-gray-300 rounded-lg text-sm
                   focus:outline-none focus:border-brand-amber"
          >
            <option value="">Tous les types</option>

            <option
              v-for="type in types"
              :key="type"
              :value="type"
            >
              {{ type }}
            </option>
          </select>
        </div>

      </div>

      <!-- Loading -->
      <div
        v-if="loading"
        class="flex justify-center py-10"
      >
        <div class="spinner"></div>
      </div>

      <!-- Erreur -->
      <div
        v-else-if="error"
        class="p-4 bg-red-50 border border-red-200 rounded-xl text-sm text-red-600"
      >
        {{ error }}
      </div>

      <!-- Résultats -->
      <div v-else>

        <div class="flex items-center justify-between mb-3">
          <p class="text-xs text-gray-500">
            {{ services.length }} service(s) disponible(s)
          </p>
        </div>

        <div
          v-if="services.length"
          class="grid grid-cols-1 md:grid-cols-2 gap-3 max-h-[420px] overflow-y-auto pr-1"
        >

          <button
            v-for="service in services"
            :key="service.id"
            type="button"
            @click="selectService(service)"
            class="group text-left p-4 border rounded-xl transition-all"
            :class="
              selected?.id === service.id
                ? 'border-brand-amber bg-brand-amberLight ring-1 ring-brand-amber'
                : 'border-gray-200 hover:border-brand-amber hover:bg-gray-50'
            "
          >

            <div class="flex gap-3">

              <div
                class="w-10 h-10 rounded-lg flex items-center justify-center shrink-0"
                :class="
                  selected?.id === service.id
                    ? 'bg-brand-amber'
                    : 'bg-gray-100'
                "
              >
                🏢
              </div>

              <div class="min-w-0 flex-1">

                <div class="flex justify-between gap-2">

                  <h3 class="font-medium text-sm text-gray-900">
                    {{ service.nomOrganisme }}
                  </h3>

                  <span
                    v-if="selected?.id === service.id"
                    class="text-brand-amberDark font-bold"
                  >
                    ✓
                  </span>

                </div>

                <p class="text-sm text-gray-700 mt-1">
                  {{ service.nomService }}
                </p>

                <div class="flex flex-wrap gap-2 mt-2">

                  <span class="text-xs text-gray-500">
                    📍 {{ service.ville }}
                  </span>

                  <span class="text-xs text-gray-500">
                    · {{ service.typeOrganisme }}
                  </span>

                </div>

              </div>

            </div>

          </button>

        </div>

        <!-- Aucun résultat -->
        <div
          v-else
          class="border border-dashed border-gray-300 rounded-xl py-12 text-center"
        >
          <div class="text-3xl mb-3">🔎</div>

          <p class="text-sm font-medium text-gray-700">
            Aucun service trouvé
          </p>

          <p class="text-xs text-gray-500 mt-1">
            Essayez une autre ville ou un autre terme de recherche.
          </p>
        </div>

      </div>

      <!-- Sélection -->
      <div
        v-if="selected"
        class="mt-5 p-4 rounded-xl bg-brand-amberLight border border-brand-amber"
      >

        <div class="flex items-center justify-between gap-4">

          <div>
            <p class="text-xs text-gray-500">
              Destinataire sélectionné
            </p>

            <p class="font-semibold text-sm text-gray-900 mt-1">
              {{ selected.nomOrganisme }}
            </p>

            <p class="text-xs text-gray-600 mt-1">
              {{ selected.nomService }} · {{ selected.ville }}
            </p>
          </div>

          <button
            type="button"
            @click="selected = null"
            class="text-xs text-gray-500 hover:text-gray-900 underline"
          >
            Modifier
          </button>

        </div>

      </div>

      <!-- Bouton -->
      <div class="flex justify-end mt-6 pt-5 border-t border-gray-100">

        <button
          type="button"
          :disabled="!selected"
          @click="continueToContent"
          class="px-6 py-2.5 rounded-xl text-sm font-semibold transition"
          :class="
            selected
              ? 'bg-brand-amber text-brand-amberDark hover:brightness-95'
              : 'bg-gray-200 text-gray-400 cursor-not-allowed'
          "
        >
          Continuer
          <span class="ml-1">→</span>
        </button>

      </div>

    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { organismesApi } from '@/services/api'

const emit = defineEmits(['next'])

const services = ref([])
const cities = ref([])
const types = ref([])

const loading = ref(false)
const error = ref('')

const search = ref('')
const selectedCity = ref('')
const selectedType = ref('')
const selected = ref(null)

let timer = null

async function loadServices() {
  loading.value = true
  error.value = ''

  try {
    services.value = await organismesApi.list({
      ville: selectedCity.value || undefined,
      type: selectedType.value || undefined,
      motCle: search.value || undefined
    })
  } catch (e) {
    console.error(e)
    services.value = []
    error.value = 'Impossible de charger les services destinataires.'
  } finally {
    loading.value = false
  }
}

function searchServices() {
  clearTimeout(timer)

  timer = setTimeout(() => {
    loadServices()
  }, 300)
}

function selectService(service) {
  selected.value = service
}

function continueToContent() {
  if (!selected.value) return

  emit('next', selected.value)
}

async function loadFilters() {
  try {
    const [citiesResult, typesResult] = await Promise.all([
      organismesApi.listVilles(),
      organismesApi.listTypes()
    ])

    cities.value = citiesResult
    types.value = typesResult
  } catch (e) {
    console.error(e)
  }
}

onMounted(async () => {
  await Promise.all([
    loadServices(),
    loadFilters()
  ])
})
</script>

<style scoped>
.spinner {
  width: 28px;
  height: 28px;
  border: 3px solid #eee;
  border-top-color: #f5b935;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>