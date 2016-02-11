Public Class PacketManager

#Region "FIELDS AND OBJECTS"

    'FIELDS AND OBJECTS

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

        'SHOULD COMMONLY HAVE TO FILETRANSFER HEADER ONE BEGIN TRANSFER WHICH SENDS THE FILE SIZE SO 
        'THAT THE BUFFER SIZE CAN BE DYNAMICALLY SET AND U CAN LIMIT THE FILE TRANSFER PER USER

    End Enum

    Private _Packer As New Serializer

    Private WithEvents _UserClient As New UserClient

    Private _UserState As New UserState

    Private _Encryption As New Encryption

    Private _Translator As New Translator


#End Region

#Region "METHODS"

    'METHODS

#Region "CONSTRUCTOR"

    'CONSTRUCTOR

    Public Sub New()

        _UserClient.MaxPacketSize = 15000000
        _UserClient.Connect("192.168.0.100", 32456)

    End Sub

#End Region

    'METHODS PERFORMING GENERAL UNIQUE FUNCTIONS
#Region "HELPER METHODS"

    'HELPER METHODS

    'PARAM ARRAY ALLOWS TO SEND ANY NO OF ARGUEMENTS AS OBJECTS
    Private Sub LogMessage(_Message As String, ParamArray _Args As Object())

        SafeFormUpdater.LogMessageUpdater(_Message, _Args)

    End Sub


    'PERFORMS SERIALIZED SENDING OF PACKETS
    Private Sub SendPacket(ParamArray _Args As Object())

        Dim _Data As Byte() = _Packer.Serialize(_Args)

        If _UserState.SecureConnection Then

            _Data = _Encryption.Encrypt(_Data)

        End If

        _UserClient.Send(_Data)

    End Sub

#End Region

    'TAKES MESSAGE AND ATTACHES THE APPROPRIATE HEADER WITH THE MESSAGE
#Region "PACKET SENDERS"

    'PACKET SENDERS

    Public Sub SendChatterPacket(message As String)

        SendPacket(CByte(PacketHeader.Chatter), message)

    End Sub

    Public Sub SendAuthorizePacket(password As String)

        SendPacket(CByte(PacketHeader.Authorize), password)

    End Sub

    Public Sub SendFileTransferPacket(filename As String, filedata As Byte())

        SendPacket(CByte(PacketHeader.FileTransfer), filename, filedata)

    End Sub

    Public Sub SendHandShakePacket(data As Byte())

        SendPacket(CByte(PacketHeader.Handshake), data)

    End Sub


#End Region

    'HANDLES THE DATA RECIEVED ACCORDING TO THE HEADER ASSOCIATED WITH IT
#Region "PACKET HANDLERS"

    'PACKET HANDLERS

    Private Sub HandleChatterPacket(values As Object())


        MessageBox.Show(_Translator.TranslateMethod(_Translator.HeaderValue, DirectCast(values(1), String), _Translator.DetectMethod(_Translator.HeaderValue, DirectCast(values(1), String)), "fr"))

        'DIRECTCAST INTODUCES A TYPE CONVERSION ON INHERITANCE OR IMPLEMENTATION
        LogMessage("Chat : {0}", DirectCast(values(1), String))

    End Sub

    Private Sub HandleMOTDPacket(values As Object())

        LogMessage("MOTD : {0}", DirectCast(values(1), String))

    End Sub

    Private Sub HandleHandShakePacket(values As Object())

        _Encryption.PublicKey = DirectCast(values(1), Byte())

        SendHandShakePacket(_Encryption.PrepareEncryption)
        _UserState.SecureConnection = True

    End Sub

#End Region

#Region "USERCLIENT EVENTS"

    'USERCLIENT EVENTS

    Private Sub Client_ReadPacket(sender As UserClient, data() As Byte) Handles _UserClient.ReadPacket


        If _UserState.SecureConnection Then

            data = _Encryption.Decrypt(data)

        End If

        Dim Values As Object() = _Packer.Deserialize(data)

        'IF THERE IS NO VALUE THEN IS NOT GOING TO HANDLE THE MESSAGE CAUSE IT MUST BE CORRUPT HENCE DISCARDING 
        If Values Is Nothing OrElse Values.Length = 0 Then Return

        Select Case DirectCast(Values(0), PacketHeader)

            Case PacketHeader.Chatter
                HandleChatterPacket(Values)

            Case PacketHeader.MOTD
                HandleMOTDPacket(Values)

            Case PacketHeader.Handshake
                HandleHandShakePacket(Values)

        End Select

    End Sub

    Private Sub Client_WritePacket(sender As UserClient, size As Integer) Handles _UserClient.WritePacket

        LogMessage("Packet Sent : {0}", size)

    End Sub

    Private Sub Client_StateChanged(sender As UserClient, connected As Boolean) Handles _UserClient.StateChanged

        LogMessage("Connected {0}", connected)

    End Sub

    Private Sub Client_ExceptionThrown(sender As UserClient, ex As Exception) Handles _UserClient.ExceptionThrown

        LogMessage("Exception {0}", ex)

    End Sub

    Private Sub Client_ReadProgressChanged(sender As UserClient, progress As Double, bytesRead As Integer, bytesToRead As Integer) Handles _UserClient.ReadProgressChanged

        SafeFormUpdater.ReadBarUpdater(progress, bytesRead, bytesToRead)

    End Sub

    Private Sub Client_WriteProgressChanged(sender As UserClient, progress As Double, bytesWritten As Integer, bytesToWrite As Integer) Handles _UserClient.WriteProgressChanged

        SafeFormUpdater.WriteBarUpdater(progress, bytesWritten, bytesToWrite)


    End Sub

    

#End Region
#End Region

End Class
