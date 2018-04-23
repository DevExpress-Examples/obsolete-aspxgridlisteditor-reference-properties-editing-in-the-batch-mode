Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace MySolution.Module.BusinessObjects
    <DefaultClassOptions>
    Public Class ExampleObject
        Inherits BaseObject

        Private _name As String
        Private _lookupReferencedObject As ReferencedObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Name", _name, value)
            End Set
        End Property

        Public Property LookupReferencedObject() As ReferencedObject
            Get
                Return _lookupReferencedObject
            End Get
            Set(ByVal value As ReferencedObject)
                SetPropertyValue("LookupReferencedObject", _lookupReferencedObject, value)
            End Set
        End Property
    End Class
    <DefaultClassOptions>
    Public Class ReferencedObject
        Inherits XPObject

        Private _name As String
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                SetPropertyValue("Name", _name, value)
            End Set
        End Property
    End Class
End Namespace
