// client/src/main.js

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'
import './assets/main.css' // adapte le nom si ton fichier CSS global s'appelle autrement

const app = createApp(App)

// ⚠️ Pinia DOIT être enregistré avant le router, car le navigation guard
// (router.beforeEach) appelle useAuthStore() dès la toute première navigation.
app.use(createPinia())
app.use(router)

app.mount('#app')
