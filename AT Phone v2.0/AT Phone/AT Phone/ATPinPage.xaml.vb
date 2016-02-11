Partial Public Class ATPinPage
    Inherits PhoneApplicationPage


    Public _UserState As New UserState

    Public Sub New()

        InitializeComponent()

    End Sub

    Private Sub ButtonVerify_Click(sender As Object, e As RoutedEventArgs) Handles ButtonVerify.Click

        SafePageUpdater.SafeAuthorizePacketSender("root")

    End Sub
End Class
