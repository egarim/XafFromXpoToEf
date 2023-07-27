using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace XafFromXpoToEf.Module.BusinessObjects.ChangeNotification
{
    // Register this entity in your DbContext (usually in the BusinessObjects folder of your project) with the "public DbSet<EntityObject1> EntityObject1s { get; set; }" syntax.
    [DefaultClassOptions]
    public class SimplePersonWithCustomNotificationTrigger : INotifyPropertyChanged, INotifyPropertyChanging
    {


        public virtual int Id
        {
            get => id;
            set
            {
                if (id == value)
                    return;
                id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        string lastName;
        string name;
        int id;

        public virtual string Name
        {
            get => name;
            set => SetPropertyValue<string>(nameof(Name), ref name, value);
        }
        void OnChanged(string PropertyName, object OldValue, object NewValue)
        {

        }
        void SetPropertyValue<T>(string PropertyName, ref T OldValue, T NewValue)
        {
            if (EqualityComparer<T>.Default.Equals(OldValue, NewValue))
                return;
            OnChanged(PropertyName, OldValue, NewValue);
            OldValue = NewValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public virtual string LastName
        {
            get => lastName;
            set => SetPropertyValue<string>(nameof(LastName), ref lastName, value);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;
    }
}