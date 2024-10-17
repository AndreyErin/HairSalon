namespace HairSalon.Model.Employees
{
    public interface IRepositoryOfEmployees
    {
        List<Employee> GetAll();
        Employee? Get(int id);
        Employee? Get(string name);
        int Add(Employee employee);
        int Update(Employee employee);
        int Delete(int id);
    }
}
