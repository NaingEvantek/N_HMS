// src/hooks/useAuth.ts
import { useEffect } from "react";
import APIClient, { axios } from "../services/api-client";
import { AuthResponse } from "../entities/User";
import useAuthStore from "../store/authStore";
import { getUserFromToken, JwtPayload } from "../utils/jwt";

export enum ErrorMessages {
  InvalidToken = "Invalid token received from server",
  LoginFailed = "Login failed",
  Unauthorized = "Unauthorized",
  Forbidden = "Access forbidden",
}

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
    } catch (error: any) {
      if (axios.isAxiosError(error) && error.response?.status === 401) {
        throw new Error(ErrorMessages.Unauthorized);
      } else if (axios.isAxiosError(error) && error.response?.status === 403) {
        throw new Error(ErrorMessages.Forbidden);
      }

      console.error(ErrorMessages.LoginFailed, error);
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
