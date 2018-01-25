using System;
using FixTest.Entities.Base;

namespace FixTest.Entities
{
    public class WebSite : Entity<long>
    {
        public WebSite()
        {
        }

        public WebSite(string url, long checkInterval = 60)
        {
            Url = url;
            CheckInterval = checkInterval;
        }

        /// <summary>
        /// URL адрес
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Дата последней проверки
        /// </summary>
        public DateTimeOffset? LastCheckedAt { get; set; }

        /// <summary>
        /// Статус последней проверки
        /// </summary>
        public bool? IsAvailable { get; set; }

        /// <summary>
        /// Интервал проверки доступности сайта в секундах
        /// </summary>
        public long CheckInterval { get; set; }
    }
}