
<template>
  <div class="min-h-screen bg-white text-slate-900">

    <!-- ================= NAVBAR ================= -->
    <header
      class="sticky top-0 z-50 border-b border-slate-100 bg-white/95 backdrop-blur"
    >
      <div
        class="mx-auto flex h-[72px] max-w-[1400px] items-center justify-between px-6 lg:px-10"
      >

        <!-- ================= LOGO ================= -->
        <RouterLink to="/" class="flex items-center">
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

        <!-- ================= MENU ================= -->
        <nav class="hidden items-center gap-9 md:flex">

          <RouterLink
            to="/"
            class="relative py-6 text-sm font-semibold text-slate-900"
          >
            Accueil
            <span
              class="absolute bottom-0 left-0 h-[3px] w-full rounded-full bg-yellow-400"
            ></span>
          </RouterLink>

          <RouterLink
            to="/courrier/soumettre"
            class="py-6 text-sm font-medium text-slate-600 transition hover:text-slate-900"
          >
            Démarches
          </RouterLink>

          <RouterLink
            to="/courrier"
            class="py-6 text-sm font-medium text-slate-600 transition hover:text-slate-900"
          >
            Mes courriers
          </RouterLink>

          <RouterLink 
  to="/a-propos"
  class="py-6 text-sm font-medium text-slate-600 transition hover:text-slate-900"
>
  À propos
</RouterLink>

          <a
  href="#contact"
  class="py-6 text-sm font-medium text-slate-600 transition hover:text-slate-900"
>
  Contact
</a>
         <button @click="setLocale('fr')">FR</button>
         <button @click="setLocale('ar')">عربي</button>

        </nav>

        <!-- ================= ACTIONS ================= -->
        <div class="flex items-center gap-3">
  <template v-if="!auth.isAuthenticated">
    <RouterLink to="/login" class="hidden items-center gap-2 rounded-xl border border-yellow-300 px-5 py-3 text-sm font-semibold text-slate-800 transition hover:bg-yellow-50 sm:flex">
      Se connecter
    </RouterLink>
    <RouterLink to="/register" class="flex items-center gap-2 rounded-xl bg-yellow-400 px-5 py-3 text-sm font-bold text-slate-900 shadow-sm transition hover:bg-yellow-500 hover:shadow-md">
      Créer un compte
    </RouterLink>
  </template>
  <button v-else @click="handleLogout" class="flex items-center gap-2 rounded-xl border border-slate-200 px-5 py-3 text-sm font-semibold text-slate-700 transition hover:bg-slate-50">
    Déconnexion
  </button>
</div>
           
          
      </div>
    </header>


    <!-- ================= HERO ================= -->
    <main>

      <section class="relative overflow-hidden">

        <!-- Décor jaune -->
        <div
          class="absolute -right-32 top-20 h-80 w-80 rounded-full bg-yellow-100 opacity-80 blur-2xl"
        ></div>

        <div
          class="absolute right-0 top-24 hidden h-72 w-72 rounded-full border-[35px] border-yellow-100 lg:block"
        ></div>

        <div
          class="mx-auto grid max-w-[1400px] items-center gap-14 px-6 py-20 lg:grid-cols-2 lg:px-10 lg:py-28"
        >

          <!-- ================= TEXTE ================= -->
          <div class="relative z-10 max-w-2xl">

            <div
              class="mb-6 inline-flex items-center gap-2 rounded-full bg-yellow-50 px-4 py-2"
            >
              <span class="h-2 w-2 rounded-full bg-yellow-400"></span>

              <span
                class="text-xs font-bold uppercase tracking-wider text-yellow-600"
              >
                Plateforme officielle
              </span>
            </div>

            <h1
              class="text-5xl font-extrabold leading-[1.08] tracking-tight text-slate-900 lg:text-6xl"
            >
              Vos démarches
              <br />

              administratives,
              <br />

              <span class="text-yellow-500">
                simplement digitales.
              </span>
            </h1>

            <p
              class="mt-7 max-w-xl text-lg leading-8 text-slate-600"
            >
              Déposez vos courriers en ligne, suivez leur traitement
              en temps réel et recevez vos réponses sans vous déplacer.
            </p>


            <!-- ================= BUTTONS ================= -->
            <div class="mt-9 flex flex-wrap gap-4">

              <!-- Déposer -->
              <RouterLink
                to="/courrier/soumettre"
                class="group flex items-center gap-3 rounded-xl bg-yellow-400 px-7 py-4 font-bold text-slate-900 shadow-lg shadow-yellow-100 transition hover:-translate-y-0.5 hover:bg-yellow-500 hover:shadow-xl"
              >

                <svg
                  class="h-5 w-5 transition group-hover:translate-x-0.5"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M12 19V5m0 0l-5 5m5-5l5 5"
                  />
                </svg>

                Déposer un courrier
              </RouterLink>


              <!-- Suivre -->
              <RouterLink
                to="/courrier"
                class="flex items-center gap-3 rounded-xl border border-yellow-300 bg-white px-7 py-4 font-bold text-slate-800 transition hover:bg-yellow-50"
              >

                <svg
                  class="h-5 w-5"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M15 12H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z"
                  />
                </svg>

                Suivre mon courrier
              </RouterLink>

            </div>


            <!-- ================= POINTS FORTS ================= -->
            <div class="mt-10 flex flex-wrap gap-x-8 gap-y-4">

              <div class="flex items-center gap-2">
                <div
                  class="flex h-8 w-8 items-center justify-center rounded-lg bg-yellow-50 text-yellow-500"
                >
                  ✓
                </div>

                <span class="text-sm font-semibold text-slate-700">
                  100% sécurisé
                </span>
              </div>


              <div class="flex items-center gap-2">
                <div
                  class="flex h-8 w-8 items-center justify-center rounded-lg bg-yellow-50 text-yellow-500"
                >
                  ↗
                </div>

                <span class="text-sm font-semibold text-slate-700">
                  Suivi en temps réel
                </span>
              </div>


              <div class="flex items-center gap-2">
                <div
                  class="flex h-8 w-8 items-center justify-center rounded-lg bg-yellow-50 text-yellow-500"
                >
                  ♢
                </div>

                <span class="text-sm font-semibold text-slate-700">
                  Notifications
                </span>
              </div>

            </div>

          </div>


          <!-- ================= MOCKUP DASHBOARD ================= -->
          <div class="relative z-10">

            <div class="relative mx-auto max-w-[570px]">

              <!-- Cercle décoratif -->
              <div
                class="absolute -right-5 top-5 h-20 w-20 rounded-full border-[10px] border-yellow-400"
              ></div>


              <!-- Dashboard -->
              <div
                class="overflow-hidden rounded-3xl border border-slate-100 bg-white shadow-[0_25px_70px_rgba(15,23,42,0.12)]"
              >

                <!-- Top dashboard -->
                <div
                  class="flex items-center justify-between border-b border-slate-100 px-5 py-4"
                >

                  <div class="flex items-center gap-3">

                    <div
                      class="flex h-9 w-9 items-center justify-center rounded-xl bg-yellow-400"
                    >
                      <img
                        :src="logo"
                        alt="Logo"
                        class="h-6 w-6 object-contain"
                      />
                    </div>

                    <div>
                      <div
                        class="h-2 w-24 rounded bg-slate-200"
                      ></div>

                      <div
                        class="mt-2 h-2 w-16 rounded bg-slate-100"
                      ></div>
                    </div>

                  </div>


                  <div class="flex items-center gap-3">

                    <div
                      class="flex h-8 w-8 items-center justify-center rounded-full bg-yellow-50"
                    >
                      ♧
                    </div>

                    <div
                      class="h-8 w-8 rounded-full bg-slate-200"
                    ></div>

                  </div>

                </div>


                <div class="flex">

                  <!-- ================= SIDEBAR ================= -->
                  <aside
                    class="hidden w-16 border-r border-slate-100 p-3 sm:block"
                  >

                    <div
                      class="mb-5 flex h-9 w-9 items-center justify-center rounded-lg bg-yellow-50 text-yellow-500"
                    >
                      ⌂
                    </div>

                    <div
                      class="mb-5 flex h-9 w-9 items-center justify-center rounded-lg text-slate-300"
                    >
                      ▣
                    </div>

                    <div
                      class="mb-5 flex h-9 w-9 items-center justify-center rounded-lg text-slate-300"
                    >
                      ◇
                    </div>

                    <div
                      class="mb-5 flex h-9 w-9 items-center justify-center rounded-lg text-slate-300"
                    >
                      ◌
                    </div>

                  </aside>


                  <!-- ================= CONTENT ================= -->
                  <div class="flex-1 p-5">

                    <div class="mb-5">

                      <p
                        class="text-xl font-bold text-slate-900"
                      >
                        Bonjour, Citoyen 👋
                      </p>

                      <p
                        class="mt-1 text-xs text-slate-500"
                      >
                        Voici l'état de vos courriers
                      </p>

                    </div>


                    <!-- ================= STATS ================= -->
                    <div
                      class="grid grid-cols-2 gap-3 sm:grid-cols-4"
                    >

                      <div
                        class="rounded-xl border border-slate-100 p-3 shadow-sm"
                      >
                        <div
                          class="text-xl font-extrabold text-slate-900"
                        >
                          12
                        </div>

                        <div
                          class="mt-1 text-[10px] text-slate-500"
                        >
                          Courriers déposés
                        </div>
                      </div>


                      <div
                        class="rounded-xl border border-slate-100 p-3 shadow-sm"
                      >
                        <div
                          class="text-xl font-extrabold text-slate-900"
                        >
                          5
                        </div>

                        <div
                          class="mt-1 text-[10px] text-slate-500"
                        >
                          En traitement
                        </div>
                      </div>


                      <div
                        class="rounded-xl border border-slate-100 p-3 shadow-sm"
                      >
                        <div
                          class="text-xl font-extrabold text-slate-900"
                        >
                          3
                        </div>

                        <div
                          class="mt-1 text-[10px] text-slate-500"
                        >
                          Réponses reçues
                        </div>
                      </div>


                      <div
                        class="rounded-xl border border-slate-100 p-3 shadow-sm"
                      >
                        <div
                          class="text-xl font-extrabold text-slate-900"
                        >
                          2
                        </div>

                        <div
                          class="mt-1 text-[10px] text-slate-500"
                        >
                          Brouillons
                        </div>
                      </div>

                    </div>


                    <!-- ================= COURRIERS ================= -->
                    <div
                      class="mt-5 rounded-2xl border border-slate-100 p-4"
                    >

                      <div
                        class="mb-4 flex items-center justify-between"
                      >

                        <h3
                          class="text-sm font-bold text-slate-900"
                        >
                          Mes derniers courriers
                        </h3>

                        <span
                          class="text-xs font-semibold text-yellow-500"
                        >
                          Voir tout
                        </span>

                      </div>


                      <div class="space-y-2">

                        <!-- Courrier 1 -->
                        <div
                          class="flex items-center justify-between rounded-xl bg-slate-50 p-3"
                        >

                          <div class="flex items-center gap-3">

                            <div
                              class="flex h-8 w-8 items-center justify-center rounded-lg bg-yellow-50 text-yellow-500"
                            >
                              ▣
                            </div>

                            <div>

                              <p
                                class="text-[11px] font-bold text-slate-800"
                              >
                                BOD-2026-0458
                              </p>

                              <p
                                class="text-[10px] text-slate-500"
                              >
                                Demande de document
                              </p>

                            </div>

                          </div>

                          <span
                            class="rounded-full bg-yellow-100 px-2 py-1 text-[9px] font-semibold text-yellow-700"
                          >
                            En traitement
                          </span>

                        </div>


                        <!-- Courrier 2 -->
                        <div
                          class="flex items-center justify-between rounded-xl bg-slate-50 p-3"
                        >

                          <div class="flex items-center gap-3">

                            <div
                              class="flex h-8 w-8 items-center justify-center rounded-lg bg-green-50 text-green-500"
                            >
                              ✓
                            </div>

                            <div>

                              <p
                                class="text-[11px] font-bold text-slate-800"
                              >
                                BOD-2026-0457
                              </p>

                              <p
                                class="text-[10px] text-slate-500"
                              >
                                Réclamation
                              </p>

                            </div>

                          </div>

                          <span
                            class="rounded-full bg-slate-100 px-2 py-1 text-[9px] font-semibold text-slate-600"
                          >
                            Reçu
                          </span>

                        </div>


                        <!-- Courrier 3 -->
                        <div
                          class="flex items-center justify-between rounded-xl bg-slate-50 p-3"
                        >

                          <div class="flex items-center gap-3">

                            <div
                              class="flex h-8 w-8 items-center justify-center rounded-lg bg-yellow-50 text-yellow-500"
                            >
                              ✓
                            </div>

                            <div>

                              <p
                                class="text-[11px] font-bold text-slate-800"
                              >
                                BOD-2026-0456
                              </p>

                              <p
                                class="text-[10px] text-slate-500"
                              >
                                Information
                              </p>

                            </div>

                          </div>

                          <span
                            class="rounded-full bg-green-100 px-2 py-1 text-[9px] font-semibold text-green-700"
                          >
                            Réponse envoyée
                          </span>

                        </div>

                      </div>

                    </div>

                  </div>
                </div>

              </div>


              <!-- Petit cercle -->
              <div
                class="absolute -bottom-7 -right-5 h-16 w-16 rounded-full border-[9px] border-yellow-400 bg-white"
              ></div>

            </div>
          </div>

        </div>
      </section>


      <!-- ================= FEATURES ================= -->
      <section
        class="border-t border-slate-50 bg-white py-20 lg:py-24"
      >

        <div
          class="mx-auto max-w-[1400px] px-6 lg:px-10"
        >

          <div class="text-center">

            <p
              class="text-xs font-bold uppercase tracking-widest text-yellow-500"
            >
              Des fonctionnalités pensées pour vous
            </p>

            <h2
              class="mt-3 text-3xl font-extrabold tracking-tight text-slate-900 lg:text-4xl"
            >
              Tout ce qu'il vous faut, au même endroit.
            </h2>

          </div>


          <div
            class="mt-12 grid gap-5 md:grid-cols-2 lg:grid-cols-5"
          >

            <!-- CARD 1 -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 text-center shadow-sm transition hover:-translate-y-1 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                ▣
              </div>

              <h3 class="mt-6 font-bold text-slate-900">
                Dépôt en ligne
              </h3>

              <p
                class="mt-3 text-sm leading-6 text-slate-500"
              >
                Déposez vos courriers et pièces jointes en toute simplicité.
              </p>

            </div>


            <!-- CARD 2 -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 text-center shadow-sm transition hover:-translate-y-1 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                ◉
              </div>

              <h3 class="mt-6 font-bold text-slate-900">
                Suivi transparent
              </h3>

              <p
                class="mt-3 text-sm leading-6 text-slate-500"
              >
                Suivez l'avancement de vos demandes en temps réel.
              </p>

            </div>


            <!-- CARD 3 -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 text-center shadow-sm transition hover:-translate-y-1 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                ♢
              </div>

              <h3 class="mt-6 font-bold text-slate-900">
                Notifications
              </h3>

              <p
                class="mt-3 text-sm leading-6 text-slate-500"
              >
                Recevez des alertes à chaque étape importante.
              </p>

            </div>


            <!-- CARD 4 -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 text-center shadow-sm transition hover:-translate-y-1 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                🔒
              </div>

              <h3 class="mt-6 font-bold text-slate-900">
                Sécurité garantie
              </h3>

              <p
                class="mt-3 text-sm leading-6 text-slate-500"
              >
                Vos données sont protégées et confidentielles.
              </p>

            </div>


            <!-- CARD 5 -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 text-center shadow-sm transition hover:-translate-y-1 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                ✦
              </div>

              <h3 class="mt-6 font-bold text-slate-900">
                Assistance intelligente
              </h3>

              <p
                class="mt-3 text-sm leading-6 text-slate-500"
              >
                Bientôt disponible : un assistant IA pour vous guider.
              </p>

            </div>

          </div>

        </div>
      </section>


      <!-- ================= HOW IT WORKS ================= -->
      <section
        class="bg-[#fffdf7] py-20 lg:py-24"
      >

        <div
          class="mx-auto max-w-[1200px] px-6 lg:px-10"
        >

          <div class="text-center">

            <p
              class="text-xs font-bold uppercase tracking-widest text-yellow-500"
            >
              Comment ça marche ?
            </p>

            <h2
              class="mt-3 text-3xl font-extrabold text-slate-900 lg:text-4xl"
            >
              Trois étapes, zéro tracas.
            </h2>

          </div>


          <div
            class="mt-14 grid gap-10 md:grid-cols-3"
          >

            <!-- STEP 1 -->
            <div
              class="relative text-center md:text-left"
            >

              <div
                class="mx-auto flex h-20 w-20 items-center justify-center rounded-full border-4 border-yellow-200 bg-white text-2xl shadow-sm md:mx-0"
              >
                👤
              </div>

              <div class="mt-5">

                <span
                  class="text-sm font-extrabold text-yellow-500"
                >
                  01
                </span>

                <h3
                  class="mt-2 text-xl font-bold text-slate-900"
                >
                  Créez votre compte
                </h3>

                <p
                  class="mt-3 text-sm leading-7 text-slate-600"
                >
                  Inscrivez-vous en quelques secondes pour accéder
                  à votre espace personnel.
                </p>

              </div>

            </div>


            <!-- STEP 2 -->
            <div
              class="relative text-center md:text-left"
            >

              <div
                class="mx-auto flex h-20 w-20 items-center justify-center rounded-full border-4 border-yellow-200 bg-white text-2xl shadow-sm md:mx-0"
              >
                ➤
              </div>

              <div class="mt-5">

                <span
                  class="text-sm font-extrabold text-yellow-500"
                >
                  02
                </span>

                <h3
                  class="mt-2 text-xl font-bold text-slate-900"
                >
                  Déposez votre courrier
                </h3>

                <p
                  class="mt-3 text-sm leading-7 text-slate-600"
                >
                  Renseignez votre demande, ajoutez vos documents
                  et envoyez votre courrier.
                </p>

              </div>

            </div>

            <!-- STEP 3 -->
            <div
              class="relative text-center md:text-left"
            >

              <div
                class="mx-auto flex h-20 w-20 items-center justify-center rounded-full border-4 border-yellow-200 bg-white text-2xl shadow-sm md:mx-0"
              >
                ✓
              </div>

              <div class="mt-5">

                <span
                  class="text-sm font-extrabold text-yellow-500"
                >
                  03
                </span>

                <h3
                  class="mt-2 text-xl font-bold text-slate-900"
                >
                  Suivez son traitement
                </h3>

                <p
                  class="mt-3 text-sm leading-7 text-slate-600"
                >
                  Consultez l'état d'avancement jusqu'à la réception
                  de la réponse.
                </p>

              </div>

            </div>
          </div>
          <div class="mt-12 text-center">

            <RouterLink
              to="/register"
              class="inline-flex items-center gap-2 rounded-xl bg-yellow-400 px-7 py-4 font-bold text-slate-900 shadow-lg shadow-yellow-100 transition hover:bg-yellow-500 hover:shadow-xl"
            >
              Commencer maintenant
              <span>→</span>
            </RouterLink>

          </div>

        </div>
      </section>


      <!-- ================= CTA ================= -->
      <section
        id="apropos"
        class="bg-white px-6 py-20"
      >

        <div
          class="mx-auto max-w-5xl rounded-3xl bg-slate-900 px-8 py-14 text-center shadow-xl lg:px-16"
        >

          <div
            class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-400 p-2"
          >
            <img
              :src="logo"
              alt="Bureau d'Ordre Digital"
              class="h-full w-full object-contain"
            />
          </div>

          <h2
            class="mt-6 text-3xl font-extrabold text-white lg:text-4xl"
          >
            Vos démarches commencent ici.
          </h2>

          <p
            class="mx-auto mt-4 max-w-2xl leading-7 text-slate-300"
          >
            Une plateforme simple, sécurisée et moderne pour gérer
            vos courriers administratifs sans vous déplacer.
          </p>

          <RouterLink
            to="/register"
            class="mt-8 inline-flex rounded-xl bg-yellow-400 px-7 py-4 font-bold text-slate-900 transition hover:bg-yellow-500"
          >
            Créer mon compte
          </RouterLink>

        </div>

      </section>

    </main>


    <!-- ================= FOOTER ================= -->
    <footer
      id="contact"
      class="border-t border-slate-100 bg-white"
    >

      <div
        class="mx-auto flex max-w-[1400px] flex-col gap-4 px-6 py-8 text-sm text-slate-500 md:flex-row md:items-center md:justify-between lg:px-10"
      >

        <div class="font-semibold text-slate-700">
          © 2026 Bureau d'Ordre Digital
        </div>

        <div class="flex gap-6">
          <span>Plateforme citoyenne</span>
          <span>•</span>
          <span>Sécurisée</span>
          <span>•</span>
          <span>Digitale</span>
        </div>

      </div>

    </footer>

  </div>
</template>


<!-- ================= SCRIPT ================= -->
<script setup>
import logo from '@/assets/logo.png'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()

function handleLogout() {
  auth.logout()
  router.push('/')
}

</script>