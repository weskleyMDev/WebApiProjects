namespace AlunosAPI.Models.DTOs;

public class ResponseMessage(string message)
{
    public string Message { get; } = message;
}