using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace XafFromXpoToEf.Module.BusinessObjects.SoftDeleteExample
{
    /// <summary>
    /// This configurator is used to add a soft delete filter to the ReportDataV2 entity
    /// </summary>
    public class ReportDataConfigurator : IEntityTypeConfiguration<ReportDataV2>
    {
        public void Configure(EntityTypeBuilder<ReportDataV2> builder)
        {

            //Extended with a property that is not part of the model
            builder.Property<bool>("IsDeleted");
            //Filter the DbSet to exclude the one marked as deleted
            builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);

        }
    }
}