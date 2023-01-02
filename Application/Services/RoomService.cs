using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<BedType> _bedTypeRepository;
        private readonly IRepository<FacilityType> _facilityTypeRepository;
        private readonly IRepository<RoomType> _roomTypeRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<RoomBed> _roomBedRepository;
        private readonly IRepository<RoomFacility> _roomFacilityRepository;
        private readonly IRepository<RoomPhoto> _roomPhotoRepository;

        public RoomService(IRepository<BedType> bedTypeRepository, IRepository<FacilityType> facilityTypeRepository, IRepository<RoomType> roomTypeRepository, IRepository<Room> roomRepository, IRepository<RoomBed> roomBedRepository, IRepository<RoomFacility> roomFacilityRepository, IRepository<RoomPhoto> roomPhotoRepository)
        {
            _bedTypeRepository = bedTypeRepository;
            _facilityTypeRepository = facilityTypeRepository;
            _roomTypeRepository = roomTypeRepository;
            _roomRepository = roomRepository;
            _roomBedRepository = roomBedRepository;
            _roomFacilityRepository = roomFacilityRepository;
            _roomPhotoRepository = roomPhotoRepository;
        }

        public async Task AddBedType(BedType bedType)
        {
            await _bedTypeRepository.AddAsync(bedType);
            await _bedTypeRepository.SaveChangesAsync();
        }

        public async Task AddFacilityType(FacilityType facilityType)
        {
            await _facilityTypeRepository.AddAsync(facilityType);
            await _facilityTypeRepository.SaveChangesAsync();
        }

        public async Task AddRoom(Room room)
        {
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task AddRoomType(RoomType roomType)
        {
            await _roomTypeRepository.AddAsync(roomType);
            await _roomTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteBedType(string id)
        {
            var bedtype = await _bedTypeRepository.GetByIdAsync(id);
            _bedTypeRepository.Remove(bedtype);
            await _bedTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteFacilityType(string id)
        {
            var facilityType = await _facilityTypeRepository.GetByIdAsync(id);
            _facilityTypeRepository.Remove(facilityType);
            await _facilityTypeRepository.SaveChangesAsync();
        }

        public async Task DeleteRoom(string id)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            _roomRepository.Remove(room);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task DeleteRoomType(string id)
        {
            var roomType = await _roomTypeRepository.GetByIdAsync(id);
            _roomTypeRepository.Remove(roomType);
            await _roomTypeRepository.SaveChangesAsync();
        }

        public BedType GetBedTypeById(string id)
        {
            return _bedTypeRepository.FirstOrDefault(x => x.BedTypeId == id);
        }

        public async Task<BedType> GetBedTypeByIdAsync(string id)
        {
            return await _bedTypeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BedType>> GetBedTypes()
        {
            return await _bedTypeRepository.GetAllAsync();
        }

        public async Task<FacilityType> GetFacilityTypeById(string id)
        {
            return await _facilityTypeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FacilityType>> GetFacilityTypes()
        {
            return await _facilityTypeRepository.GetAllAsync();
        }

        public async Task<Room> GetRoomById(string id)
        {
            return await _roomRepository.FirstOrDefaultAsync(x => x.RoomId == id, "Hotel,RoomType,RoomBeds,RoomPhotos,RoomFacilities");
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            return await _roomRepository.GetAllAsync("Hotel,RoomType,RoomBeds,RoomPhotos,RoomFacilities");
        }

        public async Task<RoomType> GetRoomTypeById(string id)
        {
            return await _roomTypeRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypes()
        {
            return await _roomTypeRepository.GetAllAsync();
        }

        public async Task UpdateBedType(BedType bedType)
        {
            _bedTypeRepository.Update(bedType);
            await _bedTypeRepository.SaveChangesAsync();
        }

        public async Task UpdateFacilityType(FacilityType facilityType)
        {
            _facilityTypeRepository.Update(facilityType);
            await _facilityTypeRepository.SaveChangesAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _roomRepository.Update(room);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task UpdateRoomType(RoomType roomType)
        {
            _roomTypeRepository.Update(roomType);
            await _roomTypeRepository.SaveChangesAsync();
        }
    }
}
