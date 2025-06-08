using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Models;

namespace Students.Infrastructure.Interfaces
{
    public interface IDocumentRiseQualificationRepository: IGenericRepository<DocumentRiseQualification>
    {
        Task<PagedPage<DocumentRiseQualificationDTO>> GetPagedRiseQualifications(int pageNumber, int pageSize);

        Task<DocumentRiseQualification> Create(DocumentRiseQualificationDTO documentDto);
    }
}
