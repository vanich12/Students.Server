using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.DBCore.Contexts;
using Students.Infrastructure.DTO;
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
        private readonly IGenericRepository<StatusRequest> _statusRequestRepository;
        private readonly IGenericRepository<EducationProgram> _educationProgramRepository;

        public async Task<PagedPage<RequestsDTO>> GetRequestPendingByPage(int page, int pageSize)
        {
            IQueryable<PendingRequest> pendingRequestQuery = this._ctx.PendingRequests;
            var dtoQuery = pendingRequestQuery.Select(x =>
                Mapper.PendingRequestToRequestDTO(x, _statusRequestRepository, _educationProgramRepository).Result);
            return await PagedPage<RequestsDTO>.ToPagedPage(dtoQuery, page, pageSize, x => x.StudentFullName);
        }

        public override Task<PendingRequest> Create(PendingRequest item)
        {
            item.CreatedAt = DateTime.Now;
            return base.Create(item);
        }

        public PendingRequestRepository(StudentContext context,
            IGenericRepository<StatusRequest> statusRequestRepository,
            IGenericRepository<EducationProgram> educationProgramRepository) : base(context)
        {
            this._ctx = context;
        }
    }
}