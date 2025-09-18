import { useInfiniteQuery } from "@tanstack/react-query";
import APIClient, { FetchResponse } from "../services/api-client";
import Room from "../entities/Room";
import ms from "ms";
import useRoomQueryStore from "../store/useRoomQueryStore";
import RoomInfo from "../entities/Room";

const apiClient = new APIClient<Room>("/room/search");

// export const useRooms = (req: QueryRequest) =>
//   useQuery<FetchResponse<Room>, Error>({
//     queryKey: ["booking-rooms", req],
//     queryFn: () => apiClient.list(req),
//     keepPreviousData: true,
//   });

const useRooms = () => {
  const roomQuery = useRoomQueryStore((s) => s.roomQuery);
  return useInfiniteQuery<FetchResponse<RoomInfo>, Error>({
    queryKey: ["rooms", roomQuery],
    queryFn: ({ pageParam = 1 }) =>
      apiClient.getAll({
        params: {
          floorId: roomQuery.floorId,
          roomtypeId: roomQuery.roomtypeId,
          roomstatusId: roomQuery.roomstatusId,
          search: roomQuery.searchText,
          orderby: roomQuery.roomstatusId,
          page: pageParam,
        },
      }),
    getNextPageParam: (lastPage, allPages) => {
      return lastPage.next ? allPages.length + 1 : undefined;
    },
    staleTime: ms("24h"),
  });
};

export default useRooms;
