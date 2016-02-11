Imports System.Text
Imports System.Security.Cryptography

Public Class ATForm


    Private _PacketManager

    Private WithEvents _SafeFormUpdater As New SafeFormUpdater

#Region "EVENT METHODS"

    'EVENT METHODS

#Region "FORM EVENTS"

    'FORM EVENTS

    Private Sub ATForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        _PacketManager = New PacketManager

    End Sub

    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click

        Dim O As New OpenFileDialog()
        O.Filter = "All Files|*.*"

        'IF NOTHING IS SELECTED THEN DO NOTHING
        If Not O.ShowDialog() = Windows.Forms.DialogResult.OK Then Return

        Dim Filename As String = IO.Path.GetFileName(O.FileName)
        Dim FileData As Byte() = IO.File.ReadAllBytes(O.FileName)

        _PacketManager.SendFileTransferPacket(Filename, FileData)

    End Sub

    Private Sub ButtonAuthorize_Click(sender As Object, e As EventArgs) Handles ButtonAuthorize.Click

        _PacketManager.SendAuthorizePacket(TextBoxPassword.Text)

    End Sub

    Private Sub ButtonSend_Click(sender As Object, e As EventArgs) Handles ButtonSend.Click

        _PacketManager.SendChatterPacket(TextBoxSend.Text)

    End Sub

    Private Sub _SafeFormUpdater_UpdateLogMessage(_Message As String, _Args() As Object) Handles _SafeFormUpdater.UpdateLogMessage

        TextBoxChatLog.AppendText(String.Format(_Message, _Args))
        'STRING FORMAT:"THE MESSAGE : {0}{1},"HELLO","ONE""
        '{0} WILL POINT TO HELLO WHILE {1} WILL POINT TO ONE WHEN PASSED IN FUNCTION

        TextBoxChatLog.AppendText(Environment.NewLine)

    End Sub

    Private Sub _SafeFormUpdater_UpdateReadBar(_Progress As Double, _BytesRead As Integer, _BytesToRead As Integer) Handles _SafeFormUpdater.UpdateReadBar

        ProgressBarRead.Value = CInt(_Progress)

        'N TELLS THE STRING FORMAT METHOD TO DISPLAY THE NO IN NUMERIC FORMAT WITH COMMAS AND STUFF
        '0 TELLS THE STRING FORMAT METHOD THAT NO DECIMAL PLACES ARE INVOLVED
        LabelRead.Text = String.Format("Read : {0:N0} / {1:N0}", _BytesRead, _BytesToRead)

    End Sub

    Private Sub _SafeFormUpdater_UpdateWriteBar(_Progress As Double, _BytesWritten As Integer, _BytesToWrite As Integer) Handles _SafeFormUpdater.UpdateWriteBar

        ProgressBarWrite.Value = CInt(_Progress)
        LabelWrite.Text = String.Format("Write : {0:N0} / {1:N0}", _BytesWritten, _BytesToWrite)

    End Sub

#End Region

#End Region
   
End Class



Public Class SafeFormUpdater

    Public Shared _ATForm As ATForm

    Public Delegate Sub UpdateLogMessageDelegate(_Message As String, _Args As Object())

    Public Delegate Sub UpdateReadBarDelegate(_Progress As Double, _BytesRead As Integer, _BytesToRead As Integer)

    Public Delegate Sub UpdateWriteBarDelegate(_Progress As Double, _BytesWritten As Integer, _BytesToWrite As Integer)

    Public Shared Event UpdateLogMessage As UpdateLogMessageDelegate

    Public Shared Event UpdateReadBar As UpdateReadBarDelegate

    Public Shared Event UpdateWriteBar As UpdateWriteBarDelegate

    Public Shared Sub LogMessageUpdater(_Message As String, ParamArray _Args As Object())
        ThreadSafeUpdateLogMessage(_Message, _Args)
    End Sub

    Public Shared Sub ReadBarUpdater(_Progress As Double, _BytesRead As Integer, _BytesToRead As Integer)
        ThreadSafeUpdateReadBar(_Progress, _BytesRead, _BytesToRead)
    End Sub


    Public Shared Sub WriteBarUpdater(_Progress As Double, _BytesWritten As Integer, _BytesToWrite As Integer)
        ThreadSafeUpdateWriteBar(_Progress, _BytesWritten, _BytesToWrite)
    End Sub


   

    Private Shared Sub ThreadSafeUpdateLogMessage(_Message As String, ParamArray _Args As Object())


        'IF WE ARE IN A DIFFERENT THREAD OTHER THEN MAIN
        If _ATForm IsNot Nothing AndAlso _ATForm.InvokeRequired Then


            _ATForm.Invoke(New UpdateLogMessageDelegate(AddressOf ThreadSafeUpdateLogMessage), New Object() {_Message, _Args})
        Else
            RaiseEvent UpdateLogMessage(_Message, _Args)
        End If
    End Sub

    Private Shared Sub ThreadSafeUpdateReadBar(_Progress As Double, _BytesRead As Integer, _BytesToRead As Integer)


        'IF WE ARE IN A DIFFERENT THREAD OTHER THEN MAIN
        If _ATForm IsNot Nothing AndAlso _ATForm.InvokeRequired Then


            _ATForm.Invoke(New UpdateReadBarDelegate(AddressOf ThreadSafeUpdateReadBar), New Object() {_Progress, _BytesRead, _BytesToRead})
        Else
            RaiseEvent UpdateReadBar(_Progress, _BytesRead, _BytesToRead)
        End If
    End Sub

    Private Shared Sub ThreadSafeUpdateWriteBar(_Progress As Double, _BytesWritten As Integer, _BytesToWrite As Integer)


        'IF WE ARE IN A DIFFERENT THREAD OTHER THEN MAIN
        If _ATForm IsNot Nothing AndAlso _ATForm.InvokeRequired Then


            _ATForm.Invoke(New UpdateWriteBarDelegate(AddressOf ThreadSafeUpdateWriteBar), New Object() {_Progress, _BytesWritten, _BytesToWrite})
        Else
            RaiseEvent UpdateWriteBar(_Progress, _BytesWritten, _BytesToWrite)
        End If
    End Sub

End Class