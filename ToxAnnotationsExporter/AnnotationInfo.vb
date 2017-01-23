Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO


Public Enum AnnotationType
    UNKNOWN
    HIGHLIGHT
    TEXT
    DRAWING
End Enum

Public Class AnnotationInfo
#Region "Private members"
    '      ===============

    Private annotationType As AnnotationType = annotationType.UNKNOWN
    Private annotationPage As [Double] = [Double].MinValue
    Private annotationMarkSourceFileRelativePath As [String] = [String].Empty
    Private annotationMarkStartPosition As [String] = [String].Empty
    Private annotationMarkEndPosition As [String] = [String].Empty
    Private annotationMark As [String] = [String].Empty
    Private annotationMarkExtended As [String] = [String].Empty
    Private annotationText As [String] = [String].Empty
    Private annotationDrawing As Bitmap = Nothing

#End Region

#Region "Fields"
    '      ======

    Public Property Type() As AnnotationType
        Get
            Return annotationType
        End Get
        Set(value As AnnotationType)
            annotationType = Value
        End Set
    End Property

    Public Property Page() As [Double]
        Get
            Return annotationPage
        End Get
        Set(value As [Double])
            annotationPage = Value
        End Set
    End Property

    Public Property MarkSourceFileRelativePath() As [String]
        Get
            Return annotationMarkSourceFileRelativePath
        End Get
        Set(value As [String])
            annotationMarkSourceFileRelativePath = Value
        End Set
    End Property

    Public Property MarkStartPosition() As [String]
        Get
            Return annotationMarkStartPosition
        End Get
        Set(value As [String])
            annotationMarkStartPosition = Value
        End Set
    End Property

    Public Property MarkEndPosition() As [String]
        Get
            Return annotationMarkEndPosition
        End Get
        Set(value As [String])
            annotationMarkEndPosition = Value
        End Set
    End Property

    Public Property Mark() As [String]
        Get
            Return annotationMark
        End Get
        Set(value As [String])
            annotationMark = Value.Trim()
        End Set
    End Property

    Public Property MarkExtended() As [String]
        Get
            Return annotationMarkExtended
        End Get
        Set(value As [String])
            annotationMarkExtended = Value
        End Set
    End Property

    Public Property Text() As [String]
        Get
            Return annotationText
        End Get
        Set(value As [String])
            annotationText = Value.Trim()
        End Set
    End Property

    Public Property Drawing() As Bitmap
        Get
            Return annotationDrawing
        End Get

        Set(value As Bitmap)
            If annotationDrawing IsNot Nothing Then
                annotationDrawing.Dispose()
            End If
            annotationDrawing = Value
        End Set
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

    Public Sub New(annotationType As AnnotationType, annotationPage As [Double], annotationMarkSourceFileRelativePath As [String], annotationMarkStartPosition As [String], annotationMarkEndPosition As [String], annotationMark As [String], _
        annotationMarkExtended As [String], annotationText As [String], annotationDrawing As Bitmap)
        Type = annotationType
        Page = annotationPage
        MarkSourceFileRelativePath = annotationMarkSourceFileRelativePath
        MarkStartPosition = annotationMarkStartPosition
        MarkEndPosition = annotationMarkEndPosition
        Mark = annotationMark
        MarkExtended = annotationMarkExtended
        Text = annotationText
        Drawing = annotationDrawing
    End Sub

    Protected Overrides Sub Finalize()
        Try
            If Drawing IsNot Nothing Then
                Drawing.Dispose()
            End If
        Finally
            MyBase.Finalize()
        End Try
    End Sub

#End Region

#Region "Methods"
    '      =======

    Public Sub SaveDrawingAsPngImage(pngFilePath As [String])
        If Drawing IsNot Nothing Then
            If False = pngFilePath.ToLower().EndsWith(".png") Then
                pngFilePath = Path.ChangeExtension(pngFilePath, ".png")
            End If
            Drawing.Save(pngFilePath, ImageFormat.Png)
        End If
    End Sub

    Public Function GetDrawingAsPngDataUrlString() As [String]
        Dim dataUrlString As [String] = [String].Empty

        If Drawing IsNot Nothing Then
            Dim imageConverter As New ImageConverter()
            Dim drawingBytes As Byte() = DirectCast(imageConverter.ConvertTo(Drawing, GetType(Byte())), Byte())
            dataUrlString = Convert.ToBase64String(drawingBytes, Base64FormattingOptions.InsertLineBreaks)
        End If

        Return dataUrlString
    End Function

    Public Function GetPageAsString(decimalPlaces As Integer) As [String]
        Dim pageAsString As [String] = [String].Empty

        If (0.0 > Page) OrElse (Int32.MaxValue < Page) Then
            pageAsString = "---"
        Else
            If 0 < decimalPlaces Then
                Dim formatString As [String] = [String].Format("{{0:0.{0}}}", New [String]("#"c, decimalPlaces))
                pageAsString = [String].Format(formatString, Page).Replace(",", ".")
            Else
                pageAsString = CInt(Math.Truncate(Page)).ToString()
            End If
        End If

        Return pageAsString
    End Function

#End Region
End Class
