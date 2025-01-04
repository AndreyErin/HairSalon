namespace HairSalon.Model.Employees
{
    public interface IRepositoryOfEmployees: IRepository<Employee>
    {
        int Update(Employee employee);
    }
}
