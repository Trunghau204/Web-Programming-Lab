using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab02_WebsiteBanHang.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace Lab02_WebsiteBanHang.Repositories
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categories;
        public MockCategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category { Id = 1, Name = "Nike" },
                new Category { Id = 2, Name = "Adidas" },
                new Category { Id = 3, Name = "New Blance" }
            };
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categories;
        }

        public Category GetCategoryById(int id)
        {
            var categories = _categories.FirstOrDefault(c => c.Id == id);
            if (categories == null)
            {
                throw new Exception("Category not found");
            }
            return categories;
        }
    
        public void Add(Category category)
        {
            category.Id = _categories.Max(c => c.Id) + 1;
            _categories.Add(category);
        }

        public void Update(Category category)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
            }
        }

        public void Delete(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _categories.Remove(category);
            }
        }
    }
}