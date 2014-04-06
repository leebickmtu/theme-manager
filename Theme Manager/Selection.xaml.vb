Imports System.Xml
Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods
Imports Theme_Manager.MainWindow
Imports Theme_Manager.Install_Apply_Remove

Class Selection

    Private isApply As Boolean = False
    Private isRemove As Boolean = False
    Private currentIndex As Integer = -1

    Private selected_theme As Integer
    Private selected_explorer As Integer
    Private selected_explorerFrame As Integer
    Private selected_OobeFldr As Integer
    Private selected_shell32 As Integer
    Private selected_timedate As Integer
    Private selected_logon As Integer
    Private selected_blur As Integer

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

    Private Sub SelectTheme_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        loadThemeInfo(themeList.currentTheme())
        refreshThemeNameList()
        If My.Settings.ListView = True Then
            chbView.IsChecked = True
            chbView_Checked(Nothing, Nothing)
        Else
            chbView.IsChecked = False
            chbView_Unchecked(Nothing, Nothing)
        End If
    End Sub

    Private Sub refreshThemeNameList()
        Dim temp() As String = themeList.getList()
        lstThemeName.Items.Clear()
        For Each s As String In temp
            lstThemeName.Items.Add(s)
        Next
        lstThemeName.SelectedIndex = 0
    End Sub

    Private Sub chbView_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles chbView.Checked
        lstThemeName.Visibility = Windows.Visibility.Visible
        btnLeftArrow.Visibility = Windows.Visibility.Hidden
        btnRightArrow.Visibility = Windows.Visibility.Hidden
        imgLeftArrow.Visibility = Windows.Visibility.Hidden
        imgRightArrow.Visibility = Windows.Visibility.Hidden
        txbThemeName.Visibility = Windows.Visibility.Hidden
        My.Settings.ListView = True
        My.Settings.Save()
    End Sub

    Private Sub chbView_Unchecked(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles chbView.Unchecked
        lstThemeName.Visibility = Windows.Visibility.Hidden
        btnLeftArrow.Visibility = Windows.Visibility.Visible
        btnRightArrow.Visibility = Windows.Visibility.Visible
        imgLeftArrow.Visibility = Windows.Visibility.Visible
        imgRightArrow.Visibility = Windows.Visibility.Visible
        txbThemeName.Visibility = Windows.Visibility.Visible
        My.Settings.ListView = False
        My.Settings.Save()
    End Sub

    Public Sub isApplySelection(ByVal b As Boolean)
        isApply = True
        'lblSelect.Content = "Apply a Theme"
        lblContinue.Content = "Apply"
        btnHasTheme.IsEnabled = True
        btnHasExplorer.IsEnabled = True
        btnHasExplorerFrame.IsEnabled = True
        btnHasShell32.IsEnabled = True
        btnHasOobeFldr.IsEnabled = True
        btnHasTimeDate.IsEnabled = True
        btnHasLogon.IsEnabled = True
        btnHasBlur.IsEnabled = True
        lblGreen.Visibility = Windows.Visibility.Visible
        imgGreen.Visibility = Windows.Visibility.Visible
        lblOrange.Visibility = Windows.Visibility.Visible
        imgOrange.Visibility = Windows.Visibility.Visible
        lblRed.Visibility = Windows.Visibility.Visible
        imgRed.Visibility = Windows.Visibility.Visible
    End Sub

    Public Sub isRemoveSelection(ByVal b As Boolean)
        isRemove = True
        'lblSelect.Content = "Remove a Theme"
        lblContinue.Content = "Remove"
    End Sub

    Private Sub btnBC_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBCstart.Click, btnBCselect.Click
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

    Private Sub btnLeftArrow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnLeftArrow.Click
        loadThemeInfo(themeList.previousTheme())
    End Sub

    Private Sub btnRightArrow_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnRightArrow.Click
        loadThemeInfo(themeList.nextTheme())
    End Sub

    Private Sub lstThemeName_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstThemeName.SelectionChanged
        loadThemeInfo(themeList.specificTheme(lstThemeName.SelectedItem))
    End Sub

    Private Sub loadThemeInfo(ByVal theme As aTheme)
        If theme.Theme <> "" Then
            imgHasTheme.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_theme = 1
        Else
            imgHasTheme.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_theme = 0
        End If
        If theme.Explorer <> "" Then
            imgHasExplorer.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_explorer = 1
        Else
            imgHasExplorer.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_explorer = 0
        End If
        If theme.OobeFldr <> "" Then
            imgHasOobeFldr.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_OobeFldr = 1
        Else
            imgHasOobeFldr.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_OobeFldr = 0
        End If
        If theme.ExplorerFrame <> "" Then
            imgHasExplorerFrame.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_explorerFrame = 1
        Else
            imgHasExplorerFrame.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_explorerFrame = 0
        End If
        If theme.Shell32 <> "" Then
            imgHasShell32.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_shell32 = 1
        Else
            imgHasShell32.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_shell32 = 0
        End If
        If theme.TimeDate <> "" Then
            imgHasTimeDate.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_timedate = 1
        Else
            imgHasTimeDate.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_timedate = 0
        End If
        If theme.Logon <> "" Then
            imgHasLogon.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_logon = 1
        Else
            imgHasLogon.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_logon = 0
        End If
        If theme.Blur Then
            imgHasBlur.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            selected_blur = 1
        Else
            imgHasBlur.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
            selected_blur = 0
        End If

        txbThemeName.Text = theme.Name

        If FileIO.FileSystem.FileExists(themePath + "\Screenshots\" + theme.Name + ".png") Then
            imgPreview.Source = getProperImage(themePath + "\Screenshots\" + theme.Name + ".png", UriKind.Absolute, True, True)
        Else
            imgPreview.Source = getProperImage("\Images\question_mark_v2.png", UriKind.Relative)
        End If

    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPreview.Click
        ' view Preview page
        Dim preview As New Preview()
        preview.getRef(mW)
        Me.NavigationService.Navigate(preview)
    End Sub

    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnContinue.Click
        If isApply Then
            apply(selected_theme, selected_explorer, selected_explorerFrame, selected_OobeFldr, selected_shell32, selected_timedate, selected_logon, selected_blur)
        ElseIf isRemove Then
            remove()
            loadThemeInfo(themeList.currentTheme)
            refreshThemeNameList()
        End If
    End Sub

    Private Sub btnHasTheme_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasTheme.Click
        If selected_theme = 1 Then
            selected_theme = 2
            imgHasTheme.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_theme = 2 Then
            selected_theme = 0
            imgHasTheme.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_theme = 0 Then
            If themeList.currentTheme.Theme <> "" Then
                selected_theme = 1
                imgHasTheme.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_theme = 2
                imgHasTheme.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasExplorer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasExplorer.Click
        If selected_explorer = 1 Then
            selected_explorer = 2
            imgHasExplorer.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_explorer = 2 Then
            selected_explorer = 0
            imgHasExplorer.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_explorer = 0 Then
            If themeList.currentTheme.Explorer <> "" Then
                selected_explorer = 1
                imgHasExplorer.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_explorer = 2
                imgHasExplorer.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasExplorerFrame_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasExplorerFrame.Click
        If selected_explorerFrame = 1 Then
            selected_explorerFrame = 2
            imgHasExplorerFrame.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_explorerFrame = 2 Then
            selected_explorerFrame = 0
            imgHasExplorerFrame.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_explorerFrame = 0 Then
            If themeList.currentTheme.ExplorerFrame <> "" Then
                selected_explorerFrame = 1
                imgHasExplorerFrame.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_explorerFrame = 2
                imgHasExplorerFrame.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasShell32_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasShell32.Click
        If selected_shell32 = 1 Then
            selected_shell32 = 2
            imgHasShell32.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_shell32 = 2 Then
            selected_shell32 = 0
            imgHasShell32.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_shell32 = 0 Then
            If themeList.currentTheme.Shell32 <> "" Then
                selected_shell32 = 1
                imgHasShell32.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_shell32 = 2
                imgHasShell32.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasOobeFldr_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasOobeFldr.Click
        If selected_OobeFldr = 1 Then
            selected_OobeFldr = 2
            imgHasOobeFldr.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_OobeFldr = 2 Then
            selected_OobeFldr = 0
            imgHasOobeFldr.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_OobeFldr = 0 Then
            If themeList.currentTheme.OobeFldr <> "" Then
                selected_OobeFldr = 1
                imgHasOobeFldr.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_OobeFldr = 2
                imgHasOobeFldr.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasTimeDate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasTimeDate.Click
        If selected_timedate = 1 Then
            selected_timedate = 2
            imgHasTimeDate.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_timedate = 2 Then
            selected_timedate = 0
            imgHasTimeDate.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_timedate = 0 Then
            If themeList.currentTheme.TimeDate <> "" Then
                selected_timedate = 1
                imgHasTimeDate.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_timedate = 2
                imgHasTimeDate.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasLogon_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasLogon.Click
        If selected_logon = 1 Then
            selected_logon = 2
            imgHasLogon.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        ElseIf selected_logon = 2 Then
            selected_logon = 0
            imgHasLogon.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        ElseIf selected_logon = 0 Then
            If themeList.currentTheme.Logon <> "" Then
                selected_logon = 1
                imgHasLogon.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
            Else
                selected_logon = 2
                imgHasLogon.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
            End If
        End If
    End Sub

    Private Sub btnHasBlur_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnHasBlur.Click
        'If selected_blur = 1 Then
        '    selected_blur = 2
        '    imgHasBlur.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        'ElseIf selected_blur = 2 Then
        '    selected_blur = 0
        '    imgHasBlur.Source = getProperImage("\Images\red_x.png", UriKind.Relative)
        'ElseIf selected_blur = 0 Then
        '    If themeList.currentTheme.Blur <> "" Then
        '        selected_blur = 1
        '        imgHasBlur.Source = getProperImage("\Images\green_check.png", UriKind.Relative)
        '    Else
        '        selected_blur = 2
        '        imgHasBlur.Source = getProperImage("\Images\orange_check.png", UriKind.Relative)
        '    End If
        'End If
    End Sub

End Class
