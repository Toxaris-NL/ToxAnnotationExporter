Imports System.Collections.Generic
Imports System.Data.SQLite
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Text
'Imports System.Windows.Forms
Imports MetroFramework.Forms
Imports System.Xml


Public Class SonyReaderInfo
#Region "Private members"
    '      ===============

    Private Shared ReadOnly DATABASE_FILE_RELATIVE_PATH As [String] = "Sony_Reader\database\books.db"
    Private Shared ReadOnly DATABASE_ANNOTATION_TYPE_HIGHLIGHT As [String] = "10"
    Private Shared ReadOnly DATABASE_ANNOTATION_TYPE_TEXT As [String] = "11"
    Private Shared ReadOnly DATABASE_ANNOTATION_TYPE_DRAWING As [String] = "12"
    Private Shared ReadOnly ANNOTATION_DRAWING_PADDING As Integer = 5

    Private readerStorageLocations As New List(Of StorageInfo)()
    Private readerBooks As New Dictionary(Of [String], BookInfo)()

#End Region

#Region "Fields"
    '      ======

    Public Shared ReadOnly Property DatabaseFileRelativePath() As [String]
        Get
            Return DATABASE_FILE_RELATIVE_PATH
        End Get
    End Property


    Public ReadOnly Property StorageLocations() As List(Of StorageInfo)
        Get
            Return readerStorageLocations
        End Get
    End Property

    Public ReadOnly Property Books() As List(Of BookInfo)
        Get
            Return New List(Of BookInfo)(readerBooks.Values)
        End Get
    End Property

    Public ReadOnly Property BooksWithAnnotations() As List(Of BookInfo)
        Get
            Dim books As New List(Of BookInfo)()
            For Each book As BookInfo In readerBooks.Values
                If 0 < book.Annotations.Count Then
                    books.Add(book)
                End If
            Next
            Return books
        End Get
    End Property

    Public ReadOnly Property BooksForExportAnnotations() As List(Of BookInfo)
        Get
            Dim books As New List(Of BookInfo)()
            For Each book As BookInfo In readerBooks.Values
                If True = book.ExportAnnotations Then
                    books.Add(book)
                End If
            Next
            Return books
        End Get
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

    Public Sub New(storageLocation As StorageInfo)
        StorageLocations.Add(storageLocation)
    End Sub

    Public Sub New(storageLocations__1 As List(Of StorageInfo))
        For Each storageLocation As StorageInfo In storageLocations__1
            StorageLocations.Add(storageLocation)
        Next
    End Sub

#End Region

#Region "Methods"
    '      =======

    Private Function ConvertAnnotationType(dbAnnotationType As [String]) As AnnotationType
        If DATABASE_ANNOTATION_TYPE_HIGHLIGHT = dbAnnotationType Then
            Return AnnotationType.HIGHLIGHT
        End If
        If DATABASE_ANNOTATION_TYPE_TEXT = dbAnnotationType Then
            Return AnnotationType.TEXT
        End If
        If DATABASE_ANNOTATION_TYPE_DRAWING = dbAnnotationType Then
            Return AnnotationType.DRAWING
        End If
        Return AnnotationType.UNKNOWN
    End Function

    Private Function ReadAnnotationTextFromFile(annotationFilePath As [String]) As [String]
        Dim annotationText As [String] = [String].Empty

        Dim xmlDocument As New XmlDocument()
        xmlDocument.Load(annotationFilePath)

        Dim xmlRootElement As XmlElement = xmlDocument.DocumentElement
        If "n0:notepad" = xmlRootElement.Name.ToLower() Then
            annotationText = xmlRootElement("text").InnerText
        End If

        Return annotationText
    End Function

    Private Function ReadAnnotationDrawingFromFile(annotationFilePath As [String]) As Bitmap
        Dim annotationDrawing As Bitmap = Nothing
        Dim annotationDrawingSize As Size = Size.Empty
        Dim annotationPolilines As New List(Of List(Of Point))()
        Dim polilinesRaw As New List(Of List(Of Point))()
        Dim minX As Integer = 1000
        Dim maxX As Integer = -1000
        Dim minY As Integer = 1000
        Dim maxY As Integer = -1000

        Dim xmlDocument As New XmlDocument()
        xmlDocument.Load(annotationFilePath)

        Dim xmlRootElement As XmlElement = xmlDocument.DocumentElement
        If "n0:notepad" = xmlRootElement.Name.ToLower() Then
            Dim xmlSvgElement As XmlElement = xmlRootElement("n0:drawing")("n0:page")("n1:svg")
            For Each xmlPolilineElement As XmlElement In xmlSvgElement
                If "n1:polyline" = xmlPolilineElement.Name.ToLower() Then
                    Dim polilineString As [String] = DirectCast(xmlPolilineElement.Attributes.GetNamedItem("points"), XmlAttribute).Value

                    If False = [String].IsNullOrEmpty(polilineString) Then
                        Dim polilineRaw As New List(Of Point)()
                        Dim pointStrings As [String]() = polilineString.Split(New Char() {" "c})
                        For Each pointString As [String] In pointStrings
                            Dim pointValueStrings As [String]() = pointString.Split(New Char() {","c})
                            If 2 = pointValueStrings.Length Then
                                Dim polilinePointX As Integer = 0
                                Dim polilinePointY As Integer = 0
                                If (True = Int32.TryParse(pointValueStrings(0), polilinePointX)) AndAlso (True = Int32.TryParse(pointValueStrings(1), polilinePointY)) Then
                                    polilineRaw.Add(New Point(polilinePointX, polilinePointY))
                                    If minX > polilinePointX Then
                                        minX = polilinePointX
                                    ElseIf maxX < polilinePointX Then
                                        maxX = polilinePointX
                                    End If
                                    If minY > polilinePointY Then
                                        minY = polilinePointY
                                    ElseIf maxY < polilinePointY Then
                                        maxY = polilinePointY
                                    End If
                                End If
                            End If
                        Next
                        If 0 < polilineRaw.Count Then
                            polilinesRaw.Add(polilineRaw)
                        End If
                    End If
                End If
            Next
        End If

        For Each polilineRaw As List(Of Point) In polilinesRaw
            Dim anotationPoliline As New List(Of Point)()
            For Each polilinePointRaw As Point In polilineRaw
                anotationPoliline.Add(New Point(polilinePointRaw.X + (ANNOTATION_DRAWING_PADDING - minX), polilinePointRaw.Y + (ANNOTATION_DRAWING_PADDING - minY)))
            Next
            If 0 < anotationPoliline.Count Then
                annotationPolilines.Add(anotationPoliline)
            End If
        Next
        If 0 < annotationPolilines.Count Then
            annotationDrawingSize = New Size((maxX - minX) + (2 * ANNOTATION_DRAWING_PADDING), (maxY - minY) + (2 * ANNOTATION_DRAWING_PADDING))
        End If

        If Size.Empty <> annotationDrawingSize Then
            annotationDrawing = New Bitmap(annotationDrawingSize.Width, annotationDrawingSize.Height)
            Using graphics__1 As Graphics = Graphics.FromImage(annotationDrawing)
                Dim brush As New SolidBrush(Color.White)
                Dim pen As New Pen(Color.Black, 2.5F)

                graphics__1.SmoothingMode = SmoothingMode.HighQuality
                pen.DashCap = DashCap.Round

                graphics__1.FillRegion(brush, New Region(New Rectangle(Point.Empty, annotationDrawingSize)))
                For Each annotationPoliline As List(Of Point) In annotationPolilines
                    graphics__1.DrawLines(pen, annotationPoliline.ToArray())
                Next

                brush.Dispose()
                pen.Dispose()
                graphics__1.Dispose()
            End Using
        End If

        Return annotationDrawing
    End Function

    Private Function GenerateDictionaryBookId(dBStorageInfo As StorageInfo, dbBookId As [String]) As [String]
        Return [String].Format("{0}@{1}", dbBookId, dBStorageInfo.BasePath)
    End Function

    Public Sub LoadBooksInfo()
        readerBooks.Clear()

        Try
            For Each storageLocation As StorageInfo In StorageLocations
                If True = [String].IsNullOrEmpty(storageLocation.BasePath) Then
                    Continue For
                End If

                Using dbConnection As New SQLiteConnection()
                    Try
                        dbConnection.ConnectionString = [String].Format("Data Source={0};FailIfMissing=True;Read Only=True;", Path.Combine(storageLocation.BasePath, DATABASE_FILE_RELATIVE_PATH))
                        dbConnection.Open()

                        Using dbCommand As New SQLiteCommand(dbConnection)
                            Try
                                dbCommand.CommandText = "SELECT _id, author, title, file_path, thumbnail FROM books ORDER BY _id"
                                Using dbResult As SQLiteDataReader = dbCommand.ExecuteReader()
                                    Try
                                        If 5 <= dbResult.FieldCount Then
                                            While True = dbResult.Read()
                                                Dim dbBookId As [String] = dbResult(0).ToString()
                                                Dim dbBookAuthor As [String] = dbResult(1).ToString()
                                                Dim dbBookTitle As [String] = dbResult(2).ToString()
                                                Dim dbBookFilePath As [String] = dbResult(3).ToString().Replace("/", "\")
                                                Dim dbBookThumbnailPath As [String] = dbResult(4).ToString().Replace("/", "\")

                                                Dim dictionaryBookId As [String] = GenerateDictionaryBookId(storageLocation, dbBookId)
                                                If False = readerBooks.ContainsKey(dictionaryBookId) Then
                                                    readerBooks.Add(dictionaryBookId, New BookInfo(dbBookAuthor, dbBookTitle, Path.Combine(storageLocation.BasePath, dbBookFilePath), Path.Combine(storageLocation.BasePath, dbBookThumbnailPath), storageLocation.TypeAsString, dbBookId))
                                                End If
                                            End While
                                        End If
                                    Finally
                                        dbResult.Close()
                                        dbResult.Dispose()
                                    End Try
                                End Using

                                dbCommand.CommandText = "SELECT content_id, page, markup_type, marked_text, name, file_path, mark, mark_end FROM annotation ORDER BY page"
                                Using dbResult As SQLiteDataReader = dbCommand.ExecuteReader()
                                    Try
                                        If 8 <= dbResult.FieldCount Then
                                            Dim UTF8 As New UTF8Encoding()
                                            Dim blobBufferSize As Integer = 1024
                                            Dim blobBuffer As [Byte]() = New Byte(blobBufferSize - 1) {}
                                            Dim blobSize As Long = 0

                                            While True = dbResult.Read()
                                                Dim dbBookId As [String] = dbResult(0).ToString()
                                                Dim dbAnnotationPage As [String] = dbResult(1).ToString()
                                                Dim dbAnnotationType As [String] = dbResult(2).ToString()
                                                Dim dbAnnotationMark As [String] = dbResult(3).ToString()
                                                Dim dbAnnotationText As [String] = dbResult(4).ToString()
                                                Dim dbAnnotationFilePath As [String] = dbResult(5).ToString().Replace("/", "\")
                                                blobSize = dbResult.GetBytes(6, 0, blobBuffer, 0, blobBufferSize)
                                                Dim dbAnnotationMarkStart As [String] = UTF8.GetString(blobBuffer, 0, CInt(blobSize))
                                                blobSize = dbResult.GetBytes(7, 0, blobBuffer, 0, blobBufferSize)
                                                Dim dbAnnotationMarkEnd As [String] = UTF8.GetString(blobBuffer, 0, CInt(blobSize))

                                                Dim dictionaryBookId As [String] = GenerateDictionaryBookId(storageLocation, dbBookId)
                                                If True = readerBooks.ContainsKey(dictionaryBookId) Then
                                                    Dim annotationType__1 As AnnotationType = ConvertAnnotationType(dbAnnotationType)
                                                    If AnnotationType.UNKNOWN <> annotationType__1 Then
                                                        Dim annotationPage As [Double] = [Double].MinValue
                                                        Dim annotationText As [String] = [String].Empty
                                                        Dim annotationDrawing As Bitmap = Nothing
                                                        Dim annotationFilePath As [String] = Path.Combine(storageLocation.BasePath, dbAnnotationFilePath)
                                                        Dim annotationMarkSourceFileRelativePath As [String] = [String].Empty
                                                        Dim annotationMarkStart As [String] = [String].Empty
                                                        Dim annotationMarkEnd As [String] = [String].Empty

                                                        If False = [Double].TryParse(dbAnnotationPage, annotationPage) Then
                                                            annotationPage = [Double].MinValue
                                                        End If

                                                        Select Case annotationType__1
                                                            Case AnnotationType.TEXT
                                                                If True Then
                                                                    If (True = annotationFilePath.ToLower().EndsWith(".memo")) AndAlso (True = File.Exists(annotationFilePath)) Then
                                                                        annotationText = ReadAnnotationTextFromFile(annotationFilePath)
                                                                        If True = [String].IsNullOrEmpty(annotationText) Then
                                                                            annotationText = dbAnnotationText
                                                                        End If
                                                                    End If
                                                                    Exit Select
                                                                End If
                                                            Case AnnotationType.DRAWING
                                                                If True Then
                                                                    If (True = annotationFilePath.ToLower().EndsWith(".svg")) AndAlso (True = File.Exists(annotationFilePath)) Then
                                                                        annotationDrawing = ReadAnnotationDrawingFromFile(annotationFilePath)
                                                                    End If
                                                                    Exit Select
                                                                End If
                                                        End Select

                                                        If True = readerBooks(dictionaryBookId).SupportsExtendedAnnotations Then
                                                            ExtendedAnnotationsImporter.GetAnnotationMarkData(readerBooks(dictionaryBookId).Type, dbAnnotationMarkStart, dbAnnotationMarkEnd, annotationMarkSourceFileRelativePath, annotationMarkStart, annotationMarkEnd)
                                                        End If

                                                        readerBooks(dictionaryBookId).Annotations.Add(New AnnotationInfo(annotationType__1, annotationPage, annotationMarkSourceFileRelativePath, annotationMarkStart, annotationMarkEnd, dbAnnotationMark,
                                                            [String].Empty, annotationText, annotationDrawing))
                                                    End If
                                                End If
                                            End While
                                        End If
                                    Finally
                                        dbResult.Close()
                                        dbResult.Dispose()
                                    End Try
                                End Using
                            Finally
                                dbCommand.Dispose()
                            End Try
                        End Using
                    Finally
                        dbConnection.Close()
                        dbConnection.Dispose()
                    End Try
                End Using
            Next
        Catch ex As SQLiteException
            Dim errorText As [String] = [String].Format("Database operation error: {0}", ex.Message)
            MetroFramework.MetroMessageBox.Show(ToxAnnotationsExporterGUI, errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        Catch ex As Exception
            Dim errorText As [String] = [String].Format("Error getting books information: {0}", ex.Message)
            MetroFramework.MetroMessageBox.Show(ToxAnnotationsExporterGUI, errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

#End Region
End Class
