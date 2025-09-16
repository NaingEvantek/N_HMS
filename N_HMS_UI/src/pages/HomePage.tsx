import { Box, Grid, GridItem, HStack, Show, Text } from "@chakra-ui/react";
import React from "react";

const HomePage = () => {
  return (
    <Grid
      templateAreas={{
        base: `"main"`,
        lg: `"aside main"`, // >1024px
      }}
      templateColumns={{
        base: "1fr",
        lg: "200px 1fr",
      }}
    >
      <Show above="lg">
        {/* to show in which screen layout */}
        <GridItem area="aside" paddingX={5}></GridItem>
      </Show>
      <GridItem area="main">
        <Box paddingLeft={2}>
          <HStack spacing={5} marginBottom={5}>
            <Text>Filter</Text>
          </HStack>
        </Box>
        <Text>Room Area</Text>
      </GridItem>
    </Grid>
  );
};

export default HomePage;
