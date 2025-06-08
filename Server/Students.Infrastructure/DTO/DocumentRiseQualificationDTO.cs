using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Infrastructure.DTO
{
    public class DocumentRiseQualificationDTO
    {
        /// <summary>
        /// Id документа
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Вид документа повышения квалификации Id
        /// </summary>
        public required Guid KindDocumentRiseQualificationId { get; set; }

        /// <summary>
        /// Имя вида документа повышения квалификации
        /// </summary>
        public string? KindDocumentRiseQualificationName { get; set; }

        /// <summary>
        /// Заявка, на которую выдается документ о повышении квалификации
        /// </summary>
        public Guid? RequestId { get; set; }

        /// <summary>
        /// Дата выдачи удостоверения назовите это нормально
        /// </summary>
        public required DateTime Date { get; set; }

        /// <summary>
        /// Номер выдачи удостоверения назовите это нормально
        /// </summary>
        public required string Number { get; set; } = string.Empty;
    }
}
