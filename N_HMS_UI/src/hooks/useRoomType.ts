import useRoomTypes from "./useRoomTypes";

const useRoomType = (id?: number) => {
  const { data } = useRoomTypes();
  return data?.find((p) => p.id === id);
};

export default useRoomType;
