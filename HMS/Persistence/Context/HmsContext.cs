using HMS.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace HMS.Persistence.Context
{
    public class HmsContext : DbContext
    {
        public HmsContext(DbContextOptions<HmsContext> options) : base(options)
        {
           
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseNpgsql("Host=localhost;Port=5432;Database=HospitalManagementSystem;Username=postgres;Password=dev_abass@2021;")
        //        .ConfigureWarnings(w =>
        //            w.Ignore(RelationalEventId.PendingModelChangesWarning));
        //}


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne(u => u.Admin)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<Admin>()
           .Property(a => a.Gender)
           .HasConversion<byte>();

            SeedAdminData(builder);


            SeedRoleData(builder);


            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            builder.Entity<User>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Patient>()
           .HasOne(p => p.PatientDetail)
           .WithOne(pd => pd.Patient)
           .HasForeignKey<PatientDetail>(pd => pd.PatientId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<DoctorSpeciality>()
            .HasKey(ds => new { ds.DoctorId, ds.SpecialityId });

            builder.Entity<DoctorSpeciality>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecialities)
                .HasForeignKey(ds => ds.DoctorId);

            builder.Entity<DoctorSpeciality>()
                .HasOne(ds => ds.Speciality)
                .WithMany(s => s.DoctorSpecialities)
                .HasForeignKey(ds => ds.SpecialityId);

            base.OnModelCreating(builder);
        }

        private static void SeedAdminData(ModelBuilder modelBuilder)
        {
            //var adminRoleId = Guid.NewGuid();
            //var adminUserId = Guid.NewGuid();

            var adminRoleId = new Guid("d2719e67-52f4-4f9c-bdb2-123456789abc");
            var adminUserId = new Guid("c8f2e5ab-9f34-4b97-8b7c-1a5e86c77e42");

            var role = new Role
            {
                Id = adminRoleId,
                Name = "Admin",
                Description = "Has full permissions",
                DateCreated = DateTime.SpecifyKind(new DateTime(2025, 11, 10), DateTimeKind.Utc)
            };

            var hasher = new PasswordHasher<object>();
            var passwordHash = hasher.HashPassword(null, "Admin@001");
            var adminUser = new User
            {
                Id = adminUserId,
                Email = "Admin001@gmail.com",
                PasswordHash = "AQAAAAIAAYagAAAAEJjieFsJGM2Xgr+WpuS3juOABbBCvbqSvpym4WzP/SDMuvGz6qH+EFgm19l8SUHUGA==",
                EmailConfirmed = true,
                DateCreated  = DateTime.SpecifyKind(new DateTime(2025, 11, 10), DateTimeKind.Utc),
               

            };
            

            var userRole = new UserRole
            {
                Id = new Guid("7ad9b1e1-4c23-46a2-b8e4-219ab417f71f"),
                RoleId = adminRoleId,
                UserId = adminUserId,
                DateCreated = DateTime.SpecifyKind(new DateTime(2025, 11, 10), DateTimeKind.Utc)
            };

            var adminProfile = new Admin
            {
                Id = new Guid("f0e25b73-7d1a-4c19-8b2f-09a3efb40d12"),
                FirstName = "Admin",
                LastName = "Hms",
                Address = "Lagos State",
                Gender = Models.Enums.Gender.Male,
                PhoneNumber = "+23470456780",
                DateOfBirth = DateTime.SpecifyKind(new DateTime(1997, 11, 10), DateTimeKind.Utc),
                UserId = adminUserId,
                DateCreated = DateTime.SpecifyKind(new DateTime(2025, 11, 10), DateTimeKind.Utc),
            };

            modelBuilder.Entity<Role>().HasData(role);
            modelBuilder.Entity<Admin>().HasData(adminProfile);
            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<UserRole>().HasData(userRole);
        }

        private void SeedRoleData(ModelBuilder modelBuilder)
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Id = new Guid("a45c9e02-1f0b-4e57-b3d8-9b77b4a302be"),
                    Name = "Doctor",
                    Description = "Can manage appointments and patient records",
                    DateCreated = DateTime.SpecifyKind(new DateTime(2025, 11, 10), DateTimeKind.Utc),
                },
                new Role
                {
                    Id = new Guid("6e3d4978-dcb0-42ea-9c48-7f6209d4a871"),
                    Name = "Patient",
                    Description = "Can view appointments",
                    DateCreated = DateTime.SpecifyKind(new DateTime(2025, 11, 10), DateTimeKind.Utc),
                }
            };

            modelBuilder.Entity<Role>().HasData(roles);
        }


        DbSet<Admin> Admins => Set<Admin>();
        DbSet<User> Users => Set<User>();
        DbSet<UserRole> UserRoles => Set<UserRole>();
        DbSet<Patient> Patients => Set<Patient>();
        DbSet<PatientDetail> PatientDetails => Set<PatientDetail>();

        DbSet<Doctor> Doctors => Set<Doctor>();
        DbSet<Speciality> Specialities => Set<Speciality>();
        DbSet<Appointment> Appointments => Set<Appointment>();
        DbSet<Role> Roles => Set<Role>();
        DbSet<DoctorSpeciality> DoctorSpecialities => Set<DoctorSpeciality>();
    }
}
