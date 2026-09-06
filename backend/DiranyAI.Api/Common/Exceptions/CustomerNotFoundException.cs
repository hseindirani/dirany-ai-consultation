namespace DiranyAI.Api.Common.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(long id)
        : base($"Customer with id '{id}' was not found.")
    {
    }
}