import { Box, HStack, Image } from "@chakra-ui/react";
import logo from "../assets/logo.webp";
import ColorModeSwitch from "./ColorModeSwitch";
import { Link } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

const NavBar = () => {
  const { isAuthenticated } = useAuth();
  return (
    <HStack padding="10px">
      <Link to="/">
        <Image src={logo} boxSize="60px" objectFit="cover" />
      </Link>
      {isAuthenticated && (
        <>
          <ColorModeSwitch />
        </>
      )}
    </HStack>
  );
};

export default NavBar;
