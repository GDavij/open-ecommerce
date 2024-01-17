namespace Core.Modules.HumanResources.Domain.Entities;

internal class State
{
   public Guid Id { get; init; }
   public string name { get; init; }
   public string ShortName { get; init; }
   public List<Address> Addresses { get; init; }
}