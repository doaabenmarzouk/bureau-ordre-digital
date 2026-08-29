// client/src/services/api.js
//
// Branché directement sur ton backend .NET :
//   POST   /api/Auth/register                                -> { message }
//   POST   /api/Auth/login                                    -> { id, nom, prenom, email, token }
//   GET    /api/Auth/verify-email?token=                      -> { message }
//   GET    /api/ServiceDestinataire?ville=&type=&motCle=      -> ServiceDestinataireDto[]
//   GET    /api/TypeCourrier                                  -> TypeCourrierDto[]
//   POST   /api/Courrier            (multipart)                -> CourrierCreateDto + fichiers
//   GET    /api/Suivi                                          -> CourrierListItemDto[]
//   GET    /api/Suivi/{id}                                     -> détail courrier
//   GET    /api/Suivi/rechercher?motCle=                       -> CourrierListItemDto[]

import { useAuthStore } from '@/stores/auth'

const BASE_URL = import.meta.env.VITE_API_URL || '/api'

// Active les données mockées tant que le backend n'est pas branché.
// Mets ça à false (ou supprime cette ligne) quand tu veux repasser sur le vrai back-end.
const USE_MOCK = false

async function request(path, options = {}) {
  const auth = useAuthStore()

  const headers = {
    Accept: 'application/json',
    ...(options.body && !(options.body instanceof FormData) ? { 'Content-Type': 'application/json' } : {}),
    ...(auth?.token ? { Authorization: `Bearer ${auth.token}` } : {}),
    ...options.headers
  }

  const res = await fetch(`${BASE_URL}${path}`, { ...options, headers })

  if (!res.ok) {
    let message = `Erreur serveur (${res.status})`
    try {
      const errBody = await res.json()
      message = errBody.message || message
    } catch {
      /* corps non-JSON */
    }
    throw new Error(message)
  }

  if (res.status === 204) return null
  return res.json()
}

// Simule un petit délai réseau pour que les mocks se comportent comme un vrai appel API
function mockDelay(data, ms = 300) {
  return new Promise((resolve) => setTimeout(() => resolve(data), ms))
}

// ---------------------------------------------------------------------
// Authentification
// ---------------------------------------------------------------------
export const authApi = {
  /**
   * POST /api/Auth/register
   * payload: { nom, prenom, email, telephone, motDePasse }
   * Retourne : { message }
   */
  register(payload) {
    if (USE_MOCK) {
      console.log('[MOCK] Inscription :', payload)
      return mockDelay({ message: "Compte créé. Vérifiez votre email pour l'activer." }, 400)
    }
    return request('/Auth/register', {
      method: 'POST',
      body: JSON.stringify(payload)
    })
  },

  /**
   * POST /api/Auth/login
   * payload: { email, motDePasse }
   * Retourne : { id, nom, prenom, email, token }
   */
  login(payload) {
    if (USE_MOCK) {
      console.log('[MOCK] Connexion :', payload)
      return mockDelay({
        id: 'mock-user-1',
        nom: 'Trabelsi',
        prenom: 'Fatma',
        email: payload.email,
        token: 'mock-jwt-token'
      }, 400)
    }
    return request('/Auth/login', {
      method: 'POST',
      body: JSON.stringify(payload)
    })
  },

  /**
   * GET /api/Auth/verify-email?token=...
   * Retourne : { message }
   */
  verifyEmail(token) {
    if (USE_MOCK) {
      return mockDelay({ message: 'Email vérifié avec succès. Vous pouvez maintenant vous connecter.' }, 400)
    }
    return request(`/Auth/verify-email?token=${encodeURIComponent(token)}`)
  }
}

// ---------------------------------------------------------------------
// Données mockées — Services destinataires
// Structure alignée sur ServiceDestinataireDto attendu du backend
// ---------------------------------------------------------------------
const MOCK_SERVICES_DESTINATAIRES = [
  { id: 1, nomService: 'Service État Civil',    nomOrganisme: 'Commune de Bizerte',        ville: 'Bizerte', typeOrganisme: 'Collectivités territoriales' },
  { id: 2, nomService: 'Service Urbanisme',     nomOrganisme: 'Commune de Bizerte',        ville: 'Bizerte', typeOrganisme: 'Collectivités territoriales' },
  { id: 3, nomService: 'Service Réclamations',  nomOrganisme: 'Commune de Tunis',          ville: 'Tunis',   typeOrganisme: 'Collectivités territoriales' },
  { id: 4, nomService: 'Bureau des Passeports', nomOrganisme: "Ministère de l'Intérieur",  ville: 'Tunis',   typeOrganisme: 'Ministères et hauts commissariats' },
  { id: 5, nomService: 'Service Urbanisme',     nomOrganisme: 'Commune de Sfax',           ville: 'Sfax',    typeOrganisme: 'Collectivités territoriales' },
  { id: 6, nomService: 'Service État Civil',    nomOrganisme: 'Commune de Sousse',         ville: 'Sousse',  typeOrganisme: 'Collectivités territoriales' },
]

// ---------------------------------------------------------------------
// Services destinataires (étape "Organisme")
// Pas d'endpoint dédié pour lister villes/types séparément côté backend :
// on charge tout une première fois et on déduit les filtres côté client.
// ---------------------------------------------------------------------
export const organismesApi = {
  /** GET /api/ServiceDestinataire?ville=&type=&motCle= */
  async search(filters = {}) {
    if (USE_MOCK) {
      let results = [...MOCK_SERVICES_DESTINATAIRES]
      if (filters.ville) {
        results = results.filter(s => s.ville.toLowerCase().includes(filters.ville.toLowerCase()))
      }
      if (filters.type) {
        results = results.filter(s => s.typeOrganisme.toLowerCase().includes(filters.type.toLowerCase()))
      }
      if (filters.motCle) {
        const q = filters.motCle.toLowerCase()
        results = results.filter(s =>
          s.nomService.toLowerCase().includes(q) ||
          s.nomOrganisme.toLowerCase().includes(q)
        )
      }
      return mockDelay(results)
    }

    const params = new URLSearchParams()
    if (filters.ville) params.set('ville', filters.ville)
    if (filters.type) params.set('type', filters.type)
    if (filters.motCle) params.set('motCle', filters.motCle)
    const qs = params.toString()
    return request(`/ServiceDestinataire${qs ? `?${qs}` : ''}`)
  },

  /** Alias de search() — même filtres, même endpoint. Certains composants appellent .list() plutôt que .search(). */
  list(filters = {}) {
    return organismesApi.search(filters)
  },

  /**
   * Liste des villes disponibles, déduite des services.
   * Pas d'endpoint dédié côté backend : on charge tout et on déduit côté client.
   */
  async listVilles() {
    const results = await organismesApi.search()
    return [...new Set(results.map(s => s.ville))].sort()
  },

  /**
   * Liste des types d'organisme disponibles, déduite des services.
   * Pas d'endpoint dédié côté backend : on charge tout et on déduit côté client.
   */
  async listTypes() {
    const results = await organismesApi.search()
    return [...new Set(results.map(s => s.typeOrganisme))].sort()
  }
}

// ---------------------------------------------------------------------
// Types de courrier (Réclamation, Demande d'information, etc.)
// ---------------------------------------------------------------------
const MOCK_TYPES_COURRIER = [
  { id: 1, libelle: "Demande d'information" },
  { id: 2, libelle: 'Réclamation' },
  { id: 3, libelle: 'Requête' },
  { id: 4, libelle: 'Suggestion' },
  { id: 5, libelle: 'Recours' },
]

export const typeCourrierApi = {

  async list() {

    console.log('➡️ Appel GET /TypeCourrier')

    const response = await request('/TypeCourrier')

    console.log('📦 Réponse TypeCourrier :', response)

    // Cas normal :
    // [
    //   { id: 1, libelle: "Réclamation" },
    //   ...
    // ]
    if (Array.isArray(response)) {
      return response
    }

    // Cas où le backend retourne :
    // { data: [...] }
    if (Array.isArray(response?.data)) {
      return response.data
    }

    // Cas où le backend retourne :
    // { items: [...] }
    if (Array.isArray(response?.items)) {
      return response.items
    }

    // Cas où le backend retourne :
    // { result: [...] }
    if (Array.isArray(response?.result)) {
      return response.result
    }

    console.error(
      '❌ Format TypeCourrier inattendu :',
      response
    )

    return []
  }

}

// ---------------------------------------------------------------------
// Données mockées — Courriers déjà soumis (pour "Mes courriers")
// Structure alignée sur CourrierListItemDto / détail attendu du backend
// ---------------------------------------------------------------------
const MOCK_COURRIERS = [
  {
    id: 'c1',
    numReference: 'BOD-2026-4F2A9C1D',
    objet: "Demande de duplicata d'acte de naissance",
    description: "Bonjour, je souhaite obtenir un duplicata de mon acte de naissance suite à sa perte. Merci de bien vouloir me le faire parvenir dans les meilleurs délais.",
    statut: 'Transmis au service',
    dateSoumission: '2026-08-21T09:14:00',
    typeCourrier: "Demande d'information",
    serviceDestinataire: 'Commune de Bizerte - Service État Civil',
    historique: [
      { statut: 'Reçu', dateAction: '2026-08-21T09:14:00', description: 'Courrier soumis' },
      { statut: 'Transmis au service', dateAction: '2026-08-21T14:02:00', description: 'Transmis au service concerné' }
    ],
    piecesJointes: [
      { nomFichier: 'carte_identite.pdf', urlStockage: '#' }
    ]
  },
  {
    id: 'c2',
    numReference: 'BOD-2026-00098',
    objet: 'Réclamation facturation eau',
    description: 'Ma facture de ce mois-ci semble erronée, merci de vérifier.',
    statut: 'Clôturé',
    dateSoumission: '2026-07-03T10:00:00',
    typeCourrier: 'Réclamation',
    serviceDestinataire: 'Commune de Tunis - Service Réclamations',
    historique: [
      { statut: 'Reçu', dateAction: '2026-07-03T10:00:00', description: 'Courrier soumis' },
      { statut: 'Traité', dateAction: '2026-07-10T11:00:00', description: 'Facture corrigée' },
      { statut: 'Clôturé', dateAction: '2026-07-11T09:00:00', description: 'Dossier clôturé' }
    ],
    piecesJointes: []
  },
  {
    id: 'c3',
    numReference: 'BOD-2026-00151',
    objet: "Demande d'attestation de résidence",
    description: "Je souhaite obtenir une attestation de résidence pour une démarche administrative.",
    statut: 'Reçu',
    dateSoumission: '2026-08-14T08:30:00',
    typeCourrier: "Demande d'information",
    serviceDestinataire: 'Commune de Bizerte - Service État Civil',
    historique: [
      { statut: 'Reçu', dateAction: '2026-08-14T08:30:00', description: 'Courrier soumis' }
    ],
    piecesJointes: []
  }
]

// ---------------------------------------------------------------------
// Courriers
// ---------------------------------------------------------------------
export const courrierApi = {
  /**
   * POST /api/Courrier (multipart/form-data)
   * payload: { typeCourrierId, serviceDestinataireId, objet, description, fichiers: File[] }
   * Correspond exactement à CourrierCreateDto + le paramètre `fichiers` du contrôleur.
   */
  submit(payload) {
    if (USE_MOCK) {
      console.log('[MOCK] Courrier soumis :', payload)
      return mockDelay({ id: Math.floor(Math.random() * 1000), reference: `2026-${Math.floor(Math.random() * 90000) + 10000}` }, 500)
    }

    const auth = useAuthStore()
    const formData = new FormData()
    formData.append('TypeCourrierId', payload.typeCourrierId)
    formData.append('ServiceDestinataireId', payload.serviceDestinataireId)
    formData.append('Objet', payload.objet)
    formData.append('Description', payload.description)
    ;(payload.fichiers || []).forEach(f => formData.append('fichiers', f))

    return fetch(`${BASE_URL}/Courrier`, {
      method: 'POST',
      headers: {
        ...(auth?.token ? { Authorization: `Bearer ${auth.token}` } : {})
        // Pas de Content-Type manuel : le navigateur gère le boundary multipart
      },
      body: formData
    }).then(async res => {
      if (!res.ok) {
        const err = await res.json().catch(() => ({}))
        throw new Error(err.message || `Erreur lors de l'envoi (${res.status})`)
      }
      return res.json()
    })
  },

  /** GET /api/Suivi -> CourrierListItemDto[] */
  list() {
    if (USE_MOCK) {
      return mockDelay(MOCK_COURRIERS)
    }
    return request('/Suivi')
  },

  /** GET /api/Suivi/{id} */
  get(id) {
    if (USE_MOCK) {
      const found = MOCK_COURRIERS.find(c => c.id === id)
      if (!found) return Promise.reject(new Error('Courrier introuvable.'))
      return mockDelay(found)
    }
    return request(`/Suivi/${id}`)
  },

  /** GET /api/Suivi/rechercher?motCle= */
  search(motCle) {
    if (USE_MOCK) {
      const q = (motCle || '').toLowerCase()
      const results = q
        ? MOCK_COURRIERS.filter(c => c.objet.toLowerCase().includes(q))
        : MOCK_COURRIERS
      return mockDelay(results)
    }
    return request(`/Suivi/rechercher?motCle=${encodeURIComponent(motCle)}`)
  }
}