Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Web.Editors.ASPx
Imports DevExpress.ExpressApp.Web.Utils
Imports DevExpress.Xpo
Imports MySolution.Module.BusinessObjects
Imports MySolution.Module.Web.Editors
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace MySolution.Module.Web.Controllers
    Public Class CreateCustomEditItemTemplateController
        Inherits ViewController(Of ListView)

        'Handle the client-side event to set the grid's cell values to the editor.
        Private Const BatchEditStartEditing As String = "function(s,e) {     " & ControlChars.CrLf & _
"                var productNameColumn = s.GetColumnByField('LookupReferencedObject.Name');" & ControlChars.CrLf & _
"                if (!e.rowValues.hasOwnProperty(productNameColumn.index))" & ControlChars.CrLf & _
"                    return;" & ControlChars.CrLf & _
"                var cellInfo = e.rowValues[productNameColumn.index];" & ControlChars.CrLf & _
"                ReferencedEdit.SetText(cellInfo.text);" & ControlChars.CrLf & _
"                ReferencedEdit.SetValue(cellInfo.value);" & ControlChars.CrLf & _
"                ReferencedEdit['grid'] = s;" & ControlChars.CrLf & _
"                if (e.focusedColumn === productNameColumn) {" & ControlChars.CrLf & _
"                    ReferencedEdit.SetFocus();" & ControlChars.CrLf & _
"                }" & ControlChars.CrLf & _
"            }"
        'Handle the event to pass the value from the editor to the grid cell.
        Private Const BatchEditEndEditing As String = "function(s,e){ " & ControlChars.CrLf & _
"                var productNameColumn = s.GetColumnByField('LookupReferencedObject.Name');" & ControlChars.CrLf & _
"                if (!e.rowValues.hasOwnProperty(productNameColumn.index))" & ControlChars.CrLf & _
"                    return;" & ControlChars.CrLf & _
"                var cellInfo = e.rowValues[productNameColumn.index];" & ControlChars.CrLf & _
"                cellInfo.value = ReferencedEdit.GetValue();" & ControlChars.CrLf & _
"                cellInfo.text = ReferencedEdit.GetText();" & ControlChars.CrLf & _
"                ReferencedEdit.SetValue(null);" & ControlChars.CrLf & _
"            }"
        Public Sub New()
            TargetObjectType = GetType(ExampleObject)
        End Sub
        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            Dim listEditor As ASPxGridListEditor = CType(View.Editor, ASPxGridListEditor)
            AddHandler listEditor.CreateCustomEditItemTemplate, AddressOf listEditor_CreateCustomEditItemTemplate
        End Sub
        Protected Overrides Sub OnDeactivated()
            Dim listEditor As ASPxGridListEditor = CType(View.Editor, ASPxGridListEditor)
            RemoveHandler listEditor.CreateCustomEditItemTemplate, AddressOf listEditor_CreateCustomEditItemTemplate
            MyBase.OnDeactivated()
        End Sub
        Private Sub listEditor_CreateCustomEditItemTemplate(ByVal sender As Object, ByVal e As CreateCustomEditItemTemplateEventArgs)
            Dim columnCaption As String = e.ModelColumn.Caption
            If columnCaption = "Lookup Referenced Object" Then
                Dim referencedObjectsList As IEnumerable(Of ReferencedObject) = ObjectSpace.CreateCollection(GetType(ReferencedObject), Nothing, New SortProperty() { New SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast(Of ReferencedObject)()
                e.Template = New ReferencedTemplate(referencedObjectsList)
                e.Handled = True
            End If
        End Sub
        Protected Sub BatchValueIsUpdated(ByVal sender As Object, ByVal e As CustomUpdateBatchValueEventArgs)
            If e.PropertyName = "LookupReferencedObject.Name" Then
                Dim exampleObject = TryCast(e.Object, ExampleObject)
                Dim newObjectKey As String = CStr(e.NewValue)
                Dim setToNotAssigned As Boolean = newObjectKey = "null"
                Dim newReferencedObjectValue = If(setToNotAssigned, Nothing, exampleObject.Session.GetObjectByKey(Of ReferencedObject)(Integer.Parse(newObjectKey)))
                exampleObject.LookupReferencedObject = newReferencedObjectValue
                e.Handled = True
            End If
        End Sub
        Protected Overrides Sub OnViewControlsCreated()
            MyBase.OnViewControlsCreated()

            Dim gridListEditor As ASPxGridListEditor = TryCast(View.Editor, ASPxGridListEditor)
            AddHandler gridListEditor.BatchEditModeHelper.CustomUpdateBatchValue, AddressOf BatchValueIsUpdated

            Dim Grid_BatchEditStartEditingKey As String = "BatchEditStartEditingKey"
            ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditing, Grid_BatchEditStartEditingKey)

            Dim Grid_BatchEditEndEditingKey As String = "BatchEditEndEditingKey"
            ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditing, Grid_BatchEditEndEditingKey)
        End Sub
    End Class
End Namespace
