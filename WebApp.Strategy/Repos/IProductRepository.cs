using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repos
{
    public interface IProductRepository
    {
        Task<Product> GetById(string Id);
        Task<List<Product>> GetAllByUserId(string userId);
        Task<Product> Save(Product product);
        Task  Update(Product product);
        Task Delete(Product product);
    }
}
