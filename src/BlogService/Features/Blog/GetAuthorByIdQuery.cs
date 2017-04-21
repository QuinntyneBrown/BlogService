using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Blog
{
    public class GetAuthorByIdQuery
    {
        public class GetAuthorByIdRequest : IRequest<GetAuthorByIdResponse> { 
			public int Id { get; set; }
		}

        public class GetAuthorByIdResponse
        {
            public AuthorApiModel Author { get; set; } 
		}

        public class GetAuthorByIdHandler : IAsyncRequestHandler<GetAuthorByIdRequest, GetAuthorByIdResponse>
        {
            public GetAuthorByIdHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetAuthorByIdResponse> Handle(GetAuthorByIdRequest request)
            {                
                return new GetAuthorByIdResponse()
                {
                    Author = AuthorApiModel.FromAuthor(await _context.Authors.FindAsync(request.Id))
                };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
