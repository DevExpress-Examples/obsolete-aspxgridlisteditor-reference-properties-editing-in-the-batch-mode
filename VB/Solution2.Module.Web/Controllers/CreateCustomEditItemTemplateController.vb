Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Web.Editors.ASPx
Imports DevExpress.ExpressApp.Web.Utils
Imports DevExpress.Web
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
"                var productNameColumn = s.GetColumnByField('LookupReferencedObject.Oid');" & ControlChars.CrLf & _
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
"                var productNameColumn = s.GetColumnByField('LookupReferencedObject.Oid');" & ControlChars.CrLf & _
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
            AddHandler listEditor.CreateCustomGridViewDataColumn, AddressOf listEditor_CreateCustomGridViewDataColumn
        End Sub
        Protected Overrides Sub OnDeactivated()
            Dim listEditor As ASPxGridListEditor = CType(View.Editor, ASPxGridListEditor)
            RemoveHandler listEditor.CreateCustomEditItemTemplate, AddressOf listEditor_CreateCustomEditItemTemplate
            RemoveHandler listEditor.CreateCustomGridViewDataColumn, AddressOf listEditor_CreateCustomGridViewDataColumn
            MyBase.OnDeactivated()
        End Sub
        Private Sub listEditor_CreateCustomGridViewDataColumn(ByVal sender As Object, ByVal e As CreateCustomGridViewDataColumnEventArgs)
            If e.ModelColumn.PropertyEditorType Is GetType(ASPxLookupPropertyEditor) Then
                If e.ModelColumn.PropertyName = "LookupReferencedObject" Then
                    Dim gridColumn = New GridViewDataComboBoxColumn()
                    gridColumn.Name = e.ModelColumn.PropertyName
                    gridColumn.FieldName = e.ModelColumn.PropertyName & ".Oid"
                    gridColumn.PropertiesComboBox.ValueType = GetType(Integer?)
                    gridColumn.PropertiesComboBox.ValueField = "Oid"
                    gridColumn.PropertiesComboBox.TextField = "Name"
                    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects(Of ReferencedObject)()
                    e.Column = gridColumn
                End If
            End If
        End Sub
        Private Sub listEditor_CreateCustomEditItemTemplate(ByVal sender As Object, ByVal e As CreateCustomEditItemTemplateEventArgs)
            If e.ModelColumn.PropertyName = "LookupReferencedObject" Then
                Dim referencedObjectsList As IEnumerable(Of ReferencedObject) = ObjectSpace.CreateCollection(GetType(ReferencedObject), Nothing, New SortProperty() { New SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast(Of ReferencedObject)()
                e.Template = New ReferencedTemplate(referencedObjectsList)
                e.Handled = True
            End If
        End Sub
        Protected Sub BatchValueIsUpdated(ByVal sender As Object, ByVal e As CustomUpdateBatchValueEventArgs)
            If e.PropertyName = "LookupReferencedObject.Oid" Then
                Dim exampleObject = TryCast(e.Object, ExampleObject)
                If e.NewValue Is Nothing Then
                    exampleObject.LookupReferencedObject = Nothing
                Else
                    exampleObject.LookupReferencedObject = exampleObject.Session.GetObjectByKey(Of ReferencedObject)(e.NewValue)
                End If
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
