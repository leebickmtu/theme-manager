Imports System
Imports System.Collections

Public Class Structures
    Public Structure aTheme : Implements IComparable
        Private _name As String
        Private _theme As String
        Private _explorer As String
        Private _oobefldr As String
        Private _explorerframe As String
        Private _shell32 As String
        Private _timedate As String
        Private _logon As String
        Private _blur As Boolean

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim otherTheme As aTheme = obj
            Return _name.CompareTo(otherTheme.Name)
        End Function

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Theme() As String
            Get
                Return _theme
            End Get
            Set(ByVal value As String)
                _theme = value
            End Set
        End Property

        Public Property Explorer() As String
            Get
                Return _explorer
            End Get
            Set(ByVal value As String)
                _explorer = value
            End Set
        End Property

        Public Property OobeFldr() As String
            Get
                Return _oobefldr
            End Get
            Set(ByVal value As String)
                _oobefldr = value
            End Set
        End Property

        Public Property ExplorerFrame() As String
            Get
                Return _explorerframe
            End Get
            Set(ByVal value As String)
                _explorerframe = value
            End Set
        End Property

        Public Property Shell32() As String
            Get
                Return _shell32
            End Get
            Set(ByVal value As String)
                _shell32 = value
            End Set
        End Property

        Public Property TimeDate() As String
            Get
                Return _timedate
            End Get
            Set(ByVal value As String)
                _timedate = value
            End Set
        End Property

        Public Property Logon() As String
            Get
                Return _logon
            End Get
            Set(ByVal value As String)
                _logon = value
            End Set
        End Property

        Public Property Blur() As Boolean
            Get
                Return _blur
            End Get
            Set(ByVal value As Boolean)
                _blur = value
            End Set
        End Property

    End Structure

End Class
