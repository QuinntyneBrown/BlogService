using BlogService.Data;
using BlogService.Features.Core;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System;

namespace BlogService.Features.Articles
{
    public class GetLatestArticlesPageQuery
    {
        public class GetLatestArticlesPageRequest : IRequest<GetLatestArticlesPageResponse>
        {
            public Guid TenantUniqueId { get; set; }
            public int Skip { get; set; }
            public int Take { get; set; }
        }

        public class GetLatestArticlesPageResponse
        {
            public List<ArticleApiModel> Articles { get; set; }
        }

        public class GetLatestArticlesPageHandler : IAsyncRequestHandler<GetLatestArticlesPageRequest, GetLatestArticlesPageResponse>
        {
            public GetLatestArticlesPageHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetLatestArticlesPageResponse> Handle(GetLatestArticlesPageRequest request)
            {
                var articles = await _context.Articles
                    .Include(x => x.Author)
                    .Include(x => x.Tenant)
                    .Include(x => x.Tags)
                    .Include(x => x.Categories)
                    .Where(x => x.Tenant != null && x.Tenant.UniqueId == request.TenantUniqueId && x.IsPublished)
                    .OrderByDescending(x => x.Published)
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToListAsync();

                return new GetLatestArticlesPageResponse()
                {
                    Articles = articles.Select(x => ArticleApiModel.FromArticle(x)).ToList()
                };
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
