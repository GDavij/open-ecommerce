
using Core.Modules.Shared.Domain.Models;
using Core.Modules.UserAccess.Domain.Constants;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Exceptions.DynamicData;
using Core.Modules.UserAccess.Domain.Exceptions.DynamicData.Resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules.UserAccess.Domain.DynamicData;

internal class CurrentCollaboratorAsyncResolver : ICurrentCollaboratorAsyncResolver
{
   private readonly IHttpContextAccessor _httpContextAccessor;
   private readonly CancellationToken _cancellationToken;
   private readonly IUserAccessContext _dbContext;
   
   public CurrentCollaboratorAsyncResolver(IHttpContextAccessor httpContextAccessor)
   {
      _httpContextAccessor = httpContextAccessor;
      var sp = httpContextAccessor.HttpContext?.RequestServices;
      if (sp is null)
      {
         throw new NullServiceProviderException();
      }

      _cancellationToken = httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
      _dbContext = sp.GetRequiredService<IUserAccessContext>();
   }
   
   public async Task<Collaborator> ResolveAsync()
   {
      Identity? identity = (Identity?)_httpContextAccessor.HttpContext?.Items[HttpContextItems.Identity];
      if (identity is null)
      {
         throw new NoResolverDynamicParameterFoundException(typeof(Identity));
      }

      var collaborator = await _dbContext.Collaborators.FirstOrDefaultAsync(c => c.CollaboratorModuleId == identity.Id, _cancellationToken);
      if (collaborator is null)
      {
         throw new NullDataResolutionException(typeof(Collaborator));
      }

      return collaborator;
   }
}