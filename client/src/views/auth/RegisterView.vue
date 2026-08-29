<!-- client/src/views/auth/RegisterView.vue -->

<template>
  <div class="min-h-screen bg-[#F7F7F5] flex items-center justify-center px-4 py-10">

    <div class="w-full max-w-md">

      <!-- Logo -->
      <div class="flex justify-center mb-8">
        <img
          :src="logo"
          alt="NeoLedge MEA"
          class="h-12 w-auto object-contain"
        />
      </div>

      <!-- Formulaire d'inscription -->
      <div
        v-if="!submitted"
        class="bg-white border border-gray-200 rounded-2xl p-7
               shadow-[0_8px_30px_rgba(0,0,0,0.04)]"
      >

        <!-- Header -->
        <div class="mb-7">
          <h1 class="text-xl font-semibold tracking-tight text-[#171717]">
            Créer mon compte
          </h1>

          <p class="text-sm text-gray-500 mt-1.5">
            Quelques informations pour démarrer.
          </p>
        </div>

        <!-- Form -->
        <form @submit.prevent="onSubmit">

          <!-- Prénom / Nom -->
          <div class="grid grid-cols-2 gap-3 mb-5">

            <div>
              <label
                for="prenom"
                class="block text-sm font-medium text-gray-700 mb-2"
              >
                Prénom
              </label>

              <input
                id="prenom"
                v-model="form.prenom"
                type="text"
                required
                autocomplete="given-name"
                placeholder="Prénom"
                class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                       text-sm text-gray-800
                       placeholder:text-gray-400
                       outline-none transition
                       focus:border-[#F5B942]
                       focus:ring-2 focus:ring-[#F5B942]/20"
              />
            </div>

            <div>
              <label
                for="nom"
                class="block text-sm font-medium text-gray-700 mb-2"
              >
                Nom
              </label>

              <input
                id="nom"
                v-model="form.nom"
                type="text"
                required
                autocomplete="family-name"
                placeholder="Nom"
                class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                       text-sm text-gray-800
                       placeholder:text-gray-400
                       outline-none transition
                       focus:border-[#F5B942]
                       focus:ring-2 focus:ring-[#F5B942]/20"
              />
            </div>

          </div>

          <!-- Email -->
          <div class="mb-5">
            <label
              for="email"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Adresse e-mail
            </label>

            <input
              id="email"
              v-model="form.email"
              type="email"
              required
              autocomplete="email"
              placeholder="exemple@email.com"
              class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                     text-sm text-gray-800
                     placeholder:text-gray-400
                     outline-none transition
                     focus:border-[#F5B942]
                     focus:ring-2 focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Téléphone -->
          <div class="mb-5">
            <label
              for="telephone"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Téléphone
            </label>

            <input
              id="telephone"
              v-model="form.telephone"
              type="tel"
              required
              autocomplete="tel"
              placeholder="+216 XX XXX XXX"
              class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                     text-sm text-gray-800
                     placeholder:text-gray-400
                     outline-none transition
                     focus:border-[#F5B942]
                     focus:ring-2 focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Mot de passe -->
          <div class="mb-5">
            <label
              for="password"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Mot de passe
            </label>

            <input
              id="password"
              v-model="form.motDePasse"
              type="password"
              required
              minlength="8"
              autocomplete="new-password"
              placeholder="Votre mot de passe"
              class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                     text-sm text-gray-800
                     placeholder:text-gray-400
                     outline-none transition
                     focus:border-[#F5B942]
                     focus:ring-2 focus:ring-[#F5B942]/20"
            />

            <p class="text-xs text-gray-400 mt-1.5">
              Au moins 8 caractères.
            </p>
          </div>

          <!-- Confirmation -->
          <div class="mb-5">
            <label
              for="confirmation"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Confirmer le mot de passe
            </label>

            <input
              id="confirmation"
              v-model="form.confirmation"
              type="password"
              required
              autocomplete="new-password"
              placeholder="Confirmez votre mot de passe"
              class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                     text-sm text-gray-800
                     placeholder:text-gray-400
                     outline-none transition
                     focus:border-[#F5B942]
                     focus:ring-2 focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Conditions -->
          <label
            class="flex items-start gap-2.5 mb-5 text-xs text-gray-600 cursor-pointer"
          >
            <input
              v-model="form.accepteConditions"
              type="checkbox"
              required
              class="mt-0.5 w-4 h-4 accent-[#F5B942]"
            />

            <span class="leading-5">
              J'accepte les
              <span class="font-medium text-[#A87500] underline">
                conditions d'utilisation
              </span>
              et la politique de confidentialité.
            </span>
          </label>

          <!-- Error -->
          <div
            v-if="error"
            class="text-sm text-red-700 bg-red-50
                   border border-red-200 rounded-lg
                   px-3.5 py-3 mb-5"
          >
            {{ error }}
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
            {{ loading ? 'Création en cours...' : 'Créer mon compte' }}
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
          Déjà un compte ?

          <RouterLink
            to="/login"
            class="ml-1 font-semibold text-[#A87500]
                   hover:text-[#7F5900]
                   underline underline-offset-2
                   transition"
          >
            Se connecter
          </RouterLink>
        </p>

      </div>

      <!-- Confirmation -->
      <div
        v-else
        class="bg-white border border-gray-200 rounded-2xl p-7
               text-center
               shadow-[0_8px_30px_rgba(0,0,0,0.04)]"
      >

        <!-- Icon -->
        <div
          class="w-14 h-14 rounded-full
                 bg-[#FFF4D6]
                 flex items-center justify-center
                 text-2xl mx-auto mb-5"
        >
          ✉️
        </div>

        <h2 class="text-xl font-semibold tracking-tight text-[#171717] mb-2">
          Vérifiez votre boîte mail
        </h2>

        <p class="text-sm text-gray-500 leading-6 mb-6">
          Un lien de confirmation a été envoyé à

          <span class="font-medium text-[#171717]">
            {{ form.email }}
          </span>.

          Cliquez dessus pour activer votre compte.
        </p>

        <RouterLink
          to="/login"
          class="inline-flex items-center justify-center
                 w-full h-11
                 bg-[#F5B942]
                 hover:bg-[#E9AA2F]
                 text-[#171717]
                 font-semibold
                 rounded-lg
                 text-sm
                 transition
                 shadow-sm"
        >
          Retour à la connexion
        </RouterLink>

      </div>

      <!-- Footer -->
      <p class="text-xs text-gray-400 text-center mt-6">
        © {{ new Date().getFullYear() }} NeoLedge MEA
      </p>

    </div>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useAuthStore } from '@/stores/auth'

import logo from '@/assets/logo.png'

const auth = useAuthStore()

const form = reactive({
  prenom: '',
  nom: '',
  email: '',
  telephone: '',
  motDePasse: '',
  confirmation: '',
  accepteConditions: false
})

const error = ref('')
const loading = ref(false)
const submitted = ref(false)

function isValidEmail(email) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)
}

async function onSubmit() {
  error.value = ''

  if (!isValidEmail(form.email)) {
    error.value = 'Merci de saisir une adresse e-mail valide.'
    return
  }

  if (form.motDePasse.length < 8) {
    error.value = 'Le mot de passe doit contenir au moins 8 caractères.'
    return
  }

  if (form.motDePasse !== form.confirmation) {
    error.value = 'Les mots de passe ne correspondent pas.'
    return
  }

  if (!form.accepteConditions) {
    error.value = "Vous devez accepter les conditions d'utilisation."
    return
  }

  loading.value = true

  try {
    await auth.register({
      prenom: form.prenom,
      nom: form.nom,
      email: form.email,
      telephone: form.telephone,
      motDePasse: form.motDePasse
    })

    submitted.value = true

  } catch (e) {
    error.value =
      e.message || 'Impossible de créer le compte. Réessayez.'
  } finally {
    loading.value = false
  }
}
</script>