using DevExpress.ExpressApp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafFromXpoToEf.Module.BusinessObjects.BaseObjectFunctionality
{
    public class BaseDbContext:DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDbContext" /> class. The
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />
        /// method will be called to configure the database (and other options) to be used for this context.
        /// </summary>
        /// <remarks>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">BaseDbContext lifetime, configuration, and initialization</see>
        /// for more information and examples.
        /// </remarks>
        [RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
        protected BaseDbContext()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDbContext" /> class using the specified options.
        /// The <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" /> method will still be called to allow further
        /// configuration of the options.
        /// </summary>
        /// <remarks>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">BaseDbContext lifetime, configuration, and initialization</see> and
        /// <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see> for more information and examples.
        /// </remarks>
        /// <param name="options">The options for this context.</param>
        [RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }
        //HACK override to update soft delete status
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        //HACK override to update soft delete status
        public override int SaveChanges()
        {
            try
            {
                UpdateSoftDeleteStatuses();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }
        protected virtual void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {

                if (entry.Members.Any(x => x.Metadata.Name == "IsDeleted"))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = true;
                            break;
                    }
                }
            }
        }
    }
}
