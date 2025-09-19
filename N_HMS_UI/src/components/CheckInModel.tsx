import React, { useState, useEffect } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  Button,
  FormControl,
  FormLabel,
  Input,
  NumberInput,
  NumberInputField,
  NumberInputStepper,
  NumberIncrementStepper,
  NumberDecrementStepper,
  VStack,
  HStack,
  Select,
  useToast,
} from "@chakra-ui/react";
import RoomInfo, {
  RoomCheckInGuest,
  RoomCheckInRequest,
} from "../entities/Room";
import { useRoomMutations } from "../hooks/useRoomMutations";

interface CheckInModalProps {
  isOpen: boolean;
  onClose: () => void;
  room: RoomInfo | null;
}

const CheckInModal: React.FC<CheckInModalProps> = ({
  isOpen,
  onClose,
  room,
}) => {
  const toast = useToast();
  const { checkIn } = useRoomMutations();

  const [numDays, setNumDays] = useState(1);
  const [numOfGuests, setNumOfGuests] = useState(1);
  const [paymentStatusId, setPaymentStatusId] = useState(1); // default Paid
  const [totalAmount, setTotalAmount] = useState(0);
  const [paidAmount, setPaidAmount] = useState(0);
  const [guests, setGuests] = useState<RoomCheckInGuest[]>([]);

  useEffect(() => {
    if (room) {
      const adult_capacity = room.room_Capacity_Adult ?? 1;
      const child_capacity = room.room_Capacity_Child ?? 0;
      const capacity = adult_capacity + child_capacity;
      setNumOfGuests(capacity);
      setGuests(
        Array.from({ length: capacity }, () => ({
          guestName: "",
          passportNo: "",
          genderId: undefined,
        }))
      );
      setNumDays(1);
    }
  }, [room]);

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

    for (const guest of guests) {
      if (!guest.guestName || !guest.passportNo) {
        toast({ title: "All guest info is required", status: "warning" });
        return;
      }
    }

    const payload: RoomCheckInRequest = {
      roomId: room.id,
      paymentStatusId,
      numOfGuests,
      numOfDays: numDays,
      totalAmount,
      paidAmount,
      guests,
    };

    try {
      await checkIn.mutateAsync(payload);
      toast({ title: "Check-In successful", status: "success" });
      onClose();
      setGuests([]);
      setNumDays(1);
      setTotalAmount(0);
      setPaidAmount(0);
    } catch (err: any) {
      toast({
        title: "Check-In failed",
        description: err?.response?.data?.message || err.message,
        status: "error",
      });
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl">
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Check-In: {room?.room_Name}</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}>
            <FormLabel>Number of Days</FormLabel>
            <NumberInput
              min={1}
              value={numDays}
              onChange={(val) => setNumDays(Number(val))}
            >
              <NumberInputField />
              <NumberInputStepper>
                <NumberIncrementStepper />
                <NumberDecrementStepper />
              </NumberInputStepper>
            </NumberInput>
          </FormControl>

          <FormControl mb={3}>
            <FormLabel>Payment Status</FormLabel>
            <Select
              value={paymentStatusId}
              onChange={(e) => setPaymentStatusId(Number(e.target.value))}
            >
              <option value={1}>Paid</option>
              <option value={2}>Pending</option>
            </Select>
          </FormControl>

          <FormControl mb={3}>
            <FormLabel>Total Amount</FormLabel>
            <NumberInput
              min={0}
              value={totalAmount}
              onChange={(val) => setTotalAmount(Number(val))}
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl mb={3}>
            <FormLabel>Paid Amount</FormLabel>
            <NumberInput
              min={0}
              value={paidAmount}
              onChange={(val) => setPaidAmount(Number(val))}
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <VStack spacing={3} mt={4}>
            {guests.map((guest, index) => (
              <HStack key={index} w="100%" spacing={3}>
                <FormControl>
                  <FormLabel>Guest {index + 1} Name</FormLabel>
                  <Input
                    value={guest.guestName}
                    onChange={(e) =>
                      handleGuestChange(index, "guestName", e.target.value)
                    }
                    placeholder="Enter guest name"
                  />
                </FormControl>
                <FormControl>
                  <FormLabel>Passport Number</FormLabel>
                  <Input
                    value={guest.passportNo}
                    onChange={(e) =>
                      handleGuestChange(index, "passportNo", e.target.value)
                    }
                    placeholder="Enter passport number"
                  />
                </FormControl>
                <FormControl>
                  <FormLabel>Gender</FormLabel>
                  <Select
                    value={guest.genderId || ""}
                    onChange={(e) =>
                      handleGuestChange(
                        index,
                        "genderId",
                        Number(e.target.value)
                      )
                    }
                  >
                    <option value="">Select</option>
                    <option value={1}>Male</option>
                    <option value={2}>Female</option>
                    <option value={3}>Other</option>
                  </Select>
                </FormControl>
              </HStack>
            ))}
          </VStack>
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="green" mr={3} onClick={handleSubmit}>
            Submit
          </Button>
          <Button variant="ghost" onClick={onClose}>
            Cancel
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default CheckInModal;
