using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Linq;
using XafFromXpoToEf.Module.BusinessObjects.BaseObjectFunctionality;
using XafFromXpoToEf.Module.BusinessObjects.ConcurrencyCheck;

namespace XafFromXpoToEf.Module.Controllers
{
    public partial class TestConcurrencyFromExtensionsController : ViewController
    {
        SimpleAction TestConcurrency;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public TestConcurrencyFromExtensionsController()
        {
            this.TargetObjectType=typeof(MyBaseEfObject1);
            TestConcurrency = new SimpleAction(this, "Test Concurrency from extension", "View");
            TestConcurrency.Execute += TestConcurrency_Execute;

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void TestConcurrency_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //NOT WORKING YET
            // Initialize the database with test data
            var OsForInitialObject = Application.CreateObjectSpace(typeof(MyBaseEfObject1));
            var Instance = OsForInitialObject.CreateObject<MyBaseEfObject1>();
            Instance.Name = "Test";
            OsForInitialObject.CommitChanges();


            // User 1 gets an instance of the entity

            var User1Os = Application.CreateObjectSpace(typeof(MyBaseEfObject1)); 
            var user1Entity = User1Os.FindObject<MyBaseEfObject1>(null);

            user1Entity.Name = "Test1"; // User 1 makes a change    

            var User2Os = Application.CreateObjectSpace(typeof(MyBaseEfObject1));
            var user2Entity = User2Os.FindObject<MyBaseEfObject1>(null);

            user2Entity.Name = "Test2"; // User 2 makes a change

            User1Os.CommitChanges();

            User2Os.CommitChanges();

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
