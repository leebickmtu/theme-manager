Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods
Imports Theme_Manager.MainWindow

Class Download

#Region "Universal Page Code"
    Dim mW As MainWindow

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub getRef(ByVal mW As MainWindow)
        Me.mW = mW
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnClose.Click
        Application.Current.MainWindow.Close()
    End Sub

    Private Sub btnMinimize_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnMinimize.Click
        Application.Current.MainWindow.WindowState = WindowState.Minimized
    End Sub

    Private Sub canTitleBar_MouseLeftButtonDown(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles canTitleBar.MouseLeftButtonDown
        Application.Current.MainWindow.DragMove()
    End Sub

    Private Sub btnBadge_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnBadge.Click
        btnBC_Click(btnBCstart, Nothing)
    End Sub
#End Region

    Private Sub btnBC_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBCstart.Click, btnBCdownload.Click
        Dim depth As Integer = CInt(CType(sender, Button).Content.ToString) - 1

        If depth < 0 Then
            Me.NavigationService.Refresh()
            Return
        End If

        For i As Integer = 1 To depth
            Me.NavigationService.RemoveBackEntry()
        Next

        Me.NavigationService.GoBack()
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        System.Diagnostics.Process.Start("http://browse.deviantart.com/customization/skins/windows7/visualstyle/?order=9")
    End Sub
End Class
