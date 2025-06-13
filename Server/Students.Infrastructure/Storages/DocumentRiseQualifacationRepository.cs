using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.Infrastructure.Storages
{
    public class DocumentRiseQualifacationRepository : GenericRepository<DocumentRiseQualification>,
        IDocumentRiseQualificationRepository
    {
        private readonly StudentContext _ctx;
        private readonly IGenericRepository<KindDocumentRiseQualification> _kindDocumentRepository;

        public async Task<PagedPage<DocumentRiseQualificationDTO>> GetPagedRiseQualifications(int pageNumber,
            int pageSize)
        {
            IQueryable<DocumentRiseQualification> query = this._ctx.DocumentRiseQualifications;

            var dtoQuery = query.Select(x =>
                Mapper.DocumentRiseQualificationToDocumentRiseQualificationDTO(x, _kindDocumentRepository).Result);

            return await PagedPage<DocumentRiseQualificationDTO>.ToPagedPage(dtoQuery, pageNumber, pageSize,
                x => x.Date);
        }

        public async Task<DocumentRiseQualification> Create(DocumentRiseQualificationDTO documentDto)
        {
            var newDocument =  Mapper.DocumentRiseQualificationDTOToDocumentRiseQualification(documentDto);
            var doc = await base.Create(newDocument);
            return doc;
        }

        public DocumentRiseQualifacationRepository(StudentContext context,IGenericRepository<KindDocumentRiseQualification> kindDocumentRepository) : base(context)
        {
            this._kindDocumentRepository = kindDocumentRepository;
            this._ctx = context;
        }
    }
}