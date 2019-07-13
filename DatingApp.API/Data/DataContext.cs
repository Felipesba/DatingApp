using Documents.GitHub.DatingAPP.DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Documents.GitHub.DatingAPP.DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :
            base(options){ }

        public DbSet<Value> Values { get; set; }


    }
}