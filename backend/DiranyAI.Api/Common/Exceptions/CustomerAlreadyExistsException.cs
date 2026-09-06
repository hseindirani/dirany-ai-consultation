namespace DiranyAI.Api.Common.Exceptions;

public class CustomerAlreadyExistsException : Exception
{
    public CustomerAlreadyExistsException(string phoneNumber)
        : base($"A customer with phone number '{phoneNumber}' already exists.")
    {
    }
}