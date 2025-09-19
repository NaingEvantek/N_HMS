import { useQuery } from "@tanstack/react-query";
import ms from "ms";
import APIClient from "../services/api-client";
import FloorInfo from "../entities/Floor";
const apiClient = new APIClient<FloorInfo>("/floor/all");

const useFloors = () =>
  useQuery({
    queryKey: ["floors"],
    queryFn: apiClient.getFilter,
    staleTime: ms("24h"),
  });

export default useFloors;
