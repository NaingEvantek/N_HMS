import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import logo from "../assets/logo.webp";
import {
  HStack,
  IconButton,
  Image,
  InputGroup,
  InputRightElement,
  Spinner,
} from "@chakra-ui/react";
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
import { FiEye, FiEyeOff } from "react-icons/fi";

const LoginPage = () => {
  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState(false);
  const [username, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const toggleShowPassword = () => setShowPassword(!showPassword);
  useEffect(() => {
    if (isAuthenticated) {
      navigate("/");
    }
  }, [isAuthenticated, navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      await login(username, password);
      navigate("/");
    } catch (err: any) {
      setError(err?.message || "Login failed");
    } finally {
      setIsLoading(false);
    }
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
      <Heading
        mb={6}
        textAlign="center"
        color={"gray.900"}
        justifyContent="center"
      >
        <HStack justify="center">
          <Image src={logo} height="75px" objectFit="cover" />
        </HStack>
      </Heading>

      {error && (
        <Text color="red.500" mb={4} textAlign="center">
          {error}
        </Text>
      )}

      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="username">
            <FormLabel color={"gray.900"}>User Name</FormLabel>
            <Input
              type="text"
              value={username}
              onChange={(e) => setUserName(e.target.value)}
              placeholder="Enter your UserName"
              variant="filled"
              color="gray.900"
              bg="white"
              borderColor={"gray.300"}
              _placeholder={{ color: "gray.500" }}
              autoComplete="off"
              disabled={isLoading}
              autoFocus
              required
            />
          </FormControl>

          <FormControl id="password">
            <FormLabel color={"gray.900"}>Password</FormLabel>
            <InputGroup>
              <Input
                type={showPassword ? "text" : "password"}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Enter your password"
                color="gray.900"
                bg="white"
                borderColor={"gray.300"}
                _placeholder={{ color: "gray.500" }}
                variant="filled"
                disabled={isLoading}
                required
              />
              <InputRightElement>
                <IconButton
                  aria-label={showPassword ? "Hide password" : "Show password"}
                  icon={showPassword ? <FiEyeOff /> : <FiEye />}
                  size="sm"
                  onClick={toggleShowPassword}
                  variant="ghost"
                  color={"gray.800"}
                />
              </InputRightElement>
            </InputGroup>
          </FormControl>

          <Button
            colorScheme="blue"
            type="submit"
            width="full"
            mt={2}
            disabled={isLoading}
            loadingText="Logging in..."
          >
            {isLoading ? <Spinner /> : "Login"}
          </Button>
        </VStack>
      </form>
    </Box>
  );
};

export default LoginPage;
