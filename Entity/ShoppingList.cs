using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DailyHelper.Entity
{
    public class ShoppingList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string UserId { get; set; }
    }
    
    public class ShopItem
    {
        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Completed { get; set; }
    }
}