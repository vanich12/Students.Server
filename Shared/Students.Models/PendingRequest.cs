using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Students.Models.Enums;

namespace Students.Models
{
    public class PendingRequest
    {
        /// <summary>
        /// Идентификатор формы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public required string Family { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public required string Patron { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [JsonPropertyName("birthDate")]
        public required string Birthday { get; set; }

        /// <summary>
        /// Уровень образования
        /// </summary>
        public required string EducationLevel { get; set; }

        /// <summary>
        /// Направление образования (программа обучения)
        /// </summary>
        [JsonPropertyName("educationProgram")]
        public required string Education { get; set; }

        /// <summary>
        /// Опыт работы в IT
        /// </summary>
        public required string IT_Experience { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        public required string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public required string Email { get; set; }

        public SexHuman Sex { get; set; }

        /// <summary>
        /// Согласие на обработку персональных данных
        /// </summary>
        public required bool Agreement { get; set; }

        /// <summary>
        /// Id сферы деятельности(1 уровень).
        /// </summary>
        public string? ScopeOfActivityLevelOneId { get; set; }

        /// <summary>
        /// Id сферы деятельности(2 уровень).
        /// </summary>
        public string? ScopeOfActivityLevelTwoId { get; set; }
        /// <summary>
        /// Дата и время создания в системе "сырой заявки"
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Признак "сырой" заявки в архиве.
        /// </summary>
        public bool? IsArchive { get; set; } = false;
    }
}
