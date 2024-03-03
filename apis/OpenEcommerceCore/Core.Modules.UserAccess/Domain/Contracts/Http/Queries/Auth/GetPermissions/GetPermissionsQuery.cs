using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Auth.GetPermissions;

public record GetPermissionsQuery : IRequest<EvaluationResult<IEnumerable<string>>>;