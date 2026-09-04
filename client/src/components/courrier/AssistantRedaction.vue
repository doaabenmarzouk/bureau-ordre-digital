<template>
  <div class="mt-2">

    <!-- Bouton déclencheur -->
    <button
      v-if="!isOpen"
      type="button"
      @click="openAssistant"
      :disabled="!modelValue.trim()"
      class="inline-flex items-center gap-1.5 rounded-lg bg-yellow-50 px-3 py-1.5 text-[11px] font-bold text-yellow-700 transition hover:bg-yellow-100 disabled:cursor-not-allowed disabled:opacity-50"
    >
      <svg class="h-3.5 w-3.5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
        <path d="M12 3v4" /><path d="M5 12H3" /><path d="M21 12h-2" /><path d="m6.5 6.5 2 2" />
        <path d="m17.5 17.5-2-2" /><path d="m17.5 6.5-2 2" /><path d="m6.5 17.5 2-2" /><path d="M12 17v4" />
        <circle cx="12" cy="12" r="3" />
      </svg>
      Améliorer avec l'IA
    </button>

    <p v-if="!isOpen && !modelValue.trim()" class="mt-1 text-[10px] text-slate-400">
      Écrivez quelques mots, l'assistant vous aidera à reformuler
    </p>

    <!-- Panneau ouvert -->
    <div v-else-if="isOpen" class="mt-2 overflow-hidden rounded-xl border border-yellow-200 bg-yellow-50/50">

      <div v-if="!suggestion" class="flex items-center justify-between gap-3 p-3">
        <div class="flex items-center gap-2 text-xs font-semibold text-yellow-700">
          <svg v-if="loading" class="h-4 w-4 animate-spin" viewBox="0 0 24 24" fill="none">
            <circle cx="12" cy="12" r="9" stroke="currentColor" stroke-opacity=".25" stroke-width="2" />
            <path d="M21 12a9 9 0 0 0-9-9" stroke="currentColor" stroke-width="2" />
          </svg>
          <span>{{ loading ? 'Reformulation en cours...' : 'Prêt à améliorer votre texte' }}</span>
        </div>
        <button
          v-if="!loading"
          type="button"
          @click="closeAssistant"
          class="text-yellow-600 hover:text-yellow-800"
          aria-label="Fermer"
        >×</button>
      </div>

      <!-- Suggestion -->
      <div v-else class="p-3">
        <p class="mb-2 text-[10px] font-bold uppercase tracking-wider text-yellow-700">Version suggérée</p>
        <div class="rounded-lg border border-yellow-200 bg-white p-3 text-xs leading-5 text-slate-700">
          {{ suggestion }}
        </div>
        <div class="mt-3 flex justify-end gap-2">
          <button
            type="button"
            @click="dismiss"
            class="rounded-lg border border-slate-200 bg-white px-3 py-1.5 text-[11px] font-bold text-slate-600 transition hover:bg-slate-50"
          >
            Garder mon texte
          </button>
          <button
            type="button"
            @click="acceptSuggestion"
            class="rounded-lg bg-yellow-400 px-3 py-1.5 text-[11px] font-bold text-slate-900 transition hover:bg-yellow-500"
          >
            Utiliser cette version
          </button>
        </div>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref } from 'vue'
import { redactionApi } from '@/services/api'

const props = defineProps({
  modelValue: { type: String, default: '' },
  champ: { type: String, default: 'description' } // 'objet' | 'description'
})

const emit = defineEmits(['update:modelValue'])

const isOpen = ref(false)
const loading = ref(false)
const suggestion = ref('')

async function openAssistant() {
  isOpen.value = true
  suggestion.value = ''
  loading.value = true

  try {
    const result = await redactionApi.ameliorer({
      texte: props.modelValue,
      champ: props.champ
    })
    suggestion.value = result.suggestion
  } catch (err) {
    console.error('Erreur amélioration IA :', err)
    isOpen.value = false
  } finally {
    loading.value = false
  }
}

function acceptSuggestion() {
  emit('update:modelValue', suggestion.value)
  closeAssistant()
}

function dismiss() {
  closeAssistant()
}

function closeAssistant() {
  isOpen.value = false
  suggestion.value = ''
}
</script>