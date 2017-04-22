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
    public class GetArticlesQuery
    {
        public class GetArticlesRequest : IRequest<GetArticlesResponse> {
            public Guid TenantUniqueId { get; set; }
            public int? Skip { get; set; }
            public int? Take { get; set; }
        }

        public class GetArticlesResponse
        {
            public ICollection<ArticleApiModel> Articles { get; set; } = new HashSet<ArticleApiModel>();
        }

        public class GetArticlesHandler : IAsyncRequestHandler<GetArticlesRequest, GetArticlesResponse>
        {
            public GetArticlesHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetArticlesResponse> Handle(GetArticlesRequest request)
            {
                var articles = await _context.Articles
                    .Include(x => x.Author)
                    .Include(x => x.Tenant)
                    .ToListAsync();

                return new GetArticlesResponse()
                {
                    Articles = articles
                    .Where(x => 
                    x.Tenant != null &&
                    x.Tenant.UniqueId == request.TenantUniqueId)
                    .Select(x => ArticleApiModel.FromArticle(x)).ToList()
                };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
