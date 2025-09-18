import { create } from "zustand";
import { getUserFromToken } from "../utils/jwt";

interface User {
  username: string;
  role: string;
}

interface AuthState {
  user: User | null;
  token: string | null;
  setAuth: (user: User, token: string) => void;
  clearAuth: () => void;
}

const storedToken = localStorage.getItem("token");
const decodedUser = storedToken ? getUserFromToken(storedToken) : null;

const useAuthStore = create<AuthState>((set) => ({
  token: storedToken,
  user: decodedUser
    ? { username: decodedUser.unique_name, role: decodedUser.role }
    : null,
  setAuth: (user, token) => set({ user, token }),
  clearAuth: () => set({ user: null, token: null }),
}));

export default useAuthStore;
