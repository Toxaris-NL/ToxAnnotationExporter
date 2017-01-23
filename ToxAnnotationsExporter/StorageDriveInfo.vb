
Public Class StorageDriveInfo
#Region "Private members"
    '      ===============

    Private m_driveLabel As [String] = [String].Empty
    Private m_drivePath As [String] = [String].Empty
    Private m_driveBrand As [String] = [String].Empty

#End Region

#Region "Fields"
    '      ======

    Public Property DriveLabel() As [String]
        Get
            Return m_driveLabel
        End Get
        Set(value As [String])
            m_driveLabel = Value.Trim()
        End Set
    End Property

    Public Property DrivePath() As [String]
        Get
            Return m_drivePath
        End Get
        Set(value As [String])
            m_drivePath = Value.Trim()
            If False = m_drivePath.EndsWith("\") Then
                m_drivePath += "\"
            End If
        End Set
    End Property
    Public Property DriveBrand() As [String]
        Get
            Return m_driveBrand
        End Get
        Set(value As [String])
            m_driveBrand = value.Trim()
        End Set
    End Property

    Public ReadOnly Property DrivePathWithoutBackslash() As [String]
        Get
            If False = [String].IsNullOrEmpty(DrivePath) Then
                Return DrivePath.Replace("\", "")
            End If
            Return DrivePath
        End Get
    End Property

#End Region

#Region "Constructors"
    '      ============

    Public Sub New()
    End Sub

    Public Sub New(driveLabel__1 As [String], driveBasePath As [String], driveBranding As [String])
        DriveLabel = driveLabel__1
        DrivePath = driveBasePath
        DriveBrand = driveBranding
    End Sub

#End Region

#Region "Methods"
    '      =======

    Public Overrides Function ToString() As String
        If False = [String].IsNullOrEmpty(DrivePath) Then
            Dim driveLabel__1 As [String] = DriveLabel
            If True = [String].IsNullOrEmpty(DriveLabel) Then
                driveLabel__1 = "Removable Disc"
            End If
            Return [String].Format("{0} ({1})", driveLabel__1, DrivePathWithoutBackslash)
        End If
        Return "---"
    End Function

    Public Overloads Function Equals(obj As StorageDriveInfo) As Boolean
        If obj Is Nothing Then
            Return False
        End If
        Return (DrivePath = obj.DrivePath)
    End Function

    Public Overrides Function Equals(obj As [Object]) As Boolean
        If obj Is Nothing Then
            Return False
        End If
        If TypeOf obj Is StorageDriveInfo Then
            Return Equals(DirectCast(obj, StorageDriveInfo))
        End If
        Return False
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode() Xor DrivePath.GetHashCode()
    End Function

    Public Shared Operator =(obj1 As StorageDriveInfo, obj2 As StorageDriveInfo) As Boolean
        If True = [Object].ReferenceEquals(obj1, obj2) Then
            Return True
        End If
        If (obj1 Is Nothing) OrElse (obj2 Is Nothing) Then
            Return False
        End If
        Return obj1.Equals(obj2)
    End Operator

    Public Shared Operator <>(obj1 As StorageDriveInfo, obj2 As StorageDriveInfo) As Boolean
        Return Not (obj1 = obj2)
    End Operator

#End Region
End Class
