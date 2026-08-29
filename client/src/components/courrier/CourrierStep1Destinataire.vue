<template>

  <section
    class="bg-white
           border border-slate-200
           rounded-2xl
           shadow-sm
           overflow-hidden"
  >

    <!-- HEADER -->
    <div
      class="px-5 sm:px-7 lg:px-8
             py-6
             border-b border-slate-100
             bg-gradient-to-r
             from-white to-slate-50/70"
    >

      <h2
        class="text-lg sm:text-xl
               font-bold text-slate-900"
      >
        Choisissez votre destinataire
      </h2>

      <p
        class="text-sm text-slate-500
               mt-1.5"
      >
        Recherchez l'organisme ou le service
        auquel vous souhaitez adresser votre demande.
      </p>

    </div>


    <!-- BODY -->
    <div class="p-5 sm:p-7 lg:p-8">

      <!-- SEARCH -->
      <div class="relative mb-6">

        <svg
          class="absolute left-4
                 top-1/2
                 -translate-y-1/2
                 w-5 h-5
                 text-slate-400"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="1.8"
        >
          <circle cx="11" cy="11" r="7"/>
          <path d="m20 20-4-4"/>
        </svg>

        <input
          v-model="search"
          type="text"
          placeholder="Rechercher un organisme, un service ou une ville..."
          class="w-full
                 h-13
                 pl-12 pr-4
                 border border-slate-200
                 rounded-xl
                 bg-slate-50/50
                 text-sm
                 outline-none
                 focus:bg-white
                 focus:border-amber-400
                 focus:ring-4
                 focus:ring-amber-400/10"
        />

      </div>


      <!-- CITY -->
      <div class="mb-7">

        <div class="flex gap-2 mb-3">

          <span
            class="text-xs
                   font-semibold
                   text-slate-700"
          >
            Filtrer par ville
          </span>

          <span
            class="text-[11px]
                   text-slate-400"
          >
            Optionnel
          </span>

        </div>


        <div class="flex flex-wrap gap-2">

          <button
            v-for="city in cities"
            :key="city"
            type="button"
            @click="toggleCity(city)"
            class="px-3.5 py-2
                   rounded-lg
                   text-xs
                   font-medium
                   border"
            :class="
              selectedCity === city
                ? 'bg-slate-900 border-slate-900 text-white'
                : 'bg-white border-slate-200 text-slate-600'
            "
          >
            {{ city }}
          </button>

        </div>

      </div>


      <!-- RESULT -->
      <div
        class="flex justify-between
               items-center
               mb-4"
      >

        <div>

          <h3
            class="text-sm
                   font-bold
                   text-slate-800"
          >
            Organismes disponibles
          </h3>

          <p
            class="text-xs
                   text-slate-400
                   mt-0.5"
          >
            Sélectionnez le service correspondant à votre demande.
          </p>

        </div>

        <span
          class="px-3 py-1.5
                 rounded-lg
                 bg-slate-100
                 text-xs
                 font-semibold
                 text-slate-500"
        >
          {{ filteredServices.length }} résultat(s)
        </span>

      </div>


      <!-- LOADING -->
      <div
        v-if="loading"
        class="py-16
               text-center"
      >

        <div
          class="spinner mx-auto mb-4"
        />

        <p
          class="text-sm
                 font-medium"
        >
          Chargement des organismes...
        </p>

      </div>


      <!-- SERVICES -->
      <div
        v-else-if="filteredServices.length"
        class="grid
               grid-cols-1
               md:grid-cols-2
               xl:grid-cols-3
               gap-4"
      >

        <button
          v-for="service in filteredServices"
          :key="service.id"
          type="button"
          @click="selectService(service)"
          class="relative
                 text-left
                 rounded-2xl
                 border
                 p-5
                 transition"
          :class="
            selectedService?.id === service.id
              ? 'border-amber-400 bg-amber-50/60 shadow-md'
              : 'border-slate-200 bg-white hover:shadow-lg'
          "
        >

          <!-- CHECK -->
          <div
            v-if="selectedService?.id === service.id"
            class="absolute
                   top-4 right-4
                   w-7 h-7
                   rounded-full
                   bg-amber-400
                   flex items-center
                   justify-center"
          >
            ✓
          </div>


          <div
            class="w-12 h-12
                   rounded-xl
                   bg-slate-100
                   flex items-center
                   justify-center"
          >

            <svg
              class="w-6 h-6
                     text-slate-600"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="1.6"
            >
              <path d="M3 21h18"/>
              <path d="M5 21V7l7-4 7 4v14"/>
              <path d="M9 21v-5h6v5"/>
            </svg>

          </div>


          <div class="mt-5">

            <h3
              class="font-bold
                     text-sm
                     text-slate-900"
            >
              {{ service.nomOrganisme }}
            </h3>

            <p
              class="text-sm
                     text-slate-500
                     mt-1"
            >
              {{ service.nomService }}
            </p>

          </div>


          <div
            class="mt-5
                   flex flex-wrap gap-2"
          >

            <span
              class="text-[11px]
                     bg-slate-50
                     border
                     border-slate-100
                     rounded-lg
                     px-2.5 py-1.5
                     text-slate-500"
            >
              {{ service.ville }}
            </span>

            <span
              v-if="service.typeOrganisme"
              class="text-[11px]
                     bg-slate-50
                     border
                     border-slate-100
                     rounded-lg
                     px-2.5 py-1.5
                     text-slate-500"
            >
              {{ service.typeOrganisme }}
            </span>

          </div>

        </button>

      </div>


      <!-- EMPTY -->
      <div
        v-else
        class="py-16
               text-center
               border border-dashed
               border-slate-200
               rounded-2xl"
      >

        <p
          class="text-sm
                 font-semibold
                 text-slate-700"
        >
          Aucun organisme trouvé
        </p>

        <p
          class="text-xs
                 text-slate-400
                 mt-1"
        >
          Essayez une autre recherche.
        </p>

      </div>


      <!-- ERROR -->
      <div
        v-if="error"
        class="mt-6
               p-4
               rounded-xl
               bg-red-50
               border border-red-100
               text-sm
               text-red-700"
      >
        {{ error }}
      </div>


      <!-- ACTION -->
      <div
        class="flex justify-end
               mt-7 pt-6
               border-t
               border-slate-100"
      >

        <button
          type="button"
          :disabled="!selectedService"
          @click="$emit('continue')"
          class="px-6 py-3.5
                 rounded-xl
                 text-sm
                 font-bold"
          :class="
            selectedService
              ? 'bg-slate-900 text-white'
              : 'bg-slate-100 text-slate-400 cursor-not-allowed'
          "
        >
          Continuer →
        </button>

      </div>

    </div>

  </section>

</template>


<script setup>

import {
  ref,
  computed
} from 'vue'


const props = defineProps({

  services: {
    type: Array,
    default: () => []
  },

  loading: Boolean,

  error: {
    type: String,
    default: ''
  },

  selectedService: {
    type: Object,
    default: null
  }

})


const emit = defineEmits([
  'select',
  'continue'
])


const search = ref('')

const selectedCity = ref('')


/* =========================
   CITIES
========================= */

const cities = computed(() => {

  return [
    ...new Set(
      props.services
        .map(service => service.ville)
        .filter(Boolean)
    )
  ]

})


/* =========================
   FILTER
========================= */

const filteredServices = computed(() => {

  let result = [...props.services]


  if (selectedCity.value) {

    result = result.filter(
      service =>
        service.ville === selectedCity.value
    )

  }


  if (search.value.trim()) {

    const q =
      search.value
        .trim()
        .toLowerCase()

    result = result.filter(service => {

      return (

        service.nomOrganisme
          ?.toLowerCase()
          .includes(q)

        ||

        service.nomService
          ?.toLowerCase()
          .includes(q)

        ||

        service.ville
          ?.toLowerCase()
          .includes(q)

        ||

        service.typeOrganisme
          ?.toLowerCase()
          .includes(q)

      )

    })

  }


  return result

})


function toggleCity(city) {

  selectedCity.value =
    selectedCity.value === city
      ? ''
      : city

}


function selectService(service) {

  emit('select', service)

}

</script>


<style scoped>

.spinner {

  width: 34px;
  height: 34px;

  border-radius: 50%;

  border: 3px solid #f1f5f9;

  border-top-color: #f59e0b;

  animation: spin .7s linear infinite;

}

@keyframes spin {

  to {
    transform: rotate(360deg);
  }

}

</style>