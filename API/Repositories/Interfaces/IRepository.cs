using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ali.Planning.API.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Get();
        TEntity Get(int id);
        TEntity Add(TEntity entity, string createdBy);
        TEntity Update(TEntity entity, string updatedBy);
        bool Delete(int id, string deletedBy);
        Task<int> SaveAllAsync();
    }
}
