using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ali.Planning.API.Repositories
{
    public  class BaseRepository<TEntity>
      : IRepository<TEntity>
      where TEntity : BaseEntity
    {
        protected PlanningDataContext _context;

        public BaseRepository(PlanningDataContext context)
        {
            _context = context;
        }

        public TEntity Add(TEntity entity, string createdBy)
        {
            entity.StampCreation(createdBy);
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public bool Delete(int id, string deletedBy)
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(t => t.Id == id);
            if (entity != null)
            {
                entity.Delete(deletedBy);
                return true;
            }
            return false;
        }

        public IEnumerable<TEntity> Get()
        {
            return _context.Set<TEntity>().Where(t => !t.Deleted);
        }

        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().FirstOrDefault(t => t.Id == id && !t.Deleted);
        }

        public Task<int> SaveAllAsync()
        {
            return _context.SaveChangesAsync();
        }

        public TEntity Update(TEntity entity, string updatedBy)
        {
            entity.StampModification(updatedBy);
            _context.Set<TEntity>().Update(entity);
            return entity;
        }
    }
}
