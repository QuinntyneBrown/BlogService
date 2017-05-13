using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Categories
{
    public class GetCategoriesQuery
    {
        public class GetCategoriesRequest : IRequest<GetCategoriesResponse> { 
            public Guid TenantUniqueId { get; set; }       
        }

        public class GetCategoriesResponse
        {
            public ICollection<CategoryApiModel> Categories { get; set; } = new HashSet<CategoryApiModel>();
        }

        public class GetCategorysHandler : IAsyncRequestHandler<GetCategoriesRequest, GetCategoriesResponse>
        {
            public GetCategorysHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetCategoriesResponse> Handle(GetCategoriesRequest request)
            {
                var categories = await _context.Categories
                    .Include(x => x.Tenant)
                    .Where(x => x.Tenant.UniqueId == request.TenantUniqueId )
                    .ToListAsync();

                return new GetCategoriesResponse()
                {
                    Categories = categories.Select(x => CategoryApiModel.FromCategory(x)).ToList()
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}