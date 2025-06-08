using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Infrastructure.DTO
{
    /// <summary>
    /// История изменений ФИО персоны
    /// </summary>
    public class PersonHistoryDTO
    {
        public string? OldFamily { get; set; }
        /// <summary>
        /// Новая фамилия
        /// </summary>
        public string? NewFamily { get; set; }
        /// <summary>
        /// Старое имя
        /// </summary>
        public string? OldName { get; set; }
        /// <summary>
        /// Новое имя
        /// </summary>
        public string? NewName { get; set; }
        /// <summary>
        /// Старое Отчество
        /// </summary>
        public string? OldPatron { get; set; }
        /// <summary>
        /// Новое Отчество
        /// </summary>
        public string? NewPatron { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateOnly? ChangeDate { get; set; }
        /// <summary>
        /// Тип изменения
        /// </summary>
        public string? ChangeType { get; set; }

        /// <summary>
        /// Последний, изменивший данные пользователь системы
        /// </summary>
        public Guid? LastChangedUserId { get; set; }
        /// <summary>
        /// Персона, чьи данные были изменены
        /// </summary>
        public Guid? PersonId { get; set; }
    }
}
