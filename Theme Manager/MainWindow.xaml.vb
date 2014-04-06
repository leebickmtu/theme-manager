'Imports System.Diagnostics.Process
'Imports System.IO

'Imports Microsoft.Win32
'Imports System.Text
'Imports Ionic.Zip
Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods

Public Class MainWindow

    Const import_export_version As Integer = 1

    Private Shared masterSystem64 As Boolean = Environment.Is64BitOperatingSystem
    Private Shared masterWinPath As String = Environ("windir")
    Private Shared masterSysPath As String = If(Environment.Is64BitOperatingSystem(), Environ("windir") + "\Sysnative", Environ("windir") + "\System32")
    Private Shared masterThemePath As String = Environ("windir") + "\Resources\Themes\Theme Manager"

    Public Shared themeList As ThemeStorage

    Private Sub NavigationWindow_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If My.Settings.FirstRun = True Then
            Dim r As MsgBoxResult = MsgBox("Your system files must be patched prior to using this software." + vbNewLine + vbNewLine + "Use with Caution. This program replaces essential system files. If your system fails to boot use the Windows Recovery Disk to restore system files.", vbOKCancel)
            If r = MsgBoxResult.Cancel Then
                Me.Close()
            Else
                If FileIO.FileSystem.DirectoryExists(themePath + "\Default") = True Then
                    FileIO.FileSystem.DeleteDirectory(themePath + "\Default", FileIO.DeleteDirectoryOption.DeleteAllContents)
                End If
                My.Settings.FirstRun = False
                My.Settings.Save()
            End If
        End If

        createEssentialFolders()
        createEssentialFiles()

        themeList = New ThemeStorage

        getAllFileOwnership()

        ' Check if system has been restarted, disable "Apply" button if it has not
        If deleteTempSystemFiles() = False Then
            MsgBox("A theme application has already been started. A system restart is required before another theme can be applied.", 48, "System Restart Required")
        End If
        
        ' load Start page
        Dim startPage As New Start()
        startPage.getRef(Me)
        Me.NavigationService.Navigate(startPage)
       
    End Sub

    Public Shared ReadOnly Property system64 As Boolean
        Get
            Return masterSystem64
        End Get
    End Property

    Public Shared ReadOnly Property winPath As String
        Get
            Return masterWinPath
        End Get
    End Property

    Public Shared ReadOnly Property sysPath As String
        Get
            Return masterSysPath
        End Get
    End Property

    Public Shared ReadOnly Property themePath As String
        Get
            Return masterThemePath
        End Get
    End Property

    Private Sub NavigationWindow_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs) Handles MyBase.KeyDown
        If (Keyboard.IsKeyDown(Key.Escape) And Me.NavigationService.CanGoBack) Then
            Me.NavigationService.GoBack()
        End If
    End Sub
End Class
