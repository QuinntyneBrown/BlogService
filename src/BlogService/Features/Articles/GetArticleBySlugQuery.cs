using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace BlogService.Features.Articles
{
    public class GetArticleBySlugQuery
    {
        public class GetArticleBySlugRequest : IRequest<GetArticleBySlugResponse>
        {
            public string Slug { get; set; }
            public Guid TenantUniqueId { get; set; }
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
                    Article = ArticleApiModel.FromArticle(await _context.Articles
                    .Include(x => x.Tenant)
                    .Include(x => x.Tags)
                    .Include(x => x.Author)
                    .SingleAsync(a => a.Slug == request.Slug && a.Tenant.UniqueId == request.TenantUniqueId ))
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
