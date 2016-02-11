Public Class UserState

#Region "FIELDS AND PROPERTIES"

    'FIELDS AND PROPERTIES

    Private _SecureConnection As Boolean

#End Region

#Region "PROPERTIES"

    'PROPERTIES

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
