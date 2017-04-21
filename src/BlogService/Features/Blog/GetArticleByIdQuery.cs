using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlogService.Features.Blog
{
    public class GetArticleByIdQuery
    {
        public class GetArticleByIdRequest : IRequest<GetArticleByIdResponse> { 
            public int Id { get; set; }
        }

        public class GetArticleByIdResponse
        {
            public ArticleApiModel Article { get; set; } 
        }

        public class GetArticleByIdHandler : IAsyncRequestHandler<GetArticleByIdRequest, GetArticleByIdResponse>
        {
            public GetArticleByIdHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetArticleByIdResponse> Handle(GetArticleByIdRequest request)
            {                
                return new GetArticleByIdResponse()
                {
                    Article = ArticleApiModel.FromArticle(await _context.Articles
                    .Include(x=>x.Author)
                    .SingleAsync(x=> x.Id == request.Id))
                };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}