// src/hooks/useAuth.ts
import { useEffect } from "react";
import APIClient from "../services/api-client";
import { AuthResponse } from "../entities/User";
import useAuthStore from "../store/authStore";
import { getUserFromToken, JwtPayload } from "../utils/jwt";

const apiClient = new APIClient<AuthResponse>("/Auth/login");

export function useAuth() {
  const { user, token, setAuth, clearAuth } = useAuthStore();

  // Initialize auth state from token only
  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    if (storedToken) {
      const decoded = getUserFromToken(storedToken);
      if (decoded) {
        setAuth(
          { username: decoded.unique_name, role: decoded.role },
          storedToken
        );
      } else {
        clearAuth();
        localStorage.removeItem("token");
      }
    }
  }, [setAuth, clearAuth]);

  const login = async (username: string, password: string) => {
    try {
      const response = await apiClient.post({ username, password });
      const decoded = getUserFromToken(response.token);
      if (!decoded) throw new Error("Invalid token received from server");

      setAuth(
        { username: decoded.unique_name, role: decoded.role },
        response.token
      );
      localStorage.setItem("token", response.token);
    } catch (error) {
      console.error("Login failed:", error);
      throw error;
    }
  };

  const logout = () => {
    clearAuth();
    localStorage.removeItem("token");
  };

  const isAuthenticated = !!token;

  return { user, token, login, logout, isAuthenticated };
}
