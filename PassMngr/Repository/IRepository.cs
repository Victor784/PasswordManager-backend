namespace PassMngr.Repository
{
    public interface IRepository<T>
        where T : class
    {
        void Add(T entity);
        T? GetById(int id);
        void Delete(T entity);
        void Update(int id , T entity);
        List<T> GetAll();
    }
}
