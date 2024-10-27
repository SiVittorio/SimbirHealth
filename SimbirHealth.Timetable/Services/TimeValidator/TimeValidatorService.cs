using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimbirHealth.Timetable.Services.TimeValidator
{
    /// <summary>
    /// Сервис для проверки времени в запросах
    /// </summary>
    public static class TimeHelperService
    {
        /// <summary>
        /// Проверка интервала. Количество минут всегда должно быть кратно
        /// 30
        /// </summary>
        /// <param name="from">Начало интервала</param>
        /// <param name="to">Конец интервала</param>
        /// <returns></returns>
        public static bool ValidateInterval(DateTime from, DateTime to){

            return to > from &&
                from.Second == 0 &&
                to.Second == 0 &&
                from.Minute % 30 == 0 && 
                to.Minute % 30 == 0 &&
                (to - from) <= TimeSpan.FromHours(12);
        }

        /*
        1. -----------------|fromA       toA|-------
        --|fromB      toB|--------------------------

        2. --|fromA       toA|----------------------
        ------------------------|fromB      toB|----
        */
        /// <summary>
        /// Проверка пересечений двух интервалов
        /// </summary>
        public static bool ValidateIntersect(
            DateTime fromA, DateTime toA,
            DateTime fromB, DateTime toB) {
            return toB <= fromA ||
                fromB >= toA;
            
        }

        /// <summary>
        /// Разделить промежуток времени от from до to
        /// на последовательность DateTime-объектов, 
        /// с интервалом в 30 минут
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> SplitDateRange(DateTime from,
        DateTime to)
        {
            TimeSpan chunk = TimeSpan.FromMinutes(30);
            while (from < to)
            {
                yield return from;
                from = from.Add(chunk);
            }
        }
    }
}