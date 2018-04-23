using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;
using DevExpress.Xpo;
using MySolution.Module.BusinessObjects;
using MySolution.Module.Web.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Module.Web.Controllers {
    public class CreateCustomEditItemTemplateController : ViewController<ListView> {
        //Handle the client-side event to set the grid's cell values to the editor.
        const string BatchEditStartEditing =
            @"function(s,e) {     
                var productNameColumn = s.GetColumnByField('LookupReferencedObject.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                ReferencedEdit.SetText(cellInfo.text);
                ReferencedEdit.SetValue(cellInfo.value);
                ReferencedEdit['grid'] = s;
                if (e.focusedColumn === productNameColumn) {
                    ReferencedEdit.SetFocus();
                }
            }";
        //Handle the event to pass the value from the editor to the grid cell.
        const string BatchEditEndEditing =
            @"function(s,e){ 
                var productNameColumn = s.GetColumnByField('LookupReferencedObject.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = ReferencedEdit.GetValue();
                cellInfo.text = ReferencedEdit.GetText();
                ReferencedEdit.SetValue(null);
            }";
        public CreateCustomEditItemTemplateController() {
            TargetObjectType = typeof(ExampleObject);
        }
        protected override void OnActivated() {
            base.OnActivated();
            ASPxGridListEditor listEditor = (ASPxGridListEditor)View.Editor;
            listEditor.CreateCustomEditItemTemplate += listEditor_CreateCustomEditItemTemplate;
            listEditor.CreateCustomGridViewDataColumn += listEditor_CreateCustomGridViewDataColumn;
        }
        protected override void OnDeactivated() {
            ASPxGridListEditor listEditor = (ASPxGridListEditor)View.Editor;
            listEditor.CreateCustomEditItemTemplate -= listEditor_CreateCustomEditItemTemplate;
            listEditor.CreateCustomGridViewDataColumn -= listEditor_CreateCustomGridViewDataColumn;
            base.OnDeactivated();
        }
        private void listEditor_CreateCustomGridViewDataColumn(object sender, CreateCustomGridViewDataColumnEventArgs e) {
            if (e.ModelColumn.PropertyEditorType == typeof(ASPxLookupPropertyEditor)) {
                if (e.ModelColumn.PropertyName == "LookupReferencedObject") {
                    var gridColumn = new GridViewDataComboBoxColumn();
                    gridColumn.Name = e.ModelColumn.PropertyName;
                    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    gridColumn.PropertiesComboBox.TextField = "Name";
                    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ReferencedObject>();
                    e.Column = gridColumn;
                }
            }
        }
        private void listEditor_CreateCustomEditItemTemplate(object sender, CreateCustomEditItemTemplateEventArgs e) {
            if (e.ModelColumn.PropertyName == "LookupReferencedObject") {
                IEnumerable<ReferencedObject> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ReferencedObject), null, new SortProperty[] { new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ReferencedObject>();
                e.Template = new ReferencedTemplate(referencedObjectsList);
                e.Handled = true;
            }
        }
        protected void BatchValueIsUpdated(object sender, CustomUpdateBatchValueEventArgs e) {
            if (e.PropertyName == "LookupReferencedObject.Oid") {
                var exampleObject = e.Object as ExampleObject;
                if (e.NewValue == null) {
                    exampleObject.LookupReferencedObject = null;
                } else {
                    exampleObject.LookupReferencedObject = exampleObject.Session.GetObjectByKey<ReferencedObject>(e.NewValue);
                }
                e.Handled = true;
            }
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();

            ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
            gridListEditor.BatchEditModeHelper.CustomUpdateBatchValue += BatchValueIsUpdated;

            string Grid_BatchEditStartEditingKey = "BatchEditStartEditingKey";
            ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
            BatchEditStartEditing, Grid_BatchEditStartEditingKey);

            string Grid_BatchEditEndEditingKey = "BatchEditEndEditingKey";
            ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
            BatchEditEndEditing, Grid_BatchEditEndEditingKey);
        }
    } 
}
