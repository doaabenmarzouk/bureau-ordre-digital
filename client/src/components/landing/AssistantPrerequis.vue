<template>
  <section class="relative overflow-hidden rounded-3xl border border-slate-100 bg-white shadow-[0_20px_60px_rgba(15,23,42,0.08)]">

    <!-- Décor -->
    <div class="absolute -right-16 -top-16 h-48 w-48 rounded-full bg-yellow-50"></div>
    <div class="absolute -left-10 -bottom-10 h-32 w-32 rounded-full bg-yellow-50"></div>

    <div class="relative p-6 sm:p-10">

      <!-- En-tête -->
      <div class="mb-6 flex flex-col items-start gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <div class="mb-2 inline-flex items-center gap-2 rounded-full bg-yellow-50 px-3 py-1.5">
            <span class="h-2 w-2 rounded-full bg-yellow-400"></span>
            <span class="text-xs font-bold uppercase tracking-wider text-yellow-600">Assistant IA · Gratuit</span>
          </div>
          <h2 class="text-2xl font-extrabold tracking-tight text-slate-900 sm:text-3xl">
            Que souhaitez-vous faire ?
          </h2>
          <p class="mt-2 max-w-xl text-sm leading-6 text-slate-500">
            Décrivez votre démarche en quelques mots. Nous vous indiquons immédiatement
            les documents à préparer, avant même de créer un compte.
          </p>
        </div>
      </div>

      <!-- Formulaire -->
      <div class="rounded-2xl border border-slate-200 bg-slate-50 p-4 sm:p-5">
        <textarea
          v-model="texte"
          rows="3"
          maxlength="500"
          placeholder="Exemple : je veux renouveler ma carte d'identité..."
          class="w-full resize-none rounded-xl border border-slate-200 bg-white p-4 text-sm leading-6 text-slate-800 outline-none transition placeholder:text-slate-400 focus:border-yellow-400 focus:ring-4 focus:ring-yellow-100"
        ></textarea>

        <div class="mt-3 flex flex-col-reverse items-stretch gap-3 sm:flex-row sm:items-center sm:justify-between">
          <span class="text-[11px] text-slate-400">{{ texte.length }}/500</span>

          <button
            type="button"
            @click="rechercher"
            :disabled="!texte.trim() || loading"
            class="inline-flex h-11 items-center justify-center gap-2 rounded-xl bg-yellow-400 px-6 text-sm font-bold text-slate-900 shadow-sm transition hover:-translate-y-0.5 hover:bg-yellow-500 hover:shadow-md disabled:cursor-not-allowed disabled:opacity-50 disabled:hover:translate-y-0"
          >
            <svg v-if="loading" class="h-4 w-4 animate-spin" viewBox="0 0 24 24" fill="none">
              <circle cx="12" cy="12" r="9" stroke="currentColor" stroke-opacity=".25" stroke-width="2" />
              <path d="M21 12a9 9 0 0 0-9-9" stroke="currentColor" stroke-width="2" />
            </svg>
            <svg v-else class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <circle cx="11" cy="11" r="7" /><path d="m20 20-4-4" />
            </svg>
            {{ loading ? 'Recherche en cours...' : 'Voir ce qu\'il me faut' }}
          </button>
        </div>
      </div>

      <!-- Erreur -->
      <div v-if="localError" class="mt-4 flex items-start gap-3 rounded-xl border border-red-100 bg-red-50 p-4 text-sm text-red-700">
        <svg class="h-5 w-5 shrink-0" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="12" cy="12" r="9" /><path d="M12 8v4M12 16h.01" />
        </svg>
        <p>{{ localError }}</p>
      </div>

      <!-- Résultat -->
      <div v-if="result" class="mt-6 overflow-hidden rounded-2xl border border-slate-200">

        <div class="flex items-center justify-between gap-3 bg-slate-900 px-5 py-4 sm:px-6">
          <div class="flex items-center gap-3">
            <div class="flex h-10 w-10 items-center justify-center rounded-xl bg-yellow-400 text-slate-900">
              <svg class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
                <path d="M9 12l2 2 4-4" /><circle cx="12" cy="12" r="9" />
              </svg>
            </div>
            <div>
              <p class="text-[10px] font-bold uppercase tracking-wider text-slate-400">Démarche identifiée</p>
              <p class="text-sm font-bold text-white">{{ result.demarche }}</p>
            </div>
          </div>
        </div>

        <div class="grid grid-cols-1 gap-5 bg-white p-5 sm:grid-cols-2 sm:p-6">

          <!-- Documents requis -->
          <div>
            <p class="mb-3 text-[11px] font-bold uppercase tracking-wider text-slate-400">Documents à préparer</p>
            <ul class="space-y-2">
              <li
                v-for="(doc, i) in result.documentsRequis"
                :key="i"
                class="flex items-start gap-2.5 text-sm text-slate-700"
              >
                <span class="mt-0.5 flex h-5 w-5 shrink-0 items-center justify-center rounded-full bg-yellow-100 text-yellow-700">
                  <svg class="h-3 w-3" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
                    <path d="M20 6 9 17l-5-5" />
                  </svg>
                </span>
                {{ doc }}
              </li>
            </ul>
          </div>

          <!-- Infos pratiques -->
          <div class="space-y-4">
            <div class="flex items-center gap-3 rounded-xl bg-slate-50 p-3.5">
              <div class="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-white text-slate-500 shadow-sm">
                <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
                  <circle cx="12" cy="12" r="9" /><path d="M12 7v5l3 3" />
                </svg>
              </div>
              <div>
                <p class="text-[10px] font-medium text-slate-400">Délai estimé</p>
                <p class="text-sm font-bold text-slate-800">{{ result.delai || 'Non précisé' }}</p>
              </div>
            </div>

            <div v-if="result.conditions" class="rounded-xl bg-slate-50 p-3.5">
              <p class="mb-1 text-[10px] font-medium text-slate-400">À savoir</p>
              <p class="text-sm leading-5 text-slate-700">{{ result.conditions }}</p>
            </div>
          </div>
        </div>

        <div class="border-t border-slate-100 bg-slate-50 px-5 py-5 sm:px-6">
          <div class="flex flex-col items-start gap-4 sm:flex-row sm:items-center sm:justify-between">
            <p class="text-sm text-slate-600">
              Vous avez tout ce qu'il faut ? Créez votre compte pour déposer votre demande.
            </p>
            <RouterLink
              to="/register"
              class="inline-flex shrink-0 items-center justify-center gap-2 rounded-xl bg-yellow-400 px-6 py-3 text-sm font-bold text-slate-900 shadow-sm transition hover:-translate-y-0.5 hover:bg-yellow-500 hover:shadow-md"
            >
              Créer mon compte
              <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M5 12h14" /><path d="m13 6 6 6-6 6" />
              </svg>
            </RouterLink>
          </div>
        </div>
      </div>

    </div>
  </section>
</template>

<script setup>
import { ref } from 'vue'
import { prerequisApi } from '@/services/api'

const texte = ref('')
const loading = ref(false)
const result = ref(null)
const localError = ref('')

async function rechercher() {
  loading.value = true
  result.value = null
  localError.value = ''

  try {
    const data = await prerequisApi.rechercher({ texte: texte.value })
    result.value = data
  } catch (err) {
    console.error('Erreur assistant prérequis :', err)
    localError.value = err.message || 'Une erreur est survenue lors de la recherche.'
  } finally {
    loading.value = false
  }
}
</script>