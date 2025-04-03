using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab04.WebsiteBanHang.Models;

namespace Lab04.WebsiteBanHang.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}