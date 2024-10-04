namespace SimbirHealth.Account.Models.Requests.Doctor
{
    /// <summary>
    /// Запрос на получение списка врачей
    /// </summary>
    /// <param name="NameFilter">Фильтр имени</param>
    /// <param name="From">Начало выборки</param>
    /// <param name="Count">Конец выборки</param>
    public record AllDoctorsRequest(string NameFilter, int From, int Count);
}
