Imports OrderSoft
Public Class userpass
    Dim Client

    Public Sub New(receivedClient As OSClient)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Client = receivedClient
    End Sub

    Private Async Sub login_onclick(sender As Object, e As RoutedEventArgs) Handles btnLogin.Click
        ' Disable login UI elements while interacting with server
        lblUser.IsEnabled = True
        lblPassword.IsEnabled = True
        txtUser.IsEnabled = True
        txtPassword.IsEnabled = True
        btnLogin.IsEnabled = True

        Dim failed = False
        Try
            Await Client.Login(txtUser.Text, txtPassword.Password)
        Catch ex As Exception
            failed = True
            MessageBox.Show("Error while logging in")
        End Try

        If failed Then
            ' Re-enable login options
            lblUser.IsEnabled = True
            lblPassword.IsEnabled = True
            txtUser.IsEnabled = True
            txtPassword.IsEnabled = True

            btnLogin.IsEnabled = True
        Else
            Dim WaiterWindow As Waiter = New Waiter(Client)
            WaiterWindow.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Helpbtn_Click(sender As Object, e As RoutedEventArgs) Handles Helpbtn.Click
        MessageBox.Show("Input a valid username and password in order to log in.")
    End Sub
End Class
