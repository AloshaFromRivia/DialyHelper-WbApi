using System.Threading.Tasks;
using DailyHelper.Models.ViewModels.Requests;
using DailyHelper.Models.ViewModels;
using IdentityServer4.ResponseHandling;

namespace DailyHelper.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
    }
}