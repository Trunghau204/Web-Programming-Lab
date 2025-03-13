using THLTW_B2.DataAccess;
using THLTW_B2.Repositories;
using THLTW_B2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EFProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public EFProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
{
    if (product == null) throw new ArgumentNullException(nameof(product));

    // Kiểm tra xem CategoryId có tồn tại trong Categories không
    var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
    if (!categoryExists)
    {
        throw new Exception("CategoryId không hợp lệ. Vui lòng chọn danh mục hợp lệ.");
    }

    _context.Products.Update(product);
    await _context.SaveChangesAsync();
}

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
