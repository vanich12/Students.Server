using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization; // <-- Не забудьте этот using
using System.Threading.Tasks;
using Students.Models.Enums;

namespace Students.Infrastructure.DTO
{
    public class NewPersonDTO
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        // JSON: "family"
        public required string Family { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        // JSON: "name"
        public string? Name { get; set; }

        /// <summary>
        /// Отчество
        /// экспорт из заявки
        /// </summary>
        // JSON: "patron"
        public string? Patron { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        // JSON: "birthDate" (формат "YYYY-MM-DD" подходит для DateOnly)
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [JsonPropertyName("phone")] // JSON поле называется "phone"
        public string? Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        // JSON: "email"
        public string? Email { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        // JSON: "nationality"
        public string? Nationality { get; set; }

        /// <summary>
        /// Опыт в It
        /// </summary>
        [JsonPropertyName("iT_Experience")] // JSON поле "iT_Experience"
        public string? It_Experience { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        // JSON: "address"
        public string? Address { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        // JSON: "sex" (например, 0)
        public SexHuman Sex { get; set; }

        /// <summary>
        /// Сфера деятельности ур.1
        /// </summary>
        // JSON: "scopeOfActivityLevelOneId"
        public Guid ScopeOfActivityLevelOneId { get; set; }

        /// <summary>
        /// Сфера деятельности ур.2
        /// </summary>
        // JSON: "scopeOfActivityLevelTwoId"
        public Guid ScopeOfActivityLevelTwoId { get; set; }

        /// <summary>
        /// Тип образования
        /// </summary>
        // JSON: "typeEducationId"
        public Guid? TypeEducationId { get; set; }

        // Поле "age" из JSON будет проигнорировано, так как для него нет свойства в DTO.
        // Если оно вам нужно, добавьте свойство, например:
        // [JsonPropertyName("age")]
        // public int? Age { get; set; }
    }
}