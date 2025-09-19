import React from "react";
import useFloors from "../hooks/useFloors";
import useRoomQueryStore from "../store/useRoomQueryStore";
import useFloor from "../hooks/useFloor";
import { Button, Menu, MenuButton, MenuItem, MenuList } from "@chakra-ui/react";
import { BsChevronDown } from "react-icons/bs";

const FloorSelector = () => {
  const { data: floors, error } = useFloors();
  const selectedFloorId = useRoomQueryStore((s) => s.roomQuery.floorId);
  const setFloorId = useRoomQueryStore((s) => s.setFloorId);

  const selectedFloor = useFloor(selectedFloorId);

  if (error) return null;

  const menuItemStyle = {
    bg: "gray.600",
    _active: { bg: "gray.600" },
    _hover: { bg: "gray.500" },
  };

  return (
    <Menu>
      <MenuButton as={Button} rightIcon={<BsChevronDown />} {...menuItemStyle}>
        {selectedFloor?.floorName || "Select Floor"}
      </MenuButton>

      <MenuList bg="gray.600">
        {selectedFloor && (
          <MenuItem {...menuItemStyle} onClick={() => setFloorId(0)} key="all">
            All Floors
          </MenuItem>
        )}
        {floors?.map((floor) => (
          <MenuItem
            {...menuItemStyle}
            onClick={() => setFloorId(floor.id)}
            key={floor.id}
          >
            {floor.floorName}
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
};

export default FloorSelector;
