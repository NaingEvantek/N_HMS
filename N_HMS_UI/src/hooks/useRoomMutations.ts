import { useMutation, useQueryClient } from "@tanstack/react-query";
import APIClient from "../services/api-client";
import RoomInfo, { RoomCheckInRequest } from "../entities/Room";

const roomApi = new APIClient<RoomInfo>("room");

export const useRoomMutations = () => {
  const queryClient = useQueryClient();

  const createRoom = useMutation(
    (data: Partial<RoomInfo>) => roomApi.create(data),
    { onSuccess: () => queryClient.invalidateQueries(["rooms"]) }
  );

  const updateRoom = useMutation(
    (data: Partial<RoomInfo> & { id: number }) => roomApi.update(data),
    { onSuccess: () => queryClient.invalidateQueries(["rooms"]) }
  );

  const checkIn = useMutation(
    (data: RoomCheckInRequest) => roomApi.postTo("checkin", data),
    { onSuccess: () => queryClient.invalidateQueries(["rooms"]) }
  );

  const checkOut = useMutation(
    (roomId: number) => roomApi.postTo(`checkout?room_id=${roomId}`, {}),
    { onSuccess: () => queryClient.invalidateQueries(["rooms"]) }
  );

  const completeCleaning = useMutation(
    (roomId: number) => roomApi.postTo(`clean?room_id=${roomId}`, {}),
    { onSuccess: () => queryClient.invalidateQueries(["rooms"]) }
  );

  return { createRoom, updateRoom, checkIn, checkOut, completeCleaning };
};
