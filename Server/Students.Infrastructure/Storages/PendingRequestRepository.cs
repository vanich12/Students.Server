using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Filters;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.Infrastructure.Storages
{
    public class PendingRequestRepository : GenericRepository<PendingRequest>, IPendingRequestRepository
    {
        private readonly StudentContext _ctx;
        private readonly IGenericRepository<TypeEducation> _typeEducationRepository;

        private readonly IGenericRepository<EducationProgram> _educationProgramRepository;

        // по идее надо бы вынести всю эту бизнес-логику в сервис
        public async Task<PagedPage<RequestsDTO>> GetRequestPendingByPage(int page, int pageSize,
            PendingRequestFilterDTO filters)
        {
            IQueryable<PendingRequest> pendingRequestQuery = this._ctx.PendingRequests;
            var dtoQuery = await pendingRequestQuery.ApplyFilters(filters, _educationProgramRepository);
            var pagedData = await PagedPage<PendingRequest>.ToPagedPage(dtoQuery, page, pageSize, x => x.Family);

            List<RequestsDTO> dtoList = new();
            foreach (var item in pagedData.Data)
            {
                var dtoItem =
                    await Mapper.PendingRequestToRequestDTO(item,
                        _educationProgramRepository, _typeEducationRepository);
                dtoList.Add(dtoItem);
            }

            return new PagedPage<RequestsDTO>(
                dtoList.OrderBy(x => x.StudentFullName).ToList(),
                pagedData.TotalCount,
                pagedData.CurrentPage,
                pagedData.PageSize
            );
        }


        public async override Task<PendingRequest> Create(PendingRequest item)
        {
            item.CreatedAt = DateTime.UtcNow;
            return await base.Create(item);
        }

        public PendingRequestRepository(StudentContext context,
            IGenericRepository<TypeEducation> typeEducationRepository,
            IGenericRepository<EducationProgram> educationProgramRepository) : base(context)
        {
            this._ctx = context;
            this._typeEducationRepository = typeEducationRepository;
            this._educationProgramRepository = educationProgramRepository;
        }
    }
}