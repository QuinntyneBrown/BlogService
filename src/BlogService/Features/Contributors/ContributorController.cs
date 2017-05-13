using MediatR;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BlogService.Features.Core;
using static BlogService.Features.Contributors.AddOrUpdateContributorCommand;
using static BlogService.Features.Contributors.GetContributorsQuery;
using static BlogService.Features.Contributors.GetContributorByIdQuery;
using static BlogService.Features.Contributors.RemoveContributorCommand;

namespace BlogService.Features.Contributors
{
    [Authorize]
    [RoutePrefix("api/contributor")]
    public class ContributorController : ApiController
    {
        public ContributorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("add")]
        [HttpPost]
        [ResponseType(typeof(AddOrUpdateContributorResponse))]
        public async Task<IHttpActionResult> Add(AddOrUpdateContributorRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("update")]
        [HttpPut]
        [ResponseType(typeof(AddOrUpdateContributorResponse))]
        public async Task<IHttpActionResult> Update(AddOrUpdateContributorRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }
        
        [Route("get")]
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(GetContributorsResponse))]
        public async Task<IHttpActionResult> Get()
        {
            var request = new GetContributorsRequest();
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("getById")]
        [HttpGet]
        [ResponseType(typeof(GetContributorByIdResponse))]
        public async Task<IHttpActionResult> GetById([FromUri]GetContributorByIdRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        [Route("remove")]
        [HttpDelete]
        [ResponseType(typeof(RemoveContributorResponse))]
        public async Task<IHttpActionResult> Remove([FromUri]RemoveContributorRequest request)
        {
            request.TenantUniqueId = Request.GetTenantUniqueId();
            return Ok(await _mediator.Send(request));
        }

        protected readonly IMediator _mediator;
    }
}
