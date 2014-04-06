Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods
Imports Theme_Manager.MainWindow

Class Install

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

    Private selected_theme As String = ""
    Private selected_explorer As String = ""
    Private selected_explorerFrame As String = ""
    Private selected_OobeFldr As String = ""
    Private selected_shell32 As String = ""
    Private selected_timedate As String = ""
    Private selected_logon As String = ""
    Private selected_blur As Boolean = True

    Private dlg As New Microsoft.Win32.OpenFileDialog()

    Private Sub btnBC_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBCstart.Click, btnBCinstall.Click
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


    Private Sub btnTheme_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTheme.Click
        dlg.DefaultExt = ".theme"
        dlg.Filter = "Theme (.theme)|*.theme"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            selected_theme = dlg.FileName
            imgHasTheme.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasTheme.IsEnabled = True
            If txtName.Text = "" Then
                txtName.Text = getFileName(selected_theme, False)
            End If
        End If
    End Sub

    Private Sub btnExplorer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnExplorer.Click
        dlg.DefaultExt = ".exe"
        dlg.Filter = "Executable (.exe)|*.exe"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            selected_explorer = dlg.FileName
            imgHasExplorer.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasExplorer.IsEnabled = True
        End If
    End Sub

    Private Sub btnExplorerFrame_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnExplorerFrame.Click
        dlg.DefaultExt = ".dll"
        dlg.Filter = "Dynamic Link Library (.dll)|*.dll"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            selected_explorerFrame = dlg.FileName
            imgHasExplorerFrame.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasExplorerFrame.IsEnabled = True
        End If
    End Sub

    Private Sub btnShell32_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnShell32.Click
        dlg.DefaultExt = ".dll"
        dlg.Filter = "Dynamic Link Library (.dll)|*.dll"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            selected_shell32 = dlg.FileName
            imgHasShell32.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasShell32.IsEnabled = True
        End If
    End Sub

    Private Sub btnOobeFldr_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnOobeFldr.Click
        dlg.DefaultExt = ".dll"
        dlg.Filter = "Dynamic Link Library (.dll)|*.dll"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            selected_OobeFldr = dlg.FileName
            imgHasOobeFldr.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasOobeFldr.IsEnabled = True
        End If
    End Sub

    Private Sub btnTimedate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTimedate.Click
        dlg.DefaultExt = ".cpl"
        dlg.Filter = "cpl File (.cpl)|*.cpl"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            selected_timedate = dlg.FileName
            imgHasTimeDate.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasTimeDate.IsEnabled = True
        End If
    End Sub

    Private Sub btnLogon_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLogon.Click
        dlg.DefaultExt = ".jpg"
        dlg.Filter = "JPG (.jpg)|*.jpg"
        Dim result As Boolean = dlg.ShowDialog()
        If (result = True) Then
            If FileIO.FileSystem.GetFileInfo(dlg.FileName).Length > 256000 Then
                MsgBox("Logon background must be less than 256 kB.", 48, "Error")
                selected_logon = ""
                imgHasLogon.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
                btnHasLogon.IsEnabled = False
                Return
            End If
            selected_logon = dlg.FileName
            imgHasLogon.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasLogon.IsEnabled = True
        End If
    End Sub

    Private Sub btnInstall_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnInstall.Click
        If txtName.Text = "" Then
            MsgBox("Please type a name for this theme.", 48, "Error")
            Return
        End If
        If selected_theme = "" And selected_explorer = "" And selected_explorerFrame = "" And selected_OobeFldr = "" And selected_shell32 = "" And selected_timedate = "" And selected_logon = "" Then
            MsgBox("At least one component must be selected.", 48, "Error")
            Return
        End If

        
        For Each c As Char In System.IO.Path.GetInvalidFileNameChars
            If txtName.Text.Contains(c) Then
                MsgBox("This name contains an invalid character (" + c + ").", 48, "Error")
                Return
            End If
        Next
        If txtName.Text.Contains(Chr(39)) Then
            MsgBox("This name contains an invalid character (" + Chr(39) + ").", 48, "Error")
            Return
        End If
        If txtName.Text.Contains("&") Then
            MsgBox("This name contains an invalid character (" + "&" + ").", 48, "Error")
            Return
        End If
        If FileIO.FileSystem.DirectoryExists(themePath + "\" + txtName.Text) Or txtName.Text = "Aero" Then
            MsgBox("A theme with this name already exists. Please choose a different name.", 48, "Error")
            Return
        End If

        Install_Apply_Remove.install(txtName.Text, selected_theme, selected_explorer, selected_explorerFrame, selected_OobeFldr, selected_shell32, selected_timedate, selected_logon, selected_blur)

        Me.NavigationService.GoBack()

    End Sub

    Private Sub btnTheme_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnTheme.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".theme")) Then
            selected_theme = (s(0))
            imgHasTheme.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasTheme.IsEnabled = True
            If txtName.Text = "" Then
                txtName.Text = getFileName(selected_theme, False)
            End If
        Else
            MsgBox("This is not a theme file.")
        End If

    End Sub

    Private Sub btnExplorer_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnExplorer.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".exe")) Then
            selected_explorer = (s(0))
            imgHasExplorer.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasExplorer.IsEnabled = True
        Else
            MsgBox("This is not an explorer file.")
        End If

    End Sub

    Private Sub btnExplorerFrame_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnExplorerFrame.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".dll")) Then
            selected_explorerFrame = (s(0))
            imgHasExplorerFrame.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasExplorerFrame.IsEnabled = True
        Else
            MsgBox("This is not an explorerFrame file.")
        End If

    End Sub

    Private Sub btnShell32_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnShell32.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".dll")) Then
            selected_shell32 = (s(0))
            imgHasShell32.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasShell32.IsEnabled = True
        Else
            MsgBox("This is not a shell32 file.")
        End If

    End Sub

    Private Sub btnOobeFldr_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnOobeFldr.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".dll")) Then
            selected_OobeFldr = (s(0))
            imgHasOobeFldr.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasOobeFldr.IsEnabled = True
        Else
            MsgBox("This is not an OobeFldr file.")
        End If

    End Sub

    Private Sub btnTimeDate_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnTimedate.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".cpl")) Then
            selected_timedate = (s(0))
            imgHasTimeDate.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasTimeDate.IsEnabled = True
        Else
            MsgBox("This is not a TimeDate file.")
        End If

    End Sub

    Private Sub btnLogon_Drop(ByVal sender As System.Object, ByVal e As System.Windows.DragEventArgs) Handles btnLogon.Drop
        Dim s() As String = e.Data.GetData("FileDrop", False)

        If (s.Length = 1 And s(0).EndsWith(".jpg")) Then
            selected_logon = (s(0))
            imgHasLogon.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            btnHasLogon.IsEnabled = True
        Else
            MsgBox("This is not a logon file.")
        End If

    End Sub

    Private Sub btnHasTheme_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasTheme.Click
        selected_theme = ""
        imgHasTheme.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasLogon.IsEnabled = False
    End Sub

    Private Sub btnHasExplorer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasExplorer.Click
        selected_explorer = ""
        imgHasExplorer.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasExplorer.IsEnabled = False
    End Sub

    Private Sub btnHasExplorerFrame_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasExplorerFrame.Click
        selected_explorerFrame = ""
        imgHasExplorerFrame.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasExplorerFrame.IsEnabled = False
    End Sub

    Private Sub btnHasShell32_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasShell32.Click
        selected_shell32 = ""
        imgHasShell32.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasShell32.IsEnabled = False
    End Sub

    Private Sub btnHasOobeFldr_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasOobeFldr.Click
        selected_OobeFldr = ""
        imgHasOobeFldr.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasOobeFldr.IsEnabled = False
    End Sub

    Private Sub btnHasTimeDate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasTimeDate.Click
        selected_timedate = ""
        imgHasTimeDate.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasTimeDate.IsEnabled = False
    End Sub

    Private Sub btnHasLogon_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasLogon.Click
        selected_logon = ""
        imgHasLogon.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        btnHasLogon.IsEnabled = False
    End Sub

    Private Sub btnHasBlur_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnHasBlur.Click
        If selected_blur = True Then
            selected_blur = False
            imgHasBlur.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        Else
            selected_blur = True
            imgHasBlur.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
        End If
    End Sub
End Class
