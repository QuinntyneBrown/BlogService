using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;
using System;
using System.Data.Entity;

namespace BlogService.Features.Articles
{
    public class RemoveArticleCommand
    {
        public class RemoveArticleRequest : IRequest<RemoveArticleResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class RemoveArticleResponse { }

        public class RemoveArticleHandler : IAsyncRequestHandler<RemoveArticleRequest, RemoveArticleResponse>
        {
            public RemoveArticleHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveArticleResponse> Handle(RemoveArticleRequest request)
            {
                var article = await _context.Articles
                    .SingleAsync( x => x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);

                article.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveArticleResponse();
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
