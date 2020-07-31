using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
            {
            }
            
            public DbSet<Product> Products { get; set; }    //  puxando no banco uma tabela de Produtos
            public DbSet<Category> Categories { get; set; } //  puxando no banco uma tabela de Categorias
            public DbSet<User> Users { get; set; }  //  puxando no banco uma tabela de Usuarios
    }
}