import { create } from "zustand";

interface User {
    username: string;
    role:string;
  }

  interface AuthState {
    user: User | null;
    token: string | null;
    setAuth: (user: User, token: string) => void;
    clearAuth: () => void;
  }

  export const authStore = create<AuthState>((set) => ({
    user: null,
    token: null,
    setAuth: (user, token) => set({ user, token }),
    clearAuth: () => set({ user: null, token: null }),
  }));