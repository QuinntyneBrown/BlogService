using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System;
using System.Threading.Tasks;

namespace BlogService.Features.Authors
{
    public class GetAuthorByIdQuery
    {
        public class GetAuthorByIdRequest : IRequest<GetAuthorByIdResponse> { 
			public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
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
