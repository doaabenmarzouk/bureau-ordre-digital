<template>
  <div class="min-h-screen bg-slate-50 py-8 px-4">
    <div class="max-w-5xl mx-auto">

      <!-- HEADER -->
      <div class="mb-8">
        <button
          type="button"
          @click="router.back()"
          class="text-sm text-slate-500 hover:text-slate-900 mb-4"
        >
          ← Retour
        </button>

        <h1 class="text-2xl font-bold text-slate-900">
          Nouvelle demande
        </h1>

        <p class="mt-1 text-sm text-slate-500">
          Soumettez votre courrier à l'organisme concerné.
        </p>
      </div>

      <!-- STEPPER -->
      <CourrierStepper
        :current-step="currentStep"
      />

      <!-- ERREUR GLOBALE -->
      <div
        v-if="error"
        class="mb-6 p-4 rounded-xl bg-red-50 border border-red-200 text-sm text-red-700"
      >
        {{ error }}
      </div>

      <!-- ÉTAPE 1 -->
      <CourrierStep1Destinataire
        v-if="currentStep === 1"
        :services="services"
        :loading="loadingServices"
        :error="error"
        :selected-service="selectedService"
        @select="selectedService = $event"
        @continue="goToStep2"
      />

      <!-- ÉTAPE 2 -->
      <CourrierStep2Demande
        v-else-if="currentStep === 2"
        :selected-service="selectedService"
        :types-courrier="typesCourrier"
        :form="form"
        :error="error"
        @back="currentStep = 1"
        @continue="goToStep3"
      />

      <!-- ÉTAPE 3 -->
      <CourrierStep3Verification
        v-else-if="currentStep === 3"
        :selected-service="selectedService"
        :type-courrier="selectedTypeCourrier"
        :form="form"
        @back="currentStep = 2"
        @continue="submitCourrier"
      />

      <!-- ÉTAPE 4 -->
      <CourrierStep4Confirmation
        v-else-if="currentStep === 4"
        :reference="reference"
        @finish="goToCourriers"
      />

    </div>
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

// --------------------------------------------------
// ÉTAT
// --------------------------------------------------

const currentStep = ref(1)

const selectedService = ref(null)

const services = ref([])
const loadingServices = ref(false)

const typesCourrier = ref([])

const error = ref('')

const reference = ref('')

// --------------------------------------------------
// FORMULAIRE
// --------------------------------------------------

const form = ref({
  typeCourrierId: '',
  objet: '',
  description: '',
  fichiers: []
})

// --------------------------------------------------
// TYPE DE COURRIER SÉLECTIONNÉ
// --------------------------------------------------

const selectedTypeCourrier = computed(() => {
  return typesCourrier.value.find(
    type => String(type.id) === String(form.value.typeCourrierId)
  ) || null
})

// --------------------------------------------------
// CHARGER LES SERVICES DESTINATAIRES
// --------------------------------------------------

async function loadServices() {
  loadingServices.value = true
  try {
    error.value = ''

    const data = await organismesApi.list()

    console.log('Services destinataires reçus :', data)

    services.value = Array.isArray(data) ? data : []

    if (services.value.length === 0) {
      error.value = 'Aucun organisme disponible.'
    }
  } catch (err) {
    console.error('Erreur chargement services destinataires :', err)

    error.value =
      err.message ||
      'Impossible de charger les organismes destinataires.'
  } finally {
    loadingServices.value = false
  }
}

// --------------------------------------------------
// CHARGER LES TYPES
// --------------------------------------------------

async function loadTypesCourrier() {
  try {
    error.value = ''

    const data = await typeCourrierApi.list()

    console.log('Types courrier reçus :', data)

    typesCourrier.value = Array.isArray(data)
      ? data
      : []

    if (typesCourrier.value.length === 0) {
      error.value = 'Aucun type de courrier disponible.'
    }

  } catch (err) {
    console.error('Erreur chargement types courrier :', err)

    error.value =
      err.message ||
      'Impossible de charger les types de courrier.'
  }
}

// --------------------------------------------------
// ÉTAPE 1 → ÉTAPE 2
// --------------------------------------------------

function goToStep2() {
  error.value = ''

  if (!selectedService.value) {
    error.value = 'Veuillez sélectionner un destinataire.'
    return
  }

  currentStep.value = 2
}

// --------------------------------------------------
// ÉTAPE 2 → ÉTAPE 3
// --------------------------------------------------

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
}

// --------------------------------------------------
// ENVOYER LE COURRIER
// --------------------------------------------------

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

    console.log('Courrier envoyé :', result)

    reference.value =
      result?.numReference ||
      result?.reference ||
      result?.numeroReference ||
      ''

    currentStep.value = 4

  } catch (err) {
    console.error('Erreur soumission courrier :', err)

    error.value =
      err.message ||
      "Une erreur est survenue lors de l'envoi du courrier."
  }
}

// --------------------------------------------------
// FIN
// --------------------------------------------------

function goToCourriers() {
  router.push('/courrier')
}

// --------------------------------------------------
// INITIALISATION
// --------------------------------------------------

onMounted(() => {
  loadTypesCourrier()
  loadServices()
})
</script>