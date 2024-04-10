using Microsoft.EntityFrameworkCore;

namespace ListingPatient.Data
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
             : base(options)
        {

        }
        public DbSet<PatientData> Patients { get; set; }
    }
}
