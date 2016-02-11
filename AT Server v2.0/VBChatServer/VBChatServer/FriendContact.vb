Public Class FriendContact

    Private _FName As String
    Private _Language As String

    Public Property FName As String

        Get

            Return _FName

        End Get

        Set(value As String)

            _FName = value

        End Set

    End Property

    Public Property Language As String

        Get

            Return _Language

        End Get

        Set(value As String)

            _Language = value

        End Set

    End Property

End Class
