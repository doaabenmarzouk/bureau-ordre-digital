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

        <!-- Header -->
        <div class="mb-7">
          <h1 class="text-xl font-semibold tracking-tight text-[#171717]">
            Connexion
          </h1>

          <p class="text-sm text-gray-500 mt-1.5">
            Accédez à votre espace citoyen.
          </p>
        </div>

        <!-- Form -->
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
              placeholder="exemple@email.com"
              class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                     text-sm text-gray-800
                     placeholder:text-gray-400
                     outline-none
                     transition
                     focus:border-[#F5B942]
                     focus:ring-2 focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Password -->
          <div class="mb-2">
            <label
              for="password"
              class="block text-sm font-medium text-gray-700 mb-2"
            >
              Mot de passe
            </label>

            <input
              id="password"
              v-model="password"
              type="password"
              required
              autocomplete="current-password"
              placeholder="Votre mot de passe"
              class="w-full h-11 border border-gray-300 rounded-lg px-3.5
                     text-sm text-gray-800
                     placeholder:text-gray-400
                     outline-none
                     transition
                     focus:border-[#F5B942]
                     focus:ring-2 focus:ring-[#F5B942]/20"
            />
          </div>

          <!-- Forgot password -->
          <div class="flex justify-end mb-5">
            <RouterLink
              to="/mot-de-passe-oublie"
              class="text-xs font-medium text-[#A87500]
                     hover:text-[#7F5900] transition"
            >
              Mot de passe oublié ?
            </RouterLink>
          </div>

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
            {{ loading ? 'Connexion...' : 'Se connecter' }}
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

        <!-- Register -->
        <p class="text-sm text-gray-500 text-center">
          Pas encore de compte ?

          <RouterLink
            to="/register"
            class="ml-1 font-semibold text-[#A87500]
                   hover:text-[#7F5900]
                   underline underline-offset-2
                   transition"
          >
            Créer un compte
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
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

// Logo
import logo from '@/assets/logo.png'

const router = useRouter()
const route = useRoute()
const auth = useAuthStore()

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

async function onSubmit() {
  error.value = ''
  loading.value = true

  try {
    await auth.login({
     email: email.value,
     motDePasse: password.value
})

    router.push(
      route.query.redirect || { name: 'landing' }
    )
  } catch (e) {
    error.value = e.message || 'Connexion impossible.'
  } finally {
    loading.value = false
  }
}
</script>