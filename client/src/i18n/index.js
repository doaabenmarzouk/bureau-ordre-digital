// src/i18n/index.js
import { createI18n } from 'vue-i18n'
import fr from './locales/fr.json'
import ar from './locales/ar.json'

const savedLocale = localStorage.getItem('locale') || 'fr'

const i18n = createI18n({
  legacy: false,        // nécessaire pour utiliser useI18n() dans <script setup>
  locale: savedLocale,
  fallbackLocale: 'fr',
  messages: { fr, ar }
})

export default i18n

/**
 * Change la langue active + adapte la direction du document (RTL pour l'arabe).
 * À appeler depuis le sélecteur de langue.
 */
export function setLocale(locale) {
  i18n.global.locale.value = locale
  localStorage.setItem('locale', locale)

  document.documentElement.setAttribute('lang', locale)
  document.documentElement.setAttribute('dir', locale === 'ar' ? 'rtl' : 'ltr')
}