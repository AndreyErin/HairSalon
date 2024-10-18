using HairSalon.Model.Employees;

namespace HairSalon.Model
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T? Get(int id);
        int Add(T entity);       
        int Delete(int id);    
    }
}
