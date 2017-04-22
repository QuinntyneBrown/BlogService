using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BlogService.Features.Authors
{
    public class RemoveAuthorCommand
    {
        public class RemoveAuthorRequest : IRequest<RemoveAuthorResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class RemoveAuthorResponse { }

        public class RemoveAuthorHandler : IAsyncRequestHandler<RemoveAuthorRequest, RemoveAuthorResponse>
        {
            public RemoveAuthorHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveAuthorResponse> Handle(RemoveAuthorRequest request)
            {
                var author = await _context.Authors
                    .Include(x => x.Tenant)
                    .SingleAsync(x => x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                author.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveAuthorResponse();
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}