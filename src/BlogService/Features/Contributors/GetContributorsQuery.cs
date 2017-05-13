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
    public class GetContributorsQuery
    {
        public class GetContributorsRequest : IRequest<GetContributorsResponse> { 
            public Guid TenantUniqueId { get; set; }       
        }

        public class GetContributorsResponse
        {
            public ICollection<ContributorApiModel> Contributors { get; set; } = new HashSet<ContributorApiModel>();
        }

        public class GetContributorsHandler : IAsyncRequestHandler<GetContributorsRequest, GetContributorsResponse>
        {
            public GetContributorsHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetContributorsResponse> Handle(GetContributorsRequest request)
            {
                var contributors = await _context.Contributors
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new GetContributorsResponse()
                {
                    Contributors = contributors.Select(x => ContributorApiModel.FromContributor(x)).ToList()
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
