import { HStack, Switch, Text, useColorMode } from "@chakra-ui/react";

const ColorModeSwitch = () => {
  const { toggleColorMode, colorMode } = useColorMode(); // destructure color mode
  return (
    <HStack>
      <Switch
        colorScheme="green" /* Switch bg color */
        isChecked={colorMode === "dark"}
        onChange={
          toggleColorMode
        } /* use Chakra Build-In toggleColorMode to change the theme color*/
      />
      <Text whiteSpace="nowrap">Dark Mode</Text>
    </HStack>
  );
};

export default ColorModeSwitch;
