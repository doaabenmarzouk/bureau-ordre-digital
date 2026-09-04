<template>
  <div class="min-h-screen bg-slate-50">

<header class="border-b border-slate-200 bg-white">

  <div class="mx-auto max-w-[1400px] px-6 py-4 lg:px-8">

    <!-- RETOUR -->
    <button
      type="button"
      @click="router.back()"
      class="group mb-4 inline-flex items-center gap-2
             text-sm font-medium text-slate-500
             transition-all duration-200
             hover:text-slate-900"
    >
      <span
        class="flex h-7 w-7 items-center justify-center
               rounded-lg border border-slate-200
               bg-slate-50
               transition-all duration-200
               group-hover:-translate-x-1
               group-hover:bg-white
               group-hover:shadow-sm"
      >
        <svg
          class="h-4 w-4"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
        >
          <path d="M19 12H5" />
          <path d="m12 19-7-7 7-7" />
        </svg>
      </span>

      Retour
    </button>


    <!-- TITRE -->
    <div class="flex items-center gap-3">

      <!-- Accent jaune -->
      <div
        class="h-10 w-1 rounded-full bg-yellow-400"
      ></div>

      <div>
        <h1
          class="text-2xl font-bold tracking-tight text-slate-900
                 sm:text-3xl"
        >
          Nouvelle demande
        </h1>

        <p class="mt-1 text-sm text-slate-500">
          Déposez votre courrier auprès du service concerné.
        </p>
      </div>

    </div>

  </div>

</header>

    <!-- =========================================================
         CONTENU PRINCIPAL
    ========================================================== -->
    <main class="mx-auto max-w-[1400px] px-6 py-6 lg:px-8 lg:py-8">

      <!-- STEPPER -->
      <div class="mb-6">
        <CourrierStepper :current-step="currentStep" />
      </div>

      <!-- ERREUR GLOBALE -->
      <div v-if="error" class="mb-6 flex items-start gap-3 rounded-xl border border-red-200 bg-red-50 px-4 py-3">
        <div class="flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-red-100 text-red-600">
          <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="9" />
            <path d="M12 8v4" />
            <path d="M12 16h.01" />
          </svg>
        </div>
        <div>
          <p class="text-xs font-bold text-red-800">Une erreur est survenue</p>
          <p class="mt-0.5 text-xs text-red-600">{{ error }}</p>
        </div>
      </div>

      <!-- ZONE PRINCIPALE -->
      <div class="rounded-2xl border border-slate-100 bg-white shadow-sm">

        <div v-if="currentStep === 1" class="p-5 sm:p-7 lg:p-8">
          <CourrierStep1Destinataire
            :services="services"
            :loading="loadingServices"
            :error="error"
            :selected-service="selectedService"
            @select="selectedService = $event"
            @continue="goToStep2"
          />
        </div>

        <div v-else-if="currentStep === 2" class="p-5 sm:p-7 lg:p-8">
          <CourrierStep2Demande
            :selected-service="selectedService"
            :types-courrier="typesCourrier"
            :form="form"
            :error="error"
            @back="currentStep = 1"
            @continue="goToStep3"
          />
        </div>

        <div v-else-if="currentStep === 3" class="p-5 sm:p-7 lg:p-8">
          <CourrierStep3Verification
            :selected-service="selectedService"
            :type-courrier="selectedTypeCourrier"
            :form="form"
            @back="currentStep = 2"
            @continue="submitCourrier"
          />
        </div>

        <div v-else-if="currentStep === 4" class="p-5 sm:p-7 lg:p-8">
          <CourrierStep4Confirmation
            :reference="reference"
            @finish="goToCourriers"
          />
        </div>

      </div>

      <!-- INFORMATIONS -->
      <div v-if="currentStep < 4" class="mt-5 flex items-center gap-2 text-xs text-slate-400">
        <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
          <path d="M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20Z" />
          <path d="M12 8v4" />
          <path d="M12 16h.01" />
        </svg>
        <span>Vos informations sont traitées de manière sécurisée.</span>
      </div>

    </main>
  </div>
</template>


<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'

import {
  typeCourrierApi,
  courrierApi,
  organismesApi
} from '@/services/api'

import CourrierStepper from '@/components/courrier/CourrierStepper.vue'
import CourrierStep1Destinataire from '@/components/courrier/CourrierStep1Destinataire.vue'
import CourrierStep2Demande from '@/components/courrier/CourrierStep2Demande.vue'
import CourrierStep3Verification from '@/components/courrier/CourrierStep3Verification.vue'
import CourrierStep4Confirmation from '@/components/courrier/CourrierStep4Confirmation.vue'

const router = useRouter()

const currentStep = ref(1)
const selectedService = ref(null)
const services = ref([])
const loadingServices = ref(false)
const typesCourrier = ref([])
const error = ref('')
const reference = ref('')

const stepTitle = computed(() => {
  switch (currentStep.value) {
    case 1: return 'Destinataire'
    case 2: return 'Votre demande'
    case 3: return 'Vérification'
    case 4: return 'Confirmation'
    default: return 'Nouvelle demande'
  }
})

const form = ref({
  typeCourrierId: '',
  objet: '',
  description: '',
  fichiers: []
})

const selectedTypeCourrier = computed(() => {
  return typesCourrier.value.find(
    type => String(type.id) === String(form.value.typeCourrierId)
  ) || null
})

async function loadServices() {
  loadingServices.value = true
  try {
    error.value = ''
    const data = await organismesApi.list()
    services.value = Array.isArray(data) ? data : []
    if (services.value.length === 0) {
      error.value = 'Aucun service destinataire disponible.'
    }
  } catch (err) {
    error.value = err.message || 'Impossible de charger les services destinataires.'
  } finally {
    loadingServices.value = false
  }
}

async function loadTypesCourrier() {
  try {
    error.value = ''
    const data = await typeCourrierApi.list()
    typesCourrier.value = Array.isArray(data) ? data : []
    if (typesCourrier.value.length === 0) {
      error.value = 'Aucun type de courrier disponible.'
    }
  } catch (err) {
    error.value = err.message || 'Impossible de charger les types de courrier.'
  }
}

function goToStep2() {
  error.value = ''
  if (!selectedService.value) {
    error.value = 'Veuillez sélectionner un destinataire.'
    return
  }
  currentStep.value = 2
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function goToStep3() {
  error.value = ''
  if (!form.value.typeCourrierId) {
    error.value = 'Veuillez sélectionner un type de courrier.'
    return
  }
  if (!form.value.objet.trim()) {
    error.value = "Veuillez saisir l'objet du courrier."
    return
  }
  if (!form.value.description.trim()) {
    error.value = 'Veuillez saisir la description de votre demande.'
    return
  }
  currentStep.value = 3
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

async function submitCourrier() {
  try {
    error.value = ''
    const result = await courrierApi.submit({
      typeCourrierId: form.value.typeCourrierId,
      serviceDestinataireId: selectedService.value.id,
      objet: form.value.objet,
      description: form.value.description,
      fichiers: form.value.fichiers
    })

    reference.value = result?.numReference || result?.reference || result?.numeroReference || ''
    currentStep.value = 4
    window.scrollTo({ top: 0, behavior: 'smooth' })

  } catch (err) {
    error.value = err.message || "Une erreur est survenue lors de l'envoi du courrier."
  }
}

function goToCourriers() {
  router.push('/courrier')
}

onMounted(() => {
  loadTypesCourrier()
  loadServices()
})
</script>