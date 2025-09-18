import { create } from "zustand";

interface RoomQuery {
  floorId?: number;
  roomtypeId?: number;
  roomstatusId?: number;
  sortOrder?: string;
  searchText?: string;
}

interface RoomQueryStore {
  roomQuery: RoomQuery;
  setSearchText: (searchText: string) => void;
  setFloorId: (floorId: number) => void;
  setRoomtypeId: (roomtypeId: number) => void;
  setRoomstatusId: (roomstatusId: number) => void;
  setSortOrder: (sortOrder: string) => void;
}

const useGameQueryStore = create<RoomQueryStore>((set) => ({
  roomQuery: {},
  setSearchText: (searchText) => set(() => ({ roomQuery: { searchText } })),
  setFloorId: (floorId) =>
    set((stroe) => ({ roomQuery: { ...stroe.roomQuery, floorId } })),
  setRoomtypeId: (roomtypeId) =>
    set((store) => ({ roomQuery: { ...store.roomQuery, roomtypeId } })),
  setRoomstatusId: (roomstatusId) =>
    set((store) => ({ roomQuery: { ...store.roomQuery, roomstatusId } })),
  setSortOrder: (sortOrder) =>
    set((store) => ({ roomQuery: { ...store.roomQuery, sortOrder } })),
}));

export default useGameQueryStore;
