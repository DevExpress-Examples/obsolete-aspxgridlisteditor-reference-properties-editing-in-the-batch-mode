Imports DevExpress.Web
Imports MySolution.Module.BusinessObjects
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace MySolution.Module.Web.Editors
    Friend Class ReferencedTemplate
        Implements ITemplate

        'Handle the editor's client-side event to emulate the behavior of standard ASPxClientTextEdit.KeyDown grid editor. 
        Private Const BatchEditKeyDown As String = "function(s, e) {" & ControlChars.CrLf & _
"                var keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);" & ControlChars.CrLf & _
"                if (keyCode !== ASPx.Key.Tab && keyCode !== ASPx.Key.Enter) " & ControlChars.CrLf & _
"                    return;" & ControlChars.CrLf & _
"                var moveActionName = e.htmlEvent.shiftKey ? 'MoveFocusBackward' : 'MoveFocusForward';" & ControlChars.CrLf & _
"                var clientGridView = s.grid;" & ControlChars.CrLf & _
"                if (clientGridView.batchEditApi[moveActionName]()) {" & ControlChars.CrLf & _
"                    ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);" & ControlChars.CrLf & _
"                    window.batchPreventEndEditOnLostFocus = true;" & ControlChars.CrLf & _
"                }" & ControlChars.CrLf & _
"            }"
        'Handle the editor's client-side event to emulate the behavior of standard ASPxClientEdit.LostFocus grid editor. 
        Private Const BatchEditLostFocus As String = "function (s, e) {" & ControlChars.CrLf & _
"                var clientGridView = s.grid;" & ControlChars.CrLf & _
"                if (!window.batchPreventEndEditOnLostFocus)" & ControlChars.CrLf & _
"                    clientGridView.batchEditApi.EndEdit();" & ControlChars.CrLf & _
"                window.batchPreventEndEditOnLostFocus = false;" & ControlChars.CrLf & _
"            }"
        Private privateObjects As IEnumerable(Of ReferencedObject)
        Public Property Objects() As IEnumerable(Of ReferencedObject)
            Get
                Return privateObjects
            End Get
            Private Set(ByVal value As IEnumerable(Of ReferencedObject))
                privateObjects = value
            End Set
        End Property
        Public Sub New(ByVal objects As IEnumerable(Of ReferencedObject))
            Me.Objects = objects
        End Sub
        Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim gridContainer As GridViewEditItemTemplateContainer = CType(container, GridViewEditItemTemplateContainer)

            Dim comboBox As New ASPxComboBox()
            comboBox.Width = Unit.Percentage(100)
            comboBox.ClientInstanceName = "ReferencedEdit"
            comboBox.ClientSideEvents.KeyDown = BatchEditKeyDown

            comboBox.ClientSideEvents.LostFocus = BatchEditLostFocus

            Dim notAssignedItem As New ListEditItem("N/A", Nothing)
            comboBox.Items.Add(notAssignedItem)

            For Each currentObject In Objects
                Dim item As New ListEditItem(currentObject.Name, currentObject.Oid)
                comboBox.Items.Add(item)
            Next currentObject

            container.Controls.Add(comboBox)
        End Sub
    End Class
End Namespace
