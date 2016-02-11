Imports System.Text
Imports System.Security.Cryptography
Imports System.Linq


Module ATServer

    Enum PacketHeader As Byte

        Chatter = 0
        MOTD = 1
        Authorize = 2
        FileTransfer = 3
        Handshake = 4
        Register = 5
        RContact = 6
        CContact = 7
        Messages = 8

    End Enum

    Private Packer As New Serializer

    Private PublicKey As Byte()
    Private RSA As New RSACryptoServiceProvider(2048)
    Private WithEvents Server As New ServerListener

    Private _LinqDb As New LinqDb



#Region "Helper Methods"

    'SENDS TO THE CLIENT PROVIDED
    Private Sub SendToClient(client As ServerClient, ParamArray args As Object())

        Dim Data As Byte() = Packer.Serialize(args)


        If DirectCast(client.UserState, User).SecureConnection Then

            Data = DirectCast(client.UserState, User).Encrypt(Data)

        End If

        client.Send(Data)

    End Sub

    'SENDS TO ALL THE CLIENT
    Private Sub Broadcast(ParamArray args As Object())

        Dim Data As Byte() = Packer.Serialize(args)

        For Each C As ServerClient In Server.Clients
            C.Send(Data)
        Next

    End Sub

    'EXCLUDES THE CLIENT THAT IS PROVIDED
    Private Sub BroadcastExclude(client As ServerClient, ParamArray args As Object())

        Dim Data As Byte() = Packer.Serialize(args)

        For Each C As ServerClient In Server.Clients

            If Not C Is client Then

                If DirectCast(C.UserState, User).SecureConnection Then

                    C.Send(DirectCast(C.UserState, User).Encrypt(Data))

                Else

                    C.Send(Data)

                End If

            End If

        Next

    End Sub

#End Region

#Region "Packet Senders"

    'TAKES MESSAGE AS PARAMETER AND CALLS SENDPACKET WITH HEADER SET AS CHATTER
    Private Sub SendChatterPacket(client As ServerClient, message As String)

        BroadcastExclude(client, CByte(PacketHeader.Chatter), message)

    End Sub

    Private Sub SendMOTDPacket(client As ServerClient, message As String)

        SendToClient(client, CByte(PacketHeader.MOTD), message)

    End Sub

    Private Sub SendHandShakePacket(client As ServerClient, publickey As Byte())

        SendToClient(client, CByte(PacketHeader.Handshake), publickey)

    End Sub



#End Region


#Region "Packet Handlers"

    Private Sub HandleChatterPacket(client As ServerClient, values As Object())

        If DirectCast(client.UserState, User).Authorized Then

            SendChatterPacket(client, DirectCast(values(1), String))

        End If

    End Sub

    Private Sub HandleAuthorizePacket(client As ServerClient, values As Object())

        Dim Password As String = DirectCast(values(1), String)

        If Password = "root" Then

            client.MaxPacketSize = 15000000
            DirectCast(client.UserState, User).Authorized = True

        End If

        Console.WriteLine("Client [{0}] Authorized", client.EndPoint)

    End Sub

    Private Sub HandleFileTransferPacket(client As ServerClient, values As Object())

        If DirectCast(client.UserState, User).Authorized Then

            Dim FileName As String = DirectCast(values(1), String)
            Dim FileData As Byte() = DirectCast(values(2), Byte())

            IO.File.WriteAllBytes(FileName, FileData)

        End If

    End Sub

    Private Sub HandleHandShakePacket(client As ServerClient, values As Object())

        Dim Data As Byte() = DirectCast(values(1), Byte())
        Data = RSA.Decrypt(Data, True)

        Dim Params As Object() = Packer.Deserialize(Data)

        Dim Key As Byte() = DirectCast(Params(0), Byte())
        Dim IV As Byte() = DirectCast(Params(1), Byte())

        DirectCast(client.UserState, User).PrepareEncryption(Key, IV)

        SendMOTDPacket(client, "Welcome to the chat plaese be kind to others")

    End Sub

    Private Sub HandleRegisterPacket(client As ServerClient, values As Object())


        Dim _FirstName As String = DirectCast(values(1), String)
        Dim _LasttName As String = DirectCast(values(2), String)
        Dim _Email As String = DirectCast(values(3), String)

        _LinqDb.DbRegister(_FirstName, _LasttName, _Email)

        Console.WriteLine("Client [{0}] Registered", client.EndPoint)

    End Sub

    Private Sub HandleRContactPacket(client As ServerClient, values As Object())

        _LinqDb.RetrieveFriendContact(DirectCast(values(1), String))

    End Sub


#End Region

    Sub Main()

        'PREVENTS USER FROM GETTING PRIVATE KEY AND THEY DECRYPTING EVERYTHING BY SETTING IT TO FALSE
        PublicKey = RSA.ExportCspBlob(False)


        Server.MaxPacketSize = 2048
        Server.Listen(32456)
        Console.ReadKey()
        '_LinqDb.DbRegister("dsdfgdfgdfg", "dfsdfsdfsdf", "rystnpinto@gmail.com", "sdfsdfsdf")
        '_LinqDb.RetrieveFriendContact("ATyston")
        '_LinqDb.RetrieveCompanyContact("AT1234567")
        '_LinqDb.RetrieveMessage("ATiceouza", "AThilraham")

    End Sub



    Private Sub Server_ClientReadPacket(sender As ServerListener, client As ServerClient, data() As Byte) Handles Server.ClientReadPacket

        Try

            If DirectCast(client.UserState, User).SecureConnection Then

                data = DirectCast(client.UserState, User).Decrypt(data)

            End If


        Catch ex As Exception

            client.Disconnect()

        End Try

        Dim Values As Object() = Packer.Deserialize(data)

        'IF THERE IS NO VALUE THEN IS NOT GOING TO HANDLE THE MESSAGE CAUSE IT MUST BE CORRUPT HENCE DISCONNECT THE CLIENT 
        If Values Is Nothing OrElse Values.Length = 0 Then

            client.Disconnect()
            Return

        End If

        Select Case DirectCast(Values(0), PacketHeader)

            Case PacketHeader.Chatter
                HandleChatterPacket(client, Values)

            Case PacketHeader.Authorize
                HandleAuthorizePacket(client, Values)

            Case PacketHeader.FileTransfer
                HandleFileTransferPacket(client, Values)

            Case PacketHeader.Handshake
                HandleHandShakePacket(client, Values)

            Case PacketHeader.Register
                HandleRegisterPacket(client, Values)

            Case PacketHeader.RContact
                HandleRContactPacket(client, Values)
            Case Else
                client.Disconnect()

        End Select

    End Sub

    Private Sub Server_ClientWritePacket(sender As ServerListener, client As ServerClient, size As Integer) Handles Server.ClientWritePacket

        Console.WriteLine("Sent [{1}]:{0}", size, client.EndPoint)

    End Sub

    Private Sub Server_StateChanged(sender As ServerListener, listening As Boolean) Handles Server.StateChanged

        Console.WriteLine("Listening :{0}", listening)

    End Sub

    'CALLED WHEN A CLIENT CONNECTS TO THE SERVER
    Private Sub Server_ClientStateChanged(sender As ServerListener, client As ServerClient, connected As Boolean) Handles Server.ClientStateChanged

        Console.WriteLine("Connected [{1}]:{0}", connected, client.EndPoint)

        If connected Then
            client.UserState = New User()
            SendHandShakePacket(client, PublicKey)

        End If

    End Sub

    Private Sub Server_ClientExceptionThrown(sender As ServerListener, client As ServerClient, ex As Exception) Handles Server.ClientExceptionThrown

        Console.WriteLine("Exception [{1}]:{0}", ex, client.EndPoint)

    End Sub


    Private Sub Server_ExceptionThrown(sender As ServerListener, ex As Exception) Handles Server.ExceptionThrown

        Console.WriteLine("Exception {0}", ex)

    End Sub
End Module

Class User

    Public Authorized As Boolean
    Public SecureConnection As Boolean

    Private Encryptor As ICryptoTransform
    Private Decryptor As ICryptoTransform

    Public Sub PrepareEncryption(key As Byte(), iv As Byte())

        Dim R As New AesManaged()
        Encryptor = R.CreateEncryptor(key, iv)
        Decryptor = R.CreateDecryptor(key, iv)

        SecureConnection = True

    End Sub

    Public Function Encrypt(data As Byte()) As Byte()

        Return Encryptor.TransformFinalBlock(data, 0, data.Length)

    End Function

    Public Function Decrypt(data As Byte()) As Byte()

        Return Decryptor.TransformFinalBlock(data, 0, data.Length)

    End Function


End Class