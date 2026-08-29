<template>

  <div
    class="bg-white
           border border-slate-200
           rounded-2xl
           shadow-sm
           p-4 sm:p-6
           mb-7"
  >

    <div class="max-w-5xl mx-auto">

      <div class="flex items-center">

        <template
          v-for="(step, index) in steps"
          :key="step.number"
        >

          <!-- STEP -->
          <div
            class="flex items-center
                   gap-3 shrink-0"
          >

            <div
              class="w-10 h-10
                     rounded-full
                     flex items-center
                     justify-center
                     text-sm font-bold"
              :class="getStepClass(step.number)"
            >

              <svg
                v-if="currentStep > step.number"
                class="w-5 h-5"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2.5"
              >
                <path d="m5 12 4 4L19 6"/>
              </svg>

              <span v-else>
                {{ step.number }}
              </span>

            </div>


            <div class="hidden sm:block">

              <p
                class="text-xs font-semibold"
                :class="
                  currentStep === step.number
                    ? 'text-slate-900'
                    : currentStep > step.number
                      ? 'text-slate-600'
                      : 'text-slate-400'
                "
              >
                {{ step.title }}
              </p>

              <p
                v-if="currentStep === step.number"
                class="text-[11px]
                       text-slate-400
                       mt-0.5"
              >
                Étape en cours
              </p>

            </div>

          </div>


          <!-- LINE -->
          <div
            v-if="index < steps.length - 1"
            class="flex-1 h-[2px]
                   mx-3 sm:mx-5
                   rounded-full"
            :class="
              currentStep > step.number
                ? 'bg-amber-400'
                : 'bg-slate-100'
            "
          />

        </template>

      </div>

    </div>

  </div>

</template>


<script setup>

const props = defineProps({

  currentStep: {
    type: Number,
    required: true
  }

})


const steps = [

  {
    number: 1,
    title: 'Destinataire'
  },

  {
    number: 2,
    title: 'Votre demande'
  },

  {
    number: 3,
    title: 'Vérification'
  },

  {
    number: 4,
    title: 'Confirmation'
  }

]


function getStepClass(step) {

  if (props.currentStep > step) {

    return 'bg-amber-400 text-slate-900'

  }

  if (props.currentStep === step) {

    return `
      bg-slate-900
      text-white
      ring-4
      ring-slate-900/10
    `

  }

  return `
    bg-slate-100
    text-slate-400
  `

}

</script>