using System;
using System.Collections.Generic;
using System.Linq;
using Lab02_WebsiteBanHang.Models;

namespace Lab02_WebsiteBanHang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Air Force 1 '07 Premium", Price = 1000, Description = "New Air Force 1 '07 Premium", ImageUrl = "/images/Air Force 1 '07 Premium.png",CategoryId = 1 },
                new Product { Id = 2, Name = "Air Jordan 1 Low", Price = 900, Description = "New Air Jordan 1 Low", ImageUrl = "/images/Air Jordan 1 Low.png",CategoryId = 1 },
                new Product { Id = 3, Name = "Samba OG Shoes", Price = 2000, Description = "New Samba OG Shoes", ImageUrl = "/images/Samba OG Shoes.jpg",CategoryId = 2 },
                new Product { Id = 4, Name = "Superstar Mule Shoes", Price = 1500, Description = "New Superstar Mule Shoes",ImageUrl = "/images/Superstar Mule Shoes.jpg", CategoryId = 2 },
                new Product { Id = 5, Name = "Fresh Foam X 1080v14", Price = 180, Description = "New Fresh Foam X 1080v14",ImageUrl = "/images/Fresh Foam X 1080v14.jpg", CategoryId = 3 },
                new Product { Id = 6, Name = "Fresh Foam X 880v15", Price = 120, Description = "New Fresh Foam X 880v15", ImageUrl = "/images/Fresh Foam X 880v15.jpg",CategoryId = 3 }
            };
        }
        public IEnumerable<Product> GetALl()
        {
            return _products;
        }
        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }
        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }   
    }
}
