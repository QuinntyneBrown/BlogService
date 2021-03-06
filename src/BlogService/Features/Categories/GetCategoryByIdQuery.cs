using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Categories
{
    public class GetCategoryByIdQuery
    {
        public class GetCategoryByIdRequest : IRequest<GetCategoryByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetCategoryByIdResponse
        {
            public CategoryApiModel Category { get; set; } 
        }

        public class GetCategoryByIdHandler : IAsyncRequestHandler<GetCategoryByIdRequest, GetCategoryByIdResponse>
        {
            public GetCategoryByIdHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetCategoryByIdResponse> Handle(GetCategoryByIdRequest request)
            {                
                return new GetCategoryByIdResponse()
                {
                    Category = CategoryApiModel.FromCategory(await _context.Categorys
                    .Include(x => x.Tenant)				
					.SingleAsync(x=>x.Id == request.Id &&  x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
