using TrendEmber.Core.Identity;

namespace TrendEmber.Service
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
