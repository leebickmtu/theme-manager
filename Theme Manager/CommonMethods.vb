Imports Theme_Manager.MainWindow
Imports System.IO
Imports Ionic.Zip

Module CommonMethods

    Public Function getProperImage(ByVal source As String, ByVal type As UriKind) As BitmapImage
        Dim img As New BitmapImage()
        img.BeginInit()
        img.UriSource = New Uri(source, type)
        img.EndInit()

        Return img
    End Function

    Public Function getProperImage(ByVal source As String, ByVal type As UriKind, ByVal cache As Boolean, ByVal ignoreCache As Boolean) As BitmapImage
        Dim img As New BitmapImage()
        img.BeginInit()
        If cache Then
            img.CacheOption = BitmapCacheOption.OnLoad
        End If
        If ignoreCache Then
            img.CreateOptions = BitmapCreateOptions.IgnoreImageCache
        End If
        img.UriSource = New Uri(source, type)
        img.EndInit()

        Return img
    End Function

    ' specify the path to a file and this routine will calculate your hash
    Public Function MD5CalcFile(ByVal filepath As String) As String

        ' open file (as read-only)
        Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider

                ' hash contents of this stream
                Dim hash() As Byte = md5.ComputeHash(reader)

                ' return formatted hash
                Return ByteArrayToString(hash)

            End Using
        End Using

    End Function

    ' utility function to convert a byte array into a hex string
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String

        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)

        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next

        Return (sb.ToString().ToLower)

    End Function

    Public Sub takeOwnership(ByVal FileName As String)
        Interaction.Shell("cmd.exe /c takeown /f " + FileName + " && icacls " + FileName + " /grant administrators:F", AppWinStyle.Hide, True)
    End Sub

    Public Sub getAllFileOwnership()
        ' Get owership and write/read rights to system files
        takeOwnership(winPath + "\explorer.exe")
        takeOwnership(sysPath + "\OobeFldr.dll")
        takeOwnership(sysPath + "\ExplorerFrame.dll")
        takeOwnership(sysPath + "\shell32.dll")
        takeOwnership(sysPath + "\timedate.cpl")
        takeOwnership(sysPath + "\dwm.exe")
        takeOwnership(sysPath + "\udwm.dll")
        ' Same for temp files
        takeOwnership(winPath + "\explorer_temp.exe")
        takeOwnership(sysPath + "\OobeFldr_temp.dll")
        takeOwnership(sysPath + "\ExplorerFrame_temp.dll")
        takeOwnership(sysPath + "\shell32_temp.dll")
        takeOwnership(sysPath + "\timedate_temp.cpl")
        takeOwnership(sysPath + "\dwm_temp.exe")
        takeOwnership(sysPath + "\udwm_temp.dll")
    End Sub

    Private Sub killExplorer()
        Dim p As Process = New Process()
        p.StartInfo.FileName = "taskKill"
        p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
        p.StartInfo.Arguments = "/F /IM explorer.exe"

        p.Start()

        p.WaitForExit()
    End Sub

    Private Sub startExplorer()
        Process.Start(winPath + "\explorer.exe")
    End Sub

    Public Sub restartExplorer()
        killExplorer()
        startExplorer()
    End Sub

    Public Function deleteTempSystemFiles() As Boolean
        Dim success As Boolean = True

        If FileIO.FileSystem.FileExists(winPath + "\explorer_temp.exe") = True Then
            Try
                FileIO.FileSystem.DeleteFile(winPath + "\explorer_temp.exe")
            Catch ex As Exception
                success = False
            End Try
        End If
        If FileIO.FileSystem.FileExists(sysPath + "\shell32_temp.dll") = True Then
            Try
                FileIO.FileSystem.DeleteFile(sysPath + "\shell32_temp.dll")
            Catch ex As Exception
                success = False
            End Try
        End If
        If FileIO.FileSystem.FileExists(sysPath + "\ExplorerFrame_temp.dll") = True Then
            Try
                FileIO.FileSystem.DeleteFile(sysPath + "\ExplorerFrame_temp.dll")
            Catch ex As Exception
                success = False
            End Try
        End If
        If FileIO.FileSystem.FileExists(sysPath + "\OobeFldr_temp.dll") = True Then
            Try
                FileIO.FileSystem.DeleteFile(sysPath + "\OobeFldr_temp.dll")
            Catch ex As Exception
                success = False
            End Try
        End If
        If FileIO.FileSystem.FileExists(sysPath + "\timedate_temp.cpl") = True Then
            Try
                FileIO.FileSystem.DeleteFile(sysPath + "\timedate_temp.cpl")
            Catch ex As Exception
                success = False
            End Try
        End If
        If FileIO.FileSystem.FileExists(sysPath + "\dwm_temp.exe") = True Then
            Try
                FileIO.FileSystem.DeleteFile(sysPath + "\dwm_temp.exe")
            Catch ex As Exception
                success = False
            End Try
        End If
        If FileIO.FileSystem.FileExists(sysPath + "\udwm_temp.dll") = True Then
            Try
                FileIO.FileSystem.DeleteFile(sysPath + "\udwm_temp.dll")
            Catch ex As Exception
                success = False
            End Try
        End If

        Return success
    End Function

    Public Function OpenDocument(ByVal DocName As String) As Boolean

        Try
            Process.Start(DocName)
            Return True
        Catch
            Return False
        End Try

    End Function

    Public Sub Delay(ByVal dblSecs As Double)

        Const OneSec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblWaitTil As Date
        Now.AddSeconds(OneSec)
        dblWaitTil = Now.AddSeconds(OneSec).AddSeconds(dblSecs)
        Do Until Now > dblWaitTil
            'DoEvents() ' Allow windows messages to be processed
            System.Windows.Forms.Application.DoEvents()
        Loop

    End Sub

    ''' <summary>
    ''' Retreives the VisualStyles Path Information from a .theme file.
    ''' </summary>
    ''' <param name="themeFilePath">The file path to the .theme file</param>
    ''' <param name="infoType">1 = Folder containing .msstyles file, 2 = .msstyles file name + extension, 3 = combo of 1 and 2</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getVisualStylesInfo(ByVal themeFilePath As String, ByVal infoType As Integer) As String
        Dim objIniFile As New IniFile(themeFilePath)
        Dim str As String = objIniFile.GetString("VisualStyles", "Path", "(none)")
        Dim peices As String() = str.Split("\")
        If infoType = 1 Then
            str = peices(peices.Length - 2)
        ElseIf infoType = 2 Then
            str = peices(peices.Length - 1)
        ElseIf infoType = 3 Then
            str = peices(peices.Length - 2) + "\" + peices(peices.Length - 1)
        End If

        Return str
    End Function

    Public Function fixFileName(ByVal s As String) As String
        s = s.Replace("&", "")
        s = s.Replace(Chr(39), "")
        Return s
    End Function

    Public Sub changeAndCopyTheme(ByVal themeFilePath As String, ByVal id As String, ByVal selected_theme As String)
        Dim oRead As System.IO.StreamReader
        Dim oWrite As System.IO.StreamWriter
        Dim LineIn As String
        oRead = File.OpenText(themeFilePath)
        oWrite = File.CreateText(themePath + "\" + id + "\" + fixFileName(getFileName(selected_theme, True)))

        While oRead.Peek <> -1
            LineIn = oRead.ReadLine()

            If LineIn.IndexOf("themes\", StringComparison.CurrentCultureIgnoreCase) >= 0 Then
                Dim position As Integer = LineIn.IndexOf("themes\", StringComparison.CurrentCultureIgnoreCase)
                Dim currentCase As String = LineIn.Substring(position)
                currentCase = currentCase.Remove(7, currentCase.Length - 7)
                LineIn = Strings.Replace(LineIn, currentCase, currentCase + "Theme Manager\" + id + "\", CompareMethod.Text)
            End If

            oWrite.Write(LineIn)
            oWrite.WriteLine()
        End While

        oRead.Close()
        oWrite.Close()
    End Sub

    ''' <summary>
    ''' Retreives a file name from a file path.
    ''' </summary>
    ''' <param name="filePath">The file path to the file</param>
    ''' <param name="extension">Return with file extension?</param>
    ''' <returns>File name with or without extension</returns>
    ''' <remarks></remarks>
    Public Function getFileName(ByVal filePath As String, ByVal extension As Boolean)
        Dim str As String
        Dim startPos As Integer
        Dim endPos As Integer

        startPos = filePath.LastIndexOfAny("\")
        startPos += 1
        If extension = False Then
            endPos = filePath.LastIndexOfAny(".")
            endPos -= 1
        ElseIf extension = True Then

            endPos = filePath.Length - 1
        End If
        str = filePath.Substring(startPos, endPos - startPos + 1)

        Return str
    End Function

    Public Sub createEssentialFolders()
        If FileIO.FileSystem.DirectoryExists(themePath) = False Then
            ' Specify path of folder in constructor
            Dim dir As New System.IO.DirectoryInfo(themePath)

            ' Create folder
            dir.Create()

            ' That property makes the job
            dir.Attributes = IO.FileAttributes.Hidden
        End If
        If FileIO.FileSystem.DirectoryExists(themePath + "\Screenshots") = False Then
            FileIO.FileSystem.CreateDirectory(themePath + "\Screenshots")
        End If
        If FileIO.FileSystem.DirectoryExists(themePath + "\Default") = False Then
            FileIO.FileSystem.CreateDirectory(themePath + "\Default")
        End If
        If FileIO.FileSystem.DirectoryExists(sysPath + "\oobe\info\backgrounds") = False Then
            FileIO.FileSystem.CreateDirectory(sysPath + "\oobe\info\backgrounds")
        End If
    End Sub

    Public Sub createEssentialFiles()
        If FileIO.FileSystem.FileExists(themePath + "\Default\explorer.exe") = False Then
            extractSystemFile("explorer.exe")
        End If
        If FileIO.FileSystem.FileExists(themePath + "\Default\ExplorerFrame.dll") = False Then
            extractSystemFile("ExplorerFrame.dll")
        End If
        If FileIO.FileSystem.FileExists(themePath + "\Default\OobeFldr.dll") = False Then
            extractSystemFile("OobeFldr.dll")
        End If
        If FileIO.FileSystem.FileExists(themePath + "\Default\shell32.dll") = False Then
            extractSystemFile("shell32.dll")
        End If
        If FileIO.FileSystem.FileExists(themePath + "\Default\timedate.cpl") = False Then
            extractSystemFile("timedate.cpl")
        End If
        If FileIO.FileSystem.FileExists(themePath + "\theme_info.xml") = False Then
            FileIO.FileSystem.WriteAllText(themePath + "\theme_info.xml", My.Resources.theme_info, False)
        End If
    End Sub

    Private Sub extractSystemFile(ByVal file As String)
        Dim systemFiles As Byte()
        If system64 Then
            systemFiles = My.Resources.SystemFiles64
        Else
            systemFiles = My.Resources.SystemFiles32
        End If

        Try
            Using zip As ZipFile = ZipFile.Read(systemFiles)
                Dim f As ZipEntry
                For Each f In zip
                    If f.FileName = file Then
                        f.Extract(themePath + "\Default", ExtractExistingFileAction.OverwriteSilently)
                    End If
                Next
            End Using
        Catch ex1 As Exception
            Console.Error.WriteLine("exception: {0}", ex1.ToString)
        End Try
    End Sub

End Module
