Imports System.Linq
Imports System.Net
Imports System.Net.Mail

Public Class LinqDb

    Private _LDatabase As New LDataBaseDataContext
    Private _ATPin As String

    Public Sub DbRegister(_Fname As String, _LName As String, _Email As String)

        Console.WriteLine()

        Try

            'CREATION OF ATPIN
            _ATPin = "AT" + Guid.NewGuid().ToString.Substring(0, 8)

            'CREATION OF ROW OBJECT OF USERDETAILS
            Dim _NewUser As New User_Detail()

            _NewUser.fname = _Fname
            _NewUser.lname = _LName
            _NewUser.email = _Email
            _NewUser.language = "French"
            _NewUser.Reg_id = _ATPin

            'SUBMITTING THE NEW ROW AND SAVING CHANGES
            _LDatabase.User_Details.InsertOnSubmit(_NewUser)
            _LDatabase.SubmitChanges()

        Catch e As Exception

            Console.WriteLine("BluBlu")

        End Try


        'SENDING THE ATPIN VIA MAIL
        SendEmail(_Fname, _Email, _ATPin)

        Return

    End Sub

    Private Sub SendEmail(_FName As String, _Email As String, _ATPin As String)

        Dim smtpAddress As String = "smtp.gmail.com"
        Dim portNumber As Integer = 587
        Dim enableSSL As Boolean = True
        Dim emailFrom As String = "alltalk.rsa@gmail.com"
        Dim password As String = "alltalk1101"
        Dim emailTo As String = _Email
        Dim subject As String = "no-reply@live.mail(RSA)Alltalk|ATPIN"
        Dim body As String = "Welcome " + "<b>" + _FName + "</b>" + "!! <br> Your ATPIN is " + "<b>" + _ATPin + "</b>" + " <br>Enter in the app and Get Started.."


        Using mail As New MailMessage()

            mail.From = New MailAddress(emailFrom)
            mail.[To].Add(emailTo)
            mail.Subject = subject
            mail.Body = body
            mail.IsBodyHtml = True
            Using smtp As New SmtpClient(smtpAddress, portNumber)
                smtp.Credentials = New NetworkCredential(emailFrom, password)
                smtp.EnableSsl = enableSSL
                smtp.Send(mail)

            End Using

        End Using

        Console.WriteLine("Email Sent to : " & _FName)
        Return

    End Sub

    Public Sub RetrieveFriendContact(_RId As String)

        Dim _ContactRow = From _Contact In _LDatabase.Contacts
                          Where _Contact.host_id = _RId
                          Select fname = _Contact.fname, language = _Contact.language

        For Each _C In _ContactRow

            Console.WriteLine(_C.fname & _C.language)

        Next _C


        Console.WriteLine("Friend Contact Retrieved")
        Console.ReadKey()

    End Sub

    Public Sub RetrieveCompanyContact(_RId As String)

        Dim _ContactRow = From _Contact In _LDatabase.C_Contacts
                          Where _Contact.host_id = _RId
                          Select company_id = _Contact.company_id, language = _Contact.language

        For Each _C In _ContactRow

            Console.WriteLine(_C.company_id & _C.language)

        Next _C


        Console.WriteLine("Company Contact Retrieved")
        Console.ReadKey()

    End Sub

    Public Sub RetrieveMessage(_SId As String, _RId As String)

        Dim _MessageRow = From _Message In _LDatabase.Messages
                          Where (_Message.sender_id = _SId And _Message.receiver_id = _RId) Or (_Message.receiver_id = _RId And _Message.sender_id = _SId)
                          Select msg_id = _Message.msg_id, msg = _Message.msg, _sender_lang = _Message.sender_lang

        For Each _C In _MessageRow

            Console.WriteLine(_C.msg)

        Next _C


        Console.WriteLine("Messages Retrieved")
        Console.ReadKey()

    End Sub

End Class
