namespace Hotel.Application.DTOs.Rooms
{
    public class RoomDto : CreateRoomDto
    {
        public int RoomId { get; set; }
        public bool IsActive { get; set; }
    }
}
