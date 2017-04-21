using BlogService.Security;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static BlogService.Features.Tags.AddOrUpdateTagCommand;
using static BlogService.Features.Tags.GetTagsQuery;
using static BlogService.Features.Tags.GetTagByIdQuery;
using static BlogService.Features.Tags.RemoveTagCommand;

namespace BlogService.Features.Tags
{
    [Authorize]
    [RoutePrefix("api/tag")]
    public class TagController : ApiController
    {
        public TagController(IMediator mediator, IUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateTagResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateTagRequest request)
            => Ok(await _mediator.Send(request));

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateTagResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateTagRequest request)
            => Ok(await _mediator.Send(request));
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetTagsResponse))]
        public async Task<IHttpActionResult> Get()
            => Ok(await _mediator.Send(new GetTagsRequest()));

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetTagByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetTagByIdRequest request)
            => Ok(await _mediator.Send(request));

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveTagResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveTagRequest request)
            => Ok(await _mediator.Send(request));

        protected readonly IMediator _mediator;
        protected readonly IUserManager _userManager;
    }
}
