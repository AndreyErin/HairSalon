namespace HairSalon.Model.Services
{
    public class FakeRepoOfServices : IRepositoryOfServices
    {
        List<Service> _services;
        public FakeRepoOfServices()
        {
            _services = new() {
                new Service { Id = 1, Name = "Полубокс", Price = 100, TimeOfService = new TimeSpan(0,20,0), Description = "Под полубоксера" },
                new Service { Id = 2, Name = "Тенис", Price = 200, TimeOfService = new TimeSpan(0,25,0), Description = "Под тенисиста" },
                new Service { Id = 3, Name = "Модельная", Price = 300, TimeOfService = new TimeSpan(0,30,0), Description = "Под модель" },
                new Service { Id = 4, Name = "Котовский", Price = 400, TimeOfService = new TimeSpan(0,15,0), Description = "Под Котовского" },
                new Service { Id = 5, Name = "Гаршок", Price = 500, TimeOfService = new TimeSpan(0,10,0), Description = "Под горшок" },
            };
        }
        public int Add(Service entity)
        {
            try
            {
                _services.Add(entity);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Delete(int id)
        {
            Service? result = _services.Find(x => x.Id == id);
            if (result != null)
            {
                _services.Remove(result);
                return 1;
            }
            return -1;
        }

        public Service? Get(int id)
        {
            return _services.Find(x => x.Id == id);
        }

        public List<Service> GetAll()
        {
            return _services;
        }

        public int Update(Service entity)
        {
            Service? result = _services.Find(x => x.Id == entity.Id);
            if (result != null)
            {
                result.Name = entity.Name;
                result.Picture = entity.Picture;
                result.Price = entity.Price;
                result.TimeOfService = entity.TimeOfService;
                result.Description = entity.Description;               
                return 1;
            }
            return -1;
        }
    }
}
