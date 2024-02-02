using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.DeleteContract;

public record DeleteContractCommand(Guid Id) : IRequest;