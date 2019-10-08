using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tauchbolde.Application.UseCases.Event.GetEventDetailsUseCase;
using Tauchbolde.Application.UseCases.Event.GetEventListUseCase;
using Tauchbolde.InterfaceAdapters.Event.Details;
using Tauchbolde.InterfaceAdapters.Event.List;
using Tauchbolde.SharedKernel;
using Tauchbolde.Web.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tauchbolde.Web.Api.Controllers
{
    [Authorize(Policy = PolicyNames.RequireTauchboldeOrAdmin)]
    [Route("api/v1/events")]
    public class EventsApiController : Controller
    {
        [NotNull] private readonly IMediator mediator;

        public EventsApiController([NotNull] IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: api/v1/events
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var outputPort = new WebApiEventListPresenter();
            var interactorResult = await mediator.Send(new GetEventList(outputPort));
            if (!interactorResult.IsSuccessful)
            {
                return StatusCode(500);
            }

            return Ok(outputPort.GetJsonObject());
        }

        // GET api/v1/events/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var outputPort = new WebApiEventDetailPresenter();
            var interactorResult = await mediator.Send(new GetEventDetails(id, outputPort));
            if (!interactorResult.IsSuccessful)
            {
                return interactorResult.ResultCategory == ResultCategory.NotFound
                    ? NotFound()
                    : StatusCode(500);
            }

            return Ok(outputPort.GetJsonObject());
        }
    }
}