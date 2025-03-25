﻿using MongoDB.Driver;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Repos
{
    public class ProductRepositoryFromMongoDb : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepositoryFromMongoDb(IConfiguration configuration)
        {
            var conString = configuration.GetConnectionString("MongoDb");

            var client = new MongoClient(conString);

            var database = client.GetDatabase("ProductDB");

            _productCollection = database.GetCollection<Product>("Products");

        }

        public async Task Delete(Product product)
        {
            await _productCollection.DeleteOneAsync(x=>x.Id == product.Id);
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _productCollection.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string Id)
        {
            return await _productCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<Product> Save(Product product)
        {
            await _productCollection.InsertOneAsync(product);

            return product;
        }

        public async Task Update(Product product)
        {
            await _productCollection.FindOneAndReplaceAsync(x=>x.Id==product.Id,product);
        }
    }
}
