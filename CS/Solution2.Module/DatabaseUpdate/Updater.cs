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

            if (ObjectSpace.GetObjectsCount(typeof(ExampleObject), null) == 0) {
                for (int i = 0; i < 5; i++) {
                    ReferencedObject refObject = ObjectSpace.CreateObject<ReferencedObject>();
                    refObject.Name = string.Format("Owner Object {0:d}", i + 1);
                    ExampleObject exampleObj = ObjectSpace.CreateObject<ExampleObject>();
                    exampleObj.Name = string.Format("Example object {0:d}", i + 1);
                    exampleObj.LookupReferencedObject = refObject;
                }
                ObjectSpace.CommitChanges();
            }
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
    }
}
