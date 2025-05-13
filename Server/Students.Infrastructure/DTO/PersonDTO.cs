using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Infrastructure.DTO
{
    /// <summary>
    /// DTO персоны для фронта
    /// </summary>
    public class PersonDTO
    {
        /// <summary>
        /// Id персоны
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Фамилия
        /// экспорт из заявки
        /// </summary>
        [Required]
        public required string PersonFamily { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? PersonName { get; set; }

        /// <summary>
        /// Отчество
        /// экспорт из заявки
        /// </summary>
        public string? PersonPatron { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string? PersonFullName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}