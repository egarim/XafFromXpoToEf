using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System;
using System.Linq;

namespace XafFromXpoToEf.Module.BusinessObjects.BaseObjectFunctionality
{
    [DefaultClassOptions()]
    public class MyBaseEfObject1 : MyBaseEfObject
    {
        public MyBaseEfObject1()
        {
            // In the constructor, initialize collection properties, e.g.: 
            // this.AssociatedEntities = new ObservableCollection<AssociatedEntityObject>();
        }
        public virtual string Name { get; set; }

    }
}