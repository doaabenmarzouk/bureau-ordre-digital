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
      err.message ||
      "Une erreur est survenue lors de l'envoi de la demande."
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div
    class="min-h-screen flex items-center justify-center
           px-4 py-10
           bg-[#FFF9EC]"
  >

    <div class="w-full max-w-md">

      <!-- Carte principale -->
      <div
        class="bg-white
               rounded-2xl
               shadow-lg
               border border-[#F5B942]/20
               p-8 sm:p-10"
      >

        <!-- Titre -->
        <div class="text-center mb-8">

          <h1
            class="text-2xl sm:text-3xl
                   font-bold
                   text-gray-800"
          >
            Mot de passe oublié ?
          </h1>

          <p
            class="text-gray-500
                   text-sm
                   leading-relaxed
                   mt-3"
          >
            Entrez votre adresse email pour recevoir
            un lien sécurisé afin de réinitialiser
            votre mot de passe.
          </p>

        </div>


        <!-- Formulaire -->
        <form @submit.prevent="submit">

          <!-- Label -->
          <label
            class="block
                   text-sm
                   font-semibold
                   text-gray-700
                   mb-2"
          >
            Adresse email
          </label>


          <!-- Champ email -->
          <div class="relative">

            <!-- Icône email -->
            <div
              class="absolute inset-y-0 left-0
                     pl-4
                     flex items-center
                     pointer-events-none"
            >

              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="w-5 h-5 text-[#D49B24]"
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

            </div>


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
                     bg-white
                     text-gray-800
                     placeholder-gray-400
                     transition
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
                   active:bg-[#D99A18]
                   text-white
                   font-semibold
                   py-3.5
                   rounded-xl
                   shadow-md
                   shadow-[#F5B942]/20
                   transition-all
                   duration-200
                   disabled:opacity-50
                   disabled:cursor-not-allowed"
          >

            <span v-if="!loading">
              Envoyer le lien
            </span>

            <span
              v-else
              class="flex items-center justify-center gap-2"
            >

              <!-- Spinner -->
              <svg
                class="animate-spin w-5 h-5"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
              >

                <circle
                  class="opacity-30"
                  cx="12"
                  cy="12"
                  r="10"
                  stroke="currentColor"
                  stroke-width="4"
                />

                <path
                  class="opacity-90"
                  fill="currentColor"
                  d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
                />

              </svg>

              Envoi en cours...

            </span>

          </button>

        </form>


        <!-- Message succès -->
        <div
          v-if="message"
          class="mt-5
                 p-4
                 bg-green-50
                 border border-green-200
                 rounded-xl"
        >

          <div class="flex items-start gap-3">

            <!-- Icône succès -->
            <div
              class="w-7 h-7
                     rounded-full
                     bg-green-100
                     flex items-center
                     justify-center
                     shrink-0"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="w-4 h-4 text-green-600"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                stroke-width="2"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M5 13l4 4L19 7"
                />
              </svg>
            </div>


            <!-- Texte -->
            <div>

              <p
                class="font-semibold
                       text-green-800
                       text-sm"
              >
                Demande envoyée
              </p>

              <p
                class="text-green-700
                       text-sm
                       leading-relaxed
                       mt-1"
              >
                {{ message }}
              </p>

              <p
                class="text-green-600
                       text-xs
                       mt-2"
              >
                Pensez également à vérifier votre dossier
                de courriers indésirables.
              </p>

            </div>

          </div>

        </div>


        <!-- Message erreur -->
        <div
          v-if="error"
          class="mt-5
                 p-4
                 bg-red-50
                 border border-red-200
                 rounded-xl"
        >

          <div class="flex items-start gap-3">

            <!-- Icône erreur -->
            <div
              class="w-7 h-7
                     rounded-full
                     bg-red-100
                     flex items-center
                     justify-center
                     shrink-0"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="w-4 h-4 text-red-600"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                stroke-width="2"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M6 18L18 6M6 6l12 12"
                />
              </svg>
            </div>


            <div>

              <p
                class="font-semibold
                       text-red-800
                       text-sm"
              >
                Échec de la demande
              </p>

              <p
                class="text-red-700
                       text-sm
                       leading-relaxed
                       mt-1"
              >
                {{ error }}
              </p>

            </div>

          </div>

        </div>


        <!-- Retour connexion -->
        <div
          class="text-center
                 mt-7
                 pt-6
                 border-t
                 border-gray-100"
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
                   transition-colors"
          >

            <span class="text-lg">
              ←
            </span>

            Retour à la connexion

          </RouterLink>

        </div>

      </div>


      <!-- Footer -->
      <p
        class="text-center
               text-xs
               text-gray-400
               mt-6"
      >
        © 2026 Bureau d'Ordre Digital
        <span class="mx-1">•</span>
        Tous droits réservés
      </p>

    </div>

  </div>
</template>