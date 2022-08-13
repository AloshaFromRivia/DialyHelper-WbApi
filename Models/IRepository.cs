using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyHelper.Entity;

namespace DailyHelper.Models
{
    public interface IRepository<T> : IDisposable where T : class
    {
        public IQueryable<T> Items { get;}
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(Guid id);
        Task PutAsync(Guid id, T item);
        Task<T> PostAsync(T item);
        Task RemoveAsync(T item);
        bool ItemExist(Guid id);
    }
}