<template>

  <div>

    <label
      class="field-label"
    >
      Pièces jointes

      <span>
        (facultatif)
      </span>
    </label>


    <!-- DROP ZONE -->
    <div
      class="border-2
             border-dashed
             border-slate-200
             rounded-2xl
             p-8
             text-center
             bg-slate-50/50
             hover:border-amber-300
             transition
             cursor-pointer"
      @click="openDialog"
      @dragover.prevent
      @drop.prevent="handleDrop"
    >

      <div
        class="w-14 h-14
               mx-auto
               rounded-2xl
               bg-white
               border
               border-slate-200
               flex
               items-center
               justify-center
               text-slate-500"
      >

        <svg
          class="w-7 h-7"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="1.6"
        >
          <path d="M12 16V4"/>
          <path d="m7 9 5-5 5 5"/>
          <path d="M5 20h14"/>
        </svg>

      </div>


      <p
        class="text-sm
               font-bold
               text-slate-800
               mt-4"
      >
        Déposez vos documents ici
      </p>


      <p
        class="text-xs
               text-slate-400
               mt-1"
      >
        ou cliquez pour sélectionner vos fichiers
      </p>


      <div
        class="flex
               flex-wrap
               justify-center
               gap-2
               mt-4"
      >

        <span
          v-for="type in fileTypes"
          :key="type"
          class="file-type"
        >
          {{ type }}
        </span>

      </div>


      <p
        class="text-[11px]
               text-slate-400
               mt-4"
      >
        Maximum 5 Mo par fichier ·
        20 Mo au total
      </p>

    </div>


    <!-- INPUT -->
    <input
      ref="fileInput"
      type="file"
      multiple
      class="hidden"
      accept=".pdf,.jpeg,.jpg,.png,.docx,.xlsx"
      @change="handleFiles"
    />


    <!-- FILE LIST -->
    <div
      v-if="modelValue.length"
      class="mt-4 space-y-2"
    >

      <div
        v-for="(file, index) in modelValue"
        :key="file.name + index"
        class="flex
               items-center
               justify-between
               gap-3
               px-4 py-3
               border
               border-slate-200
               rounded-xl"
      >

        <div class="min-w-0">

          <p
            class="text-sm
                   font-medium
                   truncate"
          >
            {{ file.name }}
          </p>

          <p
            class="text-xs
                   text-slate-400"
          >
            {{ formatSize(file.size) }}
          </p>

        </div>


        <button
          type="button"
          @click="remove(index)"
          class="text-slate-400
                 hover:text-red-600"
        >
          ✕
        </button>

      </div>

    </div>

  </div>

</template>


<script setup>

import { ref } from 'vue'


const props = defineProps({

  modelValue: {
    type: Array,
    default: () => []
  }

})


const emit = defineEmits([
  'update:modelValue',
  'error'
])


const fileInput = ref(null)


const MAX_SIZE =
  5 * 1024 * 1024

const MAX_TOTAL_SIZE =
  20 * 1024 * 1024


const fileTypes = [
  'PDF',
  'JPEG',
  'PNG',
  'DOCX',
  'XLSX'
]


const allowedTypes = [

  'application/pdf',

  'image/jpeg',

  'image/png',

  'application/vnd.openxmlformats-officedocument.wordprocessingml.document',

  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'

]


function openDialog() {

  fileInput.value?.click()

}


function handleFiles(event) {

  addFiles(event.target.files)

  event.target.value = ''

}


function handleDrop(event) {

  addFiles(
    event.dataTransfer?.files
  )

}


function addFiles(fileList) {

  emit('error', '')

  const files =
    Array.from(fileList || [])

  const currentTotal =
    props.modelValue.reduce(
      (total, file) =>
        total + file.size,
      0
    )

  let total =
    currentTotal

  const result =
    [...props.modelValue]


  for (const file of files) {

    if (file.size > MAX_SIZE) {

      emit(
        'error',
        `"${file.name}" dépasse 5 Mo.`
      )

      continue

    }


    if (!allowedTypes.includes(file.type)) {

      emit(
        'error',
        `"${file.name}" n'est pas accepté.`
      )

      continue

    }


    if (
      result.some(
        f =>
          f.name === file.name &&
          f.size === file.size
      )
    ) {

      continue

    }


    if (
      total + file.size >
      MAX_TOTAL_SIZE
    ) {

      emit(
        'error',
        'La taille totale ne doit pas dépasser 20 Mo.'
      )

      continue

    }


    result.push(file)

    total += file.size

  }


  emit(
    'update:modelValue',
    result
  )

}


function remove(index) {

  const result =
    [...props.modelValue]

  result.splice(index, 1)

  emit(
    'update:modelValue',
    result
  )

}


function formatSize(size) {

  if (size < 1024) {

    return `${size} octets`

  }

  if (size < 1024 * 1024) {

    return `${Math.round(size / 1024)} Ko`

  }

  return `${(
    size /
    (1024 * 1024)
  ).toFixed(1)} Mo`

}

</script>


<style scoped>

.field-label {

  display: block;

  font-size: .875rem;

  font-weight: 700;

  color: #334155;

  margin-bottom: .6rem;

}

.field-label span {

  color: #94a3b8;

  font-weight: 400;

}

.file-type {

  padding: .35rem .6rem;

  border-radius: .5rem;

  background: white;

  border: 1px solid #e2e8f0;

  color: #64748b;

  font-size: .65rem;

  font-weight: 700;

}

</style>