Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.XPath
'Imports Ionic.Zip


Public Class ExtendedAnnotationsImporter
#Region "Private members"
    '      ===============

    Private Shared ReadOnly mainTags As New List(Of [String])() From {
        "p",
        "h1",
        "h2",
        "h3",
        "h4",
        "h5",
        "h6"
    }
    Private Shared ReadOnly subTags As New List(Of [String])() From { _
        "i", _
        "b", _
        "u", _
        "s", _
        "em", _
        "sub", _
        "sup", _
        "li", _
        "ul", _
        "ol", _
        "dl", _
        "dt", _
        "dd" _
    }
    Private Shared ReadOnly specialChars As New List(Of [String])() From { _
        "&gt;", _
        "&lt;", _
        "&amp;" _
    }

#End Region

#Region "Methods"
    '      =======

#Region "ePub"
    '      ~~~~

    Private Shared Function EPUB_ReverseString(str As [String]) As [String]
        Dim chars As Char() = str.ToCharArray()
        Array.Reverse(chars)
        Return New [String](chars)
    End Function

    Private Shared Function EPUB_GetSubstringBase(sourceString As [String], length As Integer, stopCountingBeginChar As Char, stopCountingEndChar As Char) As [String]
        Dim subString As [String] = ""

        Dim index As Integer = 0
        Dim count As Integer = 0
        Dim stopCounting As Boolean = False
        For Each ch As Char In sourceString.ToCharArray()
            If count >= length Then
                Exit For
            End If

            If stopCountingBeginChar = ch Then
                stopCounting = True
            End If
            If (False = stopCounting) AndAlso (ControlChars.Lf <> ch) Then
                count += 1
            End If
            If stopCountingEndChar = ch Then
                stopCounting = False
            End If
            index += 1
        Next

        subString = sourceString.Substring(0, index)
        If (subString.Length < sourceString.Length) AndAlso (False = subString.EndsWith(" ")) Then
            index = sourceString.IndexOf(" ", index)
            If 0 <= index Then
                subString = sourceString.Substring(0, index + 1)
            Else
                subString = sourceString
            End If
        End If

        Return subString
    End Function

    Private Shared Function EPUB_GetSubstringLeft(sourceString As [String], length As Integer) As [String]
        Return EPUB_GetSubstringBase(sourceString, length, "<"c, ">"c)
    End Function

    Private Shared Function EPUB_GetSubstringRight(sourceString As [String], length As Integer) As [String]
        Return EPUB_ReverseString(EPUB_GetSubstringBase(EPUB_ReverseString(sourceString), length, ">"c, "<"c))
    End Function

    Private Shared Function EPUB_InsertSubstring(sourceText As [String], index As Integer, textToInsert As [String]) As [String]
        Dim UTF8 As New UTF8Encoding()
        Dim textBytes As New List(Of [Byte])(UTF8.GetBytes(sourceText))
        If -1 < index Then
            Dim byteIndex As Integer = 0
            Dim correctedIndex As Integer = Math.Min(index, textBytes.Count)
            While (byteIndex <= correctedIndex) AndAlso (correctedIndex < textBytes.Count)
                If &HD = textBytes(byteIndex) Then
                    correctedIndex += 1
                End If
                byteIndex += 1
            End While
            textBytes.InsertRange(correctedIndex, UTF8.GetBytes(textToInsert))
        Else
            textBytes.AddRange(UTF8.GetBytes(textToInsert))
        End If
        Return UTF8.GetString(textBytes.ToArray())
    End Function

    Private Shared Function EPUB_CountSubstring(sourceString As [String], substring As [String]) As Integer
        If False = [String].IsNullOrEmpty(substring) Then
            Return ((sourceString.Length - sourceString.Replace(substring, "").Length) \ substring.Length)
        End If
        Return 0
    End Function

    Private Shared Function EPUB_NavigateToTextPosition(htmlDocument As XmlDocument, ByRef textPosition As EpubTextPositionInfo, ByRef xPath As XPathNavigator) As Boolean
        Dim xmlNode As XmlNode = htmlDocument
        Dim navigationError As Boolean = False

        Dim nodeInsideHtmlAndOutsideParagraph As Boolean = False
        For Each [step] As Integer In textPosition.Steps
            If XmlNodeType.Element = xmlNode.NodeType Then
                If xmlNode.Name.ToLower() = "html" Then
                    nodeInsideHtmlAndOutsideParagraph = True
                ElseIf True = mainTags.Contains(xmlNode.Name.ToLower()) Then
                    nodeInsideHtmlAndOutsideParagraph = False
                End If
            End If

            Dim xmlNodeIndex As Integer = (If((True = nodeInsideHtmlAndOutsideParagraph), (([step] \ 2) + ([step] Mod 2)), [step])) - 1
            Dim childIndex As Integer = 0
            navigationError = True
            For Each child As XmlNode In xmlNode.ChildNodes
                If (XmlNodeType.Element = child.NodeType) OrElse (XmlNodeType.Text = child.NodeType) Then
                    If xmlNodeIndex = childIndex Then
                        xmlNode = child
                        navigationError = False
                        Exit For
                    End If
                    childIndex += 1
                End If
            Next
            If True = navigationError Then
                If xmlNode.HasChildNodes Then 'to prevent that the xmlNode is set to Nothing, check if there are childnodes
                    xmlNode = xmlNode.LastChild
                Else
                    xmlNode = xmlNode.ParentNode.FirstChild
                End If
                Exit For
            End If
        Next

        xPath = xmlNode.CreateNavigator()
        If XPathNodeType.Text <> xPath.NodeType Then
            If False = xPath.MoveToFollowing(XPathNodeType.Text) Then
                Return False
            End If
            textPosition.Offset = 0
        End If
        If True = navigationError Then
            textPosition.Offset = -1
        End If

        Return True
    End Function

    Private Shared Function EPUB_GetHtmlBody(htmlDocument As XmlDocument, ByRef htmlBodyText As [String]) As Boolean
        htmlBodyText = ""

        Dim bodyNode As XPathNavigator = htmlDocument.CreateNavigator()
        While (XPathNodeType.Element <> bodyNode.NodeType) OrElse ("body" <> bodyNode.Name.ToLower())
            If False = bodyNode.MoveToFollowing(XPathNodeType.Element) Then
                Return False
            End If
        End While

        Dim reader As XmlReader = bodyNode.ReadSubtree()
        While reader.Read()
            Select Case reader.NodeType
                Case XmlNodeType.Element
                    If True Then
                        If (True = mainTags.Contains(reader.Name)) OrElse (True = subTags.Contains(reader.Name)) Then
                            htmlBodyText += [String].Format("<{0}>", reader.Name)
                        End If
                        Exit Select
                    End If
                Case XmlNodeType.EndElement
                    If True Then
                        If (True = mainTags.Contains(reader.Name)) OrElse (True = subTags.Contains(reader.Name)) Then
                            htmlBodyText += [String].Format("</{0}>", reader.Name)
                            If True = mainTags.Contains(reader.Name.ToLower()) Then
                                htmlBodyText += vbLf
                            End If
                        End If
                        Exit Select
                    End If
                Case XmlNodeType.Text
                    If True Then
                        htmlBodyText += reader.Value
                        Exit Select
                    End If
            End Select
        End While

        Return True
    End Function

    Private Shared Sub EPUB_EncodeSpecialCharachters(ByRef htmlFileContent As [String])
        For Each specialChar As [String] In specialChars
            Dim specialCharMark As [String] = [String].Format("#_-=*{0}*=-_#", specialChar.Substring(1, specialChar.Length - 2))
            htmlFileContent = htmlFileContent.Replace(specialChar.ToLower(), specialCharMark)
            If htmlFileContent.ToLower() = htmlFileContent Then
                htmlFileContent = htmlFileContent.Replace(specialChar.ToUpper(), specialCharMark)
            ElseIf htmlFileContent.ToUpper() = htmlFileContent Then
                htmlFileContent = htmlFileContent.Replace(specialChar.ToLower(), specialCharMark)
            End If
        Next
    End Sub

    Private Shared Sub EPUB_DecodeSpecialCharachters(ByRef htmlFileContent As [String])
        For Each specialChar As [String] In specialChars
            Dim specialCharMark As [String] = [String].Format("#_-=*{0}*=-_#", specialChar.Substring(1, specialChar.Length - 2))
            htmlFileContent = htmlFileContent.Replace(specialCharMark, specialChar)
        Next
    End Sub

    Private Shared Function EPUB_GetExtendedAnnotation(htmlFilePath As [String], markStartPositionString As [String], markEndPositionString As [String], charactersCount As Integer, highlightOriginalMark As Boolean, exportStyle As HtmlExportStyle) As [String]
        Dim annotationText As [String] = [String].Empty
        Dim htmlFileContent As [String] = File.ReadAllText(htmlFilePath)
        Dim Divide As Boolean = True

        EPUB_EncodeSpecialCharachters(htmlFileContent)
        htmlFileContent = WebUtility.HtmlDecode(htmlFileContent)
        EPUB_DecodeSpecialCharachters(htmlFileContent)

        Dim markStartPosition As New EpubTextPositionInfo(markStartPositionString)
        Dim markEndPosition As New EpubTextPositionInfo(markEndPositionString)
        If (True = markStartPosition.IsValid) AndAlso (True = markEndPosition.IsValid) Then
            Dim htmlFileParserSettings As New XmlReaderSettings()
            htmlFileParserSettings.CheckCharacters = False
            htmlFileParserSettings.XmlResolver = Nothing
            htmlFileParserSettings.DtdProcessing = DtdProcessing.Parse

            Using htmlFileParser As XmlReader = XmlReader.Create(New StringReader(htmlFileContent), htmlFileParserSettings)
                Dim htmlDocument As New XmlDocument()
                htmlDocument.XmlResolver = Nothing
                htmlDocument.Load(htmlFileParser)

                Dim markStartNode As XPathNavigator = Nothing
                Dim markEndNode As XPathNavigator = Nothing
                If (True = EPUB_NavigateToTextPosition(htmlDocument, markStartPosition, markStartNode)) AndAlso (True = EPUB_NavigateToTextPosition(htmlDocument, markEndPosition, markEndNode)) Then
                    Dim highlightMarker As [String] = "#_-=*=-_#"

                    markEndNode.SetValue(EPUB_InsertSubstring(markEndNode.Value, markEndPosition.Offset, highlightMarker))
                    markStartNode.SetValue(EPUB_InsertSubstring(markStartNode.Value, markStartPosition.Offset, highlightMarker))

                    Dim bodyText As [String] = ""
                    If True = EPUB_GetHtmlBody(htmlDocument, bodyText) Then
                        Dim bodyTextParts As [String]() = bodyText.Split(New [String]() {highlightMarker}, StringSplitOptions.None)
                        If 3 = bodyTextParts.Length Then
                            Dim highlightStart As [String] = [String].Empty
                            Dim highlightEnd As [String] = [String].Empty
                            Dim highlightText As [String] = bodyTextParts(1)
                            If True = highlightOriginalMark Then
                                highlightStart = exportStyle.AnnotationHighlightStart
                                highlightEnd = exportStyle.AnnotationHighlightEnd
                                For Each tag As [String] In mainTags
                                    Dim startTag As [String] = [String].Format("<{0}>", tag)
                                    Dim endTag As [String] = [String].Format("</{0}>", tag)
                                    highlightText = highlightText.Replace(startTag, startTag & highlightStart).Replace(endTag, highlightEnd & endTag)
                                Next
                            End If

                            annotationText = [String].Format("{0}{1}{2}{3}{4}", EPUB_GetSubstringRight(bodyTextParts(0), charactersCount), highlightStart, highlightText, highlightEnd, EPUB_GetSubstringLeft(bodyTextParts(2), charactersCount))
                            If True = annotationText.StartsWith(" ") Then
                                annotationText = "[...]" & annotationText
                            End If
                            If True = annotationText.EndsWith(" ") Then
                                annotationText = annotationText & "[...]"
                            End If

                            For Each tag As [String] In subTags
                                Dim startTag As [String] = [String].Format("<{0}>", tag)
                                Dim endTag As [String] = [String].Format("</{0}>", tag)
                                If True = annotationText.Contains(startTag) Then
                                    Dim index As Integer = annotationText.LastIndexOf(startTag)
                                    If EPUB_CountSubstring(annotationText.Substring(index), startTag) > EPUB_CountSubstring(annotationText.Substring(index), endTag) Then
                                        annotationText += endTag
                                    End If
                                End If
                                If True = annotationText.Contains(endTag) Then
                                    Dim length As Integer = annotationText.IndexOf(endTag) + endTag.Length
                                    If EPUB_CountSubstring(annotationText.Substring(0, length), endTag) > EPUB_CountSubstring(annotationText.Substring(0, length), startTag) Then
                                        annotationText = startTag & annotationText
                                    End If
                                End If
                            Next
                        Else
                            Throw New Exception("Cannot extract extended annotation from HTML body.")
                        End If
                    Else
                        Throw New Exception("Cannot extract HTML body.")
                    End If
                Else
                    Throw New Exception("Cannot navigate to text position.")
                End If
            End Using
        Else
            Throw New Exception("Invalid text position.")
        End If

        Return annotationText
    End Function

    Private Shared Sub EPUB_GetAnnotationMarkData(dbAnnotationMarkStart As [String], dbAnnotationMarkEnd As [String], ByRef annotationMarkSourceFileRelativePath As [String], ByRef annotationMarkStart As [String], ByRef annotationMarkEnd As [String])
        Dim startParts As [String]() = dbAnnotationMarkStart.Split(New Char() {"#"c}, StringSplitOptions.RemoveEmptyEntries)
        Dim endParts As [String]() = dbAnnotationMarkEnd.Split(New Char() {"#"c}, StringSplitOptions.RemoveEmptyEntries)
        If (2 = startParts.Length) AndAlso (2 = endParts.Length) AndAlso (True = startParts(0).Equals(endParts(0), StringComparison.OrdinalIgnoreCase)) Then
            annotationMarkSourceFileRelativePath = Uri.UnescapeDataString(startParts(0)).Replace("/", "\")
            annotationMarkStart = startParts(1).Replace(vbNullChar, "")
            annotationMarkEnd = endParts(1).Replace(vbNullChar, "")
        End If
    End Sub

#End Region

    Public Shared Function ExtractBookContentToTempDirectory(book As BookInfo) As Boolean
        Dim extractionResult As Boolean = False

        Select Case book.Type
            Case BookType.EPUB
                If True Then
                    Try
                        If Not Directory.Exists(GeneralHelper.TempDirectoryPath) Then Directory.CreateDirectory(GeneralHelper.TempDirectoryPath)
                        GeneralHelper.RemoveTempDirectoryContent()
                        Using epubFile As ZipStorer = ZipStorer.Open(book.FilePath, FileAccess.Read)
                            Dim dir As List(Of ZipStorer.ZipFileEntry) = epubFile.ReadCentralDir()
                            For Each entry As ZipStorer.ZipFileEntry In dir
                                epubFile.ExtractFile(entry, GeneralHelper.TempDirectoryPath & Path.DirectorySeparatorChar & entry.FilenameInZip)
                            Next
                            epubFile.Close()
                            'Using epubFile As ZipFile = ZipFile.Read(book.FilePath)
                            '    epubFile.ExtractAll(GeneralHelper.TempDirectoryPath, ExtractExistingFileAction.OverwriteSilently)
                            extractionResult = True
                        End Using
                    Catch ex As Exception
                        Dim errorText As [String] = [String].Format("Error extracting ePub file '{0}': {1}", book.FilePath, ex.Message)
                        MetroFramework.MetroMessageBox.Show(ToxAnnotationsExporterGUI, errorText & vbLf & vbLf & "Annotations will be exported without expanded text", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                    End Try
                    Exit Select
                End If
        End Select

        Return extractionResult
    End Function

    Public Shared Function GetExtendedAnnotation(book As BookInfo, annotation As AnnotationInfo, charactersCount As Integer, highlightOriginalMark As Boolean, exportStyle As HtmlExportStyle) As [String]
        Dim annotationText As [String] = [String].Empty

        Select Case book.Type
            Case BookType.EPUB
                If True Then
                    Try
                        annotationText = EPUB_GetExtendedAnnotation(Path.Combine(GeneralHelper.TempDirectoryPath, annotation.MarkSourceFileRelativePath), annotation.MarkStartPosition, annotation.MarkEndPosition, charactersCount, highlightOriginalMark, exportStyle)
                    Catch ex As Exception
                        annotationText = [String].Empty

                        Dim errorText As [String] = [String].Format("Error getting extended annotation from ePub ({0} -> {1}, {2} - {3}): {4}", book.FilePath, annotation.MarkSourceFileRelativePath, annotation.MarkStartPosition, annotation.MarkEndPosition, ex.Message)
                        MetroFramework.MetroMessageBox.Show(ToxAnnotationsExporterGUI, errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
                    End Try
                    Exit Select
                End If
        End Select

        Return annotationText
    End Function

    Public Shared Sub GetAnnotationMarkData(bookType__1 As BookType, dbAnnotationMarkStart As [String], dbAnnotationMarkEnd As [String], ByRef annotationMarkSourceFileRelativePath As [String], ByRef annotationMarkStart As [String], ByRef annotationMarkEnd As [String])
        Select Case bookType__1
            Case BookType.EPUB
                If True Then
                    EPUB_GetAnnotationMarkData(dbAnnotationMarkStart, dbAnnotationMarkEnd, annotationMarkSourceFileRelativePath, annotationMarkStart, annotationMarkEnd)
                    Exit Select
                End If
        End Select
    End Sub

#End Region
End Class
