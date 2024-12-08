namespace HairSalon.Model.Employees
{
    public class FakeRepoOfEmployees : IRepositoryOfEmployees
    {
        private List<Employee> _employees;

        public FakeRepoOfEmployees()
        {
            _employees = new List<Employee> 
            {
                new(){ Id = 1, Name = "Виктория", Post = "Парикмахер"},
                new(){ Id = 2, Name = "Елизавета", Post = "Парикмахер"},
            };
        }
        public int Add(Employee employee)
        {
            //if (_employees.Count > 0)
            //{
            //    int id = _employees.Select(e => e.Id).Max();
            //    employee.Id = id + 1;//присваевам id(к макимальному в базе прибавляем 1)
            //}
            //else
            //{
            //    employee.Id = 1;//если база пустая, то наш сотрудник будет первым
            //}


            //Employee? result = _employees.FirstOrDefault(e => e.Name == employee.Name);
            //if (result == null)
            //{
            //    _employees.Add(employee);
            //    return 1;
            //}
            return 0;
        }

        public int Delete(int id)
        {
            Employee? result = _employees.FirstOrDefault(e => e.Id == id);
            if (result != null)
            {
                _employees.Remove(result);
                return 1;
            }
            return 0;
        }

        public Employee? Get(int id)
        {
            return _employees.FirstOrDefault(e => e.Id == id);
        }

        public Employee? Get(string name)
        {
            return _employees.FirstOrDefault(e => e.Name == name);            
        }

        public List<Employee> GetAll()
        {
            return _employees;
        }

        public int Update(Employee employee)
        {
            Employee? result = _employees.FirstOrDefault(e => e.Id == employee.Id);
            if (result != null)
            {
                result.Name = employee.Name;
                result.Post = employee.Post;
                return 1;
            }
            return 0;
        }
    }
}
