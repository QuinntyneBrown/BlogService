using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Articles
{
    public class GetPreviewArticleBySlugQuery
    {
        public class GetPreviewArticleBySlugRequest : IRequest<GetPreviewArticleBySlugResponse>
        {
            public string Slug { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetPreviewArticleBySlugResponse
        {
            public ArticleApiModel Article { get; set; }
        }

        public class GetPreviewArticleBySlugHandler : IAsyncRequestHandler<GetPreviewArticleBySlugRequest, GetPreviewArticleBySlugResponse>
        {
            public GetPreviewArticleBySlugHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetPreviewArticleBySlugResponse> Handle(GetPreviewArticleBySlugRequest request)
            {
                return new GetPreviewArticleBySlugResponse()
                {
                    Article = ArticleApiModel.FromArticle(await _context.Articles
                    .Include(x => x.Tenant)
                    .Include(x => x.Tags)
                    .Include(x => x.Author)
                    .SingleAsync(a => a.Slug == request.Slug && a.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
