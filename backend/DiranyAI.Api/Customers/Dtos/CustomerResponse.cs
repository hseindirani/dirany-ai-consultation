namespace DiranyAI.Api.Customers.Dtos;

public class CustomerResponse
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}