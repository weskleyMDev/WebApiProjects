using CategoryMVC.Models;

namespace CategoryMVC.Services;

public interface IAuthService
{
    Task<TokenViewModel?> AuthUser(UserViewModel userVM);
}
