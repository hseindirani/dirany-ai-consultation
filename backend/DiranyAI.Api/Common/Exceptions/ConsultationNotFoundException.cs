namespace DiranyAI.Api.Common.Exceptions;

public class ConsultationNotFoundException : Exception
{
    public ConsultationNotFoundException(long id)
        : base($"Consultation with id '{id}' was not found.")
    {
    }
}