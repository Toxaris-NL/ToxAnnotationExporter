Imports System.Collections.Generic
Imports MetroFramework.Forms

Public Class BookInfoComparer
    Implements IComparer(Of BookInfo)
#Region "Private members"
    '      ===============

    Private sortAspect As [String] = ToxAnnotationsExporterGUI.COLUMN_CAPTION_AUTHOR
    Private sortOrder As SortOrder = sortOrder.Ascending

#End Region

#Region "Fields"
    '      ======

    Public Property Aspect() As [String]
        Get
            Return sortAspect
        End Get
        Set(value As [String])
            sortAspect = value.Trim()
            If (ToxAnnotationsExporterGUI.COLUMN_CAPTION_AUTHOR.ToLower() <> sortAspect.ToLower()) AndAlso (ToxAnnotationsExporterGUI.COLUMN_CAPTION_TITLE.ToLower() <> sortAspect.ToLower()) Then
                sortAspect = ToxAnnotationsExporterGUI.COLUMN_CAPTION_AUTHOR
            End If
        End Set
    End Property

    Public Property Order() As SortOrder
        Get
            Return sortOrder
        End Get
        Set(value As SortOrder)
            sortOrder = value
            If (sortOrder.Ascending <> sortOrder) AndAlso (sortOrder.Descending <> sortOrder) Then
                sortOrder = sortOrder.Ascending
            End If
        End Set
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

    Public Sub New(aspect__1 As [String], order__2 As SortOrder)
        Aspect = aspect__1
        Order = order__2
    End Sub

#End Region

#Region "Methods"
    '      =======

#Region "IComparer<BookInfo>"
    '      ===================

    Public Function Compare(book1 As BookInfo, book2 As BookInfo) As Integer _
        Implements IComparer(Of BookInfo).Compare

        If (book1 Is Nothing) AndAlso (book2 Is Nothing) Then
            Return 0
        End If
        If book1 Is Nothing Then
            Return -1
        End If
        If book2 Is Nothing Then
            Return 1
        End If

        Dim book1CompareString As [String] = [String].Empty
        Dim book2CompareString As [String] = [String].Empty
        Dim compareResult As Integer = 0
        If ToxAnnotationsExporterGUI.COLUMN_CAPTION_TITLE.ToLower() = Aspect.ToLower() Then
            book1CompareString = [String].Format("{0}{1}", book1.Title, book1.FileName)
            book2CompareString = [String].Format("{0}{1}", book2.Title, book2.FileName)
            compareResult = book1CompareString.CompareTo(book2CompareString)
        Else
            book1CompareString = [String].Format("{0}{1}{2}", book1.Author, book1.Title, book1.FileName)
            book2CompareString = [String].Format("{0}{1}{2}", book2.Author, book2.Title, book2.FileName)
            compareResult = book1CompareString.CompareTo(book2CompareString)
        End If
        Return (If((sortOrder.Ascending = Order), compareResult, (-1 * compareResult)))
    End Function

#End Region

#End Region
End Class
