using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DailyHelper.Entity
{
    public class ToDoTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}