import useFloors from "./useFloors";

const useFloor = (id?: number) => {
  const { data } = useFloors();

  return data?.find((p) => p.id === id);
};

export default useFloor;
