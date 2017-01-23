Imports System.Collections.Generic


Public Class EpubTextPositionInfo
#Region "Private members"
    '      ===============

    Private m_steps As New List(Of Integer)()
    Private m_offset As Integer = 0
    Private m_isValid As Boolean = False

#End Region

#Region "Fields"
    '      ======

    Public ReadOnly Property Steps() As List(Of Integer)
        Get
            Return m_steps
        End Get
    End Property

    Public Property Offset() As Integer
        Get
            Return m_offset
        End Get
        Set(value As Integer)
            m_offset = Value
        End Set
    End Property

    Public ReadOnly Property IsValid() As Boolean
        Get
            Return m_isValid
        End Get
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New(pointDefinition As [String])
        m_isValid = True

        If True = pointDefinition.StartsWith("point(") Then
            pointDefinition = pointDefinition.Substring(6)
        End If
        If True = pointDefinition.EndsWith(")") Then
            pointDefinition = pointDefinition.Substring(0, pointDefinition.Length - 1)
        End If

        Dim parts As [String]() = pointDefinition.Split(New Char() {":"c})
        If 0 < parts.Length Then
            If 1 < parts.Length Then
                If False = Integer.TryParse(parts(1), m_offset) Then
                    m_isValid = False
                End If
            End If

            Dim stepsDefinition As [String] = parts(0)
            parts = stepsDefinition.Split(New Char() {"/"c}, StringSplitOptions.RemoveEmptyEntries)
            If 0 < parts.Length Then
                For Each part As [String] In parts
                    Dim [step] As Integer = 0
                    If True = Integer.TryParse(part, [step]) Then
                        m_steps.Add([step])
                    Else
                        m_isValid = False
                        Exit For
                    End If
                Next
            Else
                m_isValid = False
            End If
        Else
            m_isValid = False
        End If
    End Sub

#End Region
End Class
