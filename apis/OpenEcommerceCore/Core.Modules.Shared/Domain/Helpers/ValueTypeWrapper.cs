namespace Core.Modules.Shared.Domain.Helpers;

public record ValueTypeWrapper<T>(T Value) where T : struct;