<script setup>
import { ref } from 'vue'
import { authApi } from '@/services/api'

const email = ref('')
const loading = ref(false)
const message = ref('')
const error = ref('')

const submit = async () => {
  message.value = ''
  error.value = ''

  if (!email.value) {
    error.value = 'Veuillez saisir votre adresse email.'
    return
  }

  loading.value = true

  try {
    const response = await authApi.forgotPassword({
      email: email.value
    })

    message.value = response.message
  } catch (err) {
    error.value =
      err.message || "Une erreur est survenue."
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center
           px-4 bg-[#FFF9EC]"
  >

    <div
      class="w-full max-w-md
             bg-white
             rounded-2xl
             shadow-lg
             border border-[#F5B942]/20
             p-8"
    >

      <!-- Titre -->
      <div class="text-center mb-7">

        <h1
          class="text-2xl
                 font-bold
                 text-gray-800"
        >
          Mot de passe oublié ?
        </h1>

        <p
          class="text-gray-500
                 text-sm
                 mt-3
                 leading-relaxed"
        >
          Entrez votre adresse email pour recevoir
          un lien de réinitialisation.
        </p>

      </div>


      <!-- Formulaire -->
      <form @submit.prevent="submit">

        <label
          class="block
                 text-sm
                 font-semibold
                 text-gray-700
                 mb-2"
        >
          Adresse email
        </label>


        <!-- Email -->
        <div class="relative">

          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="absolute left-4 top-1/2
                   -translate-y-1/2
                   w-5 h-5
                   text-[#D49B24]"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            stroke-width="1.8"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M3 7.5A2.5 2.5 0 015.5 5h13A2.5 2.5 0 0121 7.5v9a2.5 2.5 0 01-2.5 2.5h-13A2.5 2.5 0 013 16.5v-9z"
            />

            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M3.5 7l8.5 6 8.5-6"
            />
          </svg>

          <input
            v-model="email"
            type="email"
            placeholder="exemple@email.com"
            required
            class="w-full
                   border border-gray-300
                   rounded-xl
                   pl-12 pr-4
                   py-3.5
                   text-gray-800
                   placeholder-gray-400
                   focus:outline-none
                   focus:border-[#F5B942]
                   focus:ring-2
                   focus:ring-[#F5B942]/20"
          />

        </div>


        <!-- Bouton -->
        <button
          type="submit"
          :disabled="loading"
          class="w-full
                 mt-6
                 bg-[#F5B942]
                 hover:bg-[#E9AA2F]
                 text-white
                 font-semibold
                 py-3.5
                 rounded-xl
                 shadow-md
                 transition
                 disabled:opacity-50
                 disabled:cursor-not-allowed"
        >
          {{ loading
            ? 'Envoi...'
            : 'Envoyer le lien'
          }}
        </button>

      </form>


      <!-- Message -->
      <div
        v-if="message"
        class="mt-5
               p-3
               bg-[#FFF9E6]
               border border-[#F5B942]/40
               rounded-xl
               text-sm
               text-[#8A6508]"
      >
        {{ message }}
      </div>


      <!-- Erreur -->
      <div
        v-if="error"
        class="mt-5
               p-3
               bg-red-50
               border border-red-200
               rounded-xl
               text-sm
               text-red-700"
      >
        {{ error }}
      </div>


      <!-- Retour -->
      <div
        class="text-center
               mt-6
               pt-5
               border-t border-gray-100"
      >

        <RouterLink
          to="/login"
          class="inline-flex
                 items-center
                 gap-2
                 text-sm
                 font-semibold
                 text-[#B8860B]
                 hover:text-[#D49B24]
                 transition"
        >
          <span class="text-lg">←</span>
          Retour à la connexion
        </RouterLink>

      </div>

    </div>

  </div>
</template>