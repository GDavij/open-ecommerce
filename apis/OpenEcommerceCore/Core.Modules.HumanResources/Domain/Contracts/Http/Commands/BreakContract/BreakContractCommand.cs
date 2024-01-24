using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;

public record BreakContractCommand(Guid Id) : IRequest;