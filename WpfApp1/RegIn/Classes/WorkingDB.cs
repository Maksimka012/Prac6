using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace RegIn.Classes
{
    public class WorkingDB : DbContext
    {
        readonly static string conncection = "server=127.0.0.1;port=3306;uid=root;pwd=;database=regin;";

        public DbSet<User> Users { get; set; }
        public WorkingDB()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(conncection);
        }
    }
}
