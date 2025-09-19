import React, { useState } from "react";
import {
  Box,
  Heading,
  Text,
  HStack,
  VStack,
  Button,
  Badge,
  Spinner,
} from "@chakra-ui/react";

import RoomWithBookingDto, { RoomStatusEnum } from "../entities/Room";

interface RoomCardProps {
  room: RoomWithBookingDto;
  onCheckIn: (room: RoomWithBookingDto) => void;
  onCheckOut: (room: RoomWithBookingDto) => void;
  onCompleteCleaning: (room: RoomWithBookingDto) => void;
  isCheckingOut?: boolean;
  isCleaning?: boolean;
}

const RoomCard: React.FC<RoomCardProps> = ({
  room,
  onCheckIn,
  onCheckOut,
  onCompleteCleaning,
  isCheckingOut,
  isCleaning,
}) => {
  // 🔹 Status-based colors
  let statusColor = "green.400";
  if (room.roomInfo.room_Status?.status === RoomStatusEnum.Cleaning)
    statusColor = "yellow.400";
  if (room.roomInfo.room_Status?.status === RoomStatusEnum.Occupied)
    statusColor = "red.400";

  const [isLoading, setIsLoading] = useState(false);
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
        {room.roomInfo.room_Name}
      </Heading>

      <VStack spacing={1} align="start" mb={3}>
        <HStack>
          <Text fontSize={"2xs"} fontWeight="bold" margin={0}>
            Status:
          </Text>
          <Badge
            fontSize={"2xs"}
            colorScheme={
              room.roomInfo.room_Status?.status === RoomStatusEnum.Available
                ? "green"
                : room.roomInfo.room_Status?.status === RoomStatusEnum.Cleaning
                ? "yellow"
                : "red"
            }
          >
            {room.roomInfo.room_Status?.status || RoomStatusEnum.Available}
          </Badge>
        </HStack>
        <Text fontSize={"2xs"} margin={0}>
          Room Type : {room.roomInfo.room_Type?.name ?? ""}
        </Text>
        <Text fontSize={"2xs"} margin={0}>
          Capacity (adult,child) : {room.roomInfo.room_Capacity_Adult ?? 0},{" "}
          {room.roomInfo.room_Capacity_Child ?? 0}
        </Text>
        <Text fontSize={"2xs"} margin={0}>
          Price per day : {room.roomInfo.price_Per_Day ?? 0}
        </Text>
      </VStack>

      <HStack spacing={2} wrap="wrap">
        <Button
          size="sm"
          colorScheme="green"
          onClick={() => onCheckIn(room)}
          w="100%"
          display={
            room.roomInfo.room_Status?.status !== RoomStatusEnum.Available
              ? "none"
              : "block"
          }
        >
          Check In
        </Button>

        <Button
          size="sm"
          colorScheme="red"
          onClick={() => onCheckOut(room)}
          isDisabled={isCheckingOut}
          w="100%"
          display={
            room.roomInfo.room_Status?.status !== RoomStatusEnum.Occupied
              ? "none"
              : "block"
          }
        >
          {isCheckingOut ? <Spinner /> : "Check Out"}
        </Button>

        {room.roomInfo.room_Status?.status === RoomStatusEnum.Cleaning && (
          <Button
            size="sm"
            colorScheme="blue"
            w="100%"
            onClick={() => onCompleteCleaning(room)}
            isDisabled={isCleaning}
          >
            {isCleaning ? <Spinner size="sm" /> : "Complete Cleaning"}
          </Button>
        )}
      </HStack>
    </Box>
  );
};

export default RoomCard;
