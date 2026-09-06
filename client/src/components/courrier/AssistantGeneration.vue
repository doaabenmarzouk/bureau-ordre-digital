<template>
  <div class="overflow-hidden rounded-2xl border border-yellow-200 bg-yellow-50/40 mb-6">

    <!-- Header -->
    <div class="flex items-start gap-3 p-5">
      <div class="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-yellow-400 text-slate-900">
        <svg class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7">
          <path d="M12 3v4" /><path d="M6.5 6.5 9 9" /><path d="M3 12h4" />
          <path d="m6.5 17.5 2.5-2.5" /><path d="M12 17v4" /><path d="m17.5 17.5-2.5-2.5" />
          <path d="M21 12h-4" /><path d="m17.5 6.5-2.5 2.5" /><circle cx="12" cy="12" r="3" />
        </svg>
      </div>

      <div class="flex-1 min-w-0">
        <div class="flex flex-wrap items-center gap-2">
          <h3 class="text-sm font-bold text-slate-900">Décrivez simplement votre problème</h3>
          <span class="rounded-md bg-yellow-100 px-2 py-1 text-[10px] font-bold uppercase tracking-wider text-yellow-700">
            IA
          </span>
        </div>
        <p class="mt-1 text-xs leading-5 text-slate-500">
          L'assistant détermine le type de courrier, l'objet et la description à votre place.
        </p>
      </div>
    </div>

    <!-- Corps -->
    <div class="px-5 pb-5">

      <textarea
        v-model="texteLibre"
        rows="4"
        maxlength="1000"
        placeholder="Exemple : j'ai reçu une facture d'eau bien plus élevée que d'habitude et je pense qu'il y a une erreur, je voudrais que ça soit vérifié..."
        class="w-full resize-none rounded-xl border border-yellow-200 bg-white p-4 text-sm leading-6 text-slate-800 outline-none transition placeholder:text-slate-400 focus:border-yellow-400 focus:ring-4 focus:ring-yellow-100"
      ></textarea>

      <div class="mt-3 flex items-center justify-between">
        <span class="text-[10px] text-slate-400">{{ texteLibre.length }}/1000</span>

        <button
          type="button"
          @click="generer"
          :disabled="!texteLibre.trim() || loading || !typesCourrier.length"
          class="inline-flex h-10 items-center justify-center gap-2 rounded-xl bg-yellow-400 px-4 text-xs font-bold text-slate-900 transition hover:bg-yellow-500 disabled:cursor-not-allowed disabled:opacity-50"
        >
          <svg v-if="loading" class="h-4 w-4 animate-spin" viewBox="0 0 24 24" fill="none">
            <circle cx="12" cy="12" r="9" stroke="currentColor" stroke-opacity=".25" stroke-width="2" />
            <path d="M21 12a9 9 0 0 0-9-9" stroke="currentColor" stroke-width="2" />
          </svg>
          {{ loading ? 'Génération en cours...' : "Générer avec l'IA" }}
        </button>
      </div>

      <p v-if="!typesCourrier.length" class="mt-2 text-[11px] text-red-500">
        Les types de courrier ne sont pas encore chargés.
      </p>

      <!-- Erreur -->
      <div v-if="localError" class="mt-3 rounded-lg bg-red-50 border border-red-100 p-3 text-xs text-red-700">
        {{ localError }}
      </div>

      <!-- Résultat -->
      <div v-if="result" class="mt-5 rounded-xl border border-slate-200 bg-white p-4">

        <p class="text-[10px] font-bold uppercase tracking-wider text-slate-400 mb-1.5">Type de courrier suggéré</p>
        <div class="inline-flex items-center rounded-lg bg-yellow-100 px-3 py-1.5 text-xs font-bold text-yellow-800 mb-4">
          {{ typeLibelle }}
        </div>

        <p class="text-[10px] font-bold uppercase tracking-wider text-slate-400 mb-1.5">Objet suggéré</p>
        <div class="rounded-lg bg-slate-50 p-3 text-xs font-semibold text-slate-800 mb-4">
          {{ result.objet }}
        </div>

        <p class="text-[10px] font-bold uppercase tracking-wider text-slate-400 mb-1.5">Description suggérée</p>
        <div class="rounded-lg bg-slate-50 p-3 text-xs leading-5 text-slate-700">
          {{ result.description }}
        </div>

        <div class="mt-4 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
          <button
            type="button"
            @click="result = null"
            class="rounded-lg border border-slate-200 bg-white px-4 py-2 text-xs font-bold text-slate-600 transition hover:bg-slate-50"
          >
            Ignorer
          </button>
          <button
            type="button"
            @click="appliquer"
            class="rounded-lg bg-yellow-400 px-4 py-2 text-xs font-bold text-slate-900 transition hover:bg-yellow-500"
          >
            Remplir le formulaire avec ceci
          </button>
        </div>
      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { generationApi } from '@/services/api'

const props = defineProps({
  typesCourrier: {
    type: Array,
    default: () => []
  }
})

const emit = defineEmits(['apply'])

const texteLibre = ref('')
const loading = ref(false)
const result = ref(null)
const localError = ref('')

const typeLibelle = computed(() => {
  if (!result.value?.typeCourrierId) return 'Non déterminé'
  const type = props.typesCourrier.find(
    t => String(t.id) === String(result.value.typeCourrierId)
  )
  return type?.libelle || 'Non déterminé'
})

async function generer() {
  loading.value = true
  result.value = null
  localError.value = ''

  try {
    const data = await generationApi.genererDepuisTexteLibre({
      texteLibre: texteLibre.value
    })
    result.value = data
  } catch (err) {
    console.error('Erreur génération IA :', err)
    localError.value = err.message || "Une erreur est survenue lors de la génération."
  } finally {
    loading.value = false
  }
}

function appliquer() {
  if (!result.value) return
  emit('apply', {
    typeCourrierId: result.value.typeCourrierId,
    objet: result.value.objet,
    description: result.value.description
  })
  result.value = null
  texteLibre.value = ''
}
</script>