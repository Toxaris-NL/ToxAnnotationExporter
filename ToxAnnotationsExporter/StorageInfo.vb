
Public Enum MemoryType
    UNKNOWN
    INTERNAL
    SDCARD
End Enum

Public Class StorageInfo
#Region "Private members"
    '      ===============

    Private storageType As MemoryType = MemoryType.UNKNOWN
    Private storageBasePath As [String] = [String].Empty

#End Region

#Region "Fields"
    '      ======

    Public Property Type() As MemoryType
        Get
            Return storageType
        End Get
        Set(value As MemoryType)
            storageType = Value
        End Set
    End Property

    Public Property BasePath() As [String]
        Get
            Return storageBasePath
        End Get
        Set(value As [String])
            storageBasePath = Value.Trim()
            If False = storageBasePath.EndsWith("\") Then
                storageBasePath += "\"
            End If
        End Set
    End Property

    Public ReadOnly Property TypeAsString() As [String]
        Get
            Select Case Type
                Case MemoryType.INTERNAL
                    If True Then
                        Return "Internal Memory"
                    End If
                Case MemoryType.SDCARD
                    If True Then
                        Return "Micro SD Card"
                    End If
                Case Else
                    If True Then
                        Return storageBasePath
                    End If
            End Select
            Return ""
        End Get
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

    Public Sub New(storageType As MemoryType, storageBasePath As [String])
        Type = storageType
        BasePath = storageBasePath
    End Sub

#End Region
End Class
