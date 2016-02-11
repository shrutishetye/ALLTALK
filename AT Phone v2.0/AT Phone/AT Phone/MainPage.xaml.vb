Imports System
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Delegate
Imports System.Windows.Controls
Imports Microsoft.Phone.Controls
Imports Microsoft.Phone.Shell

Public Class MainPage
    Inherits PhoneApplicationPage

#Region "FIELDS AND OBJECTS"

    'FIELDS AND OBJECTS

    Public _PacketManager As New PacketManager

    Public _UserState As New UserState

    Public WithEvents _SafePageUpdater As New SafePageUpdater


#End Region

#Region "METHODS"

#Region "CONSTRUCTOR"
    Public Sub New()

        _PacketManager.Connect()

        InitializeComponent()

        SupportedOrientations = SupportedPageOrientation.Portrait Or SupportedPageOrientation.Landscape



    End Sub

#End Region

#Region "MAIN PAGE EVENT METHODS"

    'MAIN PAGE EVENT METHODS


#End Region

#End Region


    Private Sub ButtonRegister_Click(sender As Object, e As RoutedEventArgs)

        UserState.PageNav = UserState.PageNavigationHeader.MainPage
        MessageBox.Show(UserState.PageNav)
        _UserState.FName = TextBoxFirstName.Text
        _UserState.LName = TextBoxLastName.Text
        _UserState.Email = TextBoxEmailId.Text
        MessageBox.Show(TextBoxEmailId.Text)
        _PacketManager.SendRegisterPacket(TextBoxFirstName.Text, TextBoxLastName.Text, TextBoxEmailId.Text)
        NavigationService.Navigate(New Uri("/ATPinPage.xaml", UriKind.RelativeOrAbsolute))

    End Sub

    Private Sub _SafePageUpdater_OnSafeAuthorizeSendPacket(_Password As String) Handles _SafePageUpdater.SafeAuthorizeSendPacket

        MessageBox.Show("TTTTTT")
        _PacketManager.SendAuthorizePacket(_Password)

    End Sub

End Class

Public Class SafePageUpdater

    Public Shared _MainPage As MainPage

    Public Delegate Sub SafeSendAuthorizePacketDelegate(_Password As String)

    Public Shared Event SafeAuthorizeSendPacket As SafeSendAuthorizePacketDelegate


    Public Shared Sub SafeAuthorizePacketSender(_Password As String)

        MessageBox.Show("TT")
        ThreadSafeSendAuthorizePacket(_Password)

    End Sub

    Private Shared Sub ThreadSafeSendAuthorizePacket(_Password As String)


        'IF WE ARE IN A DIFFERENT THREAD OTHER THEN MAIN

        MessageBox.Show("TTT")
            RaiseEvent SafeAuthorizeSendPacket(_Password)



    End Sub


End Class