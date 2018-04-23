Imports System
Imports System.Linq
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl
Imports MySolution.Module.BusinessObjects

Namespace MySolution.Module.DatabaseUpdate
    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub
        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()

            For i As Integer = 0 To 4
                Dim postfix As String = (i + 1).ToString()
                Dim refObject As ReferencedObject = ObjectSpace.CreateObject(Of ReferencedObject)()
                refObject.Name = "Owner Object " & postfix

                Dim exampleObj As ExampleObject = ObjectSpace.CreateObject(Of ExampleObject)()
                exampleObj.Name = "Example object " & i.ToString()
                exampleObj.LookupReferencedObject = refObject
            Next i

            ObjectSpace.CommitChanges()
        End Sub
        Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
            MyBase.UpdateDatabaseBeforeUpdateSchema()
        End Sub
    End Class
End Namespace
