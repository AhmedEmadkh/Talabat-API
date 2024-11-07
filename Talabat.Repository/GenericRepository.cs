﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Without Specifications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region With Specifications
        public async Task<IReadOnlyList<T>> GetAllWithSpecifications(ISepecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).ToListAsync();
        }
        public async Task<T> GetByIdWithSpecifications(ISepecifications<T> Spec)
        {
			return await ApplySpecifications(Spec).FirstOrDefaultAsync();
        } 

        private IQueryable<T> ApplySpecifications(ISepecifications<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

		public async Task<int> GetCountWithSpecAsync(ISepecifications<T> Spec)
		{
            return await ApplySpecifications(Spec).CountAsync();
		}

		public async Task AddAsync(T item)
		=> await _dbContext.Set<T>().AddAsync(item);

		public void Update(T item)
		=> _dbContext.Set<T>().Update(item);

		public void Delete(T item)
		=> _dbContext.Set<T>().Remove(item);
		#endregion
	}
}
