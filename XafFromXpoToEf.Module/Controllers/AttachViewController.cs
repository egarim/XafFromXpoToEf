using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XafFromXpoToEf.Module.BusinessObjects.ConcurrencyCheck;
using Microsoft.EntityFrameworkCore;
namespace XafFromXpoToEf.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AttachViewController : ViewController
    {
        SimpleAction AttachObject;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public AttachViewController()
        {
            InitializeComponent();
            AttachObject = new SimpleAction(this, "Attach object", "View");
            AttachObject.Execute += AttachObject_Execute;
            
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void AttachObject_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // Initialize the database with test data
            var OsForInitialObject = this.Application.CreateObjectSpace(typeof(ConcurrencyObject));
            var Instance = OsForInitialObject.CreateObject<ConcurrencyObject>();
            Instance.Name = "Attach";
            OsForInitialObject.CommitChanges();
            var ObjectId= Instance.ID;
            byte[] timestamp = Instance.Timestamp;
            
            
            
            //Attaching object

            var OtherOs = this.Application.CreateObjectSpace(typeof(ConcurrencyObject)) as EFCoreObjectSpace;

            ConcurrencyObject entity = OtherOs.DbContext.CreateProxy<ConcurrencyObject>();
            
            entity.ID = ObjectId;          
            entity.Timestamp = timestamp;

            var AttachedEntity = OtherOs.DbContext.Attach(entity);

            OtherOs.Delete(entity);

            OtherOs.CommitChanges();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
