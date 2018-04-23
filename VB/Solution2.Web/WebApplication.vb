Imports System
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Web
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Xpo

Namespace MySolution.Web
    ' For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    Partial Public Class Solution2AspNetApplication
        Inherits WebApplication

        Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
        Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
        Private module3 As MySolution.Module.Solution2Module
        Private module4 As MySolution.Module.Web.Solution2AspNetModule

        Public Sub New()
            InitializeComponent()
            Me.LinkNewObjectToParentImmediately = False
        End Sub
        Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
            CreateXPObjectSpaceProvider(args.ConnectionString, args)
            args.ObjectSpaceProviders.Add(New NonPersistentObjectSpaceProvider(TypesInfo, Nothing))
        End Sub
        Private Sub CreateXPObjectSpaceProvider(ByVal connectionString As String, ByVal e As CreateCustomObjectSpaceProviderEventArgs)
            Dim application As System.Web.HttpApplicationState = If(System.Web.HttpContext.Current IsNot Nothing, System.Web.HttpContext.Current.Application, Nothing)
            Dim dataStoreProvider As IXpoDataStoreProvider = Nothing
            If application IsNot Nothing AndAlso application("DataStoreProvider") IsNot Nothing Then
                dataStoreProvider = TryCast(application("DataStoreProvider"), IXpoDataStoreProvider)
                e.ObjectSpaceProviders.Add(New XPObjectSpaceProvider(dataStoreProvider, True))
            Else
                If Not String.IsNullOrEmpty(connectionString) Then
                    connectionString = DevExpress.Xpo.XpoDefault.GetConnectionPoolString(connectionString)
                    dataStoreProvider = New ConnectionStringDataStoreProvider(connectionString, True)
                ElseIf e.Connection IsNot Nothing Then
                    dataStoreProvider = New ConnectionDataStoreProvider(e.Connection)
                End If
                If application IsNot Nothing Then
                    application("DataStoreProvider") = dataStoreProvider
                End If
                e.ObjectSpaceProviders.Add(New XPObjectSpaceProvider(dataStoreProvider, True))
            End If
        End Sub
        Private Sub Solution2AspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
            e.Updater.Update()
            e.Handled = True
#Else
            If System.Diagnostics.Debugger.IsAttached Then
                e.Updater.Update()
                e.Handled = True
            Else
                Dim message As String = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & ControlChars.CrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & ControlChars.CrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & ControlChars.CrLf & "Anyway, refer to the following help topics for more detailed information:" & ControlChars.CrLf & "'Update Application and Database Versions' at https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112795" & ControlChars.CrLf & "'Database Security References' at https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument113237" & ControlChars.CrLf & "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/"

                If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
                    message &= ControlChars.CrLf & ControlChars.CrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
                End If
                Throw New InvalidOperationException(message)
            End If
#End If
        End Sub
        Private Sub InitializeComponent()
            Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
            Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
            Me.module3 = New MySolution.Module.Solution2Module()
            Me.module4 = New MySolution.Module.Web.Solution2AspNetModule()
            DirectCast(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            ' 
            ' Solution2AspNetApplication
            ' 
            Me.ApplicationName = "MySolution"
            Me.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema
            Me.Modules.Add(Me.module1)
            Me.Modules.Add(Me.module2)
            Me.Modules.Add(Me.module3)
            Me.Modules.Add(Me.module4)
'            Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.Solution2AspNetApplication_DatabaseVersionMismatch)
            DirectCast(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
    End Class
End Namespace
