using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.DomainModel;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<EmployeeBooking> EmployeeBookings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairBooking> RepairBookings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<CostEstimationItem> CostEstimationItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var fk in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // AppUser <-> EmployeeProfile (1-1)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.EmployeeProfile)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey<EmployeeProfile>(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppUser <-> ClientProfile (1-1)
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.ClientProfile)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<ClientProfile>(c => c.ApplicationUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // ClientProfile <-> Car (1-many Req!)
            builder.Entity<Car>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Cars)
                .HasForeignKey(c => c.ClientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Car <-> Repair (1-many Req!)
            builder.Entity<Repair>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Repairs)
                .HasForeignKey(r => r.CarId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Repair <-> RepairBooking (1-1 Req!)
            builder.Entity<Repair>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Repair)
                .HasForeignKey<RepairBooking>(b => b.RepairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Repair <-> WorkTask (1-many Req!)
            builder.Entity<WorkTask>()
                .HasOne(t => t.Repair)
                .WithMany(r => r.Tasks)
                .HasForeignKey(t => t.RepairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Repair <-> CostEstimationCollection (1-many Req!)
            builder.Entity<CostEstimationItem>()
                .HasOne(c => c.Repair)
                .WithMany(r => r.CostEstimationCollection)
                .HasForeignKey(c => c.RepairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // WorkTask <-> EmployeeProfile (many-many)
            builder.Entity<WorkTask>()
                .HasMany(t => t.EmployeesAssigned)
                .WithMany(e => e.AssignedTasks);

            // WorkTask <-> Service (optional many-to-one)
            builder.Entity<WorkTask>()
                .HasOne(t => t.Service)
                .WithMany()
                .HasForeignKey(t => t.ServiceId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);


            // EmployeeProfile <-> EmployeeBooking (1-many Req!)
            builder.Entity<EmployeeBooking>()
                .HasOne(b => b.EmployeeProfile)
                .WithMany(e => e.EmployeeBookings)
                .HasForeignKey(b => b.EmployeeProfileId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Task <-> EmployeeBooking (1-many)
            builder.Entity<EmployeeBooking>()
                .HasOne(b => b.Task)
                .WithMany()
                .HasForeignKey(b => b.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            // Repair <-> Part (1-many Req!)
            builder.Entity<Part>()
                .HasOne(p => p.Repair)
                .WithMany(r => r.PartsUsed)
                .HasForeignKey(p => p.RepairId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceType <-> Service (1-many Req!)
            builder.Entity<Service>()
                .HasOne(s => s.ServiceType)
                .WithMany(st => st.Services)
                .HasForeignKey(s => s.ServiceTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Repair <-> Service (many-many)
            builder.Entity<Repair>()
                .HasMany(r => r.ServicesSelected)
                .WithMany(s => s.Repairs);

            // Part constraints
            builder.Entity<Part>()
                .HasCheckConstraint("CK_Part_UnitPrice_NonNegative", "[UnitPrice] >= 0");
            builder.Entity<Part>()
                .HasCheckConstraint("CK_Part_Quantity_Min1", "[Quantity] >= 1");

            // Service constraints
            builder.Entity<Service>()
                .HasCheckConstraint("CK_Service_AveragePrice_NonNegative", "[AveragePrice] >= 0");
            builder.Entity<Service>()
                .HasCheckConstraint("CK_Service_Description_MinLength", "LEN([Description]) >= 30");

            // ServiceType constraints
            builder.Entity<ServiceType>()
                .HasCheckConstraint("CK_ServiceType_Description_MinLength", "LEN([Description]) >= 30");

            // Car constraints
            builder.Entity<Car>()
                .HasCheckConstraint("CK_Car_Year_Range", "[Year] >= 1886 AND [Year] <= 2100");
        }
    }
}
