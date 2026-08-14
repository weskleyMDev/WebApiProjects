using StudentsApi.Api.Models;

namespace StudentsApi.Api.DTOs;

public enum RefreshTokenResultStatus
{
    Success,
    InvalidToken,
    ExpiredToken,
    RevokedToken,
    UserNotFound
}

public class RefreshTokenResult
{
    public RefreshTokenResultStatus Status { get; set; }
    public TokenModel? Token { get; set; }
}