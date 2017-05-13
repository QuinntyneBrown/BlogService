using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Contributors
{
    public class GetContributorByIdQuery
    {
        public class GetContributorByIdRequest : IRequest<GetContributorByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetContributorByIdResponse
        {
            public ContributorApiModel Contributor { get; set; } 
        }

        public class GetContributorByIdHandler : IAsyncRequestHandler<GetContributorByIdRequest, GetContributorByIdResponse>
        {
            public GetContributorByIdHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetContributorByIdResponse> Handle(GetContributorByIdRequest request)
            {                
                return new GetContributorByIdResponse()
                {
                    Contributor = ContributorApiModel.FromContributor(await _context.Contributors
                    .Include(x => x.Tenant)				
					.SingleAsync(x=>x.Id == request.Id &&  x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
