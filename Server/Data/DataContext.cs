namespace DeviceManagment.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
               .HasOne<Office>(o => o.Offices)
               .WithMany(d => d.Departments)
               .HasForeignKey(o => o.OfficeId);

            modelBuilder.Entity<Employee>()
               .HasOne<Department>(p => p.Department)
               .WithMany(p => p.Employees)
               .HasForeignKey(p => p.DepartmentId);

            modelBuilder.Entity<Employee>()
                .HasOne<Person>(p => p.Person)
                .WithMany(p => p.Employees)
                .HasForeignKey(p => p.PersonId);

            modelBuilder.Entity<User>()
                .HasOne<Employee>(e => e.Employee)
                .WithMany(u => u.Users)
                .HasForeignKey(e => e.EmployeeId);

            modelBuilder.Entity<Device>()
                .HasOne<Department>(e => e.Department)
                .WithMany(u => u.Devices)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Device>()
                .HasOne<DeviceType>(e => e.DeviceType)
                .WithMany(u => u.Devices)
                .HasForeignKey(e => e.DeviceTypeId);

            modelBuilder.Entity<DeviceLoans>()
                .HasOne<Device>(e => e.Device)
                .WithMany(u => u.DevicesLoans)
                .HasForeignKey(e => e.DeviceId);

            modelBuilder.Entity<DeviceLoans>()
               .HasOne<User>(e => e.User)
               .WithMany(u => u.DevicesLoans)
               .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<DeviceLoans>()
               .HasOne<Employee>(e => e.Employee)
               .WithMany(u => u.DevicesLoans)
               .HasForeignKey(e => e.EmployeeId);

            //!Foreign key Relationship between Ticket -> Login tables
            modelBuilder.Entity<Ticket>()
                .HasOne<User>(p => p.Users)
                .WithMany(d => d.Tickets)
                .HasForeignKey(f => f.UserId);

            //!Foreign key Relationship between Ticket -> Login tables
            modelBuilder.Entity<Ticket>()
                .HasOne<Device>(p => p.Devices)
                .WithMany(d => d.Tickets)
                .HasForeignKey(f => f.DeviceId);

            modelBuilder.Entity<Ticket>()
               .HasOne<DeviceType>(e => e.Types)
               .WithMany(u => u.Ticktes)
               .HasForeignKey(e => e.DeviceTypeId);

        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Office>? Offices { get; set; }
        public DbSet<Department>? Departments { get; set; }
        public DbSet<Person>? People { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Device>? Devices { get; set; }
        public DbSet<DeviceType>? DeviceTypes { get; set; }
        public DbSet<DeviceLoans>? DeviceLoans { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }
    }
}
