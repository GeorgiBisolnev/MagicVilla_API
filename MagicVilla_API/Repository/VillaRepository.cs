﻿using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly AppDbContext _db;

        public VillaRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Villa entity)
        {
            await _db.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;
            if (tracked == false) 
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = _db.Villas;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();            
        }

        public async Task RemoveAsync(Villa entity)
        {
            _db.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            _db.Update(entity);
            await SaveAsync();
        }
    }
}
