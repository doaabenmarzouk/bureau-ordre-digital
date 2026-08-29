<template>

  <section
    class="max-w-2xl
           mx-auto"
  >

    <div
      class="bg-white
             border
             border-slate-200
             rounded-3xl
             shadow-sm
             overflow-hidden"
    >

      <div
        class="p-8 sm:p-12
               text-center"
      >

        <!-- SUCCESS -->
        <div
          class="success-icon
                 mx-auto mb-6"
        >

          <svg
            class="w-8 h-8"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2.5"
          >
            <path d="m5 12 4 4L19 6"/>
          </svg>

        </div>


        <span
          class="inline-flex
                 px-3 py-1.5
                 rounded-full
                 bg-emerald-50
                 text-emerald-700
                 text-xs
                 font-bold"
        >
          Demande enregistrée
        </span>


        <h2
          class="text-2xl sm:text-3xl
                 font-bold
                 text-slate-950
                 mt-4"
        >
          Courrier envoyé avec succès
        </h2>


        <p
          class="text-sm
                 text-slate-500
                 mt-3
                 leading-6"
        >
          Votre demande a bien été transmise.
          Vous pouvez maintenant suivre son avancement
          depuis votre espace citoyen.
        </p>


        <!-- REFERENCE -->
        <div
          class="mt-8
                 p-5
                 bg-slate-50
                 border
                 border-slate-200
                 rounded-2xl"
        >

          <p
            class="text-[11px]
                   uppercase
                   tracking-wider
                   font-semibold
                   text-slate-400"
          >
            Numéro de référence
          </p>

          <p
            class="text-2xl sm:text-3xl
                   font-bold
                   text-slate-900
                   tracking-wide
                   mt-2"
          >
            {{ reference }}
          </p>

          <p
            class="text-xs
                   text-slate-400
                   mt-2"
          >
            Conservez cette référence
            pour suivre votre demande.
          </p>

        </div>


        <!-- ACTIONS -->
        <div
          class="flex
                 flex-col sm:flex-row
                 justify-center
                 gap-3
                 mt-8"
        >

          <RouterLink
            to="/courrier"
            class="px-6 py-3.5
                   bg-slate-900
                   text-white
                   rounded-xl
                   text-sm
                   font-bold"
          >
            Voir mes courriers
          </RouterLink>


          <RouterLink
            to="/"
            class="px-6 py-3.5
                   border
                   border-slate-200
                   rounded-xl
                   text-sm
                   font-semibold
                   text-slate-600"
          >
            Retour à l'accueil
          </RouterLink>

        </div>

      </div>

    </div>

  </section>

</template>


<script setup>

defineProps({

  reference: {
    type: [String, Number],
    default: '—'
  }

})

</script>


<style scoped>

.success-icon {

  width: 72px;
  height: 72px;

  border-radius: 50%;

  background: #ecfdf5;

  color: #059669;

  display: flex;

  align-items: center;

  justify-content: center;

  margin-left: auto;
  margin-right: auto;

}

</style>