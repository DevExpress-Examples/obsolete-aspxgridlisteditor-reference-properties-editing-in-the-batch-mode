Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace MySolution.Module.BusinessObjects
    <DefaultClassOptions> _
    Public Class ExampleObject
        Inherits BaseObject


        Private name_Renamed As String

        Private lookupReferencedObject_Renamed As ReferencedObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Public Property Name() As String
            Get
                Return name_Renamed
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Name", name_Renamed, value)
            End Set
        End Property

        Public Property LookupReferencedObject() As ReferencedObject
            Get
                Return lookupReferencedObject_Renamed
            End Get
            Set(ByVal value As ReferencedObject)
                SetPropertyValue("LookupReferencedObject", lookupReferencedObject_Renamed, value)
            End Set
        End Property
    End Class
    <DefaultClassOptions> _
    Public Class ReferencedObject
        Inherits XPObject


        Private name_Renamed As String
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Public Property Name() As String
            Get
                Return name_Renamed
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Name", name_Renamed, value)
            End Set
        End Property
    End Class
End Namespace
