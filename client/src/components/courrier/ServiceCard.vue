<template>
  <button
    type="button"
    @click="selectService"
    class="group relative w-full overflow-hidden rounded-2xl border bg-white p-5 text-left transition-all duration-200 focus:outline-none focus:ring-4 focus:ring-brand-amberLight"
    :class="
      selected
        ? 'border-brand-amber bg-brand-amberLight/40 shadow-md shadow-brand-amberLight'
        : 'border-gray-200 hover:-translate-y-0.5 hover:border-gray-300 hover:shadow-lg'
    "
  >
    <!-- Barre de sélection -->
    <div
      v-if="selected"
      class="absolute left-0 top-0 h-full w-1 bg-brand-amber"
    ></div>

    <!-- Badge sélection -->
    <div
      v-if="selected"
      class="absolute right-4 top-4 flex h-7 w-7 items-center justify-center rounded-full bg-brand-amber text-brand-amberDark shadow-sm"
    >
      <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
        <path d="m5 12 4 4L19 6" />
      </svg>
    </div>

    <!-- Header -->
    <div class="flex items-start justify-between gap-3">

      <div
        class="flex h-11 w-11 shrink-0 items-center justify-center rounded-xl transition"
        :class="
          selected
            ? 'bg-brand-amberLight text-brand-text'
            : 'bg-gray-100 text-gray-500 group-hover:bg-brand-amber group-hover:text-brand-amberDark'
        "
      >
        <svg class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7">
          <path d="M3 21h18" />
          <path d="M5 21V6a2 2 0 0 1 2-2h10a2 2 0 0 1 2 2v15" />
          <path d="M9 8h2M13 8h2M9 12h2M13 12h2M9 16h2M13 16h2" />
        </svg>
      </div>

      <span
        v-if="service.typeOrganisme"
        class="max-w-[130px] truncate rounded-lg border border-gray-100 bg-gray-50 px-2.5 py-1.5 text-[10px] font-bold text-gray-500"
      >
        {{ service.typeOrganisme }}
      </span>
    </div>

    <!-- Contenu -->
    <div class="mt-5">

      <p
        class="text-[10px] font-bold uppercase tracking-[0.12em]"
        :class="selected ? 'text-brand-text' : 'text-gray-400'"
      >
        Organisme
      </p>

      <h3
        class="mt-1.5 line-clamp-2 pr-8 text-sm font-bold leading-5"
        :class="selected ? 'text-gray-900' : 'text-gray-800'"
      >
        {{ service.nomOrganisme || 'Organisme' }}
      </h3>

      <div class="my-4 h-px bg-gray-100"></div>

      <p class="text-[10px] font-bold uppercase tracking-[0.12em] text-gray-400">
        Service
      </p>

      <p class="mt-1.5 line-clamp-2 text-sm font-semibold leading-5 text-gray-600">
        {{ service.nomService || 'Service non renseigné' }}
      </p>
    </div>

    <!-- Localisation -->
    <div class="mt-5 flex flex-wrap gap-2">
      <span
        v-if="service.ville"
        class="inline-flex items-center gap-1.5 rounded-lg bg-gray-50 px-2.5 py-1.5 text-[11px] font-medium text-gray-500"
      >
        <svg class="h-3.5 w-3.5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
          <path d="M20 10c0 5-8 11-8 11S4 15 4 10a8 8 0 1 1 16 0Z" />
          <circle cx="12" cy="10" r="2.5" />
        </svg>
        {{ service.ville }}
      </span>
    </div>

    <!-- Action -->
    <div class="mt-5 flex items-center justify-between border-t border-gray-100 pt-4">

      <span
        class="text-[11px] font-bold transition"
        :class="selected ? 'text-brand-text' : 'text-gray-400 group-hover:text-gray-700'"
      >
        {{ selected ? 'Service sélectionné' : 'Sélectionner' }}
      </span>

      <span
        class="flex h-8 w-8 items-center justify-center rounded-lg transition"
        :class="
          selected
            ? 'bg-brand-amberLight text-brand-text'
            : 'bg-gray-50 text-gray-400 group-hover:bg-brand-amber group-hover:text-brand-amberDark'
        "
      >
        <svg class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <path d="M5 12h14" />
          <path d="m13 6 6 6-6 6" />
        </svg>
      </span>
    </div>
  </button>
</template>

<script setup>
const props = defineProps({
  service: {
    type: Object,
    required: true
  },
  selected: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['select'])

function selectService() {
  emit('select', props.service)
}
</script>