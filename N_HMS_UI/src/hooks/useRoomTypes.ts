import { RoomTypeInfo } from "./../entities/Room";
import { useQuery } from "@tanstack/react-query";
import ms from "ms";
import APIClient from "../services/api-client";

const apiClient = new APIClient<RoomTypeInfo>("/room/room-types");

const useRoomTypes = () =>
  useQuery({
    queryKey: ["room-types"],
    queryFn: apiClient.getFilter,
    staleTime: ms("24h"),
  });

export default useRoomTypes;
