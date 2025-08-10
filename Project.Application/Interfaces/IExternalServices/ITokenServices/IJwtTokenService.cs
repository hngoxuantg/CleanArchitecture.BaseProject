using Project.Domain.Entities;

namespace Project.Application.Interfaces.IExternalServices.ITokenServices
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtTokenAsync(User user, CancellationToken cancellation = default);
        string GenerateRefreshToken();
    }
}
