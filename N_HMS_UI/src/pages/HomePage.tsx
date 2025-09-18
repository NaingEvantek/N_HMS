import { Grid, GridItem, Show } from "@chakra-ui/react";
import React from "react";
import SideBar from "../components/SideBar";
import { Outlet } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

const HomePage = () => {
  const { user } = useAuth();
  console.log(user);
  const showSidebar = user?.role !== "User";

  return (
    <Grid
      templateAreas={{
        base: `"main"`,
        lg: showSidebar ? `"aside main"` : `"main"`, // sidebar only if not admin
      }}
      templateColumns={{
        base: "1fr",
        lg: showSidebar ? "250px 1fr" : "1fr", // adjust column width
      }}
      height={"83vh"}
    >
      <Show above="lg">
        {showSidebar && (
          <GridItem
            area="aside"
            paddingX={5}
            top={0}
            position={"sticky"}
            overflowY={"hidden"}
          >
            <SideBar />
          </GridItem>
        )}
      </Show>

      <GridItem
        area="main"
        borderRadius="10px"
        bg="gray.800"
        height="84vh"
        display="flex"
        flexDirection="column"
        overflowY="auto"
        padding={4}
        marginRight="15px"
      >
        <Outlet />
      </GridItem>
    </Grid>
  );
};

export default HomePage;
