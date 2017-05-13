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
    public class AddOrUpdateCategoryCommand
    {
        public class AddOrUpdateCategoryRequest : IRequest<AddOrUpdateCategoryResponse>
        {
            public CategoryApiModel Category { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateCategoryResponse { }

        public class AddOrUpdateCategoryHandler : IAsyncRequestHandler<AddOrUpdateCategoryRequest, AddOrUpdateCategoryResponse>
        {
            public AddOrUpdateCategoryHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateCategoryResponse> Handle(AddOrUpdateCategoryRequest request)
            {
                var entity = await _context.Categorys
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Category.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Categorys.Add(entity = new Category() { TenantId = tenant.Id });
                }

                entity.Name = request.Category.Name;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdateCategoryResponse();
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
