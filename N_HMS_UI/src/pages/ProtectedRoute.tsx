import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { useAuthCheck } from "../hooks/useAuthCheck";

export function ProtectedRoute() {
  const { isAuthenticated } = useAuth();
  useAuthCheck();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  return <Outlet />;
}
