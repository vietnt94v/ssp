using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ssp.Cmms.Application.Common.Models;
using Ssp.Cmms.Application.Features.WorkOrders.Commands;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;
using Ssp.Cmms.Application.Features.WorkOrders.Queries;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/work-orders")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class WorkOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<WorkOrderListItemDto>>> List(
        [FromQuery] GetWorkOrdersQuery query,
        CancellationToken ct)
    {
        return Ok(await _mediator.Send(query, ct));
    }

    [HttpGet("kanban")]
    public async Task<IActionResult> Kanban(
        [FromQuery] Guid? technicianId,
        CancellationToken ct)
    {
        return Ok(await _mediator.Send(
            new GetWorkOrdersKanbanQuery(technicianId), ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkOrderDto>> GetById(
        Guid id,
        CancellationToken ct)
    {
        return Ok(await _mediator.Send(new GetWorkOrderByIdQuery(id), ct));
    }

    [HttpPost]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<ActionResult<WorkOrderDto>> Create(
        [FromBody] CreateWorkOrderDto dto,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateWorkOrderCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<WorkOrderDto>> Update(
        Guid id,
        [FromBody] CreateWorkOrderDto dto,
        CancellationToken ct)
    {
        return Ok(await _mediator.Send(new UpdateWorkOrderCommand(id, dto), ct));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<WorkOrderDto>> ChangeStatus(
        Guid id,
        [FromBody] WorkOrderStatusChangeRequest request,
        CancellationToken ct)
    {
        return Ok(await _mediator.Send(
            new ChangeWorkOrderStatusCommand(id, request.Status), ct));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteWorkOrderCommand(id), ct);
        return NoContent();
    }
}

public record WorkOrderStatusChangeRequest(WorkOrderStatus Status);
