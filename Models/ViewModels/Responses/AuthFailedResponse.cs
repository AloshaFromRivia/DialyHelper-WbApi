using System.Collections.Generic;

namespace DailyHelper.Models.ViewModels.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}