namespace HairSalon.Model.Services
{
    public interface IRepositoryOfServices<T> where T : class
    {
        List<T> GetAll();
        T? Get(int id);
        int Add(T entity);
        int Update(T entity);
        int Delete(int id);
    }
}
