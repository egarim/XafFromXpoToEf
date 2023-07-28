using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.Linq;

namespace XafFromXpoToEf.Module.BusinessObjects.BaseObjectFunctionality
{
    [DefaultClassOptions()]
    public class MyBaseEfObject2 : MyBaseEfObject
    {
        public MyBaseEfObject2()
        {
            // In the constructor, initialize collection properties, e.g.: 
            // this.AssociatedEntities = new ObservableCollection<AssociatedEntityObject>();
        }
        public virtual string Name { get; set; }

    }
}