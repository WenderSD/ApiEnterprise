using APIEnterprise.Context;
using APIEnterprise.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIEnterprise.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _appDbContext;
        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var models = await _appDbContext.Set<T>().AsNoTracking().ToListAsync();

            if (models == null)
                throw new ArgumentNullException("Entidades não encontradas");

            return models;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var model = await _appDbContext.Set<T>().FindAsync(id);

            if (model == null)
                throw new ArgumentNullException("Entidade não encontrada");

            return model;
        }


        public T Create(T model, CancellationToken cancellationToken = default)
        {
            if(model == null)
                throw new ArgumentNullException();

            _appDbContext.Set<T>().AddAsync(model, cancellationToken);

            return model;
        }

        public T Update(T modelRequest)
        {
            var modelResult = _appDbContext.Set<T>().Update(modelRequest);

            return modelRequest;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var model = await GetByIdAsync(id);

            if (model == null)
                throw new ArgumentNullException("Entidade não encontrada");

            _appDbContext.Set<T>().Remove(model);

            return model;
        }
    }
}
