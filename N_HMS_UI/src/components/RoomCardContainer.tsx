import { Box } from "@chakra-ui/react";
import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

const RoomCardContainer = ({ children }: Props) => {
  return (
    <Box
      overflow="hidden"
      borderRadius={10}
      _hover={
        {
          // transform: "scale(1.03)",
          // transaction: "transform .15s ease-in",
        }
      }
    >
      {children}
    </Box>
  );
};

export default RoomCardContainer;
