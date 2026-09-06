using DiranyAI.Api.Customers.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DiranyAI.Api.Customers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer(
        CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateCustomerAsync(request);

        return CreatedAtAction(
            nameof(CreateCustomer),
            new { id = customer.Id },
            customer);
    }
}