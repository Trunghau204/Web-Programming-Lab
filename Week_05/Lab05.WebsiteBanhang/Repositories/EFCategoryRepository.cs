using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab04.WebsiteBanHang.Data;
using Lab04.WebsiteBanHang.Interfaces;
using Lab04.WebsiteBanHang.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab04.WebsiteBanHang.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            if (_context.Categories == null)
            {
                Console.WriteLine("DbSet Categories is null!");
                return new List<Category>();
            }

            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            return categories ?? new List<Category>();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            if (categories != null)
            {
                _context.Categories.Remove(categories);
                await _context.SaveChangesAsync();
            }
        }
    }
}