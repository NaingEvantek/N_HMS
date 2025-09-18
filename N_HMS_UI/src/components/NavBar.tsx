import { Box, HStack, Image, Text, Button } from "@chakra-ui/react";
import logo from "../assets/logo.webp";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { FiLogOut } from "react-icons/fi";

const NavBar = () => {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <HStack
      paddingX={6}
      paddingY={3}
      h="10vh"
      justifyContent="space-between"
      alignItems="center"
      bg="gray.800" // match dark sidebar
      boxShadow="sm"
      position="sticky"
      top={0}
      zIndex={1000}
    >
      {/* Logo */}
      <Link to="/">
        <HStack spacing={3}>
          <Image src={logo} width="70px" objectFit="cover" />
          <Text fontWeight="bold" fontSize="lg" color="white" margin={0}>
            Hotel Management System
          </Text>
        </HStack>
      </Link>

      {/* Auth / Logout */}
      {isAuthenticated && (
        <HStack spacing={4}>
          <Text
            fontWeight="medium"
            color="gray.200"
            margin={0}
            cursor={"default"}
          >
            Welcome,{" "}
            <Text as="span" color="blue.400" cursor={"default"}>
              {user?.username}
            </Text>
          </Text>
          <Button
            colorScheme="red"
            size="sm"
            onClick={handleLogout}
            _hover={{ bg: "red.600" }}
          >
            <FiLogOut />
          </Button>
        </HStack>
      )}
    </HStack>
  );
};

export default NavBar;
