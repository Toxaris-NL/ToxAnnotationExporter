Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports MetroFramework.Forms
Imports System.Xml
Imports BrightIdeasSoftware

Partial Public Class ToxAnnotationsExporterGUI
    Private prsT1 As New SonyReaderInfo()
    Private kobo As New KoboReaderInfo()
    Private columnsHeaderFont As Font = Nothing
    Private exportStyles As New List(Of HtmlExportStyle)()
    Private lastSelectedFolder As [String] = [String].Empty
    Private userInternalMemoryPath As [String] = [String].Empty
    Private userMicroSdCardPath As [String] = [String].Empty
    Private ReaderBrand As [String] = [String].Empty

    Public Shared ReadOnly COLUMN_CAPTION_AUTHOR As [String] = "Author"
    Public Shared ReadOnly COLUMN_CAPTION_TITLE As [String] = "Title"
    Private Shared ReadOnly COLUMN_ASPECT_SEPARATOR As [String] = "#_-=*=-_#"

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoadExportStyles(exportStyles)
        columnsHeaderFont = New Font(Font, Font.Style Or FontStyle.Bold)
        lastSelectedFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        comboboxInternalMemory.DropDownStyle = ComboBoxStyle.DropDownList
        labelInternalMemory.Text = "Internal Memory"
        comboboxMicroSdCard.DropDownStyle = ComboBoxStyle.DropDownList
        labelMicroSdCard.Text = "Micro SD Card"
        comboboxExportStyle.DropDownStyle = ComboBoxStyle.DropDownList

        comboboxExportStyle.Items.AddRange(exportStyles.ToArray())
        If 0 < comboboxExportStyle.Items.Count Then
            comboboxExportStyle.SelectedIndex = 0
        End If

        checkboxHideBooksWithoutAnnotations.Checked = My.MySettings.[Default].HideBooksWithoutAnnotations
        checkboxHideBooksWithoutAnnotations.Text = "Hide books without Annotations"
        checkboxCreateSeparateFileForEachBook.Checked = My.MySettings.[Default].CreateSeparateFileForEachBook
        checkboxCreateSeparateFileForEachBook.Text = "Create separate file for each book"
        checkboxInsertBookInformation.Checked = My.MySettings.[Default].InsertBookInformation
        checkboxInsertBookInformation.Text = "Insert book information"
        checkboxCreateInlineImages.Checked = My.MySettings.[Default].CreateInlineImages
        checkboxCreateInlineImages.Text = "Create inline images"
        checkboxExpandMarkedText.Checked = My.MySettings.[Default].ExpandMarkedText
        checkboxExpandMarkedText.Text = "ePUB Books: Expand marked text"
        textboxCharactersToExpandMarkedTextBy.Text = Math.Max(0, My.MySettings.[Default].CharactersToExpandMarkedTextBy).ToString()
        checkboxHighlightOriginalMark.Checked = My.MySettings.[Default].HighlightOriginalMark
        checkboxHighlightOriginalMark.Text = "Highlight original mark"
        checkboxInsertBookInformation.Enabled = checkboxCreateSeparateFileForEachBook.Checked
        textboxCharactersToExpandMarkedTextBy.Enabled = checkboxExpandMarkedText.Checked
        labelExpandMarkedText_By.Enabled = checkboxExpandMarkedText.Checked
        labelExpandMarkedText_By.Text = "by"
        labelExpandMarkedText_Characters.Enabled = checkboxExpandMarkedText.Checked
        labelExpandMarkedText_Characters.Text = "characters"
        labelExportStyle.Text = "Style"
        buttonExport.Text = "Export"
        buttonReload.Text = "Reload"
        ButtonDelete.Text = "Delete selected annotations"

        listviewBooks.CheckBoxes = True
        listviewBooks.CheckedAspectName = "ExportAnnotations"
        listviewBooks.MultiSelect = True
        listviewBooks.RowHeight = 24
        listviewBooks.FullRowSelect = True
        listviewBooks.SelectColumnsOnRightClick = False
        listviewBooks.UseTranslucentHotItem = True
        listviewBooks.UseTranslucentSelection = True
        listviewBooks.UseCustomSelectionColors = True
        listviewBooks.HasCollapsibleGroups = True
        listviewBooks.ShowItemCountOnGroups = True
        listviewBooks.SortGroupItemsByPrimaryColumn = False
        listviewBooks.GroupWithItemCountFormat = "{0} ({1} books)"
        listviewBooks.GroupWithItemCountSingularFormat = "{0} ({1} book)"

        columnAuthor.Text = COLUMN_CAPTION_AUTHOR
        columnAuthor.AspectGetter = AddressOf GetAuthorColumnAspect
        columnAuthor.AspectToStringConverter = AddressOf GetColumnAspectString
        columnAuthor.GroupKeyGetter = AddressOf GetGroupKeyForAuthorAndTitleColumns
        columnAuthor.HeaderFont = columnsHeaderFont
        columnAuthor.MinimumWidth = TextRenderer.MeasureText(columnAuthor.Text, columnAuthor.HeaderFont).Width + 50

        columnTitle.Text = COLUMN_CAPTION_TITLE
        columnTitle.AspectGetter = AddressOf GetTitleColumnAspect
        columnTitle.AspectToStringConverter = AddressOf GetColumnAspectString
        columnTitle.GroupKeyGetter = AddressOf GetGroupKeyForAuthorAndTitleColumns
        columnTitle.HeaderFont = columnsHeaderFont
        columnTitle.MinimumWidth = TextRenderer.MeasureText(columnTitle.Text, columnTitle.HeaderFont).Width + 50
        columnTitle.FillsFreeSpace = True

        InitInternalMemoryAndMicroSdCardComboboxes()

        ReloadBooks()

    End Sub

    Protected Overrides Sub Finalize()
        Try
            If columnsHeaderFont IsNot Nothing Then
                columnsHeaderFont.Dispose()
            End If
        Finally
            MyBase.Finalize()
        End Try
    End Sub

    Private Sub LoadExportStyles(ByRef exportStyles As List(Of HtmlExportStyle))
        Dim exportStylesDict As New Dictionary(Of [String], HtmlExportStyle)()

        If True = File.Exists(HtmlExportStyle.HTML_EXPORT_STYLE_XML_FILE) Then
            Try
                Dim htmlFileParserSettings As New XmlReaderSettings()
                htmlFileParserSettings.CheckCharacters = False
                htmlFileParserSettings.XmlResolver = Nothing

                Using reader As XmlReader = XmlReader.Create(HtmlExportStyle.HTML_EXPORT_STYLE_XML_FILE, htmlFileParserSettings)
                    Dim currentExportStyle As HtmlExportStyle = Nothing
                    While reader.Read()
                        Select Case reader.NodeType
                            Case XmlNodeType.Element
                                If True Then
                                    If HtmlExportStyle.XML_ELEMENT_HTML_EXPORT_STYLE.ToLower() = reader.Name.ToLower() Then
                                        currentExportStyle = Nothing
                                        If True = reader.HasAttributes Then
                                            While reader.MoveToNextAttribute()
                                                If HtmlExportStyle.XML_ATTRIBUTE_HTML_EXPORT_STYLE_NAME.ToLower() = reader.Name.ToLower() Then
                                                    currentExportStyle = New HtmlExportStyle()
                                                    currentExportStyle.Name = reader.Value
                                                    Exit While
                                                End If
                                            End While
                                            reader.MoveToElement()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_CSS.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.CSS = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_BOOK_HEAD.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.BookHead = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_BOOK_INFORMATION.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.BookInformation = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_ANNOTATIONS_HEAD.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.AnnotationsHead = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_ANNOTATION.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.Annotation = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_ANNOTATION_HIGHLIGHT_START.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.AnnotationHighlightStart = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_ANNOTATION_HIGHLIGHT_END.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.AnnotationHighlightEnd = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_ANNOTATIONS_SEPARATOR.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.AnnotationsSeparator = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_ANNOTATIONS_NOT_AVAILABLE.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.AnnotationsNotAvailable = reader.ReadElementString()
                                        End If
                                    ElseIf HtmlExportStyle.XML_ELEMENT_BOOK_TAIL.ToLower() = reader.Name.ToLower() Then
                                        If currentExportStyle IsNot Nothing Then
                                            currentExportStyle.BookTail = reader.ReadElementString()
                                        End If
                                    End If
                                    Exit Select
                                End If
                            Case XmlNodeType.EndElement
                                If True Then
                                    If HtmlExportStyle.XML_ELEMENT_HTML_EXPORT_STYLE.ToLower() = reader.Name.ToLower() Then
                                        If (currentExportStyle IsNot Nothing) AndAlso (False = [String].IsNullOrEmpty(currentExportStyle.Name)) Then
                                            exportStylesDict.Add(currentExportStyle.Name, currentExportStyle)
                                        End If
                                    End If
                                    Exit Select
                                End If
                        End Select
                    End While
                End Using
            Catch ex As Exception
                Dim errorText As [String] = [String].Format("Error loading HTML export styles file: {0}", ex.Message)
                MetroFramework.MetroMessageBox.Show(Me, errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        End If
        exportStyles.Clear()
        If 0 < exportStylesDict.Count Then
            exportStyles.AddRange(exportStylesDict.Values)
        Else
            Dim currentExportStyle As New HtmlExportStyle()
            currentExportStyle.Name = "Default"
            currentExportStyle.CSS = My.MySettings.[Default].HtmlExport_CSS
            currentExportStyle.BookHead = My.MySettings.[Default].HtmlExport_BookHead
            currentExportStyle.SaveLayer = My.Settings.[Default].HtmlExport_SaveLayer
            currentExportStyle.BookInformation = My.MySettings.[Default].HtmlExport_BookInformation
            currentExportStyle.AnnotationsHead = My.MySettings.[Default].HtmlExport_AnnotationsHead
            currentExportStyle.Annotation = My.MySettings.[Default].HtmlExport_Annotation
            currentExportStyle.AnnotationHighlightStart = My.MySettings.[Default].HtmlExport_AnnotationHighlightStart
            currentExportStyle.AnnotationHighlightEnd = My.MySettings.[Default].HtmlExport_AnnotationHighlightEnd
            currentExportStyle.AnnotationsSeparator = My.MySettings.[Default].HtmlExport_AnnotationsSeparator
            currentExportStyle.AnnotationsNotAvailable = My.MySettings.[Default].HtmlExport_AnnotationsNotAvailable
            currentExportStyle.BookTail = My.MySettings.[Default].HtmlExport_BookTail

            exportStyles.Add(currentExportStyle)
        End If
    End Sub

    Public Function GetAuthorColumnAspect(row As [Object]) As [Object]
        Dim aspect As [String] = Nothing
        If TypeOf row Is BookInfo Then
            Dim book As BookInfo = DirectCast(row, BookInfo)
            aspect = [String].Format("{0}{1}{2}{3}", book.Author, COLUMN_ASPECT_SEPARATOR, book.Title, book.FileName)
        End If
        Return aspect
    End Function

    Public Function GetTitleColumnAspect(row As [Object]) As [Object]
        Dim aspect As [String] = Nothing
        If TypeOf row Is BookInfo Then
            Dim book As BookInfo = DirectCast(row, BookInfo)
            aspect = [String].Format("{0}{1}{2}", book.Title, COLUMN_ASPECT_SEPARATOR, book.FileName)
        End If
        Return aspect
    End Function

    Public Function GetColumnAspectString(aspect As [Object]) As [String]
        Dim aspectString As [String] = aspect.ToString()
        If TypeOf aspect Is [String] Then
            aspectString = DirectCast(aspect, [String])
            If True = aspectString.Contains(COLUMN_ASPECT_SEPARATOR) Then
                Dim parts As [String]() = aspectString.Split(New [String]() {COLUMN_ASPECT_SEPARATOR}, StringSplitOptions.None)
                If 0 < parts.Length Then
                    aspectString = parts(0)
                End If
            End If
        End If
        Return aspectString
    End Function

    Public Function GetGroupKeyForAuthorAndTitleColumns(row As [Object]) As [Object]
        If TypeOf row Is BookInfo Then
            Return DirectCast(row, BookInfo).StorageLocationString
        End If
        Return Nothing
    End Function

    Private Sub listviewBooks_BeforeCreatingGroups(sender As Object, e As CreateGroupsEventArgs) Handles listviewBooks.BeforeCreatingGroups
        e.Parameters.GroupByOrder = SortOrder.Ascending
    End Sub

    Private Sub RefreshBooks()
        listviewBooks.BeginUpdate()
        If ReaderBrand = "Sony" Then
            listviewBooks.SetObjects(If((True = checkboxHideBooksWithoutAnnotations.Checked), prsT1.BooksWithAnnotations, prsT1.Books))
        Else
            listviewBooks.SetObjects(If((True = checkboxHideBooksWithoutAnnotations.Checked), kobo.BooksWithAnnotations, kobo.Books))
        End If
        columnAuthor.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        columnTitle.MinimumWidth = TextRenderer.MeasureText(columnTitle.Text, columnTitle.HeaderFont).Width + 50
        columnTitle.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
        columnTitle.MinimumWidth = Math.Max(columnTitle.MinimumWidth, columnTitle.Width)
        listviewBooks.Sort(columnAuthor, SortOrder.Ascending)
        listviewBooks.EndUpdate()
        listviewBooks.Refresh()
    End Sub

    Private Sub ReloadBooks()
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Dim memoryDrive As [String] = [String].Empty
        Dim memoryType__1 As MemoryType = MemoryType.UNKNOWN

        prsT1.StorageLocations.Clear()
        kobo.StorageLocations.Clear()

        memoryDrive = [String].Empty
        memoryType__1 = MemoryType.UNKNOWN
        If comboboxInternalMemory.SelectedItem IsNot Nothing Then
            If TypeOf comboboxInternalMemory.SelectedItem Is StorageDriveInfo Then
                memoryDrive = DirectCast(comboboxInternalMemory.SelectedItem, StorageDriveInfo).DrivePath
                memoryType__1 = MemoryType.INTERNAL
            ElseIf TypeOf comboboxInternalMemory.SelectedItem Is [String] Then
                memoryDrive = DirectCast(comboboxInternalMemory.SelectedItem, [String])
                memoryType__1 = MemoryType.UNKNOWN
            End If
        Else
            memoryDrive = comboboxInternalMemory.Text.Trim()
            memoryType__1 = MemoryType.UNKNOWN
        End If
        If False = [String].IsNullOrEmpty(memoryDrive) Then
            If DirectCast(comboboxInternalMemory.SelectedItem, StorageDriveInfo).DriveBrand = "Sony" Then
                prsT1.StorageLocations.Add(New StorageInfo(memoryType__1, memoryDrive))
            Else
                kobo.StorageLocations.Add(New StorageInfo(memoryType__1, memoryDrive))
            End If
        End If

            memoryDrive = [String].Empty
            memoryType__1 = MemoryType.UNKNOWN
            If comboboxMicroSdCard.SelectedItem IsNot Nothing Then
                If TypeOf comboboxMicroSdCard.SelectedItem Is StorageDriveInfo Then
                    memoryDrive = DirectCast(comboboxMicroSdCard.SelectedItem, StorageDriveInfo).DrivePath
                    memoryType__1 = MemoryType.SDCARD
                ElseIf TypeOf comboboxMicroSdCard.SelectedItem Is [String] Then
                    memoryDrive = DirectCast(comboboxMicroSdCard.SelectedItem, [String])
                    memoryType__1 = MemoryType.UNKNOWN
                End If
            Else
                memoryDrive = comboboxMicroSdCard.Text.Trim()
                memoryType__1 = MemoryType.UNKNOWN
            End If
            If False = [String].IsNullOrEmpty(memoryDrive) Then
            If DirectCast(comboboxMicroSdCard.SelectedItem, StorageDriveInfo).DriveBrand = "Sony" Then
                prsT1.StorageLocations.Add(New StorageInfo(memoryType__1, memoryDrive))
            Else
                kobo.StorageLocations.Add(New StorageInfo(memoryType__1, memoryDrive))
            End If
            End If
        ReaderBrand = DirectCast(comboboxInternalMemory.SelectedItem, StorageDriveInfo).DriveBrand
        prsT1.LoadBooksInfo()
        kobo.LoadBooksInfo()
        RefreshBooks()

            System.Windows.Forms.Cursor.Current = Cursors.[Default]
    End Sub


    Private Sub checkboxHideBooksWithoutAnnotations_CheckedChanged(sender As Object, e As EventArgs) Handles checkboxHideBooksWithoutAnnotations.CheckedChanged
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        If True = checkboxHideBooksWithoutAnnotations.Checked Then
            For Each book As BookInfo In prsT1.Books
                If 0 >= book.Annotations.Count Then
                    book.ExportAnnotations = False
                End If
            Next
        End If
        RefreshBooks()
        System.Windows.Forms.Cursor.Current = Cursors.[Default]
    End Sub

    Private Sub listviewBooks_DoubleClick(sender As Object, e As EventArgs) Handles listviewBooks.DoubleClick
        If listviewBooks.SelectedItem IsNot Nothing Then
            listviewBooks.SelectedItem.Checked = Not (listviewBooks.SelectedItem.Checked)
        End If
    End Sub

    Private Sub listviewBooks_AfterSorting(sender As Object, e As AfterSortingEventArgs) Handles listviewBooks.AfterSorting
        listviewBooks.SelectedItems.Clear()
        listviewBooks.SelectedItem = listviewBooks.GetNthItemInDisplayOrder(0)
        listviewBooks.SelectedItem.EnsureVisible()
    End Sub

    Private Function GetAvailableDrives() As List(Of StorageDriveInfo)
        Dim drives As New List(Of StorageDriveInfo)()
        For Each drive As DriveInfo In DriveInfo.GetDrives()
            If (True = drive.IsReady) AndAlso (DriveType.Removable = drive.DriveType) AndAlso (True = File.Exists(Path.Combine(drive.Name, SonyReaderInfo.DatabaseFileRelativePath)) Or True = File.Exists(Path.Combine(drive.Name, KoboReaderInfo.DatabaseFileRelativePath))) Then
                If True = File.Exists(Path.Combine(drive.Name, SonyReaderInfo.DatabaseFileRelativePath)) Then
                    drives.Add(New StorageDriveInfo(drive.VolumeLabel, drive.Name, "Sony"))
                Else
                    drives.Add(New StorageDriveInfo(drive.VolumeLabel, drive.Name, "Kobo"))
                End If
                'drives.Add(New StorageDriveInfo(drive.VolumeLabel, drive.Name))
            End If
        Next
        Return drives
    End Function

    Private Sub FillInternalMemoryCombobox()
        comboboxInternalMemory.Items.Clear()
        comboboxInternalMemory.Items.Add(New StorageDriveInfo())
        Dim availableDrives As List(Of StorageDriveInfo) = GetAvailableDrives()
        For Each drive As StorageDriveInfo In availableDrives
            If False = drive.Equals(comboboxMicroSdCard.SelectedItem) Then
                comboboxInternalMemory.Items.Add(drive)
            End If
        Next
        If False = [String].IsNullOrEmpty(userInternalMemoryPath) Then
            If userInternalMemoryPath <> comboboxMicroSdCard.Text Then
                comboboxInternalMemory.Items.Add(userInternalMemoryPath)
            End If
        End If
    End Sub

    Private Sub FillMicroSdCardCombobox()
        comboboxMicroSdCard.Items.Clear()
        comboboxMicroSdCard.Items.Add(New StorageDriveInfo())
        Dim availableDrives As List(Of StorageDriveInfo) = GetAvailableDrives()
        For Each drive As StorageDriveInfo In availableDrives
            If False = drive.Equals(comboboxInternalMemory.SelectedItem) And drive.DriveBrand = DirectCast(comboboxInternalMemory.SelectedItem, StorageDriveInfo).DriveBrand Then
                comboboxMicroSdCard.Items.Add(drive)
            End If
        Next
        If False = [String].IsNullOrEmpty(userMicroSdCardPath) Then
            If userMicroSdCardPath <> comboboxInternalMemory.Text Then
                comboboxMicroSdCard.Items.Add(userMicroSdCardPath)
            End If
        End If
    End Sub

    Private Sub InitInternalMemoryAndMicroSdCardComboboxes()
        FillInternalMemoryCombobox()
        If 1 < comboboxInternalMemory.Items.Count Then
            comboboxInternalMemory.SelectedIndex = 1
            For index As Integer = 0 To comboboxInternalMemory.Items.Count - 1
                If TypeOf comboboxInternalMemory.Items(index) Is StorageDriveInfo Then
                    If "reader" = DirectCast(comboboxInternalMemory.Items(index), StorageDriveInfo).DriveLabel.ToLower() Then
                        comboboxInternalMemory.SelectedIndex = index
                        Exit For
                    End If
                End If
            Next
        Else
            comboboxInternalMemory.SelectedIndex = 0
        End If

        FillMicroSdCardCombobox()
        If 1 < comboboxMicroSdCard.Items.Count Then
            comboboxMicroSdCard.SelectedIndex = 1
        Else
            comboboxMicroSdCard.SelectedIndex = 0
        End If
    End Sub

    Private Sub comboboxInternalMemory_DropDown(sender As Object, e As EventArgs) Handles comboboxInternalMemory.DropDown
        Dim selectedItemOld As [Object] = comboboxInternalMemory.SelectedItem
        FillInternalMemoryCombobox()
        If True = comboboxInternalMemory.Items.Contains(selectedItemOld) Then
            comboboxInternalMemory.SelectedItem = selectedItemOld
        Else
            comboboxInternalMemory.SelectedIndex = 0
        End If

    End Sub

    Private Sub comboboxMicroSdCard_DropDown(sender As Object, e As EventArgs) Handles comboboxMicroSdCard.DropDown
        Dim selectedItemOld As [Object] = comboboxMicroSdCard.SelectedItem
        FillMicroSdCardCombobox()
        If True = comboboxMicroSdCard.Items.Contains(selectedItemOld) Then
            comboboxMicroSdCard.SelectedItem = selectedItemOld
        Else
            comboboxMicroSdCard.SelectedIndex = 0
        End If
    End Sub

    Private Sub comboboxInternalMemoryAndMicroSdCard_DropDownClosed(sender As Object, e As EventArgs) Handles comboboxInternalMemory.DropDownClosed, comboboxMicroSdCard.DropDownClosed
        Dim combobox As ComboBox = DirectCast(sender, ComboBox)
        If combobox.SelectedItem Is Nothing Then
            combobox.SelectedIndex = 0
        End If

    End Sub

    Private Function SelectUserPath(folderSelectionDialogTitle As [String]) As [String]
        Dim folderSelectionDialog As New FolderBrowserDialog()
        folderSelectionDialog.Description = folderSelectionDialogTitle
        folderSelectionDialog.ShowNewFolderButton = False
        If DialogResult.OK = folderSelectionDialog.ShowDialog() Then
            Return folderSelectionDialog.SelectedPath
        End If
        Return [String].Empty
    End Function

    Private Sub labelInternalMemory_DoubleClick(sender As Object, e As EventArgs) Handles labelInternalMemory.DoubleClick
        userInternalMemoryPath = SelectUserPath("Select Internal Memory drive backup folder")
        If False = [String].IsNullOrEmpty(userInternalMemoryPath) Then
            Dim selectedItemOld As [Object] = comboboxInternalMemory.SelectedItem
            FillInternalMemoryCombobox()
            If True = comboboxInternalMemory.Items.Contains(userInternalMemoryPath) Then
                comboboxInternalMemory.SelectedItem = userInternalMemoryPath
            Else
                comboboxInternalMemory.SelectedItem = selectedItemOld
            End If
        End If
        If comboboxInternalMemory.SelectedItem Is Nothing Then
            comboboxInternalMemory.SelectedIndex = 0
        End If
    End Sub

    Private Sub labelMicroSdCard_DoubleClick(sender As Object, e As EventArgs) Handles labelMicroSdCard.DoubleClick
        userMicroSdCardPath = SelectUserPath("Select Micro SD Card drive backup folder")
        If False = [String].IsNullOrEmpty(userMicroSdCardPath) Then
            Dim selectedItemOld As [Object] = comboboxMicroSdCard.SelectedItem
            FillMicroSdCardCombobox()
            If True = comboboxMicroSdCard.Items.Contains(userMicroSdCardPath) Then
                comboboxMicroSdCard.SelectedItem = userMicroSdCardPath
            Else
                comboboxMicroSdCard.SelectedItem = selectedItemOld
            End If
        End If
        If comboboxInternalMemory.SelectedItem Is Nothing Then
            comboboxInternalMemory.SelectedIndex = 0
        End If
    End Sub


    Private Sub comboboxInternalMemoryAndMicroSdCard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboboxInternalMemory.SelectedIndexChanged
        If TypeOf sender Is ComboBox Then
            Dim combobox As ComboBox = DirectCast(sender, ComboBox)
            Dim comboboxText As [String] = combobox.Text
            If TextRenderer.MeasureText(comboboxText, combobox.Font).Width < combobox.Size.Width - 10 Then
                ' sender.toolTip.SetToolTip(combobox, " ")
            Else
                ' sender.toolTip.SetToolTip(combobox, comboboxText)
            End If
        End If
        ReaderBrand = DirectCast(comboboxInternalMemory.SelectedItem, StorageDriveInfo).DriveBrand
        ReloadBooks()
    End Sub

    Private Sub checkboxExpandMarkedText_CheckedChanged(sender As Object, e As EventArgs) Handles checkboxExpandMarkedText.CheckedChanged
        textboxCharactersToExpandMarkedTextBy.Enabled = checkboxExpandMarkedText.Checked
        labelExpandMarkedText_By.Enabled = checkboxExpandMarkedText.Checked
        labelExpandMarkedText_Characters.Enabled = checkboxExpandMarkedText.Checked
        checkboxHighlightOriginalMark.Enabled = checkboxExpandMarkedText.Checked
    End Sub

    Private Sub textboxCharactersToExpandMarkedTextBy_KeyPress(sender As Object, e As KeyPressEventArgs) Handles textboxCharactersToExpandMarkedTextBy.KeyPress
        If (False = [Char].IsDigit(e.KeyChar)) AndAlso (False = [Char].IsControl(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub checkboxCreateSeparateFileForEachBook_CheckedChanged(sender As Object, e As EventArgs) Handles checkboxCreateSeparateFileForEachBook.CheckedChanged
        checkboxInsertBookInformation.Enabled = checkboxCreateSeparateFileForEachBook.Checked
    End Sub

    Private Sub buttonExport_Click(sender As Object, e As EventArgs) Handles buttonExport.Click
        If TypeOf comboboxExportStyle.SelectedItem Is HtmlExportStyle Then
            Dim books As List(Of BookInfo)
            If ReaderBrand = "Sony" Then
                books = prsT1.BooksForExportAnnotations
            Else
                books = kobo.BooksForExportAnnotations
            End If
            If 0 < books.Count Then
                Dim targetFileOrDir As [String] = [String].Empty

                If (1 < books.Count) AndAlso (True = checkboxCreateSeparateFileForEachBook.Checked) Then
                    Dim folderSelectionDialog As New FolderBrowserDialog()
                    folderSelectionDialog.Description = "Specify export directory"
                    folderSelectionDialog.ShowNewFolderButton = True
                    folderSelectionDialog.SelectedPath = lastSelectedFolder
                    If DialogResult.OK = folderSelectionDialog.ShowDialog() Then
                        targetFileOrDir = folderSelectionDialog.SelectedPath
                        lastSelectedFolder = targetFileOrDir
                    End If
                Else
                    Dim saveFileDialog As New SaveFileDialog()
                    saveFileDialog.Title = "Specify export file"
                    saveFileDialog.FileName = (If((1 < books.Count), "AnnotationsExport.html", Path.ChangeExtension(books(0).FileName, ".html")))
                    saveFileDialog.Filter = "HTML file (*.html)|*.html"
                    saveFileDialog.FilterIndex = 0
                    saveFileDialog.InitialDirectory = lastSelectedFolder
                    If DialogResult.OK = saveFileDialog.ShowDialog() Then
                        targetFileOrDir = saveFileDialog.FileName
                        lastSelectedFolder = Path.GetDirectoryName(targetFileOrDir)
                    End If
                End If

                If False = [String].IsNullOrEmpty(targetFileOrDir) Then
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

                    Dim charactersToExpandMarkedTextBy As Integer = 0
                    Dim highlightOriginalMark As Boolean = False
                    If True = checkboxExpandMarkedText.Checked Then
                        highlightOriginalMark = checkboxHighlightOriginalMark.Checked
                        If False = Int32.TryParse(textboxCharactersToExpandMarkedTextBy.Text, charactersToExpandMarkedTextBy) Then
                            charactersToExpandMarkedTextBy = 0
                            highlightOriginalMark = False
                        End If
                    End If

                    books.Sort(New BookInfoComparer(listviewBooks.LastSortColumn.Text, listviewBooks.LastSortOrder))
                    HtmlExporter.Export(books, targetFileOrDir, (If((True = checkboxCreateSeparateFileForEachBook.Checked), checkboxInsertBookInformation.Checked, True)), checkboxCreateInlineImages.Checked, My.MySettings.[Default].PageNumberDecimalPlaces, charactersToExpandMarkedTextBy,
                        highlightOriginalMark, TryCast(comboboxExportStyle.SelectedItem, HtmlExportStyle))

                    System.Windows.Forms.Cursor.Current = Cursors.[Default]
                End If
                MetroFramework.MetroMessageBox.Show(Me, "Export of notes done.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MetroFramework.MetroMessageBox.Show(Me, "No book(s) selected. Export not possible.", "Unable to export", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub


    Private Sub linklabelAbout_Click(sender As Object, e As EventArgs) Handles linklabelAbout.Click
        Dim version As String = My.Application.Info.Version.ToString
        Dim infoText As [String] = "FREEWARE, © 2015 Toxaris" & vbLf & "Original version © 2012 by Yoths" _
                                   & vbLf & vbLf & "Version: " & version.Substring(0, version.Length - 2)

        ' MessageBox.Show(Me, infoText, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        MetroFramework.MetroMessageBox.Show(Me, infoText, Text & " for Sony PRS-Tx and Kobo", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub buttonReload_Click(sender As Object, e As EventArgs) Handles buttonReload.Click
        ReloadBooks()
    End Sub

    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
        Dim books As List(Of BookInfo)
        Dim Location As List(Of StorageInfo)
        Dim memoryDrive As [String] = [String].Empty
        Dim memoryType__1 As MemoryType = MemoryType.UNKNOWN


        If ReaderBrand = "Sony" Then
            books = prsT1.BooksForExportAnnotations
            prsT1.StorageLocations.Add(New StorageInfo(memoryType__1, memoryDrive))
            Location = prsT1.StorageLocations
        Else
            books = kobo.BooksForExportAnnotations
            kobo.StorageLocations.Add(New StorageInfo(memoryType__1, memoryDrive))
            Location = kobo.StorageLocations
        End If

        If 0 < books.Count Then
            For Each book As BookInfo In books
                remove_annotation(book, Location)
            Next
            MetroFramework.MetroMessageBox.Show(Me, "Removal of annotations done.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ReloadBooks()
        Else
            MetroFramework.MetroMessageBox.Show(Me, "No book(s) selected. Removal of annotations possible.", "Unable to remove", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class