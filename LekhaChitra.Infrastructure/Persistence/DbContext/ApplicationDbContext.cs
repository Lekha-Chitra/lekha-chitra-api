using LekhaChitra.Domain.Entities.Application.Categories;
using LekhaChitra.Domain.Entities.Application.User;
using LekhaChitra.Domain.Interface.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Infrastructure.Persistence.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
             IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        public override async Task<int> SaveChangesAsync(
          CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditableEntities()
        {
            //taking out the userId from header
            string? userId = _httpContextAccessor
                .HttpContext?.User
                ?.FindFirstValue(ClaimTypes.NameIdentifier);


            var entries = ChangeTracker //finding entities that is of following interfaces and is either added or modified
                .Entries()
                .Where(e =>
                    (
                        e.Entity is IDateAudited
                        || e.Entity is IHasCreationDate
                        || e.Entity is IFullAudited
                    ) && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entry in entries)
            {

                if (entry.Entity is IDateAudited datedEntity)
                {
                    datedEntity.ModifiedDate = DateTime.UtcNow;
                }

                if (
                    entry.Entity is IHasCreationDate hasCreationDate
                    && entry.State == EntityState.Added
                )
                {
                    hasCreationDate.AddedDate = DateTime.UtcNow;
                }
                if (entry.Entity is ISoftDelete softDelete &&
                     entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;

                    softDelete.IsDeleted = true;
                    softDelete.DeletedDate = DateTime.UtcNow;
                    softDelete.DeletedBy = userId;
                }
                if (entry.Entity is IFullAudited fullAudited)
                {
                    if (entry.State == EntityState.Added)
                    {
                        fullAudited.AddedBy = userId;
                        fullAudited.AddedDate = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        fullAudited.ModifiedBy = userId;
                        fullAudited.ModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
