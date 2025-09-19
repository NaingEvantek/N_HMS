import { BookingInfo } from "./Booking";
import FloorInfo from "./Floor";

export interface CurrencyType {
  id: number;
  name: string;
  code: string;
  symbol: string;
}

export interface RoomStatus {
  id: number;
  status: string;
}

export interface RoomTypeInfo {
  id: number;
  name: string;
}

export interface RoomInfo {
  id: number;
  room_Name: string;
  floor_Id?: number;
  room_Type_Id?: number;
  room_Status_Id?: number;
  price_Per_Day?: number;
  currency_Type_Id?: number;
  room_Capacity_Adult?: number;
  room_Capacity_Child?: number;
  modify_Date?: string; // ISO string

  // Navigation properties
  currency_Type?: CurrencyType;
  floor?: FloorInfo;
  room_Status?: RoomStatus;
  room_Type?: RoomTypeInfo;
}

export default interface RoomWithBookingDto {
  roomInfo: RoomInfo;
  bookingInfo: BookingInfo;
}

export interface RoomCheckInGuest {
  guestName: string;
  passportNo: string;
  genderId?: number;
}

export interface RoomCheckInRequest {
  roomId: number;
  paymentStatusId: number; // e.g., 1 = Paid, 2 = Pending
  numOfGuests: number;
  numOfDays: number;
  totalAmount: number;
  paidAmount: number;
  guests: RoomCheckInGuest[];
}

export enum RoomStatusEnum {
  Available = "Available",
  Cleaning = "Cleaning",
  Occupied = "Occupied",
}
