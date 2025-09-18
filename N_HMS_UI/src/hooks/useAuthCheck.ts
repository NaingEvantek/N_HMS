// src/hooks/useAuthCheck.ts
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./useAuth";
import { JwtPayload, getUserFromToken } from "../utils/jwt";

export const useAuthCheck = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    try {
      const user = getUserFromToken(token); 
      if (!user) {
        logout();
        navigate("/login");
        return;
      }

      const decoded = getUserFromToken(token); // if you still need decoded
      const currentTime = Date.now() / 1000;

      if ((decoded as JwtPayload).exp < currentTime) {
        // Token expired
        logout();
        navigate("/login");
      }
    } catch (error) {
      logout();
      navigate("/login");
    }
  }, [navigate, logout]);
};
