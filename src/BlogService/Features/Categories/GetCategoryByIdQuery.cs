using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System.Threading.Tasks;

namespace BlogService.Features.Categories
{
    public class GetCategoryByIdQuery
    {
        public class GetCategoryByIdRequest : IRequest<GetCategoryByIdResponse> {
            public int Id { get; set; }
        }

        public class GetCategoryByIdResponse
        {
            public CategoryApiModel Category { get; set; } 
		}

        public class GetCategoryByIdHandler : IAsyncRequestHandler<GetCategoryByIdRequest, GetCategoryByIdResponse>
        {
            public GetCategoryByIdHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetCategoryByIdResponse> Handle(GetCategoryByIdRequest request)
            {
                return new GetCategoryByIdResponse()
                {
                    Category = CategoryApiModel.FromCategory(await _context.Categories.FindAsync(request.Id))
                };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}