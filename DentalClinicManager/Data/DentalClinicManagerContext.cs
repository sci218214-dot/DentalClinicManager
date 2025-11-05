using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DentalClinicManager.Models;

namespace DentalClinicManager.Data
{
    public class DentalClinicManagerContext : DbContext
    {
        public DentalClinicManagerContext (DbContextOptions<DentalClinicManagerContext> options)
            : base(options)
        {
        }

        public DbSet<DentalClinicManager.Models.Patient> Patient { get; set; } = default!;
        public DbSet<DentalClinicManager.Models.Doctor> Doctor { get; set; } = default!;
        public DbSet<DentalClinicManager.Models.Appointment> Appointment { get; set; } = default!;
        public DbSet<DentalClinicManager.Models.Treatment> Treatment { get; set; } = default!;
    }
}
