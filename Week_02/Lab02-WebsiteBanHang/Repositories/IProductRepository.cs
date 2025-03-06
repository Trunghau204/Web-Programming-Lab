using System.Collections;
using System.Collections.Generic;
using Lab02_WebsiteBanHang.Models;

namespace Lab02_WebsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetALl();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
