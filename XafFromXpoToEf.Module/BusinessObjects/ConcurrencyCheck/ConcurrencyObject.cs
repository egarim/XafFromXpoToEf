using DevExpress.Data.Filtering;
using DevExpress.DataAccess.Native.Json;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.PivotGrid.OLAP.Mdx;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;
using DevExpress.CodeParser;

namespace XafFromXpoToEf.Module.BusinessObjects.ConcurrencyCheck
{
    // Register this entity in your DbContext (usually in the BusinessObjects folder of your project) with the "public DbSet<ConcurrencyObject> ConcurrencyObjects { get; set; }" syntax.
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("Name")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    // You do not need to implement the INotifyPropertyChanged interface - EF Core implements it automatically.
    // (see https://learn.microsoft.com/en-us/ef/core/change-tracking/change-detection#change-tracking-proxies for details).
    public class ConcurrencyObject : BaseObject
    {
        public ConcurrencyObject()
        {
            // In the constructor, initialize collection properties, e.g.: 
            // this.AssociatedEntities = new ObservableCollection<AssociatedEntityObject>();
        }
        public virtual string Name { get; set; }

        //HACK manual concurrency token
        //When you read a record from the database, Entity Framework Core will keep a copy of the original values.When you try to update or delete that record,
        //EF Core generates a SQL statement that includes a WHERE clause for all original values marked with the [ConcurrencyCheck] attribute and for the primary key.



        //It's important to note that while the [ConcurrencyCheck] attribute is useful, the [Timestamp] attribute is often a better option for handling concurrency. The [Timestamp] attribute causes EF Core to include that property in the WHERE clause in the same way, but it's usually used with a rowversion column in SQL Server, which is automatically updated every time a row is modified.So you don't need to guess which properties might be changed by another user, the rowversion is guaranteed to change.
        //For most cases, using [Timestamp] on a rowversion column is the best practice for handling concurrency with EF Core.However, the[ConcurrencyCheck] attribute is a useful tool when rowversion is not suitable, or when you want to include business-logic-specific properties in the concurrency check.

        //[ConcurrencyCheck]
        //public virtual int MyProperty { get; set; } // This can be a property you expect to change frequently

        //HACK by just adding the Timestamp attribute to the property, it will be automatically updated by EF Core

        [Timestamp]
        public virtual byte[] Timestamp { get; set; } // Or a Timestamp which will be automatically changed every time a row is updated.


    }


}