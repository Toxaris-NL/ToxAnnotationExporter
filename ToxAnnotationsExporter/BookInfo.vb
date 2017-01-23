Imports System.Collections.Generic
Imports System.IO


Public Enum BookType
    UNKNOWN
    PDF
    EPUB
    FB2
End Enum

Public Class BookInfo
#Region "Private members"
    '      ===============

    Private bookAuthor As [String] = [String].Empty
    Private bookTitle As [String] = [String].Empty
    Private bookFilePath As [String] = [String].Empty
    Private bookThumbnailPath As [String] = [String].Empty
    Private bookStorageLocationString As [String] = [String].Empty
    Private bookAnnotations As New List(Of AnnotationInfo)()
    Private bookId As [String] = [String].Empty
    Private bookMarkedForExportAnnotations As Boolean = False

#End Region

#Region "Fields"
    '      ======

    Public Property Author() As [String]
        Get
            Return bookAuthor
        End Get
        Set(value As [String])
            bookAuthor = Value.Trim()
        End Set
    End Property

    Public Property Title() As [String]
        Get
            Return (If((False = [String].IsNullOrEmpty(bookTitle)), bookTitle, Path.GetFileNameWithoutExtension(FileName)))
        End Get
        Set(value As [String])
            bookTitle = Value.Trim()
        End Set
    End Property

    Public Property FilePath() As [String]
        Get
            Return bookFilePath
        End Get
        Set(value As [String])
            bookFilePath = Value.Trim()
        End Set
    End Property

    Public Property ThumbnailPath() As [String]
        Get
            Return bookThumbnailPath
        End Get
        Set(value As [String])
            bookThumbnailPath = Value.Trim()
        End Set
    End Property

    Public Property StorageLocationString() As [String]
        Get
            Return bookStorageLocationString
        End Get
        Set(value As [String])
            bookStorageLocationString = Value.Trim()
        End Set
    End Property

    Public ReadOnly Property Annotations() As List(Of AnnotationInfo)
        Get
            Return bookAnnotations
        End Get
    End Property

    Public Property BookIdentification As [String]
        Get
            Return bookId
        End Get
        Set(value As [String])
            bookId = value
        End Set
    End Property

    Public Property ExportAnnotations() As Boolean
        Get
            Return bookMarkedForExportAnnotations
        End Get
        Set(value As Boolean)
            bookMarkedForExportAnnotations = Value
        End Set
    End Property

    Public ReadOnly Property FileName() As [String]
        Get
            Return Path.GetFileName(FilePath)
        End Get
    End Property

    Public ReadOnly Property Type() As BookType
        Get
            Dim bookType__1 As BookType = BookType.UNKNOWN
            Dim filePath As [String] = FileName.ToLower()
            If filePath.EndsWith(".pdf") Then
                bookType__1 = BookType.PDF
            ElseIf filePath.EndsWith(".epub") Then
                bookType__1 = BookType.EPUB
            ElseIf (filePath.EndsWith(".fb2")) OrElse (filePath.EndsWith(".fb2.zip")) Then
                bookType__1 = BookType.FB2
            End If
            Return bookType__1
        End Get
    End Property

    Public ReadOnly Property SupportsExtendedAnnotations() As Boolean
        Get
            Return (BookType.EPUB = Type)
        End Get
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

    Public Sub New(bookAuthor As [String], bookTitle As [String], bookFilePath As [String], bookThumbnailPath As [String], bookStorageLocationString As [String], bookIdent As [String])
        Author = bookAuthor
        Title = bookTitle
        FilePath = bookFilePath
        ThumbnailPath = bookThumbnailPath
        StorageLocationString = bookStorageLocationString
        BookIdentification = bookIdent
    End Sub

#End Region

#Region "Methods"
    '      =======

    Public Sub UpdateExtendedAnnotations(charactersToExpandMarkedTextBy As Integer, highlightOriginalMark As Boolean, exportStyle As HtmlExportStyle)
        If (True = SupportsExtendedAnnotations) AndAlso (0 < Annotations.Count) Then
            For Each annotation As AnnotationInfo In Annotations
                annotation.MarkExtended = [String].Empty
            Next

            If 0 < charactersToExpandMarkedTextBy Then
                If True = ExtendedAnnotationsImporter.ExtractBookContentToTempDirectory(Me) Then
                    For Each annotation As AnnotationInfo In Annotations
                        annotation.MarkExtended = ExtendedAnnotationsImporter.GetExtendedAnnotation(Me, annotation, charactersToExpandMarkedTextBy, highlightOriginalMark, exportStyle)
                    Next
                End If
            End If
        End If
    End Sub

#End Region
End Class
