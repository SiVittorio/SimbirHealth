using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimbirHealth.Timetable.Services.TimeValidatorService
{
    /// <summary>
    /// Сервис для проверки времени в запросах
    /// </summary>
    internal static class TimeValidatorService
    {
        /// <summary>
        /// Проверка интервала. Количество минут всегда должно быть кратно
        /// 30
        /// </summary>
        /// <param name="from">Начало интервала</param>
        /// <param name="to">Конец интервала</param>
        /// <returns></returns>
        internal static (bool, string) ValidateInterval(DateTime from, DateTime to){

            if (to > from &&
                from.Second == 0 &&
                to.Second == 0 &&
                from.Minute % 30 == 0 && 
                to.Minute % 30 == 0 &&
                (to - from) <= TimeSpan.FromHours(12))
                    return (true, "Ok");
            else    
                return (false, string.Format("Неверный формат дат: from = {0} to = {1}",
                    from.ToString(),
                    to.ToString()));
        }
    }
}