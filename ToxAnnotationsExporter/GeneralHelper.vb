Imports System.Collections.Generic
Imports System.IO


Public Class GeneralHelper
#Region "Private members"
    '      ===============

    Private Shared ReadOnly m_tempDirectory As New DirectoryInfo(Path.Combine(Path.GetTempPath(), "_ToxAnnotationsExporter"))

#End Region

#Region "Fields"
    '      ======

    Public Shared ReadOnly Property TempDirectory() As DirectoryInfo
        Get
            Return m_tempDirectory
        End Get
    End Property

    Public Shared ReadOnly Property TempDirectoryPath() As [String]
        Get
            Return m_tempDirectory.FullName
        End Get
    End Property

#End Region

#Region "Methods"
    '      =======

    Public Shared Sub RemoveDirectory(dirToDelete As DirectoryInfo)
        dirToDelete.Refresh()
        If True = dirToDelete.Exists Then
            Dim dirsToDelete As New Stack(Of DirectoryInfo)()
            dirsToDelete.Push(dirToDelete)
            While 0 < dirsToDelete.Count
                Dim dir As DirectoryInfo = dirsToDelete.Pop()
                dir.Attributes = dir.Attributes And Not (FileAttributes.Archive Or FileAttributes.[ReadOnly] Or FileAttributes.Hidden)
                For Each subDir As DirectoryInfo In dir.GetDirectories()
                    dirsToDelete.Push(subDir)
                Next
                For Each file As FileInfo In dir.GetFiles()
                    file.Attributes = file.Attributes And Not (FileAttributes.Archive Or FileAttributes.[ReadOnly] Or FileAttributes.Hidden)
                    file.Delete()
                Next
            End While
            dirToDelete.Delete(True)
        End If
    End Sub

    Public Shared Sub RemoveDirectory(dirToDelete As [String])
        RemoveDirectory(New DirectoryInfo(dirToDelete))
    End Sub

    Public Shared Sub RemoveDirectoryContent(dirToClear As DirectoryInfo)
        dirToClear.Refresh()
        If True = dirToClear.Exists Then
            For Each subDir As DirectoryInfo In dirToClear.GetDirectories()
                RemoveDirectory(subDir)
            Next
            For Each file As FileInfo In dirToClear.GetFiles()
                file.Attributes = file.Attributes And Not (FileAttributes.Archive Or FileAttributes.[ReadOnly] Or FileAttributes.Hidden)
                file.Delete()
            Next
        End If
    End Sub

    Public Shared Sub RemoveDirectoryContent(dirToClear As [String])
        RemoveDirectoryContent(New DirectoryInfo(dirToClear))
    End Sub

    Public Shared Sub RemoveTempDirectory()
        RemoveDirectory(m_tempDirectory)
    End Sub

    Public Shared Sub RemoveTempDirectoryContent()
        RemoveDirectoryContent(m_tempDirectory)
    End Sub

#End Region
End Class
