using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Project.Application.Interfaces.IServices;
using Project.Domain.Entities;
using Project.Domain.Entities.BaseEntities;

namespace Project.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<ApplicationDbContext> _logger;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUser,
            ILogger<ApplicationDbContext> logger) : base(options)
        {
            _currentUser = currentUser;
            _logger = logger;
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
                    _logger.LogInformation("User {User} created entity {EntityType} with Id {EntityId}",
                                   _currentUser.UserName, entry.Entity.GetType().Name, entry.Entity.Id);
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdated(userId);
                    _logger.LogInformation("User {User} updated entity {EntityType} with Id {EntityId}",
                                  _currentUser.UserName, entry.Entity.GetType().Name, entry.Entity.Id);
                }
                else if (entry.State == EntityState.Deleted && entry.Entity is SoftDeleteEntity softDelete)
                {
                    entry.State = EntityState.Modified;
                    _logger.LogWarning("User {User} soft-deleted entity {EntityType} with Id {EntityId}",
                               _currentUser.UserName, entry.Entity.GetType().Name, entry.Entity.Id);
                }
            }

            return await base.SaveChangesAsync(cancellation);
        }
    }
}
