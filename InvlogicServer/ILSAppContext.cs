
using InvlogicServer.Models;
using Microsoft.EntityFrameworkCore;

namespace InvlogicServer
{
    public class ILSAppContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING"), new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }
}