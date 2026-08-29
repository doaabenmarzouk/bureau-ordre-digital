<template>

  <section
    class="grid
           grid-cols-1
           lg:grid-cols-3
           gap-6"
  >

    <!-- SUMMARY -->
    <div
      class="lg:col-span-2
             bg-white
             border
             border-slate-200
             rounded-2xl
             shadow-sm
             overflow-hidden"
    >

      <div
        class="px-5 sm:px-7
               py-6
               border-b
               border-slate-100"
      >

        <h2
          class="text-xl
                 font-bold
                 text-slate-900"
        >
          Vérifiez votre demande
        </h2>

        <p
          class="text-sm
                 text-slate-500
                 mt-1"
        >
          Vérifiez les informations avant l'envoi définitif.
        </p>

      </div>


      <div
        class="p-5 sm:p-7
               space-y-7"
      >

        <SummaryItem
          label="Destinataire"
          :value="selectedService?.nomOrganisme"
          :secondary="
            `${selectedService?.nomService} · ${selectedService?.ville}`
          "
        />


        <SummaryItem
          label="Type de courrier"
          :value="selectedTypeLabel"
        />


        <SummaryItem
          label="Objet"
          :value="form.objet"
        />


        <div>

          <p class="summary-label">
            Description
          </p>

          <div
            class="mt-2
                   bg-slate-50
                   border
                   border-slate-100
                   rounded-xl
                   p-4
                   text-sm
                   text-slate-600
                   whitespace-pre-line
                   leading-6"
          >
            {{ form.description }}
          </div>

        </div>


        <div
          v-if="form.fichiers.length"
        >

          <p class="summary-label">
            Pièces jointes
          </p>

          <div
            class="mt-3
                   space-y-2"
          >

            <div
              v-for="file in form.fichiers"
              :key="file.name"
              class="px-3 py-2.5
                     bg-slate-50
                     border
                     border-slate-100
                     rounded-xl
                     text-sm
                     text-slate-600"
            >
              {{ file.name }}
            </div>

          </div>

        </div>

      </div>

    </div>


    <!-- USER -->
    <aside
      class="bg-white
             border
             border-slate-200
             rounded-2xl
             shadow-sm
             p-6
             h-fit"
    >

      <h3
        class="font-bold
               text-slate-900"
      >
        Vos coordonnées
      </h3>

      <p
        class="text-xs
               text-slate-400
               mt-1 mb-6"
      >
        Les informations de votre compte
        seront associées à cette demande.
      </p>


      <div class="space-y-5">

        <div>

          <p class="summary-label">
            Nom complet
          </p>

          <p class="text-sm font-semibold">
            {{ user?.prenom }}
            {{ user?.nom }}
          </p>

        </div>


        <div>

          <p class="summary-label">
            E-mail
          </p>

          <p class="text-sm break-all">
            {{ user?.email || '—' }}
          </p>

        </div>


        <div>

          <p class="summary-label">
            Téléphone
          </p>

          <p class="text-sm">
            {{ user?.telephone || '—' }}
          </p>

        </div>

      </div>

    </aside>


    <!-- ACTIONS -->
    <div
      class="lg:col-span-2
             flex
             flex-col-reverse
             sm:flex-row
             sm:justify-between
             gap-3"
    >

      <button
        type="button"
        @click="$emit('back')"
        class="px-5 py-3.5
               border
               border-slate-200
               rounded-xl
               text-sm
               font-semibold"
      >
        ← Modifier
      </button>


      <button
        type="button"
        :disabled="submitting"
        @click="$emit('submit')"
        class="px-7 py-3.5
               rounded-xl
               bg-amber-400
               text-slate-900
               text-sm
               font-bold
               disabled:opacity-60"
      >

        {{ submitting
          ? 'Envoi en cours...'
          : 'Envoyer le courrier'
        }}

      </button>

    </div>

  </section>

</template>


<script setup>

import {
  computed
} from 'vue'

import SummaryItem
  from './SummaryItem.vue'


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

  user: {
    type: Object,
    default: null
  },

  submitting: Boolean

})


defineEmits([
  'back',
  'submit'
])


const selectedTypeLabel =
  computed(() => {

    const type =
      props.typesCourrier.find(
        item =>
          Number(item.id) ===
          Number(
            props.form.typeCourrierId
          )
      )

    return type?.libelle || '—'

  })

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