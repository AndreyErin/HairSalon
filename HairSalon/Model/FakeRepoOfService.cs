
namespace HairSalon.Model
{
    public class FakeRepoOfService : IRepositoryOfServices<Service>
    {
        List<Service> _services;
        public FakeRepoOfService() 
        {
            _services = new() { 
                new Service { Id = 1, Name = "Полубокс", Price = 100, TimeOfService = new TimeSpan(0,20,0), Description = "Под полубоксера" },
                new Service { Id = 1, Name = "Тенис", Price = 200, TimeOfService = new TimeSpan(0,25,0), Description = "Под тенисиста" },
                new Service { Id = 1, Name = "Модельная", Price = 300, TimeOfService = new TimeSpan(0,30,0), Description = "Под модель" },
                new Service { Id = 1, Name = "Котовский", Price = 400, TimeOfService = new TimeSpan(0,15,0), Description = "Под Котовского" },
                new Service { Id = 1, Name = "Гаршок", Price = 500, TimeOfService = new TimeSpan(0,10,0), Description = "Под горшок" },
            };
        }
        public void Add(Service entity)
        {
            _services.Add(entity);
        }

        public void Delete(int id)
        {
            Service? result = _services.Find(x => x.Id == id);
            if (result != null)
            {
                _services.Remove(result);
            }
        }

        public Service? Get(int id)
        {
            return _services.Find(x => x.Id == id);
        }

        public List<Service> GetAll()
        {
            return _services;
        }

        public void Update(Service entity)
        {
            Service? result = _services.Find(x=>x.Id == entity.Id);
            if (result != null)
            {
                result = entity;
            }
        }
    }
}
