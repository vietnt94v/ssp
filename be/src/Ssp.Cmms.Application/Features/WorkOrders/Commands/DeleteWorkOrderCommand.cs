using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;

namespace Ssp.Cmms.Application.Features.WorkOrders.Commands;

public record DeleteWorkOrderCommand(Guid Id) : IRequest;

public class DeleteWorkOrderCommandHandler
    : IRequestHandler<DeleteWorkOrderCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteWorkOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        DeleteWorkOrderCommand request,
        CancellationToken cancellationToken)
    {
        var wo = await _db.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (wo is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WorkOrder), request.Id);
        }

        // Soft delete handled by the DbContext SaveChanges interceptor.
        _db.WorkOrders.Remove(wo);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
