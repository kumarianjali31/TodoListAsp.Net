using Microsoft.AspNetCore.Identity;

namespace TodoListAPI
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
