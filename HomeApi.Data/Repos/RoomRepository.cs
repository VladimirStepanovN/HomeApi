using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;

        public RoomRepository(HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Найти комнату по id
        /// </summary>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Получить все комнаты
        /// </summary>
        public async Task<Room[]> GetAllRooms()
        {
            return await _context.Rooms.ToArrayAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Обновляет существующую комнату
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            var entry = await _context.Rooms.Where(r => r.Name == room.Name).FirstOrDefaultAsync();
            // Если в запрос переданы параметры для обновления - проверяем их на null
            // И если нужно - обновляем комнату
            if (!string.IsNullOrEmpty(query.NewName))
                entry.Name = query.NewName;
            entry.Area = query.NewArea;
            entry.GasConnected = query.NewGasConnected;
            entry.Voltage = query.NewVoltage;

            // Сохраняем изменения в базе 
            await _context.SaveChangesAsync();
        }
    }
}
