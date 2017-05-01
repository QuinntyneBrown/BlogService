using MediatR;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BlogService.Features.Core;
using static BlogService.Features.Articles.AddOrUpdateArticleCommand;
using static BlogService.Features.Articles.GetArticlesQuery;
using static BlogService.Features.Articles.GetArticleByIdQuery;
using static BlogService.Features.Articles.RemoveArticleCommand;
using static BlogService.Features.Articles.GetArticleBySlugQuery;
using static BlogService.Features.Articles.GetPreviewArticleBySlugQuery;
using static BlogService.Features.Articles.GetLatestArticlesPageQuery;

namespace BlogService.Features.Articles
{
    [Authorize]
    [RoutePrefix("api/article")]
    public class ArticleController : ApiController
    {
        public ArticleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateArticleResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateArticleRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateArticleResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateArticleRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getBySlug")]
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(GetArticleBySlugResponse))]
        public async Task<IHttpActionResult> GetBySlug([FromUri]GetArticleBySlugRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("preview")]
        [HttpGet]
        [AllowAnonymous]
        [ResponseType(typeof(GetPreviewArticleBySlugResponse))]
        public async Task<IHttpActionResult> GetBySlug([FromUri]GetPreviewArticleBySlugRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetArticlesResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetArticlesRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getlatest")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetArticlesResponse))]
        public async Task<IHttpActionResult> GetLatest([FromUri]GetLatestArticlesPageRequest request)
        {            
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetArticleByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetArticleByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveArticleResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveArticleRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
