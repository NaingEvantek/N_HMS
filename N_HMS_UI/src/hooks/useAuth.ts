import { authStore } from "../authStore";
import APIClient from "../services/api-client";

import UserAuth from "../entities/User";

const apiClient = new APIClient<UserAuth>('/games');

export function useAuth() {
  const { user, token, setAuth, clearAuth } = authStore();

  const Authenticate = (user: any, token: string) => {
    setAuth(user, token);
    localStorage.setItem("token", token);
    localStorage.setItem("user", JSON.stringify(user));
  };

  const login = (username:string,password:string)=>{

  }

  const logout = () => {
    clearAuth();
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  };

  const isAuthenticated = !!token;

  return { user, token,Authenticate, login, logout, isAuthenticated };
}
