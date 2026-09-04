<template>
  <div class="min-h-screen bg-white text-slate-900">

    <header
      class="sticky top-0 z-50 border-b border-slate-100 bg-white/95 backdrop-blur"
    >
      <div
        class="mx-auto flex h-[72px] max-w-[1400px] items-center justify-between px-6 lg:px-10"
      >

        <!-- LOGO -->
        <RouterLink to="/" class="flex items-center">
          <img
            :src="logo"
            alt="Bureau d'Ordre Digital"
            class="h-12 w-auto object-contain"
          />

          <div class="leading-tight">
            <div class="text-[15px] font-extrabold tracking-wide text-slate-900">
              BUREAU
            </div>
            <div class="text-[15px] font-extrabold tracking-wide text-slate-900">
              D'ORDRE DIGITAL
            </div>
          </div>
        </RouterLink>

        <!-- MENU -->
        <nav class="hidden items-center gap-9 md:flex">

          <RouterLink
            to="/"
            class="py-6 text-sm font-medium text-slate-600 transition hover:text-slate-900"
          >
            Accueil
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

          <!-- ACTIF -->
          <RouterLink
            to="/a-propos"
            class="relative py-6 text-sm font-semibold text-slate-900"
          >
            À propos

            <span
              class="absolute bottom-0 left-0 h-[3px] w-full rounded-full bg-yellow-400"
            ></span>
          </RouterLink>

          <a
            href="/#contact"
            class="py-6 text-sm font-medium text-slate-600 transition hover:text-slate-900"
          >
            Contact
          </a>

        </nav>

        <!-- ACTIONS -->
        <div class="flex items-center gap-3">

          <RouterLink
            to="/login"
            class="hidden rounded-xl border border-yellow-300 px-5 py-3 text-sm font-semibold text-slate-800 transition hover:bg-yellow-50 sm:block"
          >
            Se connecter
          </RouterLink>

          <RouterLink
            to="/register"
            class="rounded-xl bg-yellow-400 px-5 py-3 text-sm font-bold text-slate-900 shadow-sm transition hover:bg-yellow-500 hover:shadow-md"
          >
            Créer un compte
          </RouterLink>

        </div>
      </div>
    </header>


    <!-- ================= CONTENU ================= -->
    <main>

      <!-- HERO -->
      <section class="relative overflow-hidden bg-[#fffdf7]">

        <!-- Décoration jaune -->
        <div
          class="absolute -right-32 -top-32 h-96 w-96 rounded-full bg-yellow-100 opacity-70 blur-3xl"
        ></div>

        <div
          class="absolute -left-20 bottom-0 h-64 w-64 rounded-full bg-yellow-50"
        ></div>

        <div
          class="relative mx-auto max-w-[1200px] px-6 py-24 text-center lg:py-32"
        >

          <!-- Badge -->
          <div
            class="mx-auto mb-6 inline-flex items-center gap-2 rounded-full bg-yellow-50 px-4 py-2"
          >
            <span class="h-2 w-2 rounded-full bg-yellow-400"></span>

            <span
              class="text-xs font-bold uppercase tracking-widest text-yellow-600"
            >
              À propos de la plateforme
            </span>
          </div>

          <h1
            class="text-4xl font-extrabold tracking-tight text-slate-900 sm:text-5xl lg:text-6xl"
          >
            Simplifier vos démarches
            <br />

            <span class="text-yellow-500">
              administratives.
            </span>
          </h1>

          <p
            class="mx-auto mt-7 max-w-2xl text-lg leading-8 text-slate-600"
          >
            Le Bureau d'Ordre Digital vous permet de déposer,
            suivre et gérer vos courriers administratifs en ligne,
            simplement et en toute sécurité.
          </p>

        </div>
      </section>


      <!-- ================= PRÉSENTATION ================= -->
      <section class="bg-white py-20 lg:py-24">

        <div
          class="mx-auto grid max-w-[1200px] items-center gap-14 px-6 lg:grid-cols-2"
        >

          <!-- TEXTE -->
          <div>

            <p
              class="text-xs font-bold uppercase tracking-widest text-yellow-500"
            >
              Notre plateforme
            </p>

            <h2
              class="mt-3 text-3xl font-extrabold tracking-tight text-slate-900 lg:text-4xl"
            >
              Un bureau d'ordre,
              <br />
              désormais digital.
            </h2>

            <p class="mt-6 leading-8 text-slate-600">
              Le Bureau d'Ordre Digital est une plateforme conçue
              pour faciliter la gestion des courriers administratifs.
            </p>

            <p class="mt-4 leading-8 text-slate-600">
              Elle permet aux citoyens de transmettre leurs demandes
              sans avoir à se déplacer, tout en bénéficiant d'un suivi
              clair de chaque étape du traitement.
            </p>

            <p class="mt-4 leading-8 text-slate-600">
              Notre objectif est de rendre les démarches administratives
              plus simples, plus rapides et plus transparentes.
            </p>

          </div>


          <!-- CARTE -->
          <div class="relative">

            <!-- Cercle -->
            <div
              class="absolute -right-5 -top-5 h-20 w-20 rounded-full border-[10px] border-yellow-400"
            ></div>

            <div
              class="relative rounded-3xl border border-slate-100 bg-white p-8 shadow-[0_25px_70px_rgba(15,23,42,0.10)]"
            >

              <div
                class="flex h-16 w-16 items-center justify-center rounded-2xl bg-yellow-400 p-3"
              >
                <img
                  :src="logo"
                  alt="Logo"
                  class="h-full w-full object-contain"
                />
              </div>

              <h3
                class="mt-7 text-2xl font-extrabold text-slate-900"
              >
                Bureau d'Ordre Digital
              </h3>

              <p class="mt-4 leading-7 text-slate-500">
                Une solution moderne pour rapprocher
                l'administration du citoyen.
              </p>

              <!-- petites infos -->
              <div class="mt-8 space-y-4">

                <div class="flex items-center gap-3">
                  <div
                    class="flex h-9 w-9 items-center justify-center rounded-xl bg-yellow-50 font-bold text-yellow-500"
                  >
                    ✓
                  </div>

                  <span class="text-sm font-semibold text-slate-700">
                    Dépôt en ligne
                  </span>
                </div>

                <div class="flex items-center gap-3">
                  <div
                    class="flex h-9 w-9 items-center justify-center rounded-xl bg-yellow-50 font-bold text-yellow-500"
                  >
                    ↗
                  </div>

                  <span class="text-sm font-semibold text-slate-700">
                    Suivi en temps réel
                  </span>
                </div>

                <div class="flex items-center gap-3">
                  <div
                    class="flex h-9 w-9 items-center justify-center rounded-xl bg-yellow-50 font-bold text-yellow-500"
                  >
                    🔒
                  </div>

                  <span class="text-sm font-semibold text-slate-700">
                    Données sécurisées
                  </span>
                </div>

              </div>

            </div>

            <!-- Petit cercle -->
            <div
              class="absolute -bottom-6 -left-5 h-14 w-14 rounded-full border-[8px] border-yellow-400 bg-white"
            ></div>

          </div>

        </div>
      </section>


      <!-- ================= VALEURS ================= -->
      <section class="bg-[#fffdf7] py-20 lg:py-24">

        <div class="mx-auto max-w-[1200px] px-6">

          <div class="text-center">

            <p
              class="text-xs font-bold uppercase tracking-widest text-yellow-500"
            >
              Nos engagements
            </p>

            <h2
              class="mt-3 text-3xl font-extrabold text-slate-900 lg:text-4xl"
            >
              Une expérience pensée pour le citoyen.
            </h2>

            <p
              class="mx-auto mt-4 max-w-2xl text-slate-600"
            >
              Chaque fonctionnalité est pensée pour rendre vos
              démarches plus simples et plus accessibles.
            </p>

          </div>


          <div class="mt-12 grid gap-5 md:grid-cols-3">

            <!-- CARD -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 shadow-sm transition duration-300 hover:-translate-y-2 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                ⚡
              </div>

              <h3 class="mt-6 text-lg font-bold text-slate-900">
                Simplicité
              </h3>

              <p class="mt-3 text-sm leading-7 text-slate-500">
                Une interface claire pour effectuer vos démarches
                sans complexité.
              </p>

            </div>


            <!-- CARD -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 shadow-sm transition duration-300 hover:-translate-y-2 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                🔒
              </div>

              <h3 class="mt-6 text-lg font-bold text-slate-900">
                Sécurité
              </h3>

              <p class="mt-3 text-sm leading-7 text-slate-500">
                Vos informations et documents sont traités
                de manière sécurisée.
              </p>

            </div>


            <!-- CARD -->
            <div
              class="group rounded-2xl border border-slate-100 bg-white p-7 shadow-sm transition duration-300 hover:-translate-y-2 hover:border-yellow-200 hover:shadow-xl"
            >

              <div
                class="flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-50 text-2xl text-yellow-500 transition group-hover:bg-yellow-400 group-hover:text-white"
              >
                ◉
              </div>

              <h3 class="mt-6 text-lg font-bold text-slate-900">
                Transparence
              </h3>

              <p class="mt-3 text-sm leading-7 text-slate-500">
                Consultez facilement l'état d'avancement
                de vos courriers.
              </p>

            </div>

          </div>

        </div>
      </section>


      <!-- ================= COMMENT ÇA MARCHE ================= -->
      <section class="bg-white py-20 lg:py-24">

        <div class="mx-auto max-w-[1200px] px-6">

          <div class="text-center">

            <p
              class="text-xs font-bold uppercase tracking-widest text-yellow-500"
            >
              Une démarche simple
            </p>

            <h2
              class="mt-3 text-3xl font-extrabold text-slate-900 lg:text-4xl"
            >
              Tout se fait en quelques étapes.
            </h2>

          </div>


          <div class="relative mt-14 grid gap-10 md:grid-cols-3">

            <!-- ÉTAPE 1 -->
            <div class="text-center">

              <div
                class="mx-auto flex h-20 w-20 items-center justify-center rounded-full border-4 border-yellow-200 bg-white text-2xl shadow-sm"
              >
                01
              </div>

              <h3 class="mt-5 text-xl font-bold text-slate-900">
                Créez votre compte
              </h3>

              <p class="mt-3 text-sm leading-7 text-slate-500">
                Inscrivez-vous rapidement pour accéder
                à votre espace personnel.
              </p>

            </div>


            <!-- ÉTAPE 2 -->
            <div class="text-center">

              <div
                class="mx-auto flex h-20 w-20 items-center justify-center rounded-full border-4 border-yellow-200 bg-white text-2xl shadow-sm"
              >
                02
              </div>

              <h3 class="mt-5 text-xl font-bold text-slate-900">
                Déposez votre courrier
              </h3>

              <p class="mt-3 text-sm leading-7 text-slate-500">
                Remplissez votre demande et ajoutez
                vos documents.
              </p>

            </div>


            <!-- ÉTAPE 3 -->
            <div class="text-center">

              <div
                class="mx-auto flex h-20 w-20 items-center justify-center rounded-full border-4 border-yellow-200 bg-white text-2xl shadow-sm"
              >
                03
              </div>

              <h3 class="mt-5 text-xl font-bold text-slate-900">
                Suivez votre demande
              </h3>

              <p class="mt-3 text-sm leading-7 text-slate-500">
                Consultez l'évolution de votre courrier
                jusqu'à la réponse.
              </p>

            </div>

          </div>

        </div>
      </section>


      <!-- ================= CTA ================= -->
      <section class="bg-[#fffdf7] px-6 py-20">

        <div
          class="mx-auto max-w-5xl rounded-3xl bg-slate-900 px-8 py-14 text-center shadow-xl lg:px-16"
        >

          <div
            class="mx-auto flex h-14 w-14 items-center justify-center rounded-2xl bg-yellow-400 p-2"
          >
            <img
              :src="logo"
              alt="Logo"
              class="h-full w-full object-contain"
            />
          </div>

          <h2
            class="mt-6 text-3xl font-extrabold text-white lg:text-4xl"
          >
            Prêt à simplifier vos démarches ?
          </h2>

          <p
            class="mx-auto mt-4 max-w-2xl leading-7 text-slate-300"
          >
            Créez votre compte et commencez à gérer
            vos courriers directement en ligne.
          </p>

          <RouterLink
            to="/register"
            class="mt-8 inline-flex rounded-xl bg-yellow-400 px-7 py-4 font-bold text-slate-900 transition hover:bg-yellow-500 hover:shadow-lg"
          >
            Créer mon compte
          </RouterLink>

        </div>

      </section>

    </main>
    <footer class="border-t border-slate-100 bg-white">

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


<script setup>
import logo from '@/assets/logo.png'
</script>