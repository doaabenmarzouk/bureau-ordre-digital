<template>

  <section
    class="grid grid-cols-1 lg:grid-cols-3 gap-6"
  >

    <!-- ===================================================== -->
    <!-- RÉSUMÉ DE LA DEMANDE -->
    <!-- ===================================================== -->

    <div
      class="lg:col-span-2 bg-white border border-slate-200
             rounded-2xl shadow-sm overflow-hidden"
    >

      <!-- HEADER -->
      <div
        class="px-5 sm:px-7 py-6
               border-b border-slate-100"
      >

        <h2
          class="text-xl font-bold text-slate-900"
        >
          Vérifiez votre demande
        </h2>

        <p
          class="text-sm text-slate-500 mt-1"
        >
          Vérifiez les informations avant l'envoi définitif.
        </p>

      </div>


      <!-- CONTENU -->
      <div
        class="p-5 sm:p-7 space-y-7"
      >

        <!-- DESTINATAIRE -->
        <SummaryItem
          label="Destinataire"
          :value="selectedService?.nomOrganisme"
          :secondary="
            [
              selectedService?.nomService,
              selectedService?.ville
            ]
              .filter(Boolean)
              .join(' · ')
          "
        />


        <!-- TYPE COURRIER -->
        <SummaryItem
          label="Type de courrier"
          :value="selectedTypeLabel"
        />


        <!-- OBJET -->
        <SummaryItem
          label="Objet"
          :value="form.objet"
        />


        <!-- DESCRIPTION -->
        <div>

          <p class="summary-label">
            Description
          </p>

          <div
            class="mt-2 bg-slate-50 border border-slate-100
                   rounded-xl p-4 text-sm text-slate-600
                   whitespace-pre-line leading-6"
          >
            {{ form.description || '—' }}
          </div>

        </div>


        <!-- PIÈCES JOINTES -->
        <div
          v-if="form.fichiers?.length"
        >

          <p class="summary-label">
            Pièces jointes
          </p>

          <div
            class="mt-3 space-y-2"
          >

            <div
              v-for="(file, index) in form.fichiers"
              :key="`${file.name}-${index}`"
              class="flex items-center justify-between
                     px-3 py-2.5 bg-slate-50
                     border border-slate-100
                     rounded-xl text-sm text-slate-600"
            >

              <span class="truncate">
                {{ file.name }}
              </span>

              <span class="ml-3 shrink-0 text-xs text-slate-400">
                {{ formatFileSize(file.size) }}
              </span>

            </div>

          </div>

        </div>

      </div>

    </div>


    <!-- ===================================================== -->
    <!-- INFORMATIONS UTILISATEUR -->
    <!-- ===================================================== -->

    <aside
      class="bg-white border border-slate-200
             rounded-2xl shadow-sm p-6 h-fit"
    >

      <h3
        class="font-bold text-slate-900"
      >
        Vos coordonnées
      </h3>

      <p
        class="text-xs text-slate-400 mt-1 mb-6 leading-5"
      >
        Les informations de votre compte seront
        associées à cette demande.
      </p>


      <div class="space-y-5">

        <!-- NOM COMPLET -->
        <div>

          <p class="summary-label">
            Nom complet
          </p>

          <p
            class="text-sm font-semibold text-slate-800"
          >
            {{ nomComplet }}
          </p>

        </div>


        <!-- EMAIL -->
        <div>

          <p class="summary-label">
            E-mail
          </p>

          <p
            class="text-sm text-slate-700 break-all"
          >
            {{ user?.email || '—' }}
          </p>

        </div>


        <!-- TÉLÉPHONE -->
        <div>

          <p class="summary-label">
            Téléphone
          </p>

          <p
            class="text-sm text-slate-700"
          >
            {{ user?.telephone || '—' }}
          </p>

        </div>

      </div>

    </aside>


    <!-- ===================================================== -->
    <!-- ACTIONS -->
    <!-- ===================================================== -->

    <div
      class="lg:col-span-2
             flex flex-col-reverse
             sm:flex-row sm:justify-between
             gap-3"
    >

      <!-- MODIFIER -->
      <button
        type="button"
        @click="$emit('back')"
        class="px-5 py-3.5
               border border-slate-200
               rounded-xl text-sm
               font-semibold text-slate-600
               hover:bg-slate-50
               transition"
      >
        ← Modifier
      </button>


      <!-- ENVOYER -->
      <button
        type="button"
        :disabled="submitting"
        @click="$emit('submit')"
        class="px-7 py-3.5
               rounded-xl
               bg-amber-400
               text-slate-900
               text-sm font-bold
               hover:bg-amber-500
               transition
               disabled:opacity-60
               disabled:cursor-not-allowed"
      >

        <span v-if="submitting">
          Envoi en cours...
        </span>

        <span v-else>
          Envoyer le courrier
        </span>

      </button>

    </div>

  </section>

</template>


<script setup>

import { computed } from 'vue'
import { useAuthStore } from '@/stores/auth'
import SummaryItem from './SummaryItem.vue'


// ==========================================================
// PROPS
// ==========================================================

const props = defineProps({

  selectedService: {
    type: Object,
    default: null
  },

  typesCourrier: {
    type: Array,
    default: () => []
  },

  form: {
    type: Object,
    required: true
  },

  submitting: {
    type: Boolean,
    default: false
  }

})


// ==========================================================
// EVENTS
// ==========================================================

defineEmits([
  'back',
  'submit'
])


// ==========================================================
// UTILISATEUR CONNECTÉ
// ==========================================================

const auth = useAuthStore()

const user = computed(() => auth.user)


// ==========================================================
// NOM COMPLET
// ==========================================================

const nomComplet = computed(() => {

  const prenom = user.value?.prenom || ''
  const nom = user.value?.nom || ''

  const resultat = `${prenom} ${nom}`.trim()

  return resultat || '—'

})


// ==========================================================
// TYPE DE COURRIER
// ==========================================================

const selectedTypeLabel = computed(() => {

  const type = props.typesCourrier.find(
    item =>
      String(item.id) ===
      String(props.form.typeCourrierId)
  )

  return type?.libelle || '—'

})


// ==========================================================
// TAILLE FICHIER
// ==========================================================

function formatFileSize(size) {

  if (!size) {
    return ''
  }

  if (size < 1024) {
    return `${size} octets`
  }

  if (size < 1024 * 1024) {
    return `${(size / 1024).toFixed(1)} Ko`
  }

  return `${(size / (1024 * 1024)).toFixed(1)} Mo`

}

</script>


<style scoped>

.summary-label {

  font-size: .68rem;

  color: #94a3b8;

  text-transform: uppercase;

  letter-spacing: .07em;

  font-weight: 700;

  margin-bottom: .4rem;

}

</style>