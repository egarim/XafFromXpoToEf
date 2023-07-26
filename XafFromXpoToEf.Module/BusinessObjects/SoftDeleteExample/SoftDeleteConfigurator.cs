using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace XafFromXpoToEf.Module.BusinessObjects.SoftDeleteExample
{
    public class SoftDeleteConfigurator : IEntityTypeConfiguration<SoftDelete>
    {
        public void Configure(EntityTypeBuilder<SoftDelete> builder)
        {

            //Extended with a property that is not part of the model
            builder.Property<bool>("IsDeleted");
            //Filter the DbSet to exclude the one marked as deleted
            builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

        }

    }
}