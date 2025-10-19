using System;
using Microsoft.EntityFrameworkCore;
using CadastroProdutos.Models;

namespace CadastroProdutos.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
}
