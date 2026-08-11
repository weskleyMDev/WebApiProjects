namespace StudentsApi.Api.DTOs;

public class ResponseMessage(string message)
{
    public string Message { get; } = message;
}