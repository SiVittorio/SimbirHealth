using SimbirHealth.Data.Models.Hospital;

namespace SimbirHealth.Hospital.Models.Responses.Hospital
{
    /// <summary>
    /// Информация об одной больнице 
    /// </summary>
    public record GetHospitalResponse(string Name, string Address, string ContactPhone, List<string> Rooms);
}
