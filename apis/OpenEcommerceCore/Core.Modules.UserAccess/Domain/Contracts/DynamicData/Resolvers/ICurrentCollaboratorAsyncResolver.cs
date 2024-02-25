using Core.Modules.UserAccess.Domain.Entities;

namespace Core.Modules.UserAccess.Domain.Contracts.DynamicData.Resolvers;

internal interface ICurrentCollaboratorAsyncResolver : IDynamicDataAsyncResolver<Collaborator>
{ }