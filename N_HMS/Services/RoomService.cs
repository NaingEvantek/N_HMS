using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using N_HMS.Database;
using N_HMS.DTO;
using N_HMS.Interfaces;
using N_HMS.Models;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Services
{
    public class RoomService : IRoomService
    {
        private readonly N_HMSContext _db;

        public RoomService(N_HMSContext db)
        {
            _db = db;
        }

        #region Room Type
        public async Task<List<RoomTypeSelectDTO>> GetRoomTypesAsync()
        {
           var roomTypes = await _db.Room_Type_Infos
                .OrderBy(rt => rt.Name)
                .Select(rt => new RoomTypeSelectDTO
                {
                    Id = rt.Id,
                    Name = rt.Name
                })
                .ToListAsync();
            return roomTypes;
        }
        #endregion

        public async Task<Room_Info> CreateRoomAsync(RoomCreateRequest req)
        {
            if (await _db.Room_Infos.AnyAsync(f => f.Room_Name == req.RoomName))
                throw new Exception("RoomName already exists");


            var room = new Room_Info
            {
                Room_Name = req.RoomName,
                Floor_Id = req.FloorId,
                Room_Type_Id = req.RoomTypeId,
                Room_Status_Id = 1, // Assuming 1 is the default status for new rooms
                Price_Per_Day = req.PricePerDay,
                Currency_Type_Id = req.CurrencyTypeId,
                Room_Capacity_Adult = req.RoomCapacityAdult,
                Room_Capacity_Child = req.RoomCapacityChild,
                Modify_Date = DateTime.UtcNow
            };

            _db.Room_Infos.Add(room);
            await _db.SaveChangesAsync();
            return room;
        }

        public async Task<Room_Info?> UpdateRoomAsync(RoomUpdateRequest req)
        {
            var room = await _db.Room_Infos.FindAsync(req.Id);
            if (room == null) return null;

            if (!string.IsNullOrEmpty(req.RoomName))
            {
                if (await _db.Room_Infos.AnyAsync(f => f.Room_Name == req.RoomName && f.Id != req.Id))
                    throw new Exception("Floor name already exists");
                room.Room_Name = req.RoomName;
            }

            if (req.FloorId.HasValue)
                room.Floor_Id = req.FloorId.Value;

            if (req.RoomTypeId.HasValue)
                room.Room_Type_Id = req.RoomTypeId.Value;

            if (req.RoomStatusId.HasValue)
                room.Room_Status_Id = req.RoomStatusId.Value;

            if (req.PricePerDay.HasValue)
                room.Price_Per_Day = req.PricePerDay.Value;

            if (req.CurrencyTypeId.HasValue)
                room.Currency_Type_Id = req.CurrencyTypeId.Value;

            if (req.RoomCapacityAdult.HasValue)
                room.Room_Capacity_Adult = req.RoomCapacityAdult.Value;

            if (req.RoomCapacityChild.HasValue)
                room.Room_Capacity_Child = req.RoomCapacityChild.Value;

            room.Modify_Date = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return room;
        }

        public async Task<RoomQueryResponse> SearchRoomAsync(RoomQueryRequest req)
        {
            var query = _db.Room_Infos
                .Include(r => r.Floor)
                .Include(r => r.Room_Type)
                .Include(r => r.Room_Status)
                .Include(r => r.Currency_Type)
                .Select(r => new RoomWithBookingDto
                {
                    Room = r,
                    ActiveBooking = r.Booking_Infos
                        .Where(b => b.CheckOut_Date == null)
                        .OrderByDescending(b => b.CheckIn_Date)
                        .FirstOrDefault()
                })
                .AsQueryable();

            // Filters
            if (req.floorId.HasValue && req.floorId > 0)
                query = query.Where(x => x.Room.Floor_Id == req.floorId.Value);

            if (req.roomtypeId.HasValue && req.roomtypeId > 0)
                query = query.Where(x => x.Room.Room_Type_Id == req.roomtypeId.Value);

            if (req.roomstatusId.HasValue && req.roomstatusId > 0)
                query = query.Where(x => x.Room.Room_Status_Id == req.roomstatusId.Value);

            if (!string.IsNullOrEmpty(req.search))
                query = query.Where(x => x.Room.Room_Name.Contains(req.search));

            // Sorting
            if (!string.IsNullOrEmpty(req.orderby))
            {
                var sortMap = new Dictionary<string, Expression<Func<RoomWithBookingDto, object>>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["roomname"] = x => x.Room.Room_Name,
                    ["floorname"] = x => x.Room.Floor.Name,
                    ["roomstatus"] = x => x.Room.Room_Status.Status,
                    ["roomtype"] = x => x.Room.Room_Type.Name
                };

                if (sortMap.TryGetValue(req.orderby, out var sortExpr))
                    query = query.OrderBy(sortExpr);
                else
                    query = query.OrderBy(x => x.Room.Id);
            }
            else
            {
                query = query.OrderBy(x => x.Room.Id);
            }

            // Pagination
            int pageSize = 15;
            int page = req.page ?? 1;
            int skip = (page - 1) * pageSize;

            var totalCount = await query.CountAsync();
            var results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return new RoomQueryResponse
            {
                count = totalCount,
                next = (skip + pageSize < totalCount) ? (page + 1).ToString() : "",
                results = results
            };
        }

        public async Task<PagedResult<RoomDTO>> GetAllRoomsAsync(QueryRequest req)
        {
            var query = _db.Room_Infos
            .Include(r => r.Floor)
            .Include(r => r.Room_Type)
            .Include(r => r.Room_Status)
            .Include(r => r.Currency_Type)
            .AsQueryable();

            // Sorting map
            var sortMap = new Dictionary<string, Expression<Func<Room_Info, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["roomname"] = r => r.Room_Name,
                ["floorname"] = r => r.Floor.Name,
                ["roomstatus"] = r => r.Room_Status.Status,
                ["roomtype"] = r => r.Room_Type.Name
            };


            // Apply sorting
            if (!string.IsNullOrEmpty(req.SortBy) && sortMap.TryGetValue(req.SortBy, out var sortExpr))
            {
                query = req.IsDescending ? query.OrderByDescending(sortExpr) : query.OrderBy(sortExpr);
            }
            else
            {
                query = query.OrderBy(r => r.Id); // default
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(r => new RoomDTO
                {
                    Id = r.Id,
                    RoomName = r.Room_Name,
                    FloorId = r.Floor_Id,
                    FloorName = r.Floor.Name,
                    RoomTypeId = r.Room_Type_Id,
                    RoomType = r.Room_Type.Name,
                    RoomStatusId = r.Room_Status_Id,
                    RoomStatus = r.Room_Status.Status,
                    CurrencyCode = r.Currency_Type.Code,
                    CurrencyType=r.Currency_Type.Code,
                    CurrencyTypeId=r.Currency_Type_Id,
                    PricePerDay = r.Price_Per_Day,
                    RoomCapacityAdult = r.Room_Capacity_Adult,
                    RoomCapacityChild = r.Room_Capacity_Child,
                    ModifyDate = r.Modify_Date ?? DateTime.MinValue
                })
                .ToListAsync();

            var items = data.Select((r, index) =>
            {
                r.No = ((req.PageIndex - 1) * req.PageSize) + index + 1;
                return r;
            }).ToList();

            return new PagedResult<RoomDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = req.PageIndex,
                PageSize = req.PageSize
            };
        }



        public async Task CheckInAsync(RoomCheckInRequest req)
        {
            // Begin transaction
            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // 1️ Find the room
                var room = await _db.Room_Infos.FindAsync(req.roomId);
                if (room == null) throw new Exception("Room not found");

                // 2️ Create booking
                var booking = new Booking_Info
                {
                    Room_Id = req.roomId,
                    CheckIn_Date = DateTime.UtcNow,
                    Created_Date = DateTime.UtcNow,
                    Modified_Date = DateTime.UtcNow,
                    Num_Guests = req.numOfGuests,
                    Paid_Amount = req.paidAmount,
                    Total_Amount = req.totalAmount,
                    Net_Amount = req.totalAmount - req.paidAmount,
                    No_Of_Days = req.numOfDays,
                    Payment_Status_Id = req.paymentStatusId
                };
                _db.Booking_Infos.Add(booking);

                // 3️ Update room status
                room.Room_Status_Id = (int)RoomStatusEnum.Occupied;

                await _db.SaveChangesAsync(); // booking.Id is generated here

                // 4️ Handle guests
                var guestList = new List<Guest_Info>();
                if (req.guests != null && req.guests.Count > 0)
                {
                    foreach (var gu in req.guests)
                    {
                        var existingGuest = await _db.Guest_Infos
                            .FirstOrDefaultAsync(g => g.Name == gu.GuestName && g.Passport_No == gu.PassportNo);

                        if (existingGuest != null)
                        {
                            guestList.Add(existingGuest);
                        }
                        else
                        {
                            var newGuest = new Guest_Info
                            {
                                Name = gu.GuestName,
                                Passport_No = gu.PassportNo,
                                Gender_Id = gu.GenderId,
                                Created_Date = DateTime.UtcNow
                            };
                            guestList.Add(newGuest);
                        }
                    }

                    // Add new guests to DB
                    var newGuests = guestList.Where(g => g.Id == 0).ToList();
                    if (newGuests.Any())
                    {
                        await _db.Guest_Infos.AddRangeAsync(newGuests);
                        await _db.SaveChangesAsync(); // ensure IDs are generated
                    }
                }

                // 5️ Create booking-guest mappings
                var mappings = guestList.Select(g => new Booking_Guest_Mapping_Info
                {
                    Booking_Id = booking.Id,
                    Guest_Id = g.Id
                });

                _db.Booking_Guest_Mapping_Infos.AddRange(mappings);
                await _db.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback if any error occurs
                await transaction.RollbackAsync();
                throw; 
            }
        }

        public async Task CheckOutAsync(int roomId)
        {
            var room = await _db.Room_Infos.FindAsync(roomId);

            if (room == null) throw new Exception("Room not found");

            var book = await _db.Booking_Infos.FirstOrDefaultAsync(b => b.Room_Id == roomId && b.CheckOut_Date == null);

            if (book == null)
                if (book == null) throw new Exception("Booking information not found!");


            book.CheckOut_Date = DateTime.UtcNow;
            if (book.Payment_Status_Id == (int)PaymentStatusEnum.Unpaid)
                book.Payment_Status_Id = (int)PaymentStatusEnum.Paid;

            book.Modified_Date = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            //  Update room status to Available
            room.Room_Status_Id = (int)RoomStatusEnum.Cleaning; // Cleaning
            //  Save changes
            await _db.SaveChangesAsync();
        }

        public async Task CompleteRoomCleaningAsync(int roomId)
        {
            var room = await _db.Room_Infos.FindAsync(roomId);
            if (room == null) throw new Exception("Room not found");
            room.Room_Status_Id = (int)RoomStatusEnum.Available; // Available
            room.Modify_Date = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}
