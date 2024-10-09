namespace SimbirHealth.Hospital.Models.Responses.Hospital
{
    /// <summary>
    /// Информация об одной больнице 
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Address"></param>
    /// <param name="ContactPhone"></param>
    public record GetHospitalDto(string Name, string Address, string ContactPhone);
}
