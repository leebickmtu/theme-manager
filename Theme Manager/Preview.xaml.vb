Imports System.Diagnostics.Process
Imports System.IO
Imports System.Xml
Imports Microsoft.Win32
Imports System.Text
Imports System.Drawing
Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods
Imports Theme_Manager.MainWindow

Class Preview

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

    Private Sub Preview_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If FileIO.FileSystem.FileExists(themePath + "\Screenshots\" + themeList.currentTheme.Name + ".png") Then
            imgPreview.Source = getProperImage(themePath + "\Screenshots\" + themeList.currentTheme.Name + ".png", UriKind.Absolute, True, True)
        Else
            imgPreview.Source = getProperImage("\Images\question_mark.png", UriKind.Relative)
        End If
    End Sub

    Private Sub btnBC_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBCstart.Click, btnBCselect.Click, btnBCpreview.Click
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

    Private Sub takeScreenshot_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles takeScreenshot.Click
        Application.Current.MainWindow.WindowState = WindowState.Minimized

        System.Windows.Forms.SendKeys.SendWait("^{ESC}")
        Delay(0.4)

        Dim ix As Integer = 0
        Dim iy As Integer = 0
        Dim iw As Integer = My.Computer.Screen.Bounds.Width
        Dim ih As Integer = My.Computer.Screen.Bounds.Height
        Dim Image As Bitmap = New Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(Image)
        g.CopyFromScreen(ix, iy, ix, iy, New System.Drawing.Size(iw, ih), CopyPixelOperation.SourceCopy)
        Image.Save(themePath + "\Screenshots\" + themeList.currentTheme.Name + ".png", System.Drawing.Imaging.ImageFormat.Png)
        NavigationService.Refresh()

        Application.Current.MainWindow.WindowState = WindowState.Normal
    End Sub

End Class
