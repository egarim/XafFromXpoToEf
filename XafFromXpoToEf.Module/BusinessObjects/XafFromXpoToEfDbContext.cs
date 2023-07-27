using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using XafFromXpoToEf.Module.BusinessObjects.SoftDeleteExample;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules;
using XafFromXpoToEf.Module.BusinessObjects.ConcurrencyCheck;
using System.Data;
using DevExpress.ExpressApp;
using XafFromXpoToEf.Module.BusinessObjects.ChangeNotification;

namespace XafFromXpoToEf.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class XafFromXpoToEfContextInitializer : DbContextTypesInfoInitializerBase
{
    protected override DbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<XafFromXpoToEfEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new XafFromXpoToEfEFCoreDbContext(optionsBuilder.Options);
    }
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class XafFromXpoToEfDesignTimeDbContextFactory : IDesignTimeDbContextFactory<XafFromXpoToEfEFCoreDbContext>
{
    public XafFromXpoToEfEFCoreDbContext CreateDbContext(string[] args)
    {
        //throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
        var optionsBuilder = new DbContextOptionsBuilder<XafFromXpoToEfEFCoreDbContext>();
        optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=XafFromXpoToEf");
        optionsBuilder.UseChangeTrackingProxies();
        optionsBuilder.UseObjectSpaceLinkProxies();
        return new XafFromXpoToEfEFCoreDbContext(optionsBuilder.Options);
    }
}
[TypesInfoInitializer(typeof(XafFromXpoToEfContextInitializer))]
public class XafFromXpoToEfEFCoreDbContext : DbContext
{
    public XafFromXpoToEfEFCoreDbContext(DbContextOptions<XafFromXpoToEfEFCoreDbContext> options) : base(options)
    {
    }
    //public DbSet<ModuleInfo> ModulesInfo { get; set; }
    public DbSet<ModelDifference> ModelDifferences { get; set; }
    public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
    public DbSet<PermissionPolicyRole> Roles { get; set; }
    public DbSet<XafFromXpoToEf.Module.BusinessObjects.ApplicationUser> Users { get; set; }
    public DbSet<XafFromXpoToEf.Module.BusinessObjects.ApplicationUserLoginInfo> UserLoginInfos { get; set; }
    public DbSet<FileData> FileData { get; set; }
    public DbSet<ReportDataV2> ReportDataV2 { get; set; }

    public DbSet<SoftDelete> SoftDelete { get; set; }

    public DbSet<ConcurrencyObject> ConcurrencyObject { get; set; }

    public DbSet<SimplePersonWithCustomNotificationTrigger> SimplePersonWithCustomNotificationTrigger { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.Entity<XafFromXpoToEf.Module.BusinessObjects.ApplicationUserLoginInfo>(b =>
        {
            b.HasIndex(nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName), nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
        });
        modelBuilder.Entity<ModelDifference>()
            .HasMany(t => t.Aspects)
            .WithOne(t => t.Owner)
            .OnDelete(DeleteBehavior.Cascade);

        //HACK using configuration class to add soft delete for an entity in the current module
        modelBuilder.ApplyConfiguration(new SoftDeleteConfigurator());

        //HACK using configuration class to add soft delete for an entity in a XAF built in module
        modelBuilder.ApplyConfiguration(new ReportDataConfigurator());

    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //HACK log to any action that accepts a string message as parameter
        //optionsBuilder
        //.LogTo((message) =>
        // {
        //     ///HACK log to Debug.WriteLine but you can write it to a file if needed
        //     Debug.WriteLine(message);
        // });

        //HACK there are several type of loggers, this is just one of them, check the nugets that start with the name Microsoft.Extensions.Logging
        //For example, Microsoft.Extensions.Logging.Console and Microsoft.Extensions.Logging.Debug

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                //HACK extra filters https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/simple-logging#custom-filters
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information)
                .AddDebug();
        });



        //optionsBuilder.UseLoggerFactory(loggerFactory);

        //HACK enable sensitive data logging EnableSensitiveDataLogging
        optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();

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
