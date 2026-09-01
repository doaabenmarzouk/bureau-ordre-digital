// client/src/router/index.js

import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes = [
  // =========================
  // PUBLIC
  // =========================

  {
    path: '/',
    name: 'landing',
    component: () => import('@/views/LandingView.vue')
  },

  {
    path: '/a-propos',
    name: 'a-propos',
    component: () => import('@/views/AboutView.vue')
  },

  {
    path: '/contact',
    name: 'contact',
    component: () => import('@/views/ContactView.vue')
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

  {
    path: '/mot-de-passe-oublie',
    name: 'mot-de-passe-oublie',
    component: () => import('@/views/auth/ForgotPasswordView.vue')
  },

  {
    path: '/reset-password',
    name: 'reset-password',
    component: () => import('@/views/auth/ResetPasswordView.vue')
  },

  // =========================
  // COURRIER - PROTÉGÉ
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
    component: () => import('@/views/courrier/NouvelleDemande.vue'),

    meta: {
      requiresAuth: true
    }
  },

  {
    path: '/courrier/soumettre',
    redirect: {
      name: 'NouvelleDemande'
    },

    meta: {
      requiresAuth: true
    }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// =========================
// AUTHENTIFICATION GUARD
// =========================

router.beforeEach((to) => {
  const auth = useAuthStore();

  console.log("➡️ Navigation vers :", to.fullPath);
  console.log("🔐 Token :", auth.token);
  console.log("🔐 Authentifié :", auth.isAuthenticated);
  console.log("🛡️ requiresAuth :", to.meta.requiresAuth);

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    console.log("❌ Accès refusé → redirection Login");

    return {
      name: "login",
      query: {
        redirect: to.fullPath,
      },
    };
  }

  console.log("✅ Accès autorisé");

  return true;
});

// =========================
// ERREURS
// =========================

router.onError((error) => {
  console.error('Erreur de navigation Vue Router :', error)
})

export default router