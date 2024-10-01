using Store.Magdy.Core.Entities;
using Store.Magdy.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        // Create Repository<T> and Return
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

    }
}
