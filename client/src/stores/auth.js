import { defineStore } from "pinia";
import { authApi } from "@/services/api";
export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: localStorage.getItem("token") || null,

    user: JSON.parse(
      localStorage.getItem("user") || "null"
    ),
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
  },

  actions: {
  
    async register(data) {
      const response = await authApi.register(data);
      return response;
    },

    async login(data) {
      const response = await authApi.login(data);

      this.setAuth(response);

      return response;
    },
  
    setAuth(response) {
      this.token = response.token;

      this.user = {
        id: response.id,
        nom: response.nom,
        prenom: response.prenom,
        email: response.email,
        telephone: response.telephone || null,
      };

      localStorage.setItem("token", this.token);

      localStorage.setItem(
        "user",
        JSON.stringify(this.user)
      );
    },
    logout() {
      this.token = null;
      this.user = null;

      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },

    
  },
});