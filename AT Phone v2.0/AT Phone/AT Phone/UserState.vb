Public Class UserState

#Region "FIELDS AND PROPERTIES"

    'FIELDS AND PROPERTIES

    Private _SecureConnection As Boolean

    Private _FName As String
    Private _LName As String
    Private _Email As String

    Private _ATPin As String
    Enum PageNavigationHeader As Byte

        MainPage = 1
        ATPinPage = 2

    End Enum

    Private Shared _PageNav As Byte

#End Region

#Region "PROPERTIES"

    'PROPERTIES

    Public Property ATPin As String
        Get
            Return _ATPin
        End Get
        Set(value As String)
            _ATPin = value
        End Set
    End Property

    Public Property FName As String

        Get

            Return _FName

        End Get
        Set(value As String)

            _FName = value
        End Set
    End Property

    Public Property LName As String

        Get
            Return _LName
        End Get
        Set(value As String)
            _LName = value
        End Set
    End Property

    Public Property Email As String

        Get
            Return _Email
        End Get
        Set(value As String)
            _Email = value
        End Set
    End Property


    Public Shared Property PageNav As Byte

        Get

            Return _PageNav

        End Get
        Set(value As Byte)

            _PageNav = value

        End Set

    End Property

    Public Property SecureConnection As Boolean

        Get

            Return _SecureConnection

        End Get

        Set(value As Boolean)

            _SecureConnection = value

        End Set

    End Property

#End Region

End Class
