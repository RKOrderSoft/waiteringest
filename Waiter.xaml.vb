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
    Dim orderToSend As New OrderObject

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

        'Increments through all categories
        For Each cat As String In cats

            Dim Dishes = Await orderClient.GetDishes(category:=cat)
            Dim menuListView As ListView = New ListView()


            Dim newTabItem As TabItem = New TabItem With {
                .Header = cat,
                .Content = menuListView
            }


            menutabs.Items.Add(newTabItem)
            'Increments through all dishes in a category
            For Each dish As DishObject In Dishes

                If dish.Sizes IsNot Nothing Then
                    'Increments through all available sizes for a dish
                    For Each size As String In dish.Sizes

                        'testMenu.Items.Add(dish)
                        'Dim Items As List(Of Orders) = New List(Of Orders)
                        'Items.Add(New Orders() With {
                        '.name = dish.Name})
                        'testMenu.Items.Add(Items)
                    Next
                End If

                Console.WriteLine(dish.Image)
                menuListView.Items.Add(dish.Name)
            Next
            menuListViewSizeButtons(menuListView, Dishes)
        Next

    End Sub

    Private Function menuListViewSizeButtons(menuListView, Dishes)


    End Function



    Public Class Orders
        Public Property name As String
        Public Property size As String
        Public Property qty As Integer

    End Class



    Public Sub Button_Click(sender As Object, e As RoutedEventArgs)
        'InitializeComponent()
        ' Dim Items As List(Of Orders) = New List(Of Orders)
        '  Items.Add(New Orders() With {
        '   .name = "Nuts",
        '    .size = "Big",
        '  .qty = 2
        '  })
        ' OrderList.Items.Add(Items)



    End Sub

    'Date and time
    Private Sub clockTick() Handles MyDateTime.Loaded
        MyDateTime.Content = DateTime.Now.ToString()
    End Sub



    Private Sub ListView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

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

    ' Private Function createDishGrid(dish As DishObject) As Grid
    'Dim gridWithDish = New Grid()

    'Dim dishNameLabel = New Label()
    '   dishNameLabel.Content = dish.Name
    '    gridWithDish.Children.Add(dishNameLabel)

    ' continue for other fields
    '     gridRamen.Children.Add(gridWithDish)
    '  End Function


    Private Sub SendOrder_Click(sender As Object, e As RoutedEventArgs) Handles SendOrder.Click

        MessageBox.Show("Order Sent!")
    End Sub
End Class
