import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import {
  Box,
  Button,
  Input,
  FormControl,
  FormLabel,
  Heading,
  Text,
  VStack,
} from "@chakra-ui/react";

const LoginPage = () => {
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/");
    }
  }, [isAuthenticated, navigate]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    login(email, password);
    navigate("/");
  };

  return (
    <Box
      maxW="md"
      mx="auto"
      mt={20}
      p={8}
      borderWidth={1}
      borderRadius="lg"
      boxShadow="lg"
      bg="white"
    >
      <Heading mb={6} textAlign="center" color={"gray.900"}>
        Login
      </Heading>

      {error && (
        <Text color="red.500" mb={4} textAlign="center">
          {error}
        </Text>
      )}

      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="email">
            <FormLabel color={"gray.900"}>Email</FormLabel>
            <Input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Enter your email"
              variant="filled"
              color="gray.900"
              borderColor={"gray.300"}
              _placeholder={{ color: "gray.500" }}
              autoComplete="new-password"
              required
            />
          </FormControl>

          <FormControl id="password">
            <FormLabel color={"gray.900"}>Password</FormLabel>
            <Input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              color="gray.900"
              borderColor={"gray.300"}
              _placeholder={{ color: "gray.500" }}
              variant="filled"
              required
            />
          </FormControl>

          <Button colorScheme="blue" type="submit" width="full" mt={2}>
            Login
          </Button>
        </VStack>
      </form>
    </Box>
  );
};

export default LoginPage;
