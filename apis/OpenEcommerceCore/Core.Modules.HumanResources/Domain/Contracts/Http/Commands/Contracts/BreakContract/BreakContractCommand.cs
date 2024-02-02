using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.BreakContract;

public record BreakContractCommand(Guid Id) : IRequest;