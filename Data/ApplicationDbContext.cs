using Microsoft.EntityFrameworkCore;
using UltimoParcialDesarolloWebChristianB.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Asegúrate de que esta línea esté presente para que puedas acceder a los cines
    public DbSet<Cine> Cines { get; set; }

    public DbSet<Pelicula> Peliculas { get; set; }

    // Otros DbSet para otras entidades...
}
