<!-- client/src/views/auth/RegisterView.vue -->

<template>
  <div class="min-h-screen bg-[#F7F7F5] flex items-center justify-center px-4 py-8">

    <div class="w-full max-w-md">

      <!-- Logo -->
      <div class="flex justify-center mb-8">
        <img
          :src="logo"
          alt="NeoLedge MEA"
          class="h-12 w-auto object-contain"
        />
      </div>

      <!-- Card -->
      <div
        class="bg-white border border-gray-200 rounded-2xl p-7
               shadow-[0_8px_30px_rgba(0,0,0,0.04)]"
      >

        <!-- Header -->
        <div class="mb-7">
          <h1 class="text-xl font-semibold tracking-tight text-[#171717]">
            Créer un compte
          </h1>

          <p class="text-sm text-gray-500 mt-1.5">
            Créez votre espace citoyen.
          </p>
        </div>

        <!-- Form -->
        <form @submit.prevent="onSubmit">

          <!-- Nom -->
          <div class="mb-4">
            <label
              for="nom"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Nom
            </label>

            <input
              id="nom"
              v-model="nom"
              type="text"
              required
              autocomplete="family-name"
              placeholder="Votre nom"
              class="w-full h-11
                     border border-gray-300
                     rounded-lg
                     px-3.5
                     text-sm
                     text-gray-800
                     placeholder:text-gray-400
                     outline-none
                     transition
                     focus:border-[#F5B942]
                     focus:ring-2
                     focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Prénom -->
          <div class="mb-4">
            <label
              for="prenom"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Prénom
            </label>

            <input
              id="prenom"
              v-model="prenom"
              type="text"
              required
              autocomplete="given-name"
              placeholder="Votre prénom"
              class="w-full h-11
                     border border-gray-300
                     rounded-lg
                     px-3.5
                     text-sm
                     text-gray-800
                     placeholder:text-gray-400
                     outline-none
                     transition
                     focus:border-[#F5B942]
                     focus:ring-2
                     focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Email -->
          <div class="mb-4">
            <label
              for="email"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Adresse e-mail
            </label>

            <input
              id="email"
              v-model="email"
              type="email"
              required
              autocomplete="email"
              placeholder="exemple@email.com"
              class="w-full h-11
                     border border-gray-300
                     rounded-lg
                     px-3.5
                     text-sm
                     text-gray-800
                     placeholder:text-gray-400
                     outline-none
                     transition
                     focus:border-[#F5B942]
                     focus:ring-2
                     focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Téléphone -->
          <div class="mb-4">
            <label
              for="telephone"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Téléphone
            </label>

            <input
              id="telephone"
              v-model="telephone"
              type="tel"
              autocomplete="tel"
              placeholder="Votre numéro de téléphone"
              class="w-full h-11
                     border border-gray-300
                     rounded-lg
                     px-3.5
                     text-sm
                     text-gray-800
                     placeholder:text-gray-400
                     outline-none
                     transition
                     focus:border-[#F5B942]
                     focus:ring-2
                     focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Mot de passe -->
          <div class="mb-4">
            <label
              for="password"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Mot de passe
            </label>

            <div class="relative">

              <input
                id="password"
                v-model="motDePasse"
                :type="afficherMotDePasse ? 'text' : 'password'"
                required
                minlength="6"
                autocomplete="new-password"
                placeholder="Votre mot de passe"
                class="w-full h-11
                       border border-gray-300
                       rounded-lg
                       px-3.5
                       pr-11
                       text-sm
                       text-gray-800
                       placeholder:text-gray-400
                       outline-none
                       transition
                       focus:border-[#F5B942]
                       focus:ring-2
                       focus:ring-[#F5B942]/20"
              />

              <button
                type="button"
                @click="afficherMotDePasse = !afficherMotDePasse"
                :aria-label="
                  afficherMotDePasse
                    ? 'Masquer le mot de passe'
                    : 'Afficher le mot de passe'
                "
                class="absolute
                       right-3
                       top-1/2
                       -translate-y-1/2
                       text-gray-400
                       hover:text-[#A87500]
                       transition"
              >
                <EyeOff
                  v-if="afficherMotDePasse"
                  class="w-5 h-5"
                />

                <Eye
                  v-else
                  class="w-5 h-5"
                />
              </button>

            </div>
          </div>

          <!-- Confirmation mot de passe -->
          <div class="mb-5">
            <label
              for="confirmPassword"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Confirmer le mot de passe
            </label>

            <div class="relative">

              <input
                id="confirmPassword"
                v-model="confirmationMotDePasse"
                :type="
                  afficherConfirmation
                    ? 'text'
                    : 'password'
                "
                required
                minlength="6"
                autocomplete="new-password"
                placeholder="Confirmer votre mot de passe"
                class="w-full h-11
                       border border-gray-300
                       rounded-lg
                       px-3.5
                       pr-11
                       text-sm
                       text-gray-800
                       placeholder:text-gray-400
                       outline-none
                       transition
                       focus:border-[#F5B942]
                       focus:ring-2
                       focus:ring-[#F5B942]/20"
              />

              <button
                type="button"
                @click="afficherConfirmation = !afficherConfirmation"
                :aria-label="
                  afficherConfirmation
                    ? 'Masquer la confirmation'
                    : 'Afficher la confirmation'
                "
                class="absolute
                       right-3
                       top-1/2
                       -translate-y-1/2
                       text-gray-400
                       hover:text-[#A87500]
                       transition"
              >
                <EyeOff
                  v-if="afficherConfirmation"
                  class="w-5 h-5"
                />

                <Eye
                  v-else
                  class="w-5 h-5"
                />
              </button>

            </div>
          </div>

          <!-- Erreur -->
          <div
            v-if="error"
            class="text-sm
                   text-red-700
                   bg-red-50
                   border border-red-200
                   rounded-lg
                   px-3.5
                   py-3
                   mb-5"
          >
            {{ error }}
          </div>

          <!-- Succès -->
          <div
            v-if="message"
            class="text-sm
                   text-[#7A5A00]
                   bg-[#FFF8E6]
                   border border-[#F5B942]/40
                   rounded-lg
                   px-3.5
                   py-3
                   mb-5"
          >
            {{ message }}
          </div>

          <!-- Submit -->
          <button
            type="submit"
            :disabled="loading"
            class="w-full h-11
                   bg-[#F5B942]
                   hover:bg-[#E9AA2F]
                   active:bg-[#D99A20]
                   text-[#171717]
                   font-semibold
                   rounded-lg
                   text-sm
                   transition
                   shadow-sm
                   disabled:opacity-60
                   disabled:cursor-not-allowed"
          >
            {{ loading ? 'Création du compte...' : 'Créer mon compte' }}
          </button>

        </form>

        <!-- Separator -->
        <div class="relative my-6">

          <div class="absolute inset-0 flex items-center">
            <div class="w-full border-t border-gray-200"></div>
          </div>

          <div class="relative flex justify-center">
            <span class="bg-white px-3 text-xs text-gray-400">
              ou
            </span>
          </div>

        </div>

        <!-- Login -->
        <p class="text-sm text-gray-500 text-center">

          Vous avez déjà un compte ?

          <RouterLink
            to="/login"
            class="ml-1
                   font-semibold
                   text-[#A87500]
                   hover:text-[#7F5900]
                   underline
                   underline-offset-2
                   transition"
          >
            Se connecter
          </RouterLink>

        </p>

      </div>

      <!-- Footer -->
      <p class="text-xs text-gray-400 text-center mt-6">
        © {{ new Date().getFullYear() }} NeoLedge MEA
      </p>

    </div>

  </div>
</template>


<script setup>

import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { Eye, EyeOff } from 'lucide-vue-next'
import { authApi } from '@/services/api'

import logo from '@/assets/logo.png'


const router = useRouter()


// Champs
const nom = ref('')
const prenom = ref('')
const email = ref('')
const telephone = ref('')

const motDePasse = ref('')
const confirmationMotDePasse = ref('')


// Affichage des mots de passe
const afficherMotDePasse = ref(false)
const afficherConfirmation = ref(false)


// État
const loading = ref(false)
const message = ref('')
const error = ref('')


async function onSubmit() {

  message.value = ''
  error.value = ''

  // Vérification mot de passe
  if (motDePasse.value !== confirmationMotDePasse.value) {
    error.value = 'Les mots de passe ne correspondent pas.'
    return
  }

  if (motDePasse.value.length < 6) {
    error.value = 'Le mot de passe doit contenir au moins 6 caractères.'
    return
  }

  loading.value = true

  try {

    const response = await authApi.register({
      nom: nom.value,
      prenom: prenom.value,
      email: email.value,
      telephone: telephone.value,
      motDePasse: motDePasse.value,
      confirmationMotDePasse: confirmationMotDePasse.value
    })

    message.value =
      response.message ||
      'Compte créé. Vérifiez votre adresse e-mail pour activer votre compte.'

    // Nettoyer les champs
    motDePasse.value = ''
    confirmationMotDePasse.value = ''

  } catch (e) {

    error.value =
      e?.message ||
      'Impossible de créer le compte.'

  } finally {

    loading.value = false

  }
}

</script>