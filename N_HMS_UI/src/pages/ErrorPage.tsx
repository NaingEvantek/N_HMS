import { Box, Heading, Text } from "@chakra-ui/react";
import { isRouteErrorResponse, useRouteError } from "react-router-dom";
import NavBar from "../components/NavBar";

const ErrorPage = () => {
  const error = useRouteError();

  let title = "Oops";
  let message = "An unexpected error occurred.";

  if (isRouteErrorResponse(error)) {
    // Handle 404
    if (error.status === 404) {
      message = "This page does not exist.";
    }
    // Handle 401 Unauthorized
    else if (error.status === 401) {
      message = "You are not authorized to view this page.";
    }
    // Handle 403 Forbidden
    else if (error.status === 403) {
      message = "Access forbidden.";
    }
    // Other known HTTP errors
    else {
      message = error.statusText || message;
    }
  } else if (error instanceof Error) {
    message = error.message;
  }

  return (
    <>
      <NavBar />
      <Box padding={5}>
        <Heading>{title}</Heading>
        <Text>{message}</Text>
      </Box>
    </>
  );
};

export default ErrorPage;
