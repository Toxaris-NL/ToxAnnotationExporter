
Public Class HtmlExportStyle
#Region "Private members"
    '      ===============

    Private styleName As [String] = [String].Empty
    Private styleCSS As [String] = [String].Empty
    Private styleSaveLayer As [String] = [String].Empty
    Private styleBookHead As [String] = [String].Empty
    Private styleBookInformation As [String] = [String].Empty
    Private styleAnnotationsHead As [String] = [String].Empty
    Private styleAnnotation As [String] = [String].Empty
    Private styleAnnotationHighlightStart As [String] = [String].Empty
    Private styleAnnotationHighlightEnd As [String] = [String].Empty
    Private styleAnnotationsSeparator As [String] = [String].Empty
    Private styleAnnotationsNotAvailable As [String] = [String].Empty
    Private styleBookTail As [String] = [String].Empty

    Public Const HTML_EXPORT_STYLE_XML_FILE As [String] = "HtmlExportStyles.xml"
    Public Const XML_ELEMENT_HTML_EXPORT_STYLE As [String] = "HtmlExportStyle"
    Public Const XML_ATTRIBUTE_HTML_EXPORT_STYLE_NAME As [String] = "name"
    Public Const XML_ELEMENT_CSS As [String] = "CSS"
    Public Const XML_ELEMENT_BOOK_HEAD As [String] = "BookHead"
    Public Const XML_ELEMENT_BOOK_INFORMATION As [String] = "BookInformation"
    Public Const XML_ELEMENT_ANNOTATIONS_HEAD As [String] = "AnnotationsHead"
    Public Const XML_ELEMENT_ANNOTATION As [String] = "Annotation"
    Public Const XML_ELEMENT_ANNOTATION_HIGHLIGHT_START As [String] = "AnnotationHighlightStart"
    Public Const XML_ELEMENT_ANNOTATION_HIGHLIGHT_END As [String] = "AnnotationHighlightEnd"
    Public Const XML_ELEMENT_ANNOTATIONS_SEPARATOR As [String] = "AnnotationsSeparator"
    Public Const XML_ELEMENT_ANNOTATIONS_NOT_AVAILABLE As [String] = "AnnotationsNotAvailable"
    Public Const XML_ELEMENT_BOOK_TAIL As [String] = "BookTail"

#End Region

#Region "Fields"
    '      ======

    Public Property Name() As [String]
        Get
            Return styleName
        End Get
        Set(value As [String])
            styleName = CorrectHtml(Value)
        End Set
    End Property

    Public Property CSS() As [String]
        Get
            Return styleCSS
        End Get
        Set(value As [String])
            styleCSS = CorrectCss(Value)
        End Set
    End Property

    Public Property SaveLayer() As [String]
        Get
            Return styleSaveLayer
        End Get
        Set(value As [String])
            styleSaveLayer = CorrectHtml(value)
        End Set
    End Property

    Public Property BookHead() As [String]
        Get
            Return styleBookHead
        End Get
        Set(value As [String])
            styleBookHead = CorrectHtml(Value)
        End Set
    End Property

    Public Property BookInformation() As [String]
        Get
            Return styleBookInformation
        End Get
        Set(value As [String])
            styleBookInformation = CorrectHtml(Value)
        End Set
    End Property

    Public Property AnnotationsHead() As [String]
        Get
            Return styleAnnotationsHead
        End Get
        Set(value As [String])
            styleAnnotationsHead = CorrectHtml(Value)
        End Set
    End Property

    Public Property Annotation() As [String]
        Get
            Return styleAnnotation
        End Get
        Set(value As [String])
            styleAnnotation = CorrectHtml(Value)
        End Set
    End Property

    Public Property AnnotationHighlightStart() As [String]
        Get
            Return styleAnnotationHighlightStart
        End Get
        Set(value As [String])
            styleAnnotationHighlightStart = CorrectHtml(Value)
        End Set
    End Property

    Public Property AnnotationHighlightEnd() As [String]
        Get
            Return styleAnnotationHighlightEnd
        End Get
        Set(value As [String])
            styleAnnotationHighlightEnd = CorrectHtml(Value)
        End Set
    End Property

    Public Property AnnotationsSeparator() As [String]
        Get
            Return styleAnnotationsSeparator
        End Get
        Set(value As [String])
            styleAnnotationsSeparator = CorrectHtml(Value)
        End Set
    End Property

    Public Property AnnotationsNotAvailable() As [String]
        Get
            Return styleAnnotationsNotAvailable
        End Get
        Set(value As [String])
            styleAnnotationsNotAvailable = CorrectHtml(Value)
        End Set
    End Property

    Public Property BookTail() As [String]
        Get
            Return styleBookTail
        End Get
        Set(value As [String])
            styleBookTail = CorrectHtml(Value)
        End Set
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

#End Region

#Region "Methods"
    '      =======

    Private Function CorrectCss(value As [String]) As [String]
        Return value.Replace("\n", vbLf).Trim()
    End Function

    Private Function CorrectHtml(value As [String]) As [String]
        Return value.Replace("\n", vbLf).Replace("[", "<").Replace("]", ">").Trim()
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function

#End Region
End Class
