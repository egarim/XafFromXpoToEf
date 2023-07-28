using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace XafFromXpoToEf.Module.BusinessObjects.BaseObjectFunctionality
{
    /// <summary>
    /// This configurator is used to add a soft delete filter to the MyBaseEfObject entity
    /// </summary>
    public class MyBaseEfObjectConfigurator<T>: IEntityTypeConfiguration<T> where T: class
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {

            //Extended with a property that is not part of the model
            builder.Property<bool>("IsDeleted");
            //Filter the DbSet to exclude the one marked as deleted
            builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

        }
    }
    /// <summary>
    /// This configurator is used to add a soft delete filter to the MyBaseEfObject entity
    /// </summary>
    public class TimeStampConcurrencyConfigurator<T> : IEntityTypeConfiguration<T> where T : class
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {

            //builder.Property<byte[]>("Timestamp").IsConcurrencyToken()
            //   .ValueGeneratedOnAddOrUpdate();
            builder.Property<byte[]>("Timestamp")
               .IsRowVersion(); // This makes the property a concurrency token
        }
    }
}