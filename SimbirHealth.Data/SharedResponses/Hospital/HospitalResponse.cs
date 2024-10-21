using SimbirHealth.Data.Models.Hospital;

namespace SimbirHealth.Data.SharedResponses.Hospital
{
    /// <summary>
    /// Информация об одной больнице 
    /// </summary>
    public record HospitalResponse(Guid Guid, string Name, string Address, string ContactPhone, List<RoomResponse> Rooms);
}
