using MediatR;
using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Core;
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
        }

        public class RemoveCategoryResponse { }

        public class RemoveCategoryHandler : IAsyncRequestHandler<RemoveCategoryRequest, RemoveCategoryResponse>
        {
            public RemoveCategoryHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveCategoryResponse> Handle(RemoveCategoryRequest request)
            {
                var category = await _context.Categories.FindAsync(request.Id);
                category.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveCategoryResponse();
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
