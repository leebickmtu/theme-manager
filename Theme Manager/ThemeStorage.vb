Imports System.Xml
Imports Theme_Manager.Structures

Public Class ThemeStorage

    Public Shared themeList() As aTheme
    Private currentIndex As Integer = 0

    Public Sub New()
        refreshThemes()
    End Sub

    Public Function getList() As String()
        Dim temp(themeList.Length) As String

        For i As Integer = 0 To (themeList.Length() - 1)
            temp(i) = themeList(i).Name
        Next

        Return temp
    End Function

    Public Function specificTheme(ByVal s As String) As aTheme
        For i As Integer = 0 To (themeList.Length() - 1)
            If themeList(i).Name = s Then
                currentIndex = i
                Exit For
            End If
        Next

        Return themeList(currentIndex)
    End Function

    Public Function previousTheme() As aTheme
        If isEmpty() Then
            Return Nothing
        End If

        If currentIndex - 1 < 0 Then
            currentIndex = themeList.Count - 1
        Else
            currentIndex = currentIndex - 1
        End If

        Return themeList(currentIndex)
    End Function

    Public Function nextTheme() As aTheme
        If isEmpty() Then
            Return Nothing
        End If

        If currentIndex + 1 = themeList.Count Then
            currentIndex = 0
        Else
            currentIndex = currentIndex + 1
        End If
        Return themeList(currentIndex)
    End Function

    Public Function currentTheme() As aTheme
        If isEmpty() Then
            Return Nothing
        End If

        Return themeList(currentIndex)
    End Function

    Public Sub refreshThemes()

        ' Load the XmlDocument.
        Dim xd As New XmlDocument()
        xd.Load(MainWindow.themePath + "\theme_info.xml")

        Dim nodes As XmlNodeList = xd.DocumentElement.ChildNodes

        ReDim themeList(0 To nodes.Count - 1)

        Dim changes As Boolean = False
        Dim i As Integer = 0
        For Each nod As XmlNode In nodes
            If nod.ChildNodes.Count < 6 Then
                Dim docFrag As XmlDocumentFragment = xd.CreateDocumentFragment()
                docFrag.InnerXml = "<Timedate></Timedate>"
                nod.AppendChild(docFrag)
                changes = True
            End If
            If nod.ChildNodes.Count < 7 Then
                Dim docFrag As XmlDocumentFragment = xd.CreateDocumentFragment()
                docFrag.InnerXml = "<Logon></Logon>"
                nod.AppendChild(docFrag)
                changes = True
            End If
            If nod.ChildNodes.Count < 8 Then
                Dim docFrag As XmlDocumentFragment = xd.CreateDocumentFragment()
                docFrag.InnerXml = "<Blur>True</Blur>"
                nod.AppendChild(docFrag)
                changes = True
            End If

            Dim temp As New aTheme
            temp.Name = nod.Attributes(0).Value
            temp.Theme = nod.ChildNodes(0).InnerText
            temp.Explorer = nod.ChildNodes(1).InnerText
            temp.OobeFldr = nod.ChildNodes(2).InnerText
            temp.ExplorerFrame = nod.ChildNodes(3).InnerText
            temp.Shell32 = nod.ChildNodes(4).InnerText
            temp.TimeDate = nod.ChildNodes(5).InnerText
            temp.Logon = nod.ChildNodes(6).InnerText
            temp.Blur = Convert.ToBoolean(nod.ChildNodes(7).InnerText)

            themeList(i) = (temp)
            i = i + 1
        Next
        Array.Sort(themeList)

        currentIndex = 0

        ' Save the Xml.
        xd.Save(MainWindow.themePath + "\theme_info.xml")

    End Sub

    Public Function isEmpty() As Boolean
        Return (themeList.Count = 0)
    End Function

End Class
