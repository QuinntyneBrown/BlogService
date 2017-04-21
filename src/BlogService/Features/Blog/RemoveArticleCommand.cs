using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;

namespace BlogService.Features.Blog
{
    public class RemoveArticleCommand
    {
        public class RemoveArticleRequest : IRequest<RemoveArticleResponse>
        {
            public int Id { get; set; }
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
                var article = await _context.Articles.FindAsync(request.Id);
                article.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveArticleResponse();
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
