Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Windows.Forms


Public Class HtmlExporter
    Private Shared ReadOnly HTML_PLACEHOLDER_AUTHOR As [String] = "%AUTHOR%"
    Private Shared ReadOnly HTML_PLACEHOLDER_TITLE As [String] = "%TITLE%"
    Private Shared ReadOnly HTML_PLACEHOLDER_FILE_NAME As [String] = "%FILE_NAME%"
    Private Shared ReadOnly HTML_PLACEHOLDER_PAGE As [String] = "%PAGE%"
    Private Shared ReadOnly HTML_PLACEHOLDER_MARKED_TEXT As [String] = "%MARKED_TEXT%"
    Private Shared ReadOnly HTML_PLACEHOLDER_ANNOTATION As [String] = "%ANNOTATION%"
  
    Private Shared Sub WriteHtmlHead(htmlFile As StreamWriter, css As [String], htmlTitle As [String])
        htmlFile.WriteLine("<?xml version=""1.0"" encoding=""utf-8""?>")
        htmlFile.WriteLine("<html>")
        htmlFile.WriteLine("<head>")
        htmlFile.WriteLine("  <meta charset='UTF-8'>")
        htmlFile.WriteLine([String].Format("<title>{0}</title>", htmlTitle))
        If False = [String].IsNullOrEmpty(css) Then
            htmlFile.WriteLine([String].Format("<style type=""text/css"">" & vbLf & "{0}" & vbLf & "</style>", css))
        End If
        htmlFile.WriteLine("</head>")
        htmlFile.WriteLine("<body>")
    End Sub

    Private Shared Sub WriteHtmlTail(htmlFile As StreamWriter)
        htmlFile.WriteLine("Export made by Annotation Exporter - © Toxaris 2015, Yoths 2012")
        htmlFile.WriteLine("</body>")
        htmlFile.WriteLine("</html>")
    End Sub

    Private Shared Sub WriteSaveLayer(htmlFile As StreamWriter, template As [String])
        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine(template)
        End If
    End Sub

    Private Shared Sub WriteBookHead(htmlFile As StreamWriter, template As [String])
        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine(template)
        End If
    End Sub

    Private Shared Sub WriteBookInformation(htmlFile As StreamWriter, template As [String], book As BookInfo)
        template = template.Replace(HTML_PLACEHOLDER_AUTHOR, "{0}")
        template = template.Replace(HTML_PLACEHOLDER_TITLE, "{1}")
        template = template.Replace(HTML_PLACEHOLDER_FILE_NAME, "{2}")

        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine([String].Format(template, book.Author, book.Title, book.FileName))
        End If
    End Sub

    Private Shared Sub WriteAnnotationsHead(htmlFile As StreamWriter, template As [String])
        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine(template)
        End If
    End Sub

    Private Shared Sub WriteAnnotation(htmlFile As StreamWriter, template As [String], annotation As AnnotationInfo, useInlineImages As Boolean, pageNumberDecimalPlaces As Integer, htmlFilePath As [String], _
        ByRef drawingAnnotationCounter As Integer)
        template = template.Replace(HTML_PLACEHOLDER_PAGE, "{0}")
        template = template.Replace(HTML_PLACEHOLDER_MARKED_TEXT, "{1}")
        template = template.Replace(HTML_PLACEHOLDER_ANNOTATION, "{2}")

        If False = [String].IsNullOrEmpty(template) Then
            Dim annotationMark As [String] = (If((False = [String].IsNullOrEmpty(annotation.MarkExtended)), annotation.MarkExtended, annotation.Mark.Replace(vbLf, "<br>")))

            Select Case annotation.Type
                Case AnnotationType.HIGHLIGHT
                    If True Then
                        htmlFile.WriteLine([String].Format(template, annotation.GetPageAsString(pageNumberDecimalPlaces), annotationMark, ""))
                        Exit Select
                    End If
                Case AnnotationType.TEXT
                    If True Then
                        htmlFile.WriteLine([String].Format(template, annotation.GetPageAsString(pageNumberDecimalPlaces), annotationMark, annotation.Text.Replace(vbLf, "<br>")))
                        Exit Select
                    End If
                Case AnnotationType.DRAWING
                    If True Then
                        If False = useInlineImages Then
                            template = template.Replace("{2}", "<img src='{2}'>")

                            Dim pngFilePath As [String] = [String].Format("{0}\{1} - {2}.png", Path.GetDirectoryName(htmlFilePath), Path.GetFileNameWithoutExtension(htmlFilePath), drawingAnnotationCounter.ToString("D3"))
                            annotation.SaveDrawingAsPngImage(pngFilePath)
                            htmlFile.WriteLine([String].Format(template, annotation.GetPageAsString(pageNumberDecimalPlaces), annotationMark, Path.GetFileName(pngFilePath)))
                            drawingAnnotationCounter += 1
                        Else
                            template = template.Replace("{2}", "<img src='data:image/png;base64,{2}'>")

                            Dim dataUrlString As [String] = annotation.GetDrawingAsPngDataUrlString()
                            htmlFile.WriteLine([String].Format(template, annotation.GetPageAsString(pageNumberDecimalPlaces), annotationMark, dataUrlString))
                        End If
                        Exit Select
                    End If
            End Select
        End If
    End Sub

    Private Shared Sub WriteAnnotationsSeparator(htmlFile As StreamWriter, template As [String])
        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine(template)
        End If
    End Sub

    Private Shared Sub WriteAnnotationsNotAvailable(htmlFile As StreamWriter, template As [String])
        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine(template)
        End If
    End Sub

    Private Shared Sub WriteBookTail(htmlFile As StreamWriter, template As [String])
        If False = [String].IsNullOrEmpty(template) Then
            htmlFile.WriteLine(template)
        End If
    End Sub

    Private Shared Sub WriteScript(htmlFile As StreamWriter, javascript As [String])
        If False = [String].IsNullOrEmpty(javascript) Then
            htmlFile.WriteLine([String].Format("<script>" & vbLf & "{0}" & vbLf & "</script>", javascript))
        End If
    End Sub

    Private Shared Sub ExportToSingleFile(books As List(Of BookInfo), htmlFilePath As [String], useBookInformation As Boolean, useInlineImages As Boolean, pageNumberDecimalPlaces As Integer, exportStyle As HtmlExportStyle)
        Try
            Dim drawingAnnotationCounter As Integer = 0
            Using htmlFile As New StreamWriter(htmlFilePath, False, New UTF8Encoding(True))
                WriteHtmlHead(htmlFile, exportStyle.CSS, Path.GetFileNameWithoutExtension(htmlFilePath))
                WriteSaveLayer(htmlFile, exportStyle.SaveLayer)
                For Each currentBook As BookInfo In books
                    WriteBookHead(htmlFile, exportStyle.BookHead)
                    If True = useBookInformation Then
                        WriteBookInformation(htmlFile, exportStyle.BookInformation, currentBook)
                    End If
                    If 0 < currentBook.Annotations.Count Then
                        WriteAnnotationsHead(htmlFile, exportStyle.AnnotationsHead)
                        Dim annotationCounter As Integer = 0
                        For Each currentAnnotation As AnnotationInfo In currentBook.Annotations
                            annotationCounter += 1
                            WriteAnnotation(htmlFile, exportStyle.Annotation, currentAnnotation, useInlineImages, pageNumberDecimalPlaces, htmlFilePath,
                                drawingAnnotationCounter)
                            If annotationCounter < currentBook.Annotations.Count Then
                                WriteAnnotationsSeparator(htmlFile, exportStyle.AnnotationsSeparator)
                            End If
                        Next
                    Else
                        WriteAnnotationsNotAvailable(htmlFile, exportStyle.AnnotationsNotAvailable)
                    End If
                    WriteBookTail(htmlFile, exportStyle.BookTail)
                Next
                WriteScript(htmlFile, exportStyle.Script)
                WriteHtmlTail(htmlFile)
                htmlFile.Close()
            End Using
        Catch ex As Exception
            Dim errorText As [String] = [String].Format("Error exporting annotations to HTML file: {0}", ex.Message)
            MetroFramework.MetroMessageBox.Show(ToxAnnotationsExporterGUI, errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        End Try
    End Sub

    Private Shared Sub ExportToMultipleFiles(books As List(Of BookInfo), htmlFilesDir As [String], useBookInformation As Boolean, useInlineImages As Boolean, pageNumberDecimalPlaces As Integer, exportStyle As HtmlExportStyle)
        Dim drawingAnnotationCounter As Integer = 0
        For Each currentBook As BookInfo In books
            Try
                Dim htmlFilePath As [String] = Path.Combine(htmlFilesDir, Path.ChangeExtension(currentBook.FileName, ".html"))
                Using htmlFile As New StreamWriter(htmlFilePath, False, New UTF8Encoding(True))
                    WriteHtmlHead(htmlFile, exportStyle.CSS, [String].Format("{0} - {1}", currentBook.Author, currentBook.Title))
                    WriteSaveLayer(htmlFile, exportStyle.SaveLayer)
                    WriteBookHead(htmlFile, exportStyle.BookHead)
                    If True = useBookInformation Then
                        WriteBookInformation(htmlFile, exportStyle.BookInformation, currentBook)
                    End If
                    If 0 < currentBook.Annotations.Count Then
                        WriteAnnotationsHead(htmlFile, exportStyle.AnnotationsHead)
                        Dim annotationCounter As Integer = 0
                        For Each currentAnnotation As AnnotationInfo In currentBook.Annotations
                            annotationCounter += 1
                            WriteAnnotation(htmlFile, exportStyle.Annotation, currentAnnotation, useInlineImages, pageNumberDecimalPlaces, htmlFilePath, _
                                drawingAnnotationCounter)
                            If annotationCounter < currentBook.Annotations.Count Then
                                WriteAnnotationsSeparator(htmlFile, exportStyle.AnnotationsSeparator)
                            End If
                        Next
                    Else
                        WriteAnnotationsNotAvailable(htmlFile, exportStyle.AnnotationsNotAvailable)
                    End If
                    WriteBookTail(htmlFile, exportStyle.BookTail)
                    WriteScript(htmlFile, exportStyle.Script)
                    WriteHtmlTail(htmlFile)
                    htmlFile.Close()
                End Using
            Catch ex As Exception
                Dim errorText As [String] = [String].Format("Error exporting annotations of '{0}' to HTML file: {1}", currentBook.FilePath, ex.Message)
                MetroFramework.MetroMessageBox.Show(ToxAnnotationsExporterGUI, errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        Next
    End Sub

    Public Shared Sub Export(books As List(Of BookInfo), tragetFileOrDir As [String], useBookInformation As Boolean, useInlineImages As Boolean, pageNumberDecimalPlaces As Integer, charactersToExpandMarkedTextBy As Integer, _
        highlightOriginalMark As Boolean, exportStyle As HtmlExportStyle)

        For Each book As BookInfo In books
            If book.FileName.Contains(".kepub") Then
                book.UpdateExtendedAnnotations(0, highlightOriginalMark, exportStyle)
            Else
                book.UpdateExtendedAnnotations(charactersToExpandMarkedTextBy, highlightOriginalMark, exportStyle)
            End If
            'book.UpdateExtendedAnnotations(charactersToExpandMarkedTextBy, highlightOriginalMark, exportStyle)
        Next
        GeneralHelper.RemoveTempDirectory()

        If True = [String].IsNullOrEmpty(Path.GetExtension(tragetFileOrDir)) Then
            ExportToMultipleFiles(books, tragetFileOrDir, useBookInformation, useInlineImages, pageNumberDecimalPlaces, exportStyle)
        Else
            ExportToSingleFile(books, (If((True = tragetFileOrDir.ToLower().EndsWith(".html")), tragetFileOrDir, Path.ChangeExtension(tragetFileOrDir, ".html"))), useBookInformation, useInlineImages, pageNumberDecimalPlaces, exportStyle)
        End If
    End Sub
End Class
