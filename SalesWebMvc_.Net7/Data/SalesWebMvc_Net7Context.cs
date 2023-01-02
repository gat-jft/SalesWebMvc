using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Models;

namespace SalesWebMvc_.Net7.Data
{
    public class SalesWebMvc_Net7Context : DbContext
    {
        public SalesWebMvc_Net7Context (DbContextOptions<SalesWebMvc_Net7Context> options)
            : base(options)
        {
        }

        public DbSet<SalesWebMvc_.Net7.Models.Department> Department { get; set; } = default!;
    }
}
