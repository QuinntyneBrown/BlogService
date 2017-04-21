using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlogService.Features.Blog
{
    public class GetArticleBySlugQuery
    {
        public class GetArticleBySlugRequest : IRequest<GetArticleBySlugResponse>
        {
            public string Slug { get; set; }
        }

        public class GetArticleBySlugResponse
        {
            public ArticleApiModel Article { get; set; }
        }

        public class GetArticleBySlugHandler : IAsyncRequestHandler<GetArticleBySlugRequest, GetArticleBySlugResponse>
        {
            public GetArticleBySlugHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetArticleBySlugResponse> Handle(GetArticleBySlugRequest request)
            {
                return new GetArticleBySlugResponse()
                {
                    Article = ArticleApiModel.FromArticle(await _context.Articles.SingleAsync(a => a.Slug == request.Slug))
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
