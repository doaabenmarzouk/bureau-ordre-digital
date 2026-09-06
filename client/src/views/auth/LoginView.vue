<!-- client/src/views/auth/LoginView.vue -->

<template>
  <div class="min-h-screen bg-[#F7F7F5] flex items-center justify-center px-4">

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

        <!-- ================================================= -->
        <!-- ÉTAPE 1 : CONNEXION -->
        <!-- ================================================= -->

        <template v-if="!otpMode">

          <!-- Header -->
          <div class="mb-7">
            <h1
              class="text-xl font-semibold tracking-tight text-[#171717]"
            >
              Connexion
            </h1>

            <p class="text-sm text-gray-500 mt-1.5">
              Accédez à votre espace citoyen.
            </p>
          </div>


          <!-- Formulaire connexion -->
          <form @submit.prevent="onSubmit">

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
                v-model="email"
                type="email"
                required
                autocomplete="email"
                placeholder="Votre adresse e-mail"
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
            <div class="mb-2">

              <label
                for="password"
                class="block text-sm font-medium text-gray-700 mb-2"
              >
                Mot de passe
              </label>


              <div class="relative">

                <input
                  id="password"
                  v-model="password"
                  :type="
                    afficherMotDePasse
                      ? 'text'
                      : 'password'
                  "
                  required
                  autocomplete="current-password"
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


                <!-- Bouton afficher / masquer -->
                <button
                  type="button"
                  @click="
                    afficherMotDePasse =
                      !afficherMotDePasse
                  "
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


            <!-- Mot de passe oublié -->
            <div class="flex justify-end mb-5">

              <RouterLink
                to="/mot-de-passe-oublie"
                class="text-xs
                       font-medium
                       text-[#A87500]
                       hover:text-[#7F5900]
                       transition"
              >
                Mot de passe oublié ?
              </RouterLink>

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


            <!-- Bouton connexion -->
            <button
              type="submit"
              :disabled="loading"
              class="w-full
                     h-11
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
              {{ loading
                ? 'Connexion...'
                : 'Se connecter'
              }}
            </button>

          </form>


          <!-- Séparateur -->
          <div class="relative my-6">

            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-gray-200"></div>
            </div>

            <div class="relative flex justify-center">

              <span
                class="bg-white px-3 text-xs text-gray-400"
              >
                ou
              </span>

            </div>

          </div>


          <!-- Inscription -->
          <p class="text-sm text-gray-500 text-center">

            Pas encore de compte ?

            <RouterLink
              to="/register"
              class="ml-1
                     font-semibold
                     text-[#A87500]
                     hover:text-[#7F5900]
                     underline
                     underline-offset-2
                     transition"
            >
              Créer un compte
            </RouterLink>

          </p>

        </template>


        <!-- ================================================= -->
        <!-- ÉTAPE 2 : VÉRIFICATION OTP -->
        <!-- ================================================= -->

        <template v-else>

          <!-- Header OTP -->
          <div class="mb-7">

            <!-- Icône sécurité -->
            <div
              class="mx-auto mb-5
                     flex h-12 w-12
                     items-center justify-center
                     rounded-full
                     bg-[#F5B942]/15"
            >

              <svg
                class="w-6 h-6 text-[#A87500]"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v2h8z"
                />
              </svg>

            </div>


            <h1
              class="text-xl
                     font-semibold
                     tracking-tight
                     text-[#171717]
                     text-center"
            >
              Vérification de connexion
            </h1>


            <p
              class="text-sm
                     text-gray-500
                     mt-2
                     text-center
                     leading-relaxed"
            >
              Un code de vérification à 6 chiffres
              a été envoyé à

              <span class="font-medium text-gray-700">
                {{ email }}
              </span>.
            </p>

          </div>


          <!-- Formulaire OTP -->
          <form @submit.prevent="onVerifyOtp">

            <!-- Code OTP -->
            <div class="mb-5">

              <label
                for="otp"
                class="block
                       text-sm
                       font-medium
                       text-gray-700
                       mb-2"
              >
                Code de vérification
              </label>


              <input
                id="otp"
                v-model="otpCode"
                type="text"
                inputmode="numeric"
                autocomplete="one-time-code"
                maxlength="6"
                pattern="[0-9]{6}"
                required
                placeholder="000000"
                @input="
                  otpCode =
                    otpCode.replace(/\D/g, '')
                "
                class="w-full
                       h-12
                       border
                       border-gray-300
                       rounded-lg
                       px-3.5
                       text-center
                       text-lg
                       tracking-[0.5em]
                       font-semibold
                       text-gray-800
                       placeholder:text-gray-300
                       outline-none
                       transition
                       focus:border-[#F5B942]
                       focus:ring-2
                       focus:ring-[#F5B942]/20"
              />

            </div>


            <!-- Erreur OTP -->
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


            <!-- Vérifier -->
            <button
              type="submit"
              :disabled="
                loading ||
                otpCode.length !== 6
              "
              class="w-full
                     h-11
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
              {{ loading
                ? 'Vérification...'
                : 'Vérifier le code'
              }}
            </button>

          </form>


          <!-- Retour connexion -->
          <div class="mt-5 text-center">

            <button
              type="button"
              @click="retourConnexion"
              :disabled="loading"
              class="text-sm
                     font-medium
                     text-[#A87500]
                     hover:text-[#7F5900]
                     transition
                     disabled:opacity-50"
            >
              ← Retour à la connexion
            </button>

          </div>

        </template>

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

import {
  useRouter,
  useRoute
} from 'vue-router'

import { useAuthStore } from '@/stores/auth'

import {
  Eye,
  EyeOff
} from 'lucide-vue-next'

import logo from '@/assets/logo.png'


// ==========================================================
// ROUTER / STORE
// ==========================================================

const router = useRouter()
const route = useRoute()
const auth = useAuthStore()


// ==========================================================
// DONNÉES CONNEXION
// ==========================================================

const email = ref('')
const password = ref('')


// ==========================================================
// DONNÉES OTP
// ==========================================================

const otpCode = ref('')
const otpMode = ref(false)


// ==========================================================
// ÉTAT INTERFACE
// ==========================================================

const error = ref('')
const loading = ref(false)

const afficherMotDePasse =
  ref(false)


// ==========================================================
// CONNEXION
// ==========================================================

async function onSubmit() {

  error.value = ''
  loading.value = true

  try {

    const response =
      await auth.login({

        email:
          email.value,

        motDePasse:
          password.value

      })


    // ======================================================
    // LE BACKEND DEMANDE LE OTP
    // ======================================================

    if (response.requiresOtp) {

      otpMode.value = true

      otpCode.value = ''

      return
    }


    // ======================================================
    // CAS TOKEN DIRECT
    // ======================================================

    if (response.token) {

      router.push(
        route.query.redirect ||
        { name: 'landing' }
      )

    }

  } catch (e) {

    error.value =
      e.message ||
      'Connexion impossible.'

  } finally {

    loading.value = false

  }

}


// ==========================================================
// VÉRIFICATION OTP
// ==========================================================

async function onVerifyOtp() {

  error.value = ''
  loading.value = true

  try {

    await auth.verifyOtp(
      otpCode.value
    )


    // ======================================================
    // OTP VALIDÉ → CONNEXION TERMINÉE
    // ======================================================

    router.push(
      route.query.redirect ||
      { name: 'landing' }
    )

  } catch (e) {

    error.value =
      e.message ||
      'Code de vérification incorrect.'

    otpCode.value = ''

  } finally {

    loading.value = false

  }

}


// ==========================================================
// RETOUR À LA CONNEXION
// ==========================================================

function retourConnexion() {

  auth.cancelLogin()

  otpMode.value = false

  otpCode.value = ''

  error.value = ''

}

</script>