using System;

namespace DailyHelper.Models.ViewModels.Requests
{
    public class TodoTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}