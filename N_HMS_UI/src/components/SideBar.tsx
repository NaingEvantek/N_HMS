import { VStack, Text, HStack, Divider } from "@chakra-ui/react";
import { Link, useLocation } from "react-router-dom";
import {
  FaHome,
  FaDoorOpen,
  FaUser,
  FaUsers,
  FaFileAlt,
  FaBuilding,
} from "react-icons/fa";

interface MenuItem {
  label: string;
  path?: string;
  icon?: React.ReactNode;
}

const menuItems: MenuItem[] = [
  { label: "Home", path: "/", icon: <FaHome /> },
  { label: "Floor", path: "/floor", icon: <FaBuilding /> },
  { label: "Room", path: "/room", icon: <FaDoorOpen /> },
  { label: "User", path: "/user", icon: <FaUser /> },
  { label: "Guest", path: "/guest", icon: <FaUsers /> },
  { label: "Booking History", path: "/booking-history", icon: <FaFileAlt /> },
];

const SideBar = () => {
  const location = useLocation();

  return (
    <VStack
      align="start"
      padding={4}
      flex="1"
      w="full"
      h="55vh"
      spacing={2}
      borderRadius="10px"
      bg="gray.800"
      borderColor="gray.700"
      overflowY="hidden" // allow scroll only if necessary
      css={{
        "&::-webkit-scrollbar": {
          width: "0px", // hide scrollbar in webkit browsers
        },
        "&::-webkit-scrollbar-track": {
          background: "transparent",
        },
        "&::-webkit-scrollbar-thumb": {
          background: "transparent",
        },
      }}
    >
      {menuItems.map((item) => {
        const isActive = item.path === location.pathname;

        return (
          <Link key={item.label} to={item.path || "/"}>
            <HStack
              p={2}
              pl={4}
              borderLeft={isActive ? "4px solid" : "4px solid transparent"}
              borderColor={isActive ? "blue.400" : "transparent"}
              spacing={3}
              cursor="pointer"
              transition="all 0.2s"
              _hover={{ color: "blue.300" }}
              color={isActive ? "blue.400" : "gray.300"}
              borderRadius="md"
            >
              {item.icon}
              <Text fontWeight={isActive ? "bold" : "medium"} margin={0}>
                {item.label}
              </Text>
            </HStack>
          </Link>
        );
      })}
    </VStack>
  );
};

export default SideBar;
