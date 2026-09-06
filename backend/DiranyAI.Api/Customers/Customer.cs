using DiranyAI.Api.Consultations;

namespace DiranyAI.Api.Customers;

public class Customer
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}