using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteContract;

public record DeleteContractCommand(Guid Id) : IRequest;