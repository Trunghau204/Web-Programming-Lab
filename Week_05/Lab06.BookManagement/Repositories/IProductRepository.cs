using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab06.BookManagement.Models;

namespace Lab06.BookManagement.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}