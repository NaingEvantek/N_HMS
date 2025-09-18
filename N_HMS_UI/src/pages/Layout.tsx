import React from "react";
import NavBar from "../components/NavBar";
import { Outlet } from "react-router-dom";
import { Box } from "@chakra-ui/react";
import { useAuth } from "../hooks/useAuth";

const Layout = () => {
  const { isAuthenticated } = useAuth();
  return (
    <>
      {isAuthenticated && <NavBar />}

      <Box paddingTop={5} h={isAuthenticated ? "90vh" : "100vh"}>
        <Outlet />
      </Box>
    </>
  );
};

export default Layout;
