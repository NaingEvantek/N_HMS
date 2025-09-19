export interface BookingInfo {
  Id: number;
  CheckInDate: Date;
  CheckOutDate?: Date;
  PaymentStatusId?: number;
  roomId?: number;
  TotalAmount?: number;
}
