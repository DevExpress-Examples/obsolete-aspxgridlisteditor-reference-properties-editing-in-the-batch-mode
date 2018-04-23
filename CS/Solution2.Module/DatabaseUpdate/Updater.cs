using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using MySolution.Module.BusinessObjects;

namespace MySolution.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            for(int i = 0; i < 5; i++) {
                string postfix = (i + 1).ToString();
                ReferencedObject refObject = ObjectSpace.CreateObject<ReferencedObject>();
                refObject.Name = "Owner Object " + postfix;

                ExampleObject exampleObj = ObjectSpace.CreateObject<ExampleObject>();
                exampleObj.Name = "Example object " + i.ToString();
                exampleObj.LookupReferencedObject = refObject;
            }

            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
    }
}
