using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Project.Application.Interfaces.IServices;
using Project.Domain.Entities;

namespace Project.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly ICurrentUserService _currentUser;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUser) : base(options)
        {
            _currentUser = currentUser;
        }
        #region DbSet Section
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        {
            Guid? userId = _currentUser.UserId;

            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreated(userId);
                    Console.WriteLine(userId);
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdated(userId);
                    Console.WriteLine(_currentUser.UserName);
                }
            }

            return await base.SaveChangesAsync(cancellation);
        }
    }
}
