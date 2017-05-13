using MediatR;
using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Categories
{
    public class RemoveCategoryCommand
    {
        public class RemoveCategoryRequest : IRequest<RemoveCategoryResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; } 
        }

        public class RemoveCategoryResponse { }

        public class RemoveCategoryHandler : IAsyncRequestHandler<RemoveCategoryRequest, RemoveCategoryResponse>
        {
            public RemoveCategoryHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveCategoryResponse> Handle(RemoveCategoryRequest request)
            {
                var category = await _context.Categorys.SingleAsync(x=>x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                category.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveCategoryResponse();
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
