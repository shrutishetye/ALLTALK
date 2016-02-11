'REQUIRES ADDITION OF FOLLOWING REFERENCES
'1.RUNTIME.SERIALIZATION
'2.WEB
'3.SERVICEMODEL

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Web
Imports System.ServiceModel.Channels
Imports System.ServiceModel
Imports System.Threading


Public Class Token

#Region "FIELDS AND OBJECTS"

    'FIELDS AND MESSAGE//

    Public Class AdmAccessToken

        Private m_access_token As String

        Public Property access_token() As String

            Get

                Return m_access_token

            End Get

            Set(value As String)

                m_access_token = value

            End Set

        End Property

        Private m_token_type As String

        Public Property token_type() As String

            Get

                Return m_token_type

            End Get

            Set(value As String)

                m_token_type = value

            End Set

        End Property


        Private m_expires_in As String

        Public Property expires_in() As String

            Get

                Return m_expires_in

            End Get

            Set(value As String)

                m_expires_in = value

            End Set
        End Property


        Private m_scope As String

        Public Property scope() As String

            Get

                Return m_scope

            End Get

            Set(value As String)

                m_scope = value

            End Set

        End Property


        Private m__AccessToken As String

        Public Property _AccessToken() As String

            Get

                Return m__AccessToken

            End Get

            Set(value As String)

                m__AccessToken = value

            End Set
        End Property


    End Class

#End Region


    Public Class AdmAuthentication

#Region "FIELDS AND OBJECTS"

        'FIELDS AND MESSAGE

        Public Shared ReadOnly _DatamarketAccessUri As String = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13"
        Private _ClientId As String
        Private _ClientSecret As String
        Private _Request As String
        Private _Token As AdmAccessToken
        Private _AccessTokenRenewer As Timer
        'ACCESS TOKEN RENEWAL DURATION IN MINUTES
        'ACCESS TOKEN EXPIRES IN 10 MINUTES
        Private Const _RefreshTokenDuration As Integer = 1

#End Region

#Region "METHODS"

        'METHODS

#Region "CONSTRUCTOR"

        'CONSTRUCTOR
        Public Sub New(_ClientId As String, _ClientSecret As String)

            Me._ClientId = _ClientId
            Me._ClientSecret = _ClientSecret

            'IF CLIENT ID OR CLIENT SECRET HAS SPECIAL CHARACTER ENCODE FOR SENDING
            Me._Request = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(_ClientId), HttpUtility.UrlEncode(_ClientSecret))
            Me._Token = HttpPost(_DatamarketAccessUri, Me._Request)

            'RENEW THE TOKEN EVERY SPECIFIED MINUTE
            _AccessTokenRenewer = New Timer(New TimerCallback(AddressOf OnTokenExpiredCallback), Me, TimeSpan.FromMinutes(_RefreshTokenDuration), TimeSpan.FromMilliseconds(-1))

        End Sub

#End Region

#Region "HELPER METHODS"

        'HELPER METHODS

        Public Function GetAccessToken() As AdmAccessToken

            Return Me._Token

        End Function


        Private Sub RenewAccessToken()

            Dim newAccessToken As AdmAccessToken = HttpPost(_DatamarketAccessUri, Me._Request)
            'SWAP THE NEW TOKEN WITH OLD ONE
            'NOTE: THE SWAP IS THREAD UNSAFE
            Me._Token = newAccessToken
            System.Windows.Forms.MessageBox.Show(String.Format("Renewed token for user: {0} is: {1}", Me._ClientId, Me._Token.access_token))

        End Sub

        Private Sub OnTokenExpiredCallback(_StateInfo As Object)

            Try

                RenewAccessToken()

            Catch ex As Exception

                System.Windows.Forms.MessageBox.Show(String.Format("Failed renewing access token. Details: {0}", ex.Message))

            Finally

                Try

                    _AccessTokenRenewer.Change(TimeSpan.FromMinutes(_RefreshTokenDuration), TimeSpan.FromMilliseconds(-1))

                Catch ex As Exception

                    System.Windows.Forms.MessageBox.Show(String.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message))

                End Try
            End Try

        End Sub


        Private Function HttpPost(_DatamarketAccessUri As String, _RequestDetails As String) As AdmAccessToken

            'PREPARE OAUTH REQUEST
            Dim _WebRequest As WebRequest = WebRequest.Create(_DatamarketAccessUri)
            _WebRequest.ContentType = "application/x-www-form-urlencoded"
            _WebRequest.Method = "POST"
            Dim bytes As Byte() = Encoding.ASCII.GetBytes(_RequestDetails)
            _WebRequest.ContentLength = bytes.Length
            Using _OutputStream As Stream = _WebRequest.GetRequestStream()
                _OutputStream.Write(bytes, 0, bytes.Length)
            End Using
            Using webResponse As WebResponse = _WebRequest.GetResponse()
                Dim serializer As New DataContractJsonSerializer(GetType(AdmAccessToken))

                'GET DESERIALIZED OBJECT FROM JSON
                Dim _Token As AdmAccessToken = DirectCast(serializer.ReadObject(webResponse.GetResponseStream()), AdmAccessToken)
                Return _Token
            End Using

        End Function

#End Region

#End Region

    End Class

End Class
