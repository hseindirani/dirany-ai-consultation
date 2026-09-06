using DiranyAI.Api.Common.Exceptions;
using DiranyAI.Api.Customers.Dtos;
using DiranyAI.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Customers;

public class CustomerService
{
    private readonly AppDbContext _dbContext;

    public CustomerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var phoneNumber = request.PhoneNumber.Trim();

        var phoneExists = await _dbContext.Customers
            .AnyAsync(c => c.PhoneNumber == phoneNumber);

        if (phoneExists)
        {
            throw new CustomerAlreadyExistsException(phoneNumber);
        }

        var customer = new Customer
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PhoneNumber = phoneNumber,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _dbContext.Customers.Add(customer);

        await _dbContext.SaveChangesAsync();

        return new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            CreatedAt = customer.CreatedAt
        };
    }
}