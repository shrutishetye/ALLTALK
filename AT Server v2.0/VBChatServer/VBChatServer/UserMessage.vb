Public Class UserMessage

    Private _MsgId As String
    Private _Message As String
    Private _SenderLanguage As String

    Public Property MsgId As String

        Get

            Return _MsgId

        End Get

        Set(value As String)

            _MsgId = value

        End Set

    End Property

    Public Property Message As String

        Get

            Return _Message

        End Get

        Set(value As String)

            _Message = value

        End Set

    End Property

    Public Property SenderLanguage As String

        Get

            Return _SenderLanguage

        End Get

        Set(value As String)

            _SenderLanguage = value

        End Set
    End Property


End Class
