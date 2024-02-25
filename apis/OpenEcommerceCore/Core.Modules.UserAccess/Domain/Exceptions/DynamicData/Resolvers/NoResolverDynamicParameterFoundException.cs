namespace Core.Modules.UserAccess.Domain.Exceptions.DynamicData.Resolvers;

internal class NoResolverDynamicParameterFoundException : Exception
{
    public NoResolverDynamicParameterFoundException(Type parameterType) : base($"Could not resolve dynamic parameter {parameterType.AssemblyQualifiedName}")
    { }
}