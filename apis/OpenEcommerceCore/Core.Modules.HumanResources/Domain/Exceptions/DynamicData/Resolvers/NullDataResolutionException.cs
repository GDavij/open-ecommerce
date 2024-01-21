namespace Core.Modules.HumanResources.Domain.Exceptions.DynamicData.Resolvers;

internal class NullDataResolutionException : Exception
{
    public NullDataResolutionException(Type typeNullResolution) : base($"Resolved type {typeNullResolution.AssemblyQualifiedName} to null")
    { }
}