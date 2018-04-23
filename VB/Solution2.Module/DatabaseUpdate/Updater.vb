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

            If ObjectSpace.GetObjectsCount(GetType(ExampleObject), Nothing) = 0 Then
                For i As Integer = 0 To 4
                    Dim refObject As ReferencedObject = ObjectSpace.CreateObject(Of ReferencedObject)()
                    refObject.Name = String.Format("Owner Object {0:d}", i + 1)
                    Dim exampleObj As ExampleObject = ObjectSpace.CreateObject(Of ExampleObject)()
                    exampleObj.Name = String.Format("Example object {0:d}", i + 1)
                    exampleObj.LookupReferencedObject = refObject
                Next i
                ObjectSpace.CommitChanges()
            End If
        End Sub
        Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
            MyBase.UpdateDatabaseBeforeUpdateSchema()
        End Sub
    End Class
End Namespace
