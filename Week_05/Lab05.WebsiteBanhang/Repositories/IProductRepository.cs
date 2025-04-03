using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab04.WebsiteBanHang.Models;

namespace Lab04.WebsiteBanHang.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}