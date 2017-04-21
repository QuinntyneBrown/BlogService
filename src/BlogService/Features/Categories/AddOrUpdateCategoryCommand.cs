using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Core;
using MediatR;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlogService.Features.Categories
{
    public class AddOrUpdateCategoryCommand
    {
        public class AddOrUpdateCategoryRequest : IRequest<AddOrUpdateCategoryResponse>
        {
            public CategoryApiModel Category { get; set; }
        }

        public class AddOrUpdateCategoryResponse { }

        public class AddOrUpdateCategoryHandler : IAsyncRequestHandler<AddOrUpdateCategoryRequest, AddOrUpdateCategoryResponse>
        {
            public AddOrUpdateCategoryHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateCategoryResponse> Handle(AddOrUpdateCategoryRequest request)
            {
                var entity = await _context.Categories
                    .SingleOrDefaultAsync(x => x.Id == request.Category.Id);
                if (entity == null) _context.Categories.Add(entity = new Category());
                entity.Name = request.Category.Name;
                await _context.SaveChangesAsync();

                return new AddOrUpdateCategoryResponse();
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
