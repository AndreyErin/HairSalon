namespace HairSalon.Model.Employees
{
    public interface IRepositoryOfEmployees: IRepository<Employee>
    {
        Employee? Get(string name);
        int Update(Employee employee);
    }
}
