namespace SimbirHealth.Hospital.Models.Requests.Hospital
{
    /// <summary>
    /// Запрос на добавление новой больницы
    /// </summary>
    /// <param name="Name">Название</param>
    /// <param name="Address">Адрес</param>
    /// <param name="ContactPhone">Телефон</param>
    /// <param name="Rooms">Кабинеты</param>
    public record AddHospitalRequest(string Name, string Address, string ContactPhone, List<string> Rooms);
}
