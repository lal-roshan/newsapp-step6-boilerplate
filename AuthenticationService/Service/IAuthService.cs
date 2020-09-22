using AuthenticationService.Models;
using System.Threading.Tasks;

namespace AuthenticationService.Service
{
    /*
    * Should not modify this interface. You have to implement these methods of interface 
    * in corresponding Implementation classes
    */
    public interface IAuthService
    {
        Task<bool> LoginUser(User user);
        Task<bool> RegisterUser(User user);
    }
}