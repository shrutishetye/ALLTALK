Public Class CompanyContact

    Private _CId As String
    Private _Language As String

    Public Property CId As String

        Get

            Return _CId

        End Get

        Set(value As String)

            _CId = value

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
