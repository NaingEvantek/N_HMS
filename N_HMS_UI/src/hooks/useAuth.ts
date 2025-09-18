import APIClient from "../services/api-client";
import UserAuth from "../entities/User";
import { AuthResponse } from "../entities/User";
import useAuthStore from "../store/authStore";
import { useEffect } from "react";
const apiClient = new APIClient<AuthResponse>("/Auth/login");
export function useAuth() {
  const { user, token, setAuth, clearAuth } = useAuthStore();

  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    const storedUsername = localStorage.getItem("username");
    const storedRole = localStorage.getItem("role");

    if (storedToken && storedUsername && storedRole) {
      setAuth({ username: storedUsername, role: storedRole }, storedToken);
    }
  }, [setAuth]);

  const Authenticate = (
    user: { username: string; role: string },
    token: string
  ) => {
    setAuth(user, token);
    // save separately
    localStorage.setItem("token", token);
    localStorage.setItem("username", user.username);
    localStorage.setItem("role", user.role);
  };

  const login = async (username: string, password: string) => {
    try {
      const response = await apiClient.post({ username, password });
      Authenticate(
        { username: response.username, role: response.role },
        response.token
      );
    } catch (error) {
      console.error("Login failed:", error);
      throw error;
    }
  };

  const logout = () => {
    clearAuth();
    localStorage.removeItem("token");
    localStorage.removeItem("username");
    localStorage.removeItem("role");
  };

  const isAuthenticated = !!token;

  return { user, token, Authenticate, login, logout, isAuthenticated };
}
