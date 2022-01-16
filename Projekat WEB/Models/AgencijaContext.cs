using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class AgencijaContext : DbContext
    {
        
        public DbSet<Majstor> Majstori { get; set; }  // da nisam naveo ono [Table("naziv tabele")] u klasama, ovako bi se zvale tabele Majstori, Poslovi i Dani
        public DbSet<Posao> Poslovi { get; set; }
        public  DbSet<Dan> Dani { get; set; }
        public DbSet<Agencija> Agencije { get; set; }
        public DbSet<Spoj> MajstoriPoslovi { get; set; }


        public AgencijaContext(DbContextOptions options) : base(options)
        {
            
        }
        
    }
}