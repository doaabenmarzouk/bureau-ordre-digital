<!-- client/src/views/courrier/CourrierListView.vue -->

<template>
  <div class="min-h-screen bg-white text-slate-900">

    <!-- ================= HEADER ================= -->
    <header class="border-b border-slate-100 bg-white sticky top-0 z-30">
      <div class="max-w-7xl mx-auto px-5 lg:px-8 h-20 flex items-center justify-between">

        <!-- ================= LOGO ================= -->
        <RouterLink to="/" class="flex items-center gap-3">

          <img
            :src="logo"
            alt="Bureau d'Ordre Digital"
            class="h-12 w-auto object-contain"
          />

          <!-- Nom de la plateforme -->
          <div class="leading-tight">
            <div class="text-[15px] font-extrabold tracking-wide text-slate-900">
              BUREAU
            </div>
            <div class="text-[15px] font-extrabold tracking-wide text-slate-900">
              D'ORDRE DIGITAL
            </div>
          </div>

        </RouterLink>


        <!-- ================= NAVIGATION ================= -->
        <nav class="hidden md:flex items-center gap-8 text-sm font-medium">

          <RouterLink
            to="/"
            class="text-slate-500 hover:text-slate-900 transition"
          >
            Accueil
          </RouterLink>

          <RouterLink
            to="/courrier/soumettre"
            class="text-slate-500 hover:text-slate-900 transition"
          >
            Démarches
          </RouterLink>

          <RouterLink
            to="/courrier"
            class="relative text-slate-900 font-semibold"
          >
            Mes courriers

            <span
              class="absolute -bottom-7 left-0 right-0 h-1 bg-amber-400 rounded-full"
            ></span>
          </RouterLink>

          <a
            href="#"
            class="text-slate-500 hover:text-slate-900 transition"
          >
            À propos
          </a>

          <a
            href="#"
            class="text-slate-500 hover:text-slate-900 transition"
          >
            Contact
          </a>

        </nav>


        <!-- ================= ACTION ================= -->
        <div class="flex items-center gap-3">
  <RouterLink to="/courrier/soumettre" class="hidden sm:inline-flex items-center gap-2 bg-amber-400 hover:bg-amber-500 text-slate-950 font-bold px-5 py-3 rounded-xl transition shadow-sm">
    <span class="text-lg">+</span>
    Nouveau courrier
  </RouterLink>

  <button @click="handleLogout" class="text-sm font-medium text-slate-500 hover:text-slate-900 transition">
    Déconnexion
  </button>
</div>

      </div>
    </header>
    <main class="max-w-7xl mx-auto px-5 lg:px-8 py-10">

      <section class="mb-9">

        <div
          class="flex flex-col lg:flex-row lg:items-end lg:justify-between gap-5"
        >

          <div>

            <p
              class="text-amber-500 font-bold text-xs tracking-widest uppercase mb-2"
            >
              ESPACE CITOYEN
            </p>

            <h1
              class="text-3xl md:text-4xl font-extrabold tracking-tight text-slate-900"
            >
              Mes courriers
            </h1>

            <p class="mt-2 text-slate-500 max-w-xl">
              Retrouvez toutes vos demandes administratives et suivez leur
              traitement en temps réel.
            </p>

          </div>


          <RouterLink
            to="/courrier/soumettre"
            class="inline-flex items-center justify-center gap-2 bg-slate-900 hover:bg-slate-800 text-white font-semibold px-5 py-3 rounded-xl transition"
          >
            <span class="text-amber-400 text-lg">+</span>
            Déposer un courrier
          </RouterLink>

        </div>

      </section>
      <section class="grid grid-cols-2 lg:grid-cols-4 gap-4 mb-8">

        <!-- Total -->
        <div
          class="bg-white border border-slate-100 rounded-2xl p-5 shadow-[0_4px_20px_rgba(15,23,42,0.04)]"
        >

          <div class="flex items-center justify-between mb-4">

            <div
              class="w-10 h-10 rounded-xl bg-amber-50 flex items-center justify-center text-amber-500"
            >
              📄
            </div>

            <span class="text-xs text-slate-400">
              Total
            </span>

          </div>

          <div class="text-2xl font-extrabold text-slate-900">
            {{ courriers.length }}
          </div>

          <p class="text-sm text-slate-500 mt-1">
            Courriers déposés
          </p>

        </div>
        <div
          class="bg-white border border-slate-100 rounded-2xl p-5 shadow-[0_4px_20px_rgba(15,23,42,0.04)]"
        >

          <div class="flex items-center justify-between mb-4">

            <div
              class="w-10 h-10 rounded-xl bg-amber-50 flex items-center justify-center text-amber-500"
            >
              ◷
            </div>

            <span class="text-xs text-slate-400">
              En cours
            </span>

          </div>

          <div class="text-2xl font-extrabold text-slate-900">
            {{ nombreEnCours }}
          </div>

          <p class="text-sm text-slate-500 mt-1">
            En traitement
          </p>

        </div>
        <div
          class="bg-white border border-slate-100 rounded-2xl p-5 shadow-[0_4px_20px_rgba(15,23,42,0.04)]"
        >

          <div class="flex items-center justify-between mb-4">

            <div
              class="w-10 h-10 rounded-xl bg-green-50 flex items-center justify-center text-green-600"
            >
              ✓
            </div>

            <span class="text-xs text-slate-400">
              Réponses
            </span>

          </div>

          <div class="text-2xl font-extrabold text-slate-900">
            {{ nombreTraites }}
          </div>

          <p class="text-sm text-slate-500 mt-1">
            Courriers traités
          </p>

        </div>
        <div
          class="bg-white border border-slate-100 rounded-2xl p-5 shadow-[0_4px_20px_rgba(15,23,42,0.04)]"
        >

          <div class="flex items-center justify-between mb-4">

            <div
              class="w-10 h-10 rounded-xl bg-slate-100 flex items-center justify-center text-slate-600"
            >
              ✎
            </div>

            <span class="text-xs text-slate-400">
              Brouillons
            </span>

          </div>

          <div class="text-2xl font-extrabold text-slate-900">
            0
          </div>

          <p class="text-sm text-slate-500 mt-1">
            En attente
          </p>

        </div>

      </section>


      <!-- ================= OUTILS ================= -->
      <section
        class="bg-white border border-slate-100 rounded-2xl p-4 md:p-5 mb-6 shadow-[0_4px_20px_rgba(15,23,42,0.04)]"
      >

        <div
          class="flex flex-col lg:flex-row gap-4 lg:items-center lg:justify-between"
        >

          <!-- Recherche -->
          <div class="relative flex-1">

            <span
              class="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400"
            >
              🔍
            </span>

            <input
              v-model="query"
              type="text"
              placeholder="Rechercher un courrier par objet ou référence..."
              class="w-full bg-slate-50 border border-slate-100 rounded-xl pl-11 pr-4 py-3 text-sm outline-none focus:bg-white focus:border-amber-300 focus:ring-4 focus:ring-amber-50 transition"
            />

          </div>


          <!-- Filtres -->
          <div class="flex items-center gap-2 overflow-x-auto">

            <button
              v-for="filter in filters"
              :key="filter.value"
              @click="activeFilter = filter.value"
              class="whitespace-nowrap px-4 py-2.5 rounded-xl text-sm font-medium transition"
              :class="
                activeFilter === filter.value
                  ? 'bg-amber-400 text-slate-950 shadow-sm'
                  : 'bg-slate-50 text-slate-500 hover:bg-slate-100'
              "
            >
              {{ filter.label }}
            </button>

          </div>

        </div>

      </section>
      <div
        v-if="loading"
        class="flex flex-col items-center justify-center py-20"
      >

        <span class="spinner mb-4"></span>

        <p class="text-sm text-slate-500">
          Chargement de vos courriers...
        </p>

      </div>
      <div
        v-if="!loading && filteredCourriers.length === 0"
        class="bg-white border border-dashed border-slate-200 rounded-2xl py-20 text-center"
      >

        <div
          class="mx-auto w-16 h-16 rounded-2xl bg-amber-50 flex items-center justify-center text-3xl mb-5"
        >
          📭
        </div>

        <h2 class="text-lg font-bold text-slate-900">

          {{
            query || activeFilter !== 'all'
              ? 'Aucun courrier trouvé'
              : 'Vous n’avez encore soumis aucun courrier'
          }}

        </h2>

        <p class="text-sm text-slate-500 mt-2 mb-6">

          {{
            query || activeFilter !== 'all'
              ? 'Essayez de modifier votre recherche ou votre filtre.'
              : 'Déposez votre première demande administrative en ligne.'
          }}

        </p>

        <RouterLink
          v-if="!query && activeFilter === 'all'"
          to="/courrier/soumettre"
          class="inline-flex items-center gap-2 bg-amber-400 hover:bg-amber-500 text-slate-950 font-bold px-5 py-3 rounded-xl transition"
        >
          <span>+</span>
          Déposer mon premier courrier
        </RouterLink>

      </div>
      <section
        v-if="!loading && filteredCourriers.length > 0"
        class="space-y-3"
      >

        <RouterLink
          v-for="c in filteredCourriers"
          :key="c.id"
          :to="{ name: 'courrier-detail', params: { id: c.id } }"
          class="group block bg-white border border-slate-100 rounded-2xl p-5 hover:border-amber-300 hover:shadow-[0_8px_30px_rgba(245,158,11,0.08)] transition-all"
        >

          <div class="flex items-start gap-4">

            <!-- Icône -->
            <div
              class="hidden sm:flex w-12 h-12 rounded-xl bg-amber-50 items-center justify-center text-amber-500 shrink-0"
            >
              📄
            </div>


            <!-- Contenu -->
            <div class="min-w-0 flex-1">

              <div
                class="flex flex-col md:flex-row md:items-start md:justify-between gap-3"
              >

                <div class="min-w-0">

                  <h3
                    class="font-bold text-slate-900 truncate group-hover:text-amber-600 transition"
                  >
                    {{ c.objet }}
                  </h3>

                  <p class="text-sm text-slate-500 mt-1">
                    {{ c.organismeName || 'Administration' }}
                  </p>

                </div>


                <!-- Statut -->
                <span
                  class="inline-flex items-center gap-1.5 self-start text-xs font-semibold px-3 py-1.5 rounded-full shrink-0"
                  :class="statusClass(c.statut)"
                >

                  <span
                    class="w-1.5 h-1.5 rounded-full bg-current"
                  ></span>

                  {{ c.statut }}

                </span>

              </div>


              <!-- Informations -->
              <div
                class="flex flex-wrap items-center gap-x-5 gap-y-2 mt-4 text-xs text-slate-400"
              >

                <span>
                  Réf.
                  <strong class="text-slate-500">
                    {{ c.reference }}
                  </strong>
                </span>

                <span>
                  Soumis le {{ formatDate(c.dateSoumission) }}
                </span>

              </div>

            </div>
            <div
              class="hidden sm:flex w-9 h-9 rounded-full bg-slate-50 items-center justify-center text-slate-400 group-hover:bg-amber-50 group-hover:text-amber-500 transition shrink-0"
            >
              →
            </div>

          </div>

        </RouterLink>

      </section>

    </main>

  </div>
</template>


<script setup>
import { ref, computed, onMounted } from 'vue'
import { courrierApi } from '@/services/api'

// ================= LOGO =================
import logo from '@/assets/logo.png'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()




const courriers = ref([])
const loading = ref(false)
const error = ref('')
const query = ref('')
const activeFilter = ref('all')


const filters = [
  { label: 'Tous', value: 'all' },
  { label: 'En cours', value: 'En cours' },
  { label: 'Traités', value: 'Traité' },
  { label: 'Réponse envoyée', value: 'Réponse envoyée' }
]


// ================= CHARGEMENT =================

async function fetchCourriers() {

  loading.value = true
  error.value = ''

  try {

    courriers.value = await courrierApi.list()

  } catch (e) {

    error.value =
      'Impossible de charger vos courriers. Réessayez plus tard.'

    console.error(e)

  } finally {

    loading.value = false

  }

}

onMounted(fetchCourriers)


// ================= RECHERCHE + FILTRE =================

const filteredCourriers = computed(() => {

  let result = courriers.value

  // Recherche
  if (query.value.trim()) {

    const q = query.value.toLowerCase()

    result = result.filter(c =>
      (c.objet || '').toLowerCase().includes(q) ||
      (c.reference || '').toLowerCase().includes(q)
    )

  }

  // Filtre
  if (activeFilter.value !== 'all') {

    if (activeFilter.value === 'En cours') {

      result = result.filter(c =>
        [
          'En cours de traitement',
          'Transmis au service',
          'En attente de réponse'
        ].includes(c.statut)
      )

    } else {

      result = result.filter(c =>
        c.statut === activeFilter.value
      )

    }

  }

  return result

})


// ================= STATISTIQUES =================

const nombreEnCours = computed(() =>
  courriers.value.filter(c =>
    [
      'En cours de traitement',
      'Transmis au service',
      'En attente de réponse'
    ].includes(c.statut)
  ).length
)


const nombreTraites = computed(() =>
  courriers.value.filter(c =>
    [
      'Traité',
      'Réponse envoyée',
      'Clôturé',
      'Classé'
    ].includes(c.statut)
  ).length
)


function formatDate(dateStr) {

  if (!dateStr) return '—'

  return new Date(dateStr).toLocaleDateString(
    'fr-FR',
    {
      day: '2-digit',
      month: 'long',
      year: 'numeric'
    }
  )

}


const STATUS_STYLES = {

  'Reçu':
    'bg-slate-100 text-slate-600',

  'En cours de traitement':
    'bg-amber-50 text-amber-700',

  'Transmis au service':
    'bg-amber-50 text-amber-700',

  'En attente de réponse':
    'bg-orange-50 text-orange-700',

  'Traité':
    'bg-green-50 text-green-700',

  'Réponse envoyée':
    'bg-green-50 text-green-700',

  'Clôturé':
    'bg-slate-100 text-slate-500',

  'Classé':
    'bg-slate-100 text-slate-500'

}


function statusClass(statut) {

  return STATUS_STYLES[statut]
    || 'bg-slate-100 text-slate-600'

}
function handleLogout() {
  auth.logout()
  router.push('/')
}
</script>


<style scoped>

.spinner {
  display: block;
  width: 30px;
  height: 30px;
  border: 3px solid #f1f5f9;
  border-top-color: #f59e0b;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

</style>