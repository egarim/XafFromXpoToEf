using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XafFromXpoToEf.Module.BusinessObjects.ConcurrencyCheck;

namespace XafFromXpoToEf.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TestConcurrencyController : ViewController
    {
        SimpleAction TestConcurrency;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public TestConcurrencyController()
        {
            InitializeComponent();
            TestConcurrency = new SimpleAction(this, "Test Concurrency", "View");
            TestConcurrency.Execute += TestConcurrency_Execute;
            
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private  void TestConcurrency_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // Initialize the database with test data
            var OsForInitialObject=this.Application.CreateObjectSpace(typeof(ConcurrencyObject));
            var Instance= OsForInitialObject.CreateObject<ConcurrencyObject>();
            Instance.Name= "Test";
            OsForInitialObject.CommitChanges();


            // User 1 gets an instance of the entity

            var User1Os = this.Application.CreateObjectSpace(typeof(ConcurrencyObject));
            var user1Entity = User1Os.FindObject<ConcurrencyObject>(null);

            user1Entity.Name = "Test1"; // User 1 makes a change    

            var User2Os = this.Application.CreateObjectSpace(typeof(ConcurrencyObject));
            var user2Entity = User2Os.FindObject<ConcurrencyObject>(null);

            user2Entity.Name = "Test2"; // User 1 makes a change

            User1Os.CommitChanges();
            
            User2Os.CommitChanges();

            //// User 2 also gets an instance of the entity
            //using (var user2Context = new MyContext(options))
            //    {
            //        var user2Entity = await user2Context.YourEntities.SingleAsync(e => e.Id == 1);

            //        user2Entity.MyProperty = 1; // User 2 makes a change
            //        await user2Context.SaveChangesAsync(); // and saves it

            //        user1Entity.MyProperty = 2; // User 1 makes a different change

            //        // When user 1 tries to save, a DbUpdateConcurrencyException should be thrown
            //        var exceptionThrown = false;
            //        try
            //        {
            //            await user1Context.SaveChangesAsync();
            //        }
            //        catch (DbUpdateConcurrencyException)
            //        {
            //            exceptionThrown = true;
            //        }


            //    }

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
