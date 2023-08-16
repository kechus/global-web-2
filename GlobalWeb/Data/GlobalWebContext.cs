using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GlobalWeb.Models;

namespace GlobalWeb.Data
{
    public class GlobalWebContext : DbContext
    {
        public GlobalWebContext (DbContextOptions<GlobalWebContext> options)
            : base(options)
        {
        }

        public DbSet<GlobalWeb.Models.LoginUser> LoginUser { get; set; } = default!;

        public DbSet<GlobalWeb.Models.Student>? Student { get; set; }

        public DbSet<GlobalWeb.Models.Course>? Course { get; set; }
    }
}
