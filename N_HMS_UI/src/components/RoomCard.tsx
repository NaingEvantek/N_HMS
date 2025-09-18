import React from "react";
import {
  Box,
  Heading,
  Text,
  HStack,
  VStack,
  Button,
  Badge,
} from "@chakra-ui/react";
import RoomInfo from "../entities/Room";

interface RoomCardProps {
  room: RoomInfo;
  onCheckIn: (room: RoomInfo) => void;
  onCheckOut: (room: RoomInfo) => void;
  onCompleteCleaning: (room: RoomInfo) => void;
}

const RoomCard: React.FC<RoomCardProps> = ({
  room,
  onCheckIn,
  onCheckOut,
  onCompleteCleaning,
}) => {
  // 🔹 Status-based colors
  let statusColor = "green.400";
  if (room.room_Status?.status === "Cleaning") statusColor = "yellow.400";
  if (room.room_Status?.status === "Occupied") statusColor = "red.400";

  return (
    <Box
      borderRadius="md"
      shadow="md"
      overflow="hidden"
      border="1px"
      borderColor="gray.600"
      p={4}
      bg="gray.700"
    >
      <Heading
        size="md"
        mb={2}
        px={2}
        py={1}
        fontSize={"1rem"}
        borderRadius="md"
        bg={statusColor}
        color="black"
        textAlign="center"
      >
        {room.room_Name}
      </Heading>

      <VStack spacing={1} align="start" mb={3}>
        <HStack>
          <Text fontSize={"2xs"} fontWeight="bold" margin={0}>
            Status:
          </Text>
          <Badge
            fontSize={"2xs"}
            colorScheme={
              room.room_Status?.status === "Free"
                ? "green"
                : room.room_Status?.status === "Cleaning"
                ? "yellow"
                : "red"
            }
          >
            {room.room_Status?.status || "Free"}
          </Badge>
        </HStack>
        <Text fontSize={"2xs"} margin={0}>
          Room Type : {room.room_Type?.name ?? ""}
        </Text>
        <Text fontSize={"2xs"} margin={0}>
          Capacity (adult,child) : {room.room_Capacity_Adult ?? 0},{" "}
          {room.room_Capacity_Child ?? 0}
        </Text>
      </VStack>

      <HStack spacing={2} wrap="wrap">
        <Button
          size="sm"
          colorScheme="green"
          onClick={() => onCheckIn(room)}
          w="100%"
          display={room.room_Status?.status !== "Free" ? "none" : "block"}
        >
          Check In
        </Button>

        <Button
          size="sm"
          colorScheme="red"
          onClick={() => onCheckOut(room)}
          w="100%"
          display={room.room_Status?.status !== "Occupied" ? "none" : "block"}
        >
          Check Out
        </Button>

        {room.room_Status?.status === "Cleaning" && (
          <Button
            size="sm"
            colorScheme="blue"
            w="100%"
            onClick={() => onCompleteCleaning(room)}
          >
            Complete Cleaning
          </Button>
        )}
      </HStack>
    </Box>
  );
};

export default RoomCard;
