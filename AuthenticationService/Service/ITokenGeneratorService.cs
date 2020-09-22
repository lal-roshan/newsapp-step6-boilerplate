using System.Threading.Tasks;

namespace AuthenticationService.Service
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(string userId);
    }
}