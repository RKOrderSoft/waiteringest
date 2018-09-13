Imports OrderSoft

Class MainWindow
    Dim client As New OSClient()

    Private Async Sub connect_onclick(sender As Object, e As RoutedEventArgs) Handles btnConnect.Click
        ' Set IP UI elements as disabled while loading
        lblIP.IsEnabled = False
        txtIP.IsEnabled = False
        btnConnect.IsEnabled = False

        Dim failed = False
        Try
            ' Initiate client with given IP
            Await client.Init("http://" + txtIP.Text + "/")
        Catch ex As Exception
            ' Error was thrown during initialisation
            failed = True
            MessageBox.Show("An error occurred while connecting")
        End Try

        If failed Then
            ' Re-enable connection UI elements to allow retry
            lblIP.IsEnabled = True
            txtIP.IsEnabled = True
            btnConnect.IsEnabled = True
        Else
            ' Enable login options for next state
            Dim userpasswindow As userpass = New userpass(client)
            userpasswindow.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim WaiterShow As Waiter = New Waiter(client)
        WaiterShow.Show()
        Me.Close()

    End Sub

    Private Sub Helpbtn_Click(sender As Object, e As RoutedEventArgs) Handles Helpbtn.Click
        MessageBox.Show("Input the server IP without the enclosing http:// and /")
    End Sub
End Class

