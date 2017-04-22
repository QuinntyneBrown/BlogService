using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System;

namespace BlogService.Features.Authors
{
    public class GetAuthorsQuery
    {
        public class GetAuthorsRequest : IRequest<GetAuthorsResponse> {
            public Guid TenantUniqueId { get; set; }
        }

        public class GetAuthorsResponse
        {
            public ICollection<AuthorApiModel> Authors { get; set; } = new HashSet<AuthorApiModel>();
        }

        public class GetAuthorsHandler : IAsyncRequestHandler<GetAuthorsRequest, GetAuthorsResponse>
        {
            public GetAuthorsHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetAuthorsResponse> Handle(GetAuthorsRequest request)
            {
                var authors = await _context.Authors
                    .Include(x=>x.Tenant)
                    .Where(x =>x.Tenant.UniqueId == request.TenantUniqueId)                    
                    .ToListAsync();

                return new GetAuthorsResponse()
                {
                    Authors = authors.Select(x => AuthorApiModel.FromAuthor(x)).ToList()
                };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
