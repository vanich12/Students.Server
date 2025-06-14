﻿using Students.Models;

namespace Students.Infrastructure.DTO
{
    /// <summary>
    /// DTO приказа о зачислении/ отчислении.
    /// </summary>
    public class OrderDTO
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Номер приказа.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Дата приказа.
        /// </summary>
        public required DateTime Date { get; set; }

        /// <summary>
        /// Вид приказа.
        /// </summary>
        public string? KindOrderName { get; set; }

        /// <summary>
        /// Вид приказа.
        /// </summary>
        public Guid? KindOrderId { get; set; }

        /// <summary>
        /// заявки в приказе
        /// </summary>
        public Guid RequestId { get; set; }

        /// <summary>
        /// Имя заявителя
        /// </summary>
        public string? RequestFullName { get; set; }

        /// <summary>
        /// Группы.
        /// </summary>
        public IEnumerable<Group>? Groups { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string? GroupName { get; set; }
    }
}