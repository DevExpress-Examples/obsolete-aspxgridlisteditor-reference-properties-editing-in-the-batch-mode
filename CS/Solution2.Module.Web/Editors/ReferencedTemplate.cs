using DevExpress.Web;
using MySolution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MySolution.Module.Web.Editors {
    class ReferencedTemplate : ITemplate {
        //Handle the editor's client-side event to emulate the behavior of standard ASPxClientTextEdit.KeyDown grid editor. 
        const string BatchEditKeyDown =
            @"function(s, e) {
                var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
                if (keyCode !== ASPx.Key.Tab && keyCode !== ASPx.Key.Enter) 
                    return;
                var moveActionName = e.htmlEvent.shiftKey ? 'MoveFocusBackward' : 'MoveFocusForward';
                var clientGridView = s.grid;
                if (clientGridView.batchEditApi[moveActionName]()) {
                    ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
                    window.batchPreventEndEditOnLostFocus = true;
                }
            }";
        //Handle the editor's client-side event to emulate the behavior of standard ASPxClientEdit.LostFocus grid editor. 
        const string BatchEditLostFocus =
            @"function (s, e) {
                var clientGridView = s.grid;
                if (!window.batchPreventEndEditOnLostFocus)
                    clientGridView.batchEditApi.EndEdit();
                window.batchPreventEndEditOnLostFocus = false;
            }";
        public IEnumerable<ReferencedObject> Objects { get; private set; }
        public ReferencedTemplate(IEnumerable<ReferencedObject> objects) {
            Objects = objects;
        }
        public void InstantiateIn(Control container) {
            GridViewEditItemTemplateContainer gridContainer = (GridViewEditItemTemplateContainer)container;

            ASPxComboBox comboBox = new ASPxComboBox();
            comboBox.Width = Unit.Percentage(100);
            comboBox.ClientInstanceName = "ReferencedEdit";
            comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown;

            comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus;

            ListEditItem notAssignedItem = new ListEditItem("N/A", null);
            comboBox.Items.Add(notAssignedItem);

            foreach (var currentObject in Objects) {
                ListEditItem item = new ListEditItem(currentObject.Name, currentObject.Oid);
                comboBox.Items.Add(item);
            }

            container.Controls.Add(comboBox);
        }
    }
}
