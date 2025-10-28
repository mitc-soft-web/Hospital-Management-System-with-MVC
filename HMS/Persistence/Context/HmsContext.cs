using HMS.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HMS.Persistence.Context
{
    public class HmsContext : DbContext
    {
        public HmsContext(DbContextOptions<HmsContext> options) : base(options)
        {
          

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
                builder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                        SeedAdminData(builder);


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

        private void SeedAdminData(ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.NewGuid();
            var adminUserId = Guid.NewGuid();

            var role = new Role
            {
                Id = adminRoleId,
                Name = "Admin",
                Description = "Has full permissions",
                DateCreated = DateTime.UtcNow,
            };

            var adminUser = new User
            {
                Id = adminUserId,
                Email = "Admin001@gmail.com",
                EmailConfirmed = true,
                DateCreated = DateTime.UtcNow,
            };
            var hasher = new PasswordHasher<User>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@001");

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                RoleId = adminRoleId,
                UserId = adminUserId,
                DateCreated = DateTime.UtcNow
            };

            var adminProfile = new Admin
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "Hms",
                Address = "Lagos State",
                Gender = Models.Enums.Gender.Male,
                PhoneNumber = "+23470456780",
                DateOfBirth = new DateTime(1994, 04, 15),
                UserId = adminUserId,
                DateCreated = DateTime.UtcNow
            };

            modelBuilder.Entity<Role>().HasData(role);
            modelBuilder.Entity<Admin>().HasData(adminProfile);
            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<UserRole>().HasData(userRole);
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
