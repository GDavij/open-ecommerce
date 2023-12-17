namespace Core.Modules.Stock.Domain.Entities;

internal class Collaborator
{
    public Guid Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool Deleted { get; set; }

    // Relationships
    public List<DemandMessage> DemandMessages { get; set; } = new List<DemandMessage>();
    
    private Collaborator()
    {}
    
    public static Collaborator Create(
        Guid id,
        string firstName,
        string lastName,
        string email)
    {
        return new Collaborator
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Deleted = false
        };
    }
}