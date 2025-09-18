import { SimpleGrid, Spinner, Text } from "@chakra-ui/react";
import React from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import useRooms from "../hooks/useRooms";
import RoomCard from "./RoomCard";
import RoomCardContainer from "./RoomCardContainer";
import RoomCardSkeleton from "./RoomCardSkeleton";

const RoomGrid = () => {
  const {
    data,
    error,
    isLoading,
    isFetchingNextPage,
    fetchNextPage,
    hasNextPage,
  } = useRooms();
  const skeletons = [1, 2, 3, 4, 5, 6];

  if (error) return <Text>{error.message}</Text>;

  const fetchedRomessCount =
    data?.pages.reduce((total, page) => total + page.results.length, 0) || 0;

  return (
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
              <RoomCardContainer key={room.id}>
                <RoomCard
                  room={room}
                  onCheckIn={(info) => console.log(info)}
                  onCheckOut={(info) => console.log(info)}
                  onCompleteCleaning={(info) => console.log(info)}
                />
              </RoomCardContainer>
            ))}
          </React.Fragment>
        ))}
      </SimpleGrid>
    </InfiniteScroll>
  );
};

export default RoomGrid;
