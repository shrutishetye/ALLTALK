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
Imports System.Media

Public Class Translator


#Region "FIELDS AND OBJECTS"

    'FIELDS AND OBJECTS

    Private _AdmToken As Token.AdmAccessToken
    Private _HeaderValue As String
    Private m__Response As WebResponse

#End Region

#Region "PROPERTIES"

    'PROPERTIES

    Public ReadOnly Property HeaderValue() As String

        Get

            Return _HeaderValue

        End Get

    End Property

    Public Property _Response() As WebResponse

        Get

            Return m__Response

        End Get

        Set(value As WebResponse)
            m__Response = value

        End Set

    End Property



#End Region

#Region "METHODS"

    'METHODS

#Region "CONSTRUCTOR"

    'COSTRUCTOR

    Public Sub New()


        'INSERT CLIENT ID AND CLIENT SECRET FOR CONNECTION
        Dim _AdmAuth As New Token.AdmAuthentication("rsa1101", "XjyPN5buwwwOx0gxXIAZ106tlwT7faHL/eoG/+m01rw=")

        Try
            _AdmToken = _AdmAuth.GetAccessToken()

            'CREATES HEADER WITH ACCESS TOKEN PROPERTY OF RETURNED HEADER
            _HeaderValue = "Bearer " + _AdmToken.access_token
            Console.WriteLine(_HeaderValue)
            System.Windows.Forms.MessageBox.Show(_HeaderValue)

        Catch e As WebException

            ProcessWebException(e)

        Catch ex As Exception

            System.Windows.Forms.MessageBox.Show(ex.Message)

        End Try

    End Sub

#End Region

#Region "HELPER METHODS"

    'HELPER METHODS

    'PROCCESSES THE WEB EXCEPTION THROWN DUE TO TOKEN CREATION
    Private Shared Sub ProcessWebException(e As WebException)

        System.Windows.Forms.MessageBox.Show("{0}", e.ToString())

        'OBTAIN DETAIL ERROR INFORMATION
        Dim strResponse As String = String.Empty

        Using _Response As HttpWebResponse = DirectCast(e.Response, HttpWebResponse)

            Using _ResponseStream As Stream = _Response.GetResponseStream()

                Using _Sr As New StreamReader(_ResponseStream, System.Text.Encoding.ASCII)
                    strResponse = _Sr.ReadToEnd()

                End Using

            End Using

        End Using

        System.Windows.Forms.MessageBox.Show([String].Format("Http status code={0}, error message={1}", e.Status, strResponse))

    End Sub

    'DETECTS THE LANGUAGE OF THE TEXT PROVIDED AS INPUT PARAMETER
    Public Function DetectMethod(_AuthToken As String, _Message As String) As String

        'KEEPS APP ID PARAMETER BLANK AS WE ARE SENDING ACCESS TOKEN IN HEADER
        Dim _Uri As String = Convert.ToString("http://api.microsofttranslator.com/v2/Http.svc/Detect?text=") & _Message
        Dim _HttpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(_Uri), HttpWebRequest)
        _HttpWebRequest.Headers.Add("Authorization", _AuthToken)
        Dim _Response As WebResponse = Nothing

        Try

            _Response = _HttpWebRequest.GetResponse()
            Using stream As Stream = _Response.GetResponseStream()

                Dim _Dcs As New System.Runtime.Serialization.DataContractSerializer(Type.[GetType]("System.String"))
                Dim _LanguageDetected As String = DirectCast(_Dcs.ReadObject(stream), String)
                Return _LanguageDetected

            End Using

        Catch

            Throw

        Finally

            If _Response IsNot Nothing Then

                _Response.Close()
                _Response = Nothing

            End If

        End Try

    End Function


    'GETS THE LIST OF LANGUAGES AVAILABLE FOR TRANSLATION
    Public Function GetLanguagesForTranslate(_AuthToken As String) As List(Of String)

        Dim _Uri As String = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForTranslate"
        Dim _HttpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(_Uri), HttpWebRequest)
        _HttpWebRequest.Headers.Add("Authorization", _AuthToken)
        Dim _Response As WebResponse = Nothing

        Try

            _Response = _HttpWebRequest.GetResponse()

            Using _Stream As Stream = _Response.GetResponseStream()

                Dim _Dcs As New System.Runtime.Serialization.DataContractSerializer(GetType(List(Of String)))

                Dim _LanguagesForTranslate As List(Of String) = DirectCast(_Dcs.ReadObject(_Stream), List(Of String))
                Return _LanguagesForTranslate

            End Using

        Catch
            Throw
        Finally

            If _Response IsNot Nothing Then

                _Response.Close()
                _Response = Nothing

            End If

        End Try

    End Function

    'GETS THE AVAILABLE LANGUAGE OF SPEAK METHOD
    Public Function GetLanguagesForSpeakMethod(_AuthToken As String) As List(Of String)

        Dim uri As String = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguagesForSpeak"
        Dim _HttpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
        _HttpWebRequest.Headers.Add("Authorization", _AuthToken)
        Dim _Response As WebResponse = Nothing

        Try

            _Response = _HttpWebRequest.GetResponse()

            Using stream As Stream = _Response.GetResponseStream()

                Dim dcs As New System.Runtime.Serialization.DataContractSerializer(GetType(List(Of String)))
                Dim _LanguagesForSpeak As List(Of String) = DirectCast(dcs.ReadObject(stream), List(Of String))
                Return _LanguagesForSpeak

            End Using

        Catch

            Throw

        Finally

            If _Response IsNot Nothing Then

                _Response.Close()
                _Response = Nothing

            End If

        End Try

    End Function

    'GETS THE LANGUAGE NAME FOR GIVEN LANGUAGE CODE
    Public Function GetLanguageNamesMethod(_AuthToken As String, _LanguageCodes As String()) As String()

        Dim _Uri As String = "http://api.microsofttranslator.com/v2/Http.svc/GetLanguageNames?locale=en"

        'CREATES THE REQUEST
        Dim _Request As HttpWebRequest = DirectCast(WebRequest.Create(_Uri), HttpWebRequest)
        _Request.Headers.Add("Authorization", _AuthToken)
        _Request.ContentType = "text/xml"
        _Request.Method = "POST"
        Dim dcs As New System.Runtime.Serialization.DataContractSerializer(Type.[GetType]("System.String[]"))
        Dim languageNames As String()

        Using _Stream As System.IO.Stream = _Request.GetRequestStream()
            dcs.WriteObject(_Stream, _LanguageCodes)
        End Using

        Dim _Response As WebResponse = Nothing

        Try

            _Response = _Request.GetResponse()

            Using stream As Stream = _Response.GetResponseStream()
                languageNames = DirectCast(dcs.ReadObject(stream), String())
            End Using
            Return languageNames

        Catch

            Throw

        Finally

            If _Response IsNot Nothing Then
                _Response.Close()
                _Response = Nothing
            End If

        End Try

    End Function

    'CONVERTS THE GIVEN STRING INTO SPEECH
    Public Sub SpeakMethod(_AuthToken As String, _Text As String, _SpeakLanguage As String)


        Dim _Uri As String = (Convert.ToString((Convert.ToString("http://api.microsofttranslator.com/v2/Http.svc/Speak?text=") & _Text) + "&language=") & _SpeakLanguage) + "&format=" + HttpUtility.UrlEncode("audio/wav") + "&options=MaxQuality"

        Dim _WebRequest As WebRequest = WebRequest.Create(_Uri)
        _WebRequest.Headers.Add("Authorization", _AuthToken)
        Dim _Response As WebResponse = Nothing
        Try
            _Response = _WebRequest.GetResponse()
            Using _Stream As Stream = _Response.GetResponseStream()
                Using player As New SoundPlayer(_Stream)
                    player.PlaySync()
                End Using
            End Using
        Catch

            Throw
        Finally
            If _Response IsNot Nothing Then
                _Response.Close()
                _Response = Nothing
            End If
        End Try
    End Sub

    'TRANSLATES THE GIVEN STRING FROM SOURCE LANGUAGE TO GIVEN LANGUAGE
    Public Function TranslateMethod(_AuthToken As String, _SourceText As String, _SourceLaguage As String, _TargetLanguage As String) As String

        'Dim text As String = "Use pixels to express measurements for padding and margins."
        'Dim from As String = "en"
        'Dim [to] As String = "de"

        Dim _Uri As String = Convert.ToString((Convert.ToString("http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(_SourceText) + "&from=") & _SourceLaguage) + "&to=") & _TargetLanguage

        Dim _HttpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(_Uri), HttpWebRequest)
        _HttpWebRequest.Headers.Add("Authorization", _AuthToken)
        Dim _Response As WebResponse = Nothing
        Dim _Translation As String

        Try
            _Response = _HttpWebRequest.GetResponse()
            Using _Stream As Stream = _Response.GetResponseStream()
                Dim _Dcs As New System.Runtime.Serialization.DataContractSerializer(Type.[GetType]("System.String"))
                _Translation = DirectCast(_Dcs.ReadObject(_Stream), String)

            End Using

            Return _Translation


        Catch
            Throw
        Finally
            If _Response IsNot Nothing Then
                _Response.Close()
                _Response = Nothing
            End If
        End Try
    End Function

#End Region

#End Region


End Class
