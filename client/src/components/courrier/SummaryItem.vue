<template>

  <div
    class="pb-6
           border-b
           border-slate-100"
  >

    <p class="summary-label">
      {{ label }}
    </p>

    <p
      class="text-sm
             font-bold
             text-slate-800"
    >
      {{ value || '—' }}
    </p>

    <p
      v-if="secondary"
      class="text-sm
             text-slate-500
             mt-1"
    >
      {{ secondary }}
    </p>

  </div>

</template>


<script setup>

defineProps({

  label: String,

  value: String,

  secondary: String

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