namespace Core.Modules.Stock.Domain.Entities;

internal class Collaborator
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool Deleted { get; set; }

    private Collaborator(
        Guid id,
        string firstName,
        string lastName,
        string email,
        bool deleted)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Deleted = deleted;
    }

    public Collaborator Create(
        Guid id,
        string firstName,
        string lastName,
        string email)
    {
        return new Collaborator(
            id,
            firstName,
            lastName,
            email,
            false);
    }
}