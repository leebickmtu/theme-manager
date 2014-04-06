Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods
Imports Theme_Manager.MainWindow

Class Start

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

    Private Sub Page_Loaded() Handles MyBase.Loaded
        While Me.NavigationService.CanGoBack
            Me.NavigationService.RemoveBackEntry()
        End While
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnApply.Click
        If MainWindow.themeList.isEmpty Then
            MsgBox("There are no themes currently installed.")
            Return
        End If

        ' view SelectTheme page
        Dim selectTheme As New Selection()
        selectTheme.getRef(mW)
        selectTheme.isApplySelection(True)
        Me.NavigationService.Navigate(selectTheme)
    End Sub

    Private Sub btnInstall_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnInstall.Click
        ' view SelectTheme page
        Dim installTheme As New Install()
        installTheme.getRef(mW)
        Me.NavigationService.Navigate(installTheme)
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRemove.Click
        If MainWindow.themeList.isEmpty Then
            MsgBox("There are no themes currently installed.")
            Return
        End If

        ' view SelectTheme page
        Dim selectTheme As New Selection()
        selectTheme.getRef(mW)
        selectTheme.isRemoveSelection(True)
        Me.NavigationService.Navigate(selectTheme)
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDownload.Click
        ' view DownloadTheme page
        Dim downloadTheme As New Download()
        downloadTheme.getRef(mW)
        Me.NavigationService.Navigate(downloadTheme)
    End Sub

    Private Sub btnBC_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBCstart.Click
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

End Class
