using System.Collections;
using System.Collections.Generic;
using Lab02_WebsiteBanHang.Models;

namespace Lab02_WebsiteBanHang.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}
    