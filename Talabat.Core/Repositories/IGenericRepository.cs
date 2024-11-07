using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specifications
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specifications

        Task<IReadOnlyList<T>> GetAllWithSpecifications(ISepecifications<T> Spec);

        Task<T> GetByIdWithSpecifications(ISepecifications<T> Spec);

        Task<int> GetCountWithSpecAsync(ISepecifications<T> Spec);

        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);

        #endregion
    }
}
