using ProjetParking.Models;
using System.Data.Entity;

namespace ProjetParking
{
    public class Context : DbContext
    {
        public DbSet<UserParking> UserParkings { get; set; }
    }
}