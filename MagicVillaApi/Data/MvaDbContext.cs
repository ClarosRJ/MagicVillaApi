using MagicVillaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApi.Data
{
    public class MvaDbContext : DbContext
    {
        public MvaDbContext(DbContextOptions<MvaDbContext> options): base(options)
        {
            
        }
        public DbSet<Villa> villas { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Villa>().HasData(
        //        new Villa()
        //        {
        //            Id = 1,
        //            Nombre = "Villa Real",
        //            Detalle = "Detalle de la Villa...",
        //            ImageUrl = "",
        //            Ocupantes = 5,
        //            MetrosCuadrados = 50,
        //            Tarifa = 200,
        //            Amenidad = "",
        //            FechaCreacion = DateTime.Now,
        //            FechaActualizacion = DateTime.Now,
        //        },
        //        new Villa()
        //        {
        //            Id = 2,
        //            Nombre = "New Villa Real",
        //            Detalle = "New Detalle de la Villa...",
        //            ImageUrl = "",
        //            Ocupantes = 4,
        //            MetrosCuadrados = 40,
        //            Tarifa = 400,
        //            Amenidad = "",
        //            FechaCreacion = DateTime.Now,
        //            FechaActualizacion = DateTime.Now,
        //        }
        //        );
        //}
    }
}
