import React from "react";
import useRoomTypes from "../hooks/useRoomTypes";
import useRoomQueryStore from "../store/useRoomQueryStore";
import useRoomType from "../hooks/useRoomType";
import { Button, Menu, MenuButton, MenuItem, MenuList } from "@chakra-ui/react";
import { BsChevronDown } from "react-icons/bs";

const RoomTypeSelector = () => {
  const { data: roomTypes, error } = useRoomTypes();
  const selectedRoomTypeId = useRoomQueryStore((s) => s.roomQuery.roomtypeId);
  const setRoomTypeId = useRoomQueryStore((s) => s.setRoomtypeId);

  const selectedRoomType = useRoomType(selectedRoomTypeId);

  if (error) return null;

  const menuItemStyle = {
    bg: "gray.600",
    _active: { bg: "gray.600" },
    _hover: { bg: "gray.500" },
  };

  return (
    <Menu>
      <MenuButton as={Button} rightIcon={<BsChevronDown />} {...menuItemStyle}>
        {selectedRoomType?.name || "Select Room Type"}
      </MenuButton>

      <MenuList bg="gray.600">
        {selectedRoomType && (
          <MenuItem
            {...menuItemStyle}
            onClick={() => setRoomTypeId(0)}
            key="all"
          >
            All Room Types
          </MenuItem>
        )}
        {roomTypes?.map((roomType) => (
          <MenuItem
            {...menuItemStyle}
            onClick={() => setRoomTypeId(roomType.id)}
            key={roomType.id}
          >
            {roomType.name}
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
};

export default RoomTypeSelector;
