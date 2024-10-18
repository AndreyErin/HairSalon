namespace HairSalon.Model.Services
{
    public interface IRepositoryOfServices : IRepository<Service>
    {
        int Update(Service entity);
    }
}
