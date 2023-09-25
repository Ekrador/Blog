using DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _db;
        public DbSet<T> Set { get; private set; }

        public Repository(BlogContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public async Task<bool> Create(T item)
        {
            Set.Add(item);
            return await _db.SaveChangesAsync() == 1;
        }

        public async Task<bool> Delete(T item)
        {
            Set.Remove(item);
            return await _db.SaveChangesAsync() == 1;
        }

        public async Task<T> Get(string id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Set.ToListAsync();
        }

        public async Task<bool> Update(T item)
        {
            Set.Update(item);
            return await _db.SaveChangesAsync() == 1;
        }
    }
}
