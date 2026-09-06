// client/src/stores/auth.js

import { defineStore } from 'pinia'
import { authApi } from '@/services/api'


export const useAuthStore = defineStore('auth', {

  // ==========================================================
  // STATE
  // ==========================================================

  state: () => ({

    token:
      localStorage.getItem('token') || null,

    user: JSON.parse(
      localStorage.getItem('user') || 'null'
    ),

    // Challenge temporaire de connexion OTP
    loginChallenge:
      sessionStorage.getItem('loginChallenge') || null

  }),


  // ==========================================================
  // GETTERS
  // ==========================================================

  getters: {

    isAuthenticated: (state) =>
      !!state.token,

    otpRequired: (state) =>
      !!state.loginChallenge

  },


  // ==========================================================
  // ACTIONS
  // ==========================================================

  actions: {

    // ========================================================
    // REGISTER
    // ========================================================

    async register(data) {

      const response =
        await authApi.register(data)

      return response
    },


    // ========================================================
    // LOGIN - PREMIÈRE ÉTAPE
    // ========================================================
    //
    // Email + mot de passe
    //
    // Le backend vérifie :
    // - email
    // - mot de passe
    // - email vérifié
    //
    // Puis il envoie un OTP par email.
    //
    // IMPORTANT :
    // Aucun JWT n'est encore stocké.
    //
    // ========================================================

    async login(data) {

      const response =
        await authApi.login(data)


      // Le backend demande une vérification OTP
      if (response.requiresOtp) {

        // Sauvegarder uniquement le challenge temporaire
        this.loginChallenge =
          response.challenge

        sessionStorage.setItem(
          'loginChallenge',
          response.challenge
        )

        return response
      }


      // Sécurité :
      // normalement le login doit maintenant
      // toujours passer par OTP.
      //
      // Mais si le backend retourne directement
      // un token, on peut quand même gérer le cas.

      if (response.token) {

        this.setAuth(response)

      }


      return response
    },


    // ========================================================
    // VERIFY OTP - DEUXIÈME ÉTAPE
    // ========================================================
    //
    // Envoie :
    //
    // {
    //   challenge,
    //   code
    // }
    //
    // Le backend vérifie le code OTP.
    //
    // Si correct :
    // → génération du JWT
    // → connexion définitive
    //
    // ========================================================

    async verifyOtp(code) {

      if (!this.loginChallenge) {

        throw new Error(
          'Session de connexion expirée. Veuillez recommencer.'
        )

      }


      const response =
        await authApi.verifyOtp({

          challenge:
            this.loginChallenge,

          code: code

        })


      // Le backend doit retourner le JWT
      if (!response.token) {

        throw new Error(
          'Authentification impossible : token manquant.'
        )

      }


      // Connexion définitive
      this.setAuth(response)


      // Le challenge ne sert plus
      this.loginChallenge = null

      sessionStorage.removeItem(
        'loginChallenge'
      )


      return response
    },


    // ========================================================
    // SET AUTH
    // ========================================================

    setAuth(response) {

      this.token =
        response.token


      this.user = {

        id:
          response.id,

        nom:
          response.nom,

        prenom:
          response.prenom,

        email:
          response.email,

        telephone:
          response.telephone || null

      }


      localStorage.setItem(
        'token',
        this.token
      )


      localStorage.setItem(
        'user',
        JSON.stringify(this.user)
      )
    },


    // ========================================================
    // ANNULER LA CONNEXION OTP
    // ========================================================

    cancelLogin() {

      this.loginChallenge = null

      sessionStorage.removeItem(
        'loginChallenge'
      )
    },


    // ========================================================
    // LOGOUT
    // ========================================================

    logout() {

      this.token = null

      this.user = null

      this.loginChallenge = null


      localStorage.removeItem(
        'token'
      )

      localStorage.removeItem(
        'user'
      )

      sessionStorage.removeItem(
        'loginChallenge'
      )
    }

  }

})