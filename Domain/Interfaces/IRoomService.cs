using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRoomService
    {

        Task<IEnumerable<Room>> GetRooms();
        Task<Room> GetRoomById(string id);
        Task AddRoom(Room room);
        Task UpdateRoom(Room room);
        Task DeleteRoom(string id);



        Task<IEnumerable<BedType>> GetBedTypes();
        Task<BedType> GetBedTypeByIdAsync(string id);
        BedType GetBedTypeById(string id);
        Task AddBedType(BedType bedType);
        Task UpdateBedType(BedType bedType);
        Task DeleteBedType(string id);


        Task<IEnumerable<FacilityType>> GetFacilityTypes();
        Task<FacilityType> GetFacilityTypeById(string id);
        Task AddFacilityType(FacilityType facilityType);
        Task UpdateFacilityType(FacilityType facilityType);
        Task DeleteFacilityType(string id);

        Task<IEnumerable<RoomType>> GetRoomTypes();
        Task<RoomType> GetRoomTypeById(string id);
        Task AddRoomType(RoomType roomType);
        Task UpdateRoomType(RoomType roomType);
        Task DeleteRoomType(string id);
    }
}
