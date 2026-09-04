<template>
  <section
    class="bg-white border border-slate-200 rounded-2xl shadow-sm overflow-hidden"
  >

    <!-- DESTINATAIRE -->
    <div
      class="px-5 sm:px-7 py-5 border-b border-slate-100 bg-slate-50/50"
    >
      <div class="flex items-center justify-between gap-4">

        <div>
          <p
            class="text-[11px] uppercase tracking-wide text-slate-400 font-semibold"
          >
            Destinataire
          </p>

          <h2 class="text-base font-bold text-slate-900">
            {{ selectedService?.nomOrganisme || 'Destinataire' }}
          </h2>

          <p class="text-xs text-slate-500">
            {{ selectedService?.nomService || '' }}
            <span v-if="selectedService?.ville"> · </span>
            {{ selectedService?.ville || '' }}
          </p>
        </div>

        <button
          type="button"
          @click="$emit('back')"
          class="text-xs font-semibold text-slate-500 hover:text-slate-900"
        >
          Modifier
        </button>

      </div>
    </div>

    <!-- FORMULAIRE -->
    <div class="p-5 sm:p-7 lg:p-8">

      <div class="max-w-4xl">

        <!-- ASSISTANT DE GÉNÉRATION -->
        <AssistantGeneration
          :types-courrier="typesCourrier"
          @apply="appliquerGeneration"
        />

        <!-- TYPE DE COURRIER -->
        <div class="form-group">

          <label class="field-label">
            Type de courrier
            <span>*</span>
          </label>

          <select
            v-model="form.typeCourrierId"
            class="field"
          >
            <option value="">
              Sélectionnez le type de votre demande
            </option>

            <option
              v-for="type in typesCourrier"
              :key="type.id"
              :value="type.id"
            >
              {{ type.libelle }}
            </option>
          </select>

          <!-- DEBUG -->
          <p
            v-if="typesCourrier.length === 0"
            class="mt-2 text-xs text-red-500"
          >
            Aucun type de courrier chargé.
          </p>

        </div>

        <!-- OBJET -->
        <div class="form-group">

          <label class="field-label">
            Objet du courrier
            <span>*</span>
          </label>

          <input
            v-model="form.objet"
            type="text"
            maxlength="200"
            class="field"
            placeholder="Ex. Demande de renseignement..."
          />

          <div class="flex justify-between mt-2">

            <p class="helper">
              Utilisez un intitulé court et précis.
            </p>

            <span class="text-[11px] text-slate-400">
              {{ form.objet.length }}/200
            </span>

          </div>

          <AssistantRedaction
            v-model="form.objet"
            champ="objet"
          />

        </div>

        <!-- DESCRIPTION -->
        <div class="form-group">

          <label class="field-label">
            Description de votre demande
            <span>*</span>
          </label>

          <textarea
            v-model="form.description"
            rows="8"
            maxlength="3000"
            class="field resize-none leading-6"
            placeholder="Expliquez votre demande avec suffisamment de détails..."
          ></textarea>

          <div class="flex justify-between mt-2">

            <p class="helper">
              Décrivez précisément votre demande.
            </p>

            <span class="text-[11px] text-slate-400">
              {{ form.description.length }}/3000
            </span>

          </div>

          <AssistantRedaction
            v-model="form.description"
            champ="description"
          />

        </div>

        <!-- FICHIERS -->
        <FileUpload
          v-model="form.fichiers"
          @error="fileError = $event"
        />

        <!-- ERREUR -->
        <div
          v-if="error || fileError"
          class="mt-6 p-4 rounded-xl bg-red-50 border border-red-100 text-sm text-red-700"
        >
          {{ error || fileError }}
        </div>

        <!-- ACTIONS -->
        <div
          class="flex flex-col-reverse sm:flex-row sm:justify-between gap-3 mt-8 pt-6 border-t border-slate-100"
        >

          <button
            type="button"
            @click="$emit('back')"
            class="px-5 py-3.5 border border-slate-200 rounded-xl text-sm font-semibold text-slate-600 hover:bg-slate-50"
          >
            ← Retour
          </button>

          <button
            type="button"
            @click="$emit('continue')"
            class="px-6 py-3.5 rounded-xl bg-yellow-400 text-slate-900 text-sm font-bold hover:bg-yellow-500"
          >
            Continuer →
          </button>

        </div>

      </div>
    </div>

  </section>
</template>

<script setup>
import { ref } from 'vue'
import FileUpload from './FileUpload.vue'
import AssistantRedaction from './AssistantRedaction.vue'
import AssistantGeneration from './AssistantGeneration.vue'

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

  error: {
    type: String,
    default: ''
  }
})

defineEmits([
  'back',
  'continue'
])

const fileError = ref('')

function appliquerGeneration({ typeCourrierId, objet, description }) {
  if (typeCourrierId) {
    props.form.typeCourrierId = typeCourrierId
  }
  props.form.objet = objet
  props.form.description = description
}
</script>

<style scoped>
.form-group {
  margin-bottom: 1.75rem;
}

.field-label {
  display: block;
  font-size: .875rem;
  font-weight: 700;
  color: #334155;
  margin-bottom: .6rem;
}

.field-label span {
  color: #f59e0b;
  margin-left: 2px;
}

.field {
  width: 100%;
  min-height: 50px;
  border: 1px solid #e2e8f0;
  border-radius: .8rem;
  padding: .8rem 1rem;
  font-size: .875rem;
  outline: none;
  background: white;
}

.field:focus {
  border-color: #f59e0b;
  box-shadow: 0 0 0 4px rgba(245,158,11,.08);
}

.helper {
  font-size: .72rem;
  color: #94a3b8;
}
</style>