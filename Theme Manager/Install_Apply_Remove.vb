Imports Theme_Manager.Structures
Imports Theme_Manager.CommonMethods
Imports Theme_Manager.MainWindow
Imports System.Xml

Module Install_Apply_Remove

    Public Sub install(ByVal selected_name As String, ByVal selected_theme As String, ByVal selected_explorer As String, ByVal selected_explorerFrame As String, ByVal selected_OobeFldr As String, ByVal selected_shell32 As String, ByVal selected_timedate As String, ByVal selected_logon As String, ByVal selected_blur As Boolean)
        If selected_explorer <> "" Then
            If (Dll64Detect.UnmanagedDllIs64Bit(selected_explorer) <> system64) Then
                MsgBox("The selected explorer.exe is not compatible with this system. This is most likely caused by using a 32-bit file in a 64-bit system or vice-versa.", 48, "Error")
                Return
            End If
        End If
        If selected_OobeFldr <> "" Then
            If (Dll64Detect.UnmanagedDllIs64Bit(selected_OobeFldr) <> system64) Then
                MsgBox("The selected OobeFldr.dll is not compatible with this system. This is most likely caused by using a 32-bit file in a 64-bit system or vice-versa.", 48, "Error")
                Return
            End If
        End If
        If selected_explorerFrame <> "" Then
            If (Dll64Detect.UnmanagedDllIs64Bit(selected_explorerFrame) <> system64) Then
                MsgBox("The selected ExplorerFrame.dll is not compatible with this system. This is most likely caused by using a 32-bit file in a 64-bit system or vice-versa.", 48, "Error")
                Return
            End If
        End If
        If selected_shell32 <> "" Then
            If (Dll64Detect.UnmanagedDllIs64Bit(selected_shell32) <> system64) Then
                MsgBox("The selected Shell32.dll is not compatible with this system. This is most likely caused by using a 32-bit file in a 64-bit system or vice-versa.", 48, "Error")
                Return
            End If
        End If
        If selected_timedate <> "" Then
            If (Dll64Detect.UnmanagedDllIs64Bit(selected_timedate) <> system64) Then
                MsgBox("The selected timedate.cpl is not compatible with this system. This is most likely caused by using a 32-bit file in a 64-bit system or vice-versa.", 48, "Error")
                Return
            End If
        End If

        Dim id As String = selected_name
        Dim theme As String = ""
        Dim explorer As String = ""
        Dim oobefldr As String = ""
        Dim explorerframe As String = ""
        Dim shell32 As String = ""
        Dim timedate As String = ""
        Dim logon As String = ""


        If selected_theme <> "" Then
            theme = fixFileName(getFileName(selected_theme, True))

            Dim themeFolder As String = getVisualStylesInfo(selected_theme, 1)
            Dim fullThemeFolderPath = selected_theme.Substring(0, selected_theme.LastIndexOfAny("\")) + "\" + themeFolder

            FileIO.FileSystem.CreateDirectory(themePath + "\" + id)
            changeAndCopyTheme(selected_theme, id, selected_theme)
            FileIO.FileSystem.CopyDirectory(fullThemeFolderPath, themePath + "\" + id + "\" + themeFolder)
        End If

        If selected_explorer <> "" Then
            explorer = MD5CalcFile(selected_explorer)
            FileIO.FileSystem.CopyFile(selected_explorer, themePath + "\" + id + "\explorer.exe")
        End If

        If selected_OobeFldr <> "" Then
            oobefldr = MD5CalcFile(selected_OobeFldr)
            FileIO.FileSystem.CopyFile(selected_OobeFldr, themePath + "\" + id + "\OobeFldr.dll")
        End If

        If selected_explorerFrame <> "" Then
            explorerframe = MD5CalcFile(selected_explorerFrame)
            FileIO.FileSystem.CopyFile(selected_explorerFrame, themePath + "\" + id + "\ExplorerFrame.dll")
        End If

        If selected_shell32 <> "" Then
            shell32 = MD5CalcFile(selected_shell32)
            FileIO.FileSystem.CopyFile(selected_shell32, themePath + "\" + id + "\shell32.dll")
        End If

        If selected_timedate <> "" Then
            timedate = MD5CalcFile(selected_timedate)
            FileIO.FileSystem.CopyFile(selected_timedate, themePath + "\" + id + "\timedate.cpl")
        End If

        If selected_logon <> "" Then
            logon = MD5CalcFile(selected_logon)
            FileIO.FileSystem.CopyFile(selected_logon, themePath + "\" + id + "\backgroundDefault.jpg")
        End If

        ' Create our string that holds our new Theme information.
        Dim cr As String = Environment.NewLine
        Dim newTheme As String = _
            "<Theme ID='" & id & "'>" & cr & _
            "    <Theme>" & theme & "</Theme>" & cr & _
            "    <Explorer>" & explorer & "</Explorer>" & cr & _
            "    <OobeFldr>" & oobefldr & "</OobeFldr>" & cr & _
            "    <ExplorerFrame>" & explorerframe & "</ExplorerFrame>" & cr & _
            "    <Shell32>" & shell32 & "</Shell32>" & cr & _
            "    <Timedate>" & timedate & "</Timedate>" & cr & _
            "    <Logon>" & logon & "</Logon>" & cr & _
            "    <Blur>" & selected_blur & "</Blur>" & cr & _
            "  </Theme>"

        ' Load the XmlDocument.
        Dim xd As New XmlDocument()
        xd.Load(themePath + "\theme_info.xml")
        ' Create a new XmlDocumentFragment for our document.
        Dim docFrag As XmlDocumentFragment = xd.CreateDocumentFragment()
        ' The Xml for this fragment is our newTheme string.
        docFrag.InnerXml = newTheme
        ' The root element of our file is found using
        ' the DocumentElement property of the XmlDocument.
        Dim root As XmlNode = xd.DocumentElement
        ' Append our new Theme to the root element.
        root.AppendChild(docFrag)

        ' Save the Xml.
        xd.Save(themePath + "\theme_info.xml")

        themeList.refreshThemes()
    End Sub





    Public Sub apply(ByVal selected_theme As Integer, ByVal selected_explorer As Integer, ByVal selected_explorerFrame As Integer, ByVal selected_OobeFldr As Integer, ByVal selected_shell32 As Integer, ByVal selected_timedate As Integer, ByVal selected_logon As Integer, ByVal selected_blur As Integer)
        Dim restartRequired As Boolean = False
        Dim selection As String = themeList.currentTheme.Name

        If deleteTempSystemFiles() = False Then
            MsgBox("A theme application has already been started. A system restart is required before another theme can be applied.", 48, "System Restart Required")
            Return
        End If

        'Explorer
        If selected_explorer = 1 Then
            If themeList.currentTheme.Explorer <> MD5CalcFile(winPath + "\explorer.exe") Then
                FileIO.FileSystem.RenameFile(winPath + "\explorer.exe", "explorer_temp.exe")
                FileIO.FileSystem.CopyFile(themePath + "\" + selection + "\explorer.exe", winPath + "\explorer.exe")
            End If
        ElseIf selected_explorer = 0 Then
            If MD5CalcFile(themePath + "\Default\explorer.exe") <> MD5CalcFile(winPath + "\explorer.exe") Then
                FileIO.FileSystem.RenameFile(winPath + "\explorer.exe", "explorer_temp.exe")
                FileIO.FileSystem.CopyFile(themePath + "\Default\explorer.exe", winPath + "\explorer.exe")
            End If
        End If

        'OobeFldr
        If selected_OobeFldr = 1 Then
            If themeList.currentTheme.OobeFldr <> MD5CalcFile(sysPath + "\OobeFldr.dll") Then
                FileIO.FileSystem.RenameFile(sysPath + "\OobeFldr.dll", "OobeFldr_temp.dll")
                FileIO.FileSystem.CopyFile(themePath + "\" + selection + "\OobeFldr.dll", sysPath + "\OobeFldr.dll")
                restartRequired = True
            End If
        ElseIf selected_OobeFldr = 0 Then
            If MD5CalcFile(themePath + "\Default\OobeFldr.dll") <> MD5CalcFile(sysPath + "\OobeFldr.dll") Then
                FileIO.FileSystem.RenameFile(sysPath + "\OobeFldr.dll", "OobeFldr_temp.dll")
                FileIO.FileSystem.CopyFile(themePath + "\Default\OobeFldr.dll", sysPath + "\OobeFldr.dll")
                restartRequired = True
            End If
        End If

        'ExplorerFrame
        If selected_explorerFrame = 1 Then
            If themeList.currentTheme.ExplorerFrame <> MD5CalcFile(sysPath + "\ExplorerFrame.dll") Then
                FileIO.FileSystem.RenameFile(sysPath + "\ExplorerFrame.dll", "ExplorerFrame_temp.dll")
                FileIO.FileSystem.CopyFile(themePath + "\" + selection + "\ExplorerFrame.dll", sysPath + "\ExplorerFrame.dll")
            End If
        ElseIf selected_explorerFrame = 0 Then
            If MD5CalcFile(themePath + "\Default\ExplorerFrame.dll") <> MD5CalcFile(sysPath + "\ExplorerFrame.dll") Then
                FileIO.FileSystem.RenameFile(sysPath + "\ExplorerFrame.dll", "ExplorerFrame_temp.dll")
                FileIO.FileSystem.CopyFile(themePath + "\Default\ExplorerFrame.dll", sysPath + "\ExplorerFrame.dll")
            End If
        End If

        'Shell32
        If selected_shell32 = 1 Then
            If themeList.currentTheme.Shell32 <> MD5CalcFile(sysPath + "\shell32.dll") Then
                FileIO.FileSystem.RenameFile(sysPath + "\shell32.dll", "shell32_temp.dll")
                FileIO.FileSystem.CopyFile(themePath + "\" + selection + "\shell32.dll", sysPath + "\shell32.dll")
                restartRequired = True
            End If
        ElseIf selected_shell32 = 0 Then
            If MD5CalcFile(themePath + "\Default\shell32.dll") <> MD5CalcFile(sysPath + "\shell32.dll") Then
                FileIO.FileSystem.RenameFile(sysPath + "\shell32.dll", "shell32_temp.dll")
                FileIO.FileSystem.CopyFile(themePath + "\Default\shell32.dll", sysPath + "\shell32.dll")
                restartRequired = True
            End If
        End If

        'Timedate
        If selected_timedate = 1 Then
            If themeList.currentTheme.TimeDate <> MD5CalcFile(sysPath + "\timedate.cpl") Then
                FileIO.FileSystem.RenameFile(sysPath + "\timedate.cpl", "timedate_temp.cpl")
                FileIO.FileSystem.CopyFile(themePath + "\" + selection + "\timedate.cpl", sysPath + "\timedate.cpl")
            End If
        ElseIf selected_timedate = 0 Then
            If MD5CalcFile(themePath + "\Default\timedate.cpl") <> MD5CalcFile(sysPath + "\timedate.cpl") Then
                FileIO.FileSystem.RenameFile(sysPath + "\timedate.cpl", "timedate_temp.cpl")
                FileIO.FileSystem.CopyFile(themePath + "\Default\timedate.cpl", sysPath + "\timedate.cpl")
            End If
        End If

        'Logon
        If selected_logon = 1 Then
            If FileIO.FileSystem.FileExists(sysPath + "\oobe\info\backgrounds\backgroundDefault.jpg") Then
                FileIO.FileSystem.DeleteFile(sysPath + "\oobe\info\backgrounds\backgroundDefault.jpg")
            End If
            FileIO.FileSystem.CopyFile(themePath + "\" + selection + "\backgroundDefault.jpg", sysPath + "\oobe\info\backgrounds\backgroundDefault.jpg")
            Interaction.Shell(sysPath + "\cmd.exe /c reg add HKLM\Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background /v OEMBackground /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
        ElseIf selected_logon = 0 Then
            If FileIO.FileSystem.FileExists(sysPath + "\oobe\info\backgrounds\backgroundDefault.jpg") Then
                FileIO.FileSystem.DeleteFile(sysPath + "\oobe\info\backgrounds\backgroundDefault.jpg")
            End If
            Interaction.Shell(sysPath + "\cmd.exe /c reg add HKLM\Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background /v OEMBackground /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        restartExplorer()
        If (selection = "Aero" And selected_theme = 1) Or selected_theme = 0 Then
            OpenDocument(Environ("windir") + "\Resources\Themes\" + themeList.currentTheme.Theme)
        ElseIf selected_theme = 1 Then
            OpenDocument(themePath + "\" + selection + "\" + themeList.currentTheme.Theme)
        End If

        If restartRequired = True Then
            MsgBox("A system restart will be required before the theme can be fully applied.", 48, "System Restart Required")
        End If
    End Sub





    Public Sub remove()
        Dim selection As String = themeList.currentTheme.Name
        If selection = "Aero" Then
            MsgBox("Aero is the default theme and cannot be removed.", 48, "Error")
            Return
        End If

        Dim currentTheme As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ThemeManager", "DllName", "")
        If currentTheme.Contains("Theme Manager\" + selection) Then
            MsgBox(selection + " is the current theme and cannot be removed." + vbNewLine + vbNewLine + "Apply a different theme first.", 48, "Error")
            Return
        End If

        ' Load the XmlDocument.
        Dim xd As New XmlDocument()
        xd.Load(themePath + "\theme_info.xml")

        ' Find Theme node with the attribute ID of selection
        ' using the XPath again.
        Dim nod As XmlNode = xd.SelectSingleNode("/Themes/Theme[@ID='" & selection & "']")
        If nod IsNot Nothing Then
            ' Since we found the Theme node, we will remove
            ' it and all of its information.
            If FileIO.FileSystem.DirectoryExists(themePath + "\" + selection) Then
                FileIO.FileSystem.DeleteDirectory(themePath + "\" + selection, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
            If FileIO.FileSystem.FileExists(themePath + "\Screenshots\" + selection + ".png") Then
                'PicScreenshot.Image.Dispose()
                FileIO.FileSystem.DeleteFile(themePath + "\Screenshots\" + selection + ".png")
            End If
            nod.ParentNode.RemoveChild(nod)
        Else
            ' Could not find the Theme?
            MsgBox("Couldn't find " & selection & ".", 48, "Error")
        End If

        ' Save the Xml.
        xd.Save(themePath + "\theme_info.xml")

        themeList.refreshThemes()
    End Sub

End Module
