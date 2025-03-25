using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.DbContexts;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repos
{
    public class ProductRepositoryFromSqlServer : IProductRepository
    {
        private readonly AppIdentityDbContext _context;

        public ProductRepositoryFromSqlServer(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _context.Products.Where(x=>x.UserId == userId).ToListAsync();  
        }

        public async Task<Product> GetById(string Id)
        {
            return await _context.Products.FindAsync(Id);
        }

        public async Task<Product> Save(Product product)
        {
            // SQL Server için Guid üretme
            product.Id = Guid.NewGuid().ToString();

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task Update(Product product)
        {
            _context.Update(product);

            await _context.SaveChangesAsync();
        }
    }
}
