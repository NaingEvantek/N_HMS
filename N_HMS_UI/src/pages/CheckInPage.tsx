import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useRoomMutations } from "../hooks/useRoomMutations";
import useRooms from "../hooks/useRooms";
import RoomInfo, {
  RoomCheckInGuest,
  RoomCheckInRequest,
} from "../entities/Room";
import {
  VStack,
  HStack,
  Input,
  Select,
  NumberInput,
  NumberInputField,
  NumberInputStepper,
  NumberIncrementStepper,
  NumberDecrementStepper,
  Button,
  Text,
  useToast,
  FormLabel,
  IconButton,
  Box,
  SimpleGrid,
} from "@chakra-ui/react";
import { AiOutlineArrowLeft } from "react-icons/ai";

const CheckInPage: React.FC = () => {
  const { roomId } = useParams<{ roomId: string }>();
  const { data: roomsData } = useRooms();
  const room = roomsData?.pages
    .flatMap((p) => p.results)
    .find((r) => r.id === Number(roomId));

  const { checkIn } = useRoomMutations();
  const navigate = useNavigate();
  const toast = useToast();

  const [numDays, setNumDays] = useState(1);
  const [paymentStatusId, setPaymentStatusId] = useState(2);
  const [totalAmount, setTotalAmount] = useState(0);
  const [paidAmount, setPaidAmount] = useState(0);
  const [guests, setGuests] = useState<RoomCheckInGuest[]>([]);

  // Initialize guests array based on room capacity
  useEffect(() => {
    if (room) {
      const adult_capacity = room.room_Capacity_Adult ?? 1;
      const child_capacity = room.room_Capacity_Child ?? 0;
      const capacity = adult_capacity + child_capacity;
      setGuests(
        Array.from({ length: capacity }, () => ({
          guestName: "",
          passportNo: "",
          genderId: undefined,
        }))
      );
    }
  }, [room]);

  // Auto-calculate total amount
  useEffect(() => {
    if (room) {
      setTotalAmount((room.price_Per_Day ?? 0) * numDays);
      setPaidAmount((room.price_Per_Day ?? 0) * numDays);
    }
  }, [room, numDays]);

  const handleGuestChange = (
    index: number,
    field: keyof RoomCheckInGuest,
    value: string | number
  ) => {
    setGuests((currentGuests) =>
      currentGuests.map((guest, i) =>
        i === index ? { ...guest, [field]: value } : guest
      )
    );
  };

  const handleSubmit = async () => {
    if (!room) return;

    // Validate guests
    for (const guest of guests) {
      if (!guest.guestName || !guest.passportNo) {
        toast({ title: "All guest info is required", status: "warning" });
        return;
      }
    }

    const payload: RoomCheckInRequest = {
      roomId: room.id,
      paymentStatusId,
      numOfGuests: guests.length,
      numOfDays: numDays,
      totalAmount: totalAmount,
      paidAmount: paidAmount,
      guests: guests,
    };

    try {
      await checkIn.mutateAsync(payload);
      toast({ title: "Check-In successful", status: "success" });
      navigate("/"); // go back to room list
    } catch (err: any) {
      toast({
        title: "Check-In failed",
        description: err?.response?.data?.message || err.message,
        status: "error",
      });
    }
  };

  if (!room) return <Text>Room not found</Text>;

  return (
    <VStack
      spacing={4}
      padding={4}
      align="stretch"
      h="100vh" // full height for button positioning
    >
      {/* Header */}
      <HStack justifyContent="space-between" w="100%">
        <Text fontSize="xl" fontWeight="bold">
          Check-In: {room.room_Name}
        </Text>
        <IconButton
          aria-label="Back"
          bg="red.500"
          color="white"
          _hover={{ bg: "red.600" }}
          icon={<AiOutlineArrowLeft />}
          colorScheme="red"
          onClick={() => navigate(-1)}
        />
      </HStack>

      {/* Columns */}
      <SimpleGrid
        columns={2}
        spacing={6}
        w="100%"
        templateColumns="70% 30%"
        flex="1"
      >
        {/* Left Column: Guest Info Group Box */}
        <Box borderWidth="1px" borderRadius="md" p={4} w="100%">
          <Text fontWeight="bold" mb={3} textDecoration={"underline"}>
            Guest Information
          </Text>
          <VStack
            align="stretch"
            spacing={3}
            w="100%"
            maxH="calc(100vh - 200px)"
            overflowY="auto"
          >
            {guests.map((guest, index) => (
              <HStack key={index} spacing={3} w="100%">
                <VStack align="start" spacing={1} flex="2" w="100%">
                  <FormLabel margin={0}>Guest {index + 1} Name</FormLabel>
                  <Input
                    placeholder={`Guest ${index + 1} Name`}
                    value={guest.guestName}
                    onChange={(e) =>
                      handleGuestChange(index, "guestName", e.target.value)
                    }
                    w="100%"
                  />
                </VStack>
                <VStack align="start" spacing={1} flex="2" w="100%">
                  <FormLabel margin={0}>Passport Number</FormLabel>
                  <Input
                    placeholder="Passport Number"
                    value={guest.passportNo}
                    onChange={(e) =>
                      handleGuestChange(index, "passportNo", e.target.value)
                    }
                    w="100%"
                  />
                </VStack>
                <VStack align="start" spacing={1} flex="1" w="100%">
                  <FormLabel margin={0}>Gender</FormLabel>
                  <Select
                    value={guest.genderId || ""}
                    onChange={(e) =>
                      handleGuestChange(
                        index,
                        "genderId",
                        Number(e.target.value)
                      )
                    }
                    w="100%"
                  >
                    <option value="">Gender</option>
                    <option value={1}>Male</option>
                    <option value={2}>Female</option>
                    <option value={3}>Other</option>
                  </Select>
                </VStack>
              </HStack>
            ))}
          </VStack>
        </Box>

        {/* Left Column */}
        <VStack align="start" spacing={4} w="100%" paddingRight={4}>
          <VStack align="start" spacing={1} w="100%">
            <FormLabel htmlFor="numDays" margin={0}>
              Number of Days
            </FormLabel>
            <NumberInput
              id="numDays"
              min={1}
              value={numDays}
              w="100%"
              onChange={(val) => setNumDays(Number(val))}
            >
              <NumberInputField w="100%" />
              <NumberInputStepper>
                <NumberIncrementStepper />
                <NumberDecrementStepper />
              </NumberInputStepper>
            </NumberInput>
          </VStack>

          <VStack align="start" spacing={1} w="100%">
            <FormLabel htmlFor="paymentStatus" margin={0}>
              Payment Status
            </FormLabel>
            <Select
              id="paymentStatus"
              value={paymentStatusId}
              onChange={(e) => setPaymentStatusId(Number(e.target.value))}
              w="100%"
            >
              <option value={2}>Pending</option>
              <option value={1}>Paid</option>
            </Select>
          </VStack>

          <VStack align="start" spacing={1} w="100%">
            <FormLabel htmlFor="totalAmount" margin={0}>
              Total Amount
            </FormLabel>
            <NumberInput
              id="totalAmount"
              w="100%"
              value={totalAmount}
              isReadOnly
            >
              <NumberInputField w="100%" />
            </NumberInput>
          </VStack>

          <Button colorScheme="green" onClick={handleSubmit} w="100%">
            Check In
          </Button>
        </VStack>
      </SimpleGrid>

      {/* Check In Button */}
    </VStack>
  );
};

export default CheckInPage;
