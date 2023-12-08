using Microsoft.EntityFrameworkCore;
using RepharmServiceCalendar.Entities;
using System;

public class BackendContext : DbContext
{
    public BackendContext(DbContextOptions<BackendContext> options) : base(options) { }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //ServiceType
        var barberServiceTypeId = Guid.NewGuid();
        modelBuilder.Entity<ServiceType>().HasData(new ServiceType
        {
            Id = barberServiceTypeId,
            Name = "BarberShop",
            StartTime = new TimeSpan(9, 0, 0), 
            EndTime = new TimeSpan(17, 0, 0),
            DateCreated = DateTime.UtcNow
        });

        //Services
        modelBuilder.Entity<Service>().HasData(
            new Service
            {
                Id = Guid.NewGuid(),
                ServiceName = "Haircut",
                Price = 30,
                ServiceTypeId = barberServiceTypeId,
                DateCreated = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                ServiceName = "Beard Trim",
                Price = 30,
                ServiceTypeId = barberServiceTypeId,
                DateCreated = DateTime.UtcNow
            }
        );

        //Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                Phone = "+37112345678",
                DateCreated = DateTime.UtcNow
            },
            new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Jane Smith",
                Email = "janesmith@example.com",
                Phone = "+37192345678",
                DateCreated = DateTime.UtcNow
            }
        );
    }
}
