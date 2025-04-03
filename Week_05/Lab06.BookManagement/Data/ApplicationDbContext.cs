using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab06.BookManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab06.BookManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}