import { Box, HStack, SimpleGrid, Spinner, Text } from "@chakra-ui/react";
import React from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import useRooms from "../hooks/useRooms";
import RoomCard from "./RoomCard";
import RoomCardContainer from "./RoomCardContainer";
import RoomCardSkeleton from "./RoomCardSkeleton";
import { useNavigate } from "react-router-dom";
import { useRoomMutations } from "../hooks/useRoomMutations";
import FloorSelector from "./FloorSelector";
import RoomTypeSelector from "./RoomTypeSelector";
import RoomWithBookingDto from "../entities/Room";

const RoomGrid = () => {
  const {
    data,
    error,
    isLoading,
    isFetchingNextPage,
    fetchNextPage,
    hasNextPage,
  } = useRooms();

  const navigate = useNavigate();
  const { checkOut, completeCleaning } = useRoomMutations();
  // const [selectedRoom, setSelectedRoom] = useState<RoomInfo | null>(null);
  // const [isModalOpen, setIsModalOpen] = useState(false);

  const skeletons = [1, 2, 3, 4, 5, 6];

  if (error) return <Text>{error.message}</Text>;

  const fetchedRomessCount =
    data?.pages.reduce((total, page) => total + page.results.length, 0) || 0;

  const handleCheckInClick = (room: RoomWithBookingDto) => {
    navigate(`/checkin/${room.roomInfo.id}`);
  };

  const handleCheckOut = (room: RoomWithBookingDto) => {
    checkOut.mutate(room.roomInfo.id);
  };

  const handleCompleteCleaning = (room: RoomWithBookingDto) => {
    completeCleaning.mutate(room.roomInfo.id);
  };

  // const handleModalClose = () => {
  //   setSelectedRoom(null);
  //   setIsModalOpen(false);
  // };

  return (
    <>
      <Box paddingLeft={2} borderBottom={5} paddingRight={2}>
        <HStack spacing={5} bg={"gray.700"} borderRadius={10} padding={3}>
          <FloorSelector />
          <RoomTypeSelector />
        </HStack>
      </Box>
      <InfiniteScroll
        dataLength={fetchedRomessCount}
        hasMore={!!hasNextPage}
        next={() => fetchNextPage()}
        loader={<Spinner />}
      >
        <SimpleGrid
          padding="10px"
          overflowX={"hidden"}
          columns={{ sm: 1, md: 2, lg: 3, xl: 4 }} /* Responsive Grid Control */
          spacing={6} /* space between two items */
        >
          {isLoading &&
            skeletons.map((skeleton) => (
              <RoomCardContainer key={skeleton}>
                <RoomCardSkeleton></RoomCardSkeleton>
              </RoomCardContainer>
            ))}
          {data?.pages.map((page, index) => (
            <React.Fragment key={index}>
              {page.results.map((room) => (
                <RoomCardContainer key={index}>
                  <RoomCard
                    room={room}
                    onCheckIn={handleCheckInClick}
                    onCheckOut={handleCheckOut}
                    onCompleteCleaning={handleCompleteCleaning}
                    isCheckingOut={
                      checkOut.isLoading &&
                      checkOut.variables === room.roomInfo.id
                    }
                    isCleaning={
                      completeCleaning.isLoading &&
                      completeCleaning.variables === room.roomInfo.id
                    }
                  />
                </RoomCardContainer>
              ))}
            </React.Fragment>
          ))}
        </SimpleGrid>
      </InfiniteScroll>

      {/* <CheckInModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        room={selectedRoom}
      /> */}
    </>
  );
};

export default RoomGrid;
