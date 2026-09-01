// client/src/services/api.js

import { useAuthStore } from '@/stores/auth'

const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5029/api'

async function request(path, options = {}) {
  const auth = useAuthStore()

  const headers = {
    Accept: 'application/json',

    ...(options.body && !(options.body instanceof FormData)
      ? { 'Content-Type': 'application/json' }
      : {}),

    ...(auth?.token
      ? { Authorization: `Bearer ${auth.token}` }
      : {}),

    ...(options.headers || {})
  }

  console.log('➡️ API:', `${BASE_URL}${path}`)
  console.log('auth')

  const response = await fetch(`${BASE_URL}${path}`, {
    ...options,
    headers
  })

  console.log('⬅️ API status:', response.status)

  if (!response.ok) {
    let message = `Erreur serveur (${response.status})`

    try {
      const errorBody = await response.json()

      console.error('❌ API error:', errorBody)

      message =
        errorBody?.message ||
        errorBody?.title ||
        errorBody?.detail ||
        message
    } catch {
      // réponse non JSON
    }

    throw new Error(message)
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}


export const authApi = {

  register(payload) {
    return request('/Auth/register', {
      method: 'POST',
      body: JSON.stringify(payload)
    })
  },

  login(payload) {
    return request('/Auth/login', {
      method: 'POST',
      body: JSON.stringify(payload)
    })
  },


  // GET /api/Auth/verify-email?token=
  verifyEmail(token) {
    return request(
      `/Auth/verify-email?token=${encodeURIComponent(token)}`
    )
  },
  forgotPassword(payload) {
  return request('/Auth/forgot-password', {
    method: 'POST',
    body: JSON.stringify(payload)
  })
},

resetPassword(payload) {
  return request('/Auth/reset-password', {
    method: 'POST',
    body: JSON.stringify(payload)
  })
}
}


export const organismesApi = {

 
  async search(filters = {}) {

    const params = new URLSearchParams()

    if (filters.ville) {
      params.set('ville', filters.ville)
    }

    if (filters.type) {
      params.set('type', filters.type)
    }

    if (filters.motCle) {
      params.set('motCle', filters.motCle)
    }

    const queryString = params.toString()

    const path = queryString
      ? `/ServiceDestinataire?${queryString}`
      : '/ServiceDestinataire'

    console.log(' Chargement destinataires:', path)

    const response = await request(path)


    // Le backend retourne directement un tableau
    if (Array.isArray(response)) {
      return response
    }

    // Sécurité si ton backend retourne un objet
    if (Array.isArray(response?.data)) {
      return response.data
    }

    if (Array.isArray(response?.items)) {
      return response.items
    }

    if (Array.isArray(response?.result)) {
      return response.result
    }

    console.error(
      '❌ Format destinataires invalide:',
      response
    )

    return []
  },


  // Liste complète
  list() {
    return organismesApi.search()
  },


  // Liste des villes
  async listVilles() {

    const services = await organismesApi.list()

    return [
      ...new Set(
        services
          .map(service => service.ville)
          .filter(Boolean)
      )
    ].sort()
  },


  // Liste des types d'organismes
  async listTypes() {

    const services = await organismesApi.list()

    return [
      ...new Set(
        services
          .map(service => service.typeOrganisme)
          .filter(Boolean)
      )
    ].sort()
  }
}


export const typeCourrierApi = {

  /**
   * GET /api/TypeCourrier
   */

  async list() {

    console.log('➡️ Appel GET /TypeCourrier')

    const response = await request('/TypeCourrier')

    console.log(
      '📦 Types courrier reçus:',
      response
    )

    if (Array.isArray(response)) {
      return response
    }

    if (Array.isArray(response?.data)) {
      return response.data
    }

    if (Array.isArray(response?.items)) {
      return response.items
    }

    if (Array.isArray(response?.result)) {
      return response.result
    }

    console.error(
      '❌ Format TypeCourrier invalide:',
      response
    )

    return []
  }
}


export const courrierApi = {

  /**
   * POST /api/Courrier
   *
   * multipart/form-data
   */

  async submit(payload) {

    const auth = useAuthStore()

    const formData = new FormData()

    formData.append(
      'TypeCourrierId',
      String(payload.typeCourrierId)
    )

    formData.append(
      'ServiceDestinataireId',
      String(payload.serviceDestinataireId)
    )

    formData.append(
      'Objet',
      payload.objet
    )

    formData.append(
      'Description',
      payload.description
    )

    if (payload.fichiers?.length) {

      payload.fichiers.forEach(file => {

        formData.append(
          'fichiers',
          file
        )

      })
    }

    console.log(
      '➡️ Envoi courrier vers:',
      `${BASE_URL}/Courrier`
    )

    const response = await fetch(
      `${BASE_URL}/Courrier`,
      {
        method: 'POST',

        headers: {
          ...(auth?.token
            ? {
                Authorization:
                  `Bearer ${auth.token}`
              }
            : {})
        },

        body: formData
      }
    )

    console.log(
      '⬅️ Réponse courrier:',
      response.status
    )

    if (!response.ok) {

      let message =
        `Erreur lors de l'envoi (${response.status})`

      try {

        const errorBody =
          await response.json()

        console.error(
          '❌ Erreur courrier:',
          errorBody
        )

        message =
          errorBody?.message ||
          errorBody?.title ||
          errorBody?.detail ||
          message

      } catch {
        // réponse non JSON
      }

      throw new Error(message)
    }

    return response.json()
  },


  // GET /api/Suivi
  list() {
    return request('/Suivi')
  },


  // GET /api/Suivi/{id}
  get(id) {
    return request(`/Suivi/${id}`)
  },


  // GET /api/Suivi/rechercher?motCle=
  search(motCle) {

    return request(
      `/Suivi/rechercher?motCle=${encodeURIComponent(motCle || '')}`
    )
  }
}