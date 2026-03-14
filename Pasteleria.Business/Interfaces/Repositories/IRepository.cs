namespace Pasteleria.Business.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
