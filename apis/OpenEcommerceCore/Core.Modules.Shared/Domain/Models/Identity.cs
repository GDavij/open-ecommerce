namespace Core.Modules.Shared.Domain.Models;

public class Identity
{
    public Guid Id { get; private set; }

    private Identity(Guid id)
    {
        Id = id;
    }

    public static Identity Create(Guid id)
    {
        return new Identity(id);
    }
}