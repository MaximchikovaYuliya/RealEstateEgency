using REA.Models.Context;
using REA.Models.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace REA.Models.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private readonly REAContext _context;
        private Dictionary<string, object> repositories;
        private bool disposed;

        public UnitOfWork()
        {
            _context = new REAContext();
        }

        public UnitOfWork(REAContext context)
        {
            _context = context;
        }

        public GenericRepository<T> GenericRepository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)repositories[type];
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
