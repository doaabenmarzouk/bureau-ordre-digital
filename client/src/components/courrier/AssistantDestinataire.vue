<template>
  <div class="w-full overflow-hidden rounded-2xl border border-gray-200 bg-white shadow-sm">

    <!-- ===================== -->
    <!-- VERSION FERMÉE -->
    <!-- ===================== -->
    <div v-if="!isOpen" class="relative overflow-hidden">

      <div
        class="absolute -right-20 -top-20 h-40 w-40 rounded-full bg-brand-amberLight"
      ></div>

      <div
        class="relative flex flex-col gap-4 p-4
               sm:p-6
               md:flex-row md:items-center md:justify-between"
      >

        <!-- Texte -->
        <div class="flex min-w-0 items-start gap-3 sm:gap-4">

          <div
            class="flex h-10 w-10 shrink-0 items-center justify-center
                   rounded-xl bg-brand-amber text-brand-amberDark shadow-sm
                   sm:h-11 sm:w-11"
          >
            <svg
              class="h-5 w-5"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="1.7"
            >
              <path d="M12 3v4" />
              <path d="M6.5 6.5 9 9" />
              <path d="M3 12h4" />
              <path d="m6.5 17.5 2.5-2.5" />
              <path d="M12 17v4" />
              <path d="m17.5 17.5-2.5-2.5" />
              <path d="M21 12h-4" />
              <path d="m17.5 6.5-2.5 2.5" />
              <circle cx="12" cy="12" r="3" />
            </svg>
          </div>

          <div class="min-w-0">
            <div class="flex flex-wrap items-center gap-2">
              <h3 class="text-sm font-bold text-gray-900 sm:text-base">
                Besoin d'aide pour choisir ?
              </h3>

              <span
                class="rounded-md border border-brand-amber/30
                       bg-brand-amberLight px-2 py-1
                       text-[9px] font-bold uppercase tracking-wider
                       text-brand-text sm:text-[10px]"
              >
                Assistant IA
              </span>
            </div>

            <p
              class="mt-1 max-w-2xl text-xs leading-5 text-gray-500 sm:text-sm"
            >
              Décrivez votre demande ou joignez votre document.
              L'assistant vous proposera le service le plus pertinent.
            </p>
          </div>
        </div>

        <!-- Bouton -->
        <button
          type="button"
          @click="openAssistant"
          class="inline-flex h-11 w-full shrink-0 items-center
                 justify-center gap-2 rounded-xl
                 bg-brand-amber px-4 text-xs font-bold
                 text-brand-amberDark transition
                 hover:-translate-y-0.5 hover:brightness-95
                 hover:shadow-md
                 sm:w-auto"
        >
          <svg
            class="h-4 w-4"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="1.8"
          >
            <path d="M12 3v4" />
            <path d="M5 12H3" />
            <path d="M21 12h-2" />
            <path d="m6.5 6.5 2 2" />
            <path d="m17.5 17.5-2-2" />
            <path d="m17.5 6.5-2 2" />
            <path d="m6.5 17.5 2-2" />
            <path d="M12 17v4" />
            <circle cx="12" cy="12" r="3" />
          </svg>

          Trouver mon service
        </button>

      </div>
    </div>


    <!-- ===================== -->
    <!-- VERSION OUVERTE -->
    <!-- ===================== -->
    <div v-else>

      <!-- Header -->
      <div class="bg-brand-amber px-4 py-4 text-brand-amberDark sm:px-6 sm:py-5">

        <div
          class="flex flex-col gap-4
                 sm:flex-row sm:items-start sm:justify-between"
        >

          <!-- Titre -->
          <div class="flex min-w-0 items-start gap-3">

            <div
              class="flex h-10 w-10 shrink-0 items-center justify-center
                     rounded-xl bg-white/30"
            >
              <svg
                class="h-5 w-5"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="1.7"
              >
                <path d="M12 3v4" />
                <path d="M6.5 6.5 9 9" />
                <path d="M3 12h4" />
                <path d="m6.5 17.5 2.5-2.5" />
                <path d="M12 17v4" />
                <path d="m17.5 17.5-2.5-2.5" />
                <path d="M21 12h-4" />
                <path d="m17.5 6.5-2.5 2.5" />
                <circle cx="12" cy="12" r="3" />
              </svg>
            </div>

            <div class="min-w-0">

              <div class="flex flex-wrap items-center gap-2">

                <h3 class="text-sm font-bold sm:text-base">
                  Assistant de choix
                </h3>

                <span
                  class="rounded-md bg-white/30 px-2 py-1
                         text-[9px] font-bold uppercase tracking-wider
                         text-brand-amberDark"
                >
                  IA
                </span>

              </div>

              <p
                class="mt-1 text-xs leading-5
                       text-brand-amberDark/80 sm:text-sm"
              >
                Aidez-nous à comprendre votre besoin.
              </p>

            </div>
          </div>

          <!-- Fermer -->
          <button
            type="button"
            @click="closeAssistant"
            class="flex h-9 w-9 shrink-0 items-center justify-center
                   self-end rounded-lg
                   text-brand-amberDark/70 transition
                   hover:bg-white/30 hover:text-brand-amberDark
                   sm:self-start"
            aria-label="Fermer l'assistant"
          >
            <svg
              class="h-5 w-5"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
            >
              <path d="M6 6l12 12M18 6 6 18" />
            </svg>
          </button>

        </div>
      </div>


      <!-- ===================== -->
      <!-- BODY -->
      <!-- ===================== -->
      <div class="p-4 sm:p-6">

        <!-- Description + document -->
        <div class="grid grid-cols-1 gap-5 lg:grid-cols-2">

          <!-- ================= -->
          <!-- DESCRIPTION -->
          <!-- ================= -->
          <div class="min-w-0">

            <div class="mb-2 flex items-center justify-between gap-2">

              <label class="text-xs font-bold text-gray-700 sm:text-sm">
                Décrivez votre demande
              </label>

              <span class="shrink-0 text-[10px] font-medium text-gray-400">
                {{ description.length }}/3000
              </span>

            </div>

            <textarea
              v-model="description"
              maxlength="3000"
              rows="7"
              placeholder="Exemple : Je souhaite déposer une demande concernant..."
              class="min-h-[160px] w-full resize-none rounded-xl
                     border border-gray-200 bg-gray-50 p-3
                     text-sm leading-6 text-gray-800 outline-none
                     transition
                     placeholder:text-gray-400
                     focus:border-brand-amber
                     focus:bg-white
                     focus:ring-4
                     focus:ring-brand-amberLight
                     sm:min-h-[180px] sm:p-4"
            ></textarea>

          </div>


          <!-- ================= -->
          <!-- DOCUMENT -->
          <!-- ================= -->
          <div class="min-w-0">

            <label class="mb-2 block text-xs font-bold text-gray-700 sm:text-sm">
              Ajouter un document
              <span class="font-normal text-gray-400">
                (optionnel)
              </span>
            </label>

            <input
              ref="fileInput"
              type="file"
              accept=".pdf,application/pdf"
              class="hidden"
              @change="handleFile"
            />

            <button
              type="button"
              @click="fileInput?.click()"
              class="flex min-h-[160px] w-full flex-col
                     items-center justify-center
                     rounded-xl border border-dashed
                     border-gray-300 bg-gray-50
                     px-4 py-6 text-center
                     transition
                     hover:border-brand-amber
                     hover:bg-white
                     sm:min-h-[180px] sm:px-5"
            >

              <div
                class="flex h-11 w-11 items-center justify-center
                       rounded-xl bg-white text-brand-text shadow-sm"
              >
                <svg
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="1.7"
                >
                  <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8Z" />
                  <path d="M14 2v6h6" />
                  <path d="M8 13h8M8 17h5" />
                </svg>
              </div>

              <p class="mt-3 text-xs font-bold text-gray-700 sm:text-sm">
                Importer un PDF
              </p>

              <p class="mt-1 text-[10px] text-gray-400 sm:text-[11px]">
                PDF uniquement · 5 Mo maximum
              </p>

            </button>


            <!-- Fichier sélectionné -->
            <div
              v-if="selectedFile"
              class="mt-3 flex items-center
                     justify-between gap-3 rounded-xl
                     border border-gray-200 bg-white p-3"
            >

              <div class="flex min-w-0 items-center gap-3">

                <div
                  class="flex h-9 w-9 shrink-0 items-center
                         justify-center rounded-lg
                         bg-red-50 text-red-600"
                >
                  <svg
                    class="h-4 w-4"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="1.8"
                  >
                    <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8Z" />
                    <path d="M14 2v6h6" />
                  </svg>
                </div>

                <div class="min-w-0">
                  <p class="truncate text-xs font-bold text-gray-700">
                    {{ selectedFile.name }}
                  </p>

                  <p class="mt-0.5 text-[10px] text-gray-400">
                    {{ formatFileSize(selectedFile.size) }}
                  </p>
                </div>

              </div>

              <button
                type="button"
                @click="removeFile"
                class="flex h-8 w-8 shrink-0
                       items-center justify-center
                       rounded-lg text-gray-400
                       transition
                       hover:bg-red-50 hover:text-red-600"
                aria-label="Supprimer le fichier"
              >
                ×
              </button>

            </div>

          </div>
        </div>


        <!-- Erreur -->
        <div
          v-if="localError"
          class="mt-5 rounded-xl border border-red-200
                 bg-red-50 p-3 text-xs font-medium text-red-700"
        >
          {{ localError }}
        </div>


        <!-- ===================== -->
        <!-- BOUTON ANALYSER -->
        <!-- ===================== -->
        <div
          class="mt-6 flex flex-col gap-3
                 border-t border-gray-100 pt-5
                 sm:flex-row sm:justify-end"
        >

          <button
            type="button"
            @click="analyze"
            :disabled="loading"
            class="inline-flex h-11 w-full items-center
                   justify-center gap-2 rounded-xl
                   bg-brand-amber px-5
                   text-xs font-bold
                   text-brand-amberDark
                   transition
                   hover:brightness-95
                   disabled:cursor-not-allowed
                   disabled:opacity-60
                   sm:w-auto"
          >

            <svg
              v-if="loading"
              class="h-4 w-4 animate-spin"
              viewBox="0 0 24 24"
              fill="none"
            >
              <circle
                cx="12"
                cy="12"
                r="9"
                stroke="currentColor"
                stroke-opacity=".25"
                stroke-width="2"
              />
              <path
                d="M21 12a9 9 0 0 0-9-9"
                stroke="currentColor"
                stroke-width="2"
              />
            </svg>

            <svg
              v-else
              class="h-4 w-4"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="1.8"
            >
              <path d="M12 3v4" />
              <path d="M5 12H3" />
              <path d="M21 12h-2" />
              <path d="m6.5 6.5 2 2" />
              <path d="m17.5 17.5-2-2" />
              <path d="m17.5 6.5-2 2" />
              <path d="m6.5 17.5 2-2" />
              <path d="M12 17v4" />
              <circle cx="12" cy="12" r="3" />
            </svg>

            {{ loading ? 'Analyse en cours...' : 'Analyser ma demande' }}

          </button>

        </div>


        <!-- ===================== -->
        <!-- RESULTAT -->
        <!-- ===================== -->
        <div
          v-if="result"
          class="mt-6 overflow-hidden rounded-2xl
                 border border-gray-200 bg-gray-50"
        >

          <!-- Header résultat -->
          <div class="border-b border-gray-200 bg-white p-4 sm:p-5">

            <div
              class="flex flex-col gap-3
                     sm:flex-row sm:items-center
                     sm:justify-between"
            >

              <div class="min-w-0">

                <p
                  class="text-[10px] font-bold uppercase
                         tracking-[0.12em] text-gray-400"
                >
                  Recommandation
                </p>

                <h4 class="mt-1 text-base font-bold text-gray-900">
                  Service recommandé
                </h4>

              </div>

              <div
                class="flex w-fit items-center gap-2
                       rounded-xl bg-emerald-50
                       px-3 py-2"
              >
                <span class="h-2 w-2 rounded-full bg-emerald-500"></span>

                <span class="text-xs font-bold text-emerald-700">
                  {{ result.confidence }}% de correspondance
                </span>
              </div>

            </div>
          </div>


          <!-- Contenu résultat -->
          <div class="p-4 sm:p-5">

            <!-- Service -->
            <div
              class="rounded-xl border border-gray-200
                     bg-white p-4"
            >

              <div class="flex items-start gap-3">

                <div
                  class="flex h-10 w-10 shrink-0
                         items-center justify-center
                         rounded-xl bg-brand-amberLight
                         text-brand-text"
                >
                  <svg
                    class="h-5 w-5"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    stroke-width="1.7"
                  >
                    <path d="M3 21h18" />
                    <path d="M5 21V6a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v15" />
                    <path d="M9 8h2M13 8h2M9 12h2M13 12h2" />
                  </svg>
                </div>

                <div class="min-w-0">

                  <p
                    class="truncate text-[10px] font-bold
                           uppercase tracking-wider text-gray-400"
                  >
                    {{ result.service?.nomOrganisme || 'Organisme' }}
                  </p>

                  <p class="mt-1 break-words text-sm font-bold text-gray-900">
                    {{ result.service?.nomService || 'Service recommandé' }}
                  </p>

                  <p
                    v-if="result.service?.ville"
                    class="mt-1 text-xs text-gray-500"
                  >
                    {{ result.service.ville }}
                  </p>

                </div>
              </div>


              <!-- Pertinence -->
              <div class="mt-4">

                <div
                  class="mb-1.5 flex justify-between
                         text-[10px] font-semibold"
                >
                  <span class="text-gray-400">
                    Pertinence
                  </span>

                  <span class="text-gray-600">
                    {{ result.confidence }}%
                  </span>
                </div>

                <div
                  class="h-1.5 overflow-hidden
                         rounded-full bg-gray-100"
                >
                  <div
                    class="h-full rounded-full
                           bg-brand-amber
                           transition-all duration-700"
                    :style="{ width: `${result.confidence}%` }"
                  ></div>
                </div>

              </div>

            </div>


            <!-- Pourquoi -->
            <div
              class="mt-4 rounded-xl border
                     border-gray-200 bg-white p-4"
            >

              <p
                class="text-[10px] font-bold uppercase
                       tracking-wider text-gray-400"
              >
                Pourquoi ce service ?
              </p>

              <p class="mt-2 text-xs leading-5 text-gray-600">
                {{ result.reason }}
              </p>

            </div>


            <!-- Actions -->
            <div
              class="mt-5 flex flex-col gap-3
                     sm:flex-row sm:justify-end"
            >

              <button
                type="button"
                @click="result = null"
                class="h-10 w-full rounded-xl
                       border border-gray-200
                       bg-white px-4
                       text-xs font-bold text-gray-600
                       transition
                       hover:border-gray-300
                       hover:bg-gray-50
                       sm:w-auto"
              >
                Modifier ma demande
              </button>

              <button
                type="button"
                @click="useRecommendation"
                class="h-10 w-full rounded-xl
                       bg-brand-amber px-4
                       text-xs font-bold
                       text-brand-amberDark
                       transition hover:brightness-95
                       sm:w-auto"
              >
                Utiliser ce service
              </button>

            </div>

            <p
              class="mt-4 text-center
                     text-[10px] leading-4 text-gray-400"
            >
              Vous gardez le contrôle : la recommandation doit être
              confirmée avant de poursuivre.
            </p>

          </div>
        </div>

      </div>
    </div>
  </div>
</template>


<script setup>
import { ref } from 'vue'
import { assistantApi } from '@/services/api'

const props = defineProps({
  services: {
    type: Array,
    default: () => []
  }
})

const emit = defineEmits(['select', 'close'])

const isOpen = ref(false)
const description = ref('')
const selectedFile = ref(null)
const fileInput = ref(null)

const loading = ref(false)
const localError = ref('')
const result = ref(null)


function openAssistant() {
  isOpen.value = true
  localError.value = ''
}


function closeAssistant() {
  isOpen.value = false
  localError.value = ''
  result.value = null
  emit('close')
}


function handleFile(event) {
  localError.value = ''

  const file = event.target.files?.[0]

  if (!file) return

  const isPdf =
    file.type === 'application/pdf' ||
    file.name.toLowerCase().endsWith('.pdf')

  if (!isPdf) {
    localError.value = 'Veuillez sélectionner un fichier PDF.'
    event.target.value = ''
    return
  }

  const maxSize = 5 * 1024 * 1024

  if (file.size > maxSize) {
    localError.value = 'Le fichier PDF ne doit pas dépasser 5 Mo.'
    event.target.value = ''
    return
  }

  selectedFile.value = file
}


function removeFile() {
  selectedFile.value = null

  if (fileInput.value) {
    fileInput.value.value = ''
  }
}


function formatFileSize(bytes) {
  if (!bytes) return '0 Ko'

  const mb = bytes / (1024 * 1024)

  if (mb >= 1) {
    return `${mb.toFixed(2)} Mo`
  }

  return `${Math.max(1, Math.round(bytes / 1024))} Ko`
}


async function analyze() {
  localError.value = ''
  result.value = null

  if (!description.value.trim() && !selectedFile.value) {
    localError.value =
      "Décrivez votre demande ou importez un document PDF pour lancer l'analyse."

    return
  }

  loading.value = true

  try {
    const reponse = await assistantApi.suggererDestinataire({
      texte: description.value,
      fichier: selectedFile.value
    })

    const service = props.services.find(
      s =>
        String(s.id) ===
        String(reponse.serviceDestinataireId)
    )

    result.value = {
      service: service || null,
      confidence: Math.round(
        (reponse.confidence ?? 0) * 100
      ),
      reason: reponse.reason
    }

  } catch (err) {

    console.error(
      'Erreur assistant destinataire :',
      err
    )

    localError.value =
      err.message ||
      "Une erreur est survenue lors de l'analyse."

  } finally {
    loading.value = false
  }
}


function useRecommendation() {
  if (!result.value?.service) return

  emit('select', result.value.service)
}
</script>