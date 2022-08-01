using System.ComponentModel.DataAnnotations;

namespace DailyHelper.Models.ViewModels.Requests
{
    public class UserRegistrationRequest
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}