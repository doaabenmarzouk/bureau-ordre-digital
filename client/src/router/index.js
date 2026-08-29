// client/src/router/index.js

import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  // =========================
  // ACCUEIL
  // =========================
  {
    path: '/',
    name: 'landing',
    component: () => import('@/views/LandingView.vue')
  },

  // =========================
  // AUTHENTIFICATION
  // =========================
  {
    path: '/login',
    name: 'login',
    component: () => import('@/views/auth/LoginView.vue')
  },

  {
    path: '/register',
    name: 'register',
    component: () => import('@/views/auth/RegisterView.vue')
  },

  // =========================
  // COURRIER
  // =========================
  {
    path: '/courrier',
    name: 'courrier-list',
    component: () => import('@/views/courrier/CourrierListView.vue'),
    meta: {
      requiresAuth: true
    }
  },

  {
    path: '/courrier/nouvelle-demande',
    name: 'NouvelleDemande',
    component: () => import('@/views/Courrier/NouvelleDemande.vue'),
    meta: {
      requiresAuth: true
    }
  },

  // Ancienne URL utilisée par certains boutons
  // On la redirige vers la nouvelle route
  {
    path: '/courrier/soumettre',
    redirect: {
      name: 'NouvelleDemande'
    }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// =========================
// NAVIGATION GUARD
// =========================

router.beforeEach((to) => {
  const auth = useAuthStore()

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return {
      name: 'login',
      query: {
        redirect: to.fullPath
      }
    }
  }

  return true
})

// =========================
// GESTION DES ERREURS
// =========================

router.onError((error) => {
  console.error('Erreur de navigation Vue Router :', error)
})

export default router