Imports OrderSoft
Imports System.Windows.Threading

'Public Class WaiterDish
'Public Property dishId As Integer
'Public Property Name As String
'Public Property size As String
'Public Property upgradePrice As Single
'Public Property totalPrice As Single
'End Class

'Public Class WaiterOrder
'Public Property origOrder As OrderObject
'Public Property orderId As String
'Public Property tableNumber As Integer
'Public Property serverID As String
'Public Property notes As String
'
'Public Property dishIds As List(Of WaiterDish)

'End Class


Public Class Waiter
    Dim clockTimer As New DispatcherTimer()
    'Dim orderInfo As New WaiterOrder
    Dim orderClient As OSClient
    Dim orderToSend As New Orders
    Dim tabs As New Dictionary(Of String, TabItem)
    Dim SendingOrder As New OrderObject


    Public Sub New(receivedClient As OSClient)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        orderClient = receivedClient
        ' Add any initialization after the InitializeComponent() call.
        ' Set up game timer
        clockTimer.Interval = New TimeSpan(0, 0, 1)
        AddHandler clockTimer.Tick, AddressOf clockTick

        clockTimer.Start()
        initialiseWindow()
    End Sub

    Public Async Sub initialiseWindow()

        Dim cats = Await orderClient.GetCategories()
        Add.IsEnabled = False
        'Increments through all categories
        For Each cat As String In cats
            Dim Dishes = Await orderClient.GetDishes(category:=cat)
            Dim menuListView As ListView = New ListView()


            Dim nameColumn As New GridViewColumn()
            nameColumn.Header = "Dish name"
            Dim nameBinding = New Binding("Name")
            nameColumn.DisplayMemberBinding = nameBinding

            Dim gridView As New GridView()
            gridView.Columns.Add(nameColumn)
            menuListView.View = gridView

            AddHandler menuListView.SelectionChanged, AddressOf selectionChanged



            Dim newTabItem As TabItem = New TabItem With {
                .Header = cat,
                .Content = menuListView
            }


            menutabs.Items.Add(newTabItem)
            'Increments through all dishes in a category
            For Each dish As DishObject In Dishes



                Console.WriteLine(dish.Image)
                menuListView.Items.Add(dish)
            Next

            ' Add tab to dict
            tabs.Add(cat, newTabItem)
        Next

    End Sub

    Public Sub selectionChanged(sender As Object, e As RoutedEventArgs)
        Add.IsEnabled = True
        orderToSend.name = sender.SelectedItem.Name
        orderToSend.originDish = sender.SelectedItem
        orderToSend.DishId = sender.SelectedItem.DishId
        Console.WriteLine(orderToSend.DishId)
        SizeComboBox.IsEnabled = True
        SizeComboBox.Items.Clear()

        If sender.SelectedItem.Sizes IsNot Nothing Then

            SizeComboBox.IsEnabled = True
            For Each size As String In sender.SelectedItem.Sizes
                'Dim newSize As MenuItem = New MenuItem With {
                'Header = sender.SelectedItem.size
                '}
                SizeComboBox.Items.Add(size)
            Next
            AddHandler SizeComboBox.SelectionChanged, AddressOf selectionComboBox
        Else
            SizeComboBox.IsEnabled = False


        End If
    End Sub

    Private Sub selectionComboBox(sender As Object, e As SelectionChangedEventArgs)
        orderToSend.size = sender.SelectedItem
        Console.WriteLine(orderToSend.size)
    End Sub

    Private Sub tabselectionChanged(sender As Object, e As SelectionChangedEventArgs)
        'should disable drop down menu
        If TypeOf e.Source Is TabControl Then
            SizeComboBox.IsEnabled = False
        End If
    End Sub


    Public Class Orders
        Public Property name As String
        Public Property size As String
        Public Property qty As Integer
        Public Property DishId As Integer
        Public Property originDish As DishObject

    End Class


    'Date and time
    Private Sub clockTick() Handles MyDateTime.Loaded
        MyDateTime.Content = DateTime.Now.ToString()
    End Sub





    'Clears all items in Listview
    Private Sub Clear_Click(sender As Object, e As RoutedEventArgs) Handles Clear.Click

        OrderList.Items.Clear()
    End Sub

    Private Sub Delete_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles Delete.Click
        OrderList.Items.Remove(OrderList.SelectedItem)
    End Sub
    Private Sub btnLogout_Click(sender As Object, e As RoutedEventArgs) Handles btnLogout.Click
        If MessageBox.Show("Are you sure you want to log out?", "My App", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) = MessageBoxResult.Yes Then
            End
        End If
    End Sub
    Private Sub btnHelp_Click(sender As Object, e As RoutedEventArgs) Handles btnHelp.Click
        Dim HelpWindow As Window = New Window
        HelpWindow.Show()
    End Sub

    ' Private Function createDishGrid(dish As DishObject) As Grid
    'Dim gridWithDish = New Grid()

    'Dim dishNameLabel = New Label()
    '   dishNameLabel.Content = dish.Name
    '    gridWithDish.Children.Add(dishNameLabel)

    ' continue for other fields
    '     gridRamen.Children.Add(gridWithDish)
    '  End Function
    Private Sub QtyBox_Loaded(sender As Object, e As RoutedEventArgs) Handles QtyBox.Loaded
        If QtyBox.Text = Nothing Then
            MessageBox.Show("Please specify a quantity")
        End If
    End Sub

    Private Async Sub SendOrder_Click(sender As Object, e As RoutedEventArgs) Handles SendOrder.Click
        If TblNum.Text = Nothing Then
            MessageBox.Show("Please specify a table number")
        ElseIf TblNum.Text = "0" Then
            MessageBox.Show("0 is not a valid table number")
        Else
            Dim allDishesInOrderList As String() = New String(OrderList.SelectedItems.Count - 1) {}

            For i As Integer = 0 To allDishesInOrderList.Length - 1
                allDishesInOrderList(i) = OrderList.SelectedItems(i).DishID.ToString() + "/" + OrderList.SelectedItems(i).size
            Next
            Console.WriteLine(allDishesInOrderList)
            SendingOrder.Dishes = allDishesInOrderList
            SendingOrder.TableNumber = TblNum.Text
            SendingOrder.TimeSubmitted = DateTime.Now
            SendingOrder.Notes = ""
            SendingOrder.TimeCompleted = Nothing
            SendingOrder.TimePaid = Nothing
            SendingOrder.AmtPaid = Nothing
            SendingOrder.ServerId = Nothing
            SendingOrder.OrderId = Nothing

            Dim SendNewOrder = Await orderClient.SetOrder(SendingOrder)
        End If

        MessageBox.Show("Order Sent!")
    End Sub

    Private Sub QtyBox_KeyPress(ByVal sender As Object, ByVal e As KeyEventArgs) Handles QtyBox.PreviewKeyDown
        If (e.Key < 34) Or (e.Key > 43) Then
            If (e.Key < 74) Or (e.Key > 83) Then
                If (e.Key = 2) Then
                    Return
                End If
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Add_Click(sender As Object, e As RoutedEventArgs) Handles Add.Click
        Dim n As Integer
        n = 0

        If QtyBox.Text = Nothing Then
            MessageBox.Show("Please specify a quantity")
        Else
            If orderToSend.size IsNot Nothing Then
                While n < QtyBox.Text
                    OrderList.Items.Add(orderToSend)
                    n = n + 1
                End While
            Else
                If orderToSend.originDish.Sizes IsNot Nothing Then
                    MessageBox.Show("You need to pick a size")
                Else
                    While n < QtyBox.Text
                        OrderList.Items.Add(orderToSend)
                        n = n + 1
                    End While
                End If
            End If

        End If
    End Sub
End Class
