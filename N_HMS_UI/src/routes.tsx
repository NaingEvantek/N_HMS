import { createBrowserRouter } from "react-router-dom";
import ErrorPage from "./pages/ErrorPage";
import HomePage from "./pages/HomePage";
import Layout from "./pages/Layout";
import LoginPage from "./pages/LoginPage";
import { ProtectedRoute } from "./pages/ProtectedRoute";
import RoomArea from "./pages/RoomArea";
import FloorPage from "./pages/FloorPage";
import Booking from "./pages/Booking";
import RoomGrid from "./components/RoomGrid";
import CheckInPage from "./pages/CheckInPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
      { path: "/login", element: <LoginPage /> },
      {
        element: <ProtectedRoute />,
        children: [
          {
            path: "/",
            element: <HomePage />,
            children: [
              { index: true, element: <RoomGrid /> },
              { path: "room", element: <RoomArea /> },
              { path: "floor", element: <FloorPage /> },
              { path: "checkin/:roomId", element: <CheckInPage /> },
            ],
          },
        ],
      },
    ],
  },
]);

export default router;
