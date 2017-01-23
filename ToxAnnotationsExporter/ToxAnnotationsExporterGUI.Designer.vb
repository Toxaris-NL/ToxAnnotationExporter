<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ToxAnnotationsExporterGUI
    Inherits MetroFramework.Forms.MetroForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ToxAnnotationsExporterGUI))
        Me.msmMain = New MetroFramework.Components.MetroStyleManager(Me.components)
        Me.comboboxInternalMemory = New MetroFramework.Controls.MetroComboBox()
        Me.comboboxMicroSdCard = New MetroFramework.Controls.MetroComboBox()
        Me.comboboxExportStyle = New MetroFramework.Controls.MetroComboBox()
        Me.textboxCharactersToExpandMarkedTextBy = New MetroFramework.Controls.MetroTextBox()
        Me.labelExpandMarkedText_By = New MetroFramework.Controls.MetroLabel()
        Me.labelExpandMarkedText_Characters = New MetroFramework.Controls.MetroLabel()
        Me.listviewBooks = New BrightIdeasSoftware.ObjectListView()
        Me.columnAuthor = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.columnTitle = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.checkboxHideBooksWithoutAnnotations = New MetroFramework.Controls.MetroCheckBox()
        Me.labelInternalMemory = New MetroFramework.Controls.MetroLabel()
        Me.labelMicroSdCard = New MetroFramework.Controls.MetroLabel()
        Me.MetroPanel1 = New MetroFramework.Controls.MetroPanel()
        Me.checkboxInsertBookInformation = New MetroFramework.Controls.MetroCheckBox()
        Me.labelExportStyle = New MetroFramework.Controls.MetroLabel()
        Me.buttonExport = New MetroFramework.Controls.MetroButton()
        Me.checkboxHighlightOriginalMark = New MetroFramework.Controls.MetroCheckBox()
        Me.checkboxCreateSeparateFileForEachBook = New MetroFramework.Controls.MetroCheckBox()
        Me.checkboxExpandMarkedText = New MetroFramework.Controls.MetroCheckBox()
        Me.checkboxCreateInlineImages = New MetroFramework.Controls.MetroCheckBox()
        Me.MetroLabel1 = New MetroFramework.Controls.MetroLabel()
        Me.linklabelAbout = New MetroFramework.Controls.MetroLink()
        Me.buttonReload = New MetroFramework.Controls.MetroButton()
        Me.ButtonDelete = New MetroFramework.Controls.MetroButton()
        CType(Me.msmMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.listviewBooks, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MetroPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'msmMain
        '
        Me.msmMain.Owner = Me
        Me.msmMain.Style = MetroFramework.MetroColorStyle.Green
        '
        'comboboxInternalMemory
        '
        Me.comboboxInternalMemory.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboboxInternalMemory.FormattingEnabled = True
        Me.comboboxInternalMemory.ItemHeight = 23
        Me.comboboxInternalMemory.Location = New System.Drawing.Point(490, 85)
        Me.comboboxInternalMemory.Margin = New System.Windows.Forms.Padding(2)
        Me.comboboxInternalMemory.Name = "comboboxInternalMemory"
        Me.comboboxInternalMemory.Size = New System.Drawing.Size(222, 29)
        Me.comboboxInternalMemory.TabIndex = 0
        Me.comboboxInternalMemory.UseSelectable = True
        '
        'comboboxMicroSdCard
        '
        Me.comboboxMicroSdCard.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboboxMicroSdCard.FormattingEnabled = True
        Me.comboboxMicroSdCard.ItemHeight = 23
        Me.comboboxMicroSdCard.Location = New System.Drawing.Point(490, 136)
        Me.comboboxMicroSdCard.Margin = New System.Windows.Forms.Padding(2)
        Me.comboboxMicroSdCard.Name = "comboboxMicroSdCard"
        Me.comboboxMicroSdCard.Size = New System.Drawing.Size(222, 29)
        Me.comboboxMicroSdCard.TabIndex = 1
        Me.comboboxMicroSdCard.UseSelectable = True
        '
        'comboboxExportStyle
        '
        Me.comboboxExportStyle.FormattingEnabled = True
        Me.comboboxExportStyle.ItemHeight = 23
        Me.comboboxExportStyle.Location = New System.Drawing.Point(57, 9)
        Me.comboboxExportStyle.Margin = New System.Windows.Forms.Padding(2)
        Me.comboboxExportStyle.Name = "comboboxExportStyle"
        Me.comboboxExportStyle.Size = New System.Drawing.Size(152, 29)
        Me.comboboxExportStyle.TabIndex = 1
        Me.comboboxExportStyle.UseSelectable = True
        '
        'textboxCharactersToExpandMarkedTextBy
        '
        Me.textboxCharactersToExpandMarkedTextBy.Lines = New String() {"100"}
        Me.textboxCharactersToExpandMarkedTextBy.Location = New System.Drawing.Point(38, 124)
        Me.textboxCharactersToExpandMarkedTextBy.Margin = New System.Windows.Forms.Padding(2)
        Me.textboxCharactersToExpandMarkedTextBy.MaxLength = 32767
        Me.textboxCharactersToExpandMarkedTextBy.Name = "textboxCharactersToExpandMarkedTextBy"
        Me.textboxCharactersToExpandMarkedTextBy.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.textboxCharactersToExpandMarkedTextBy.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.textboxCharactersToExpandMarkedTextBy.SelectedText = ""
        Me.textboxCharactersToExpandMarkedTextBy.Size = New System.Drawing.Size(34, 19)
        Me.textboxCharactersToExpandMarkedTextBy.TabIndex = 6
        Me.textboxCharactersToExpandMarkedTextBy.Text = "100"
        Me.textboxCharactersToExpandMarkedTextBy.Theme = MetroFramework.MetroThemeStyle.Light
        Me.textboxCharactersToExpandMarkedTextBy.UseSelectable = True
        '
        'labelExpandMarkedText_By
        '
        Me.labelExpandMarkedText_By.AutoSize = True
        Me.labelExpandMarkedText_By.Enabled = False
        Me.labelExpandMarkedText_By.FontSize = MetroFramework.MetroLabelSize.Small
        Me.labelExpandMarkedText_By.FontWeight = MetroFramework.MetroLabelWeight.Regular
        Me.labelExpandMarkedText_By.Location = New System.Drawing.Point(18, 125)
        Me.labelExpandMarkedText_By.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelExpandMarkedText_By.Name = "labelExpandMarkedText_By"
        Me.labelExpandMarkedText_By.Size = New System.Drawing.Size(20, 15)
        Me.labelExpandMarkedText_By.TabIndex = 10
        Me.labelExpandMarkedText_By.Text = "by"
        '
        'labelExpandMarkedText_Characters
        '
        Me.labelExpandMarkedText_Characters.AutoSize = True
        Me.labelExpandMarkedText_Characters.Enabled = False
        Me.labelExpandMarkedText_Characters.FontSize = MetroFramework.MetroLabelSize.Small
        Me.labelExpandMarkedText_Characters.FontWeight = MetroFramework.MetroLabelWeight.Regular
        Me.labelExpandMarkedText_Characters.Location = New System.Drawing.Point(76, 125)
        Me.labelExpandMarkedText_Characters.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelExpandMarkedText_Characters.Name = "labelExpandMarkedText_Characters"
        Me.labelExpandMarkedText_Characters.Size = New System.Drawing.Size(73, 15)
        Me.labelExpandMarkedText_Characters.TabIndex = 11
        Me.labelExpandMarkedText_Characters.Text = "MetroLabel1"
        '
        'listviewBooks
        '
        Me.listviewBooks.AllColumns.Add(Me.columnAuthor)
        Me.listviewBooks.AllColumns.Add(Me.columnTitle)
        Me.listviewBooks.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.listviewBooks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnAuthor, Me.columnTitle})
        Me.listviewBooks.Location = New System.Drawing.Point(18, 64)
        Me.listviewBooks.Margin = New System.Windows.Forms.Padding(2)
        Me.listviewBooks.Name = "listviewBooks"
        Me.listviewBooks.Size = New System.Drawing.Size(452, 368)
        Me.listviewBooks.TabIndex = 12
        Me.listviewBooks.UseCompatibleStateImageBehavior = False
        Me.listviewBooks.View = System.Windows.Forms.View.Details
        '
        'checkboxHideBooksWithoutAnnotations
        '
        Me.checkboxHideBooksWithoutAnnotations.AutoSize = True
        Me.checkboxHideBooksWithoutAnnotations.Location = New System.Drawing.Point(17, 435)
        Me.checkboxHideBooksWithoutAnnotations.Margin = New System.Windows.Forms.Padding(2)
        Me.checkboxHideBooksWithoutAnnotations.Name = "checkboxHideBooksWithoutAnnotations"
        Me.checkboxHideBooksWithoutAnnotations.Size = New System.Drawing.Size(113, 15)
        Me.checkboxHideBooksWithoutAnnotations.TabIndex = 4
        Me.checkboxHideBooksWithoutAnnotations.Text = "MetroCheckBox1"
        Me.checkboxHideBooksWithoutAnnotations.UseSelectable = True
        '
        'labelInternalMemory
        '
        Me.labelInternalMemory.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labelInternalMemory.AutoSize = True
        Me.labelInternalMemory.Location = New System.Drawing.Point(490, 64)
        Me.labelInternalMemory.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelInternalMemory.Name = "labelInternalMemory"
        Me.labelInternalMemory.Size = New System.Drawing.Size(81, 19)
        Me.labelInternalMemory.TabIndex = 6
        Me.labelInternalMemory.Text = "MetroLabel1"
        '
        'labelMicroSdCard
        '
        Me.labelMicroSdCard.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labelMicroSdCard.AutoSize = True
        Me.labelMicroSdCard.Location = New System.Drawing.Point(490, 117)
        Me.labelMicroSdCard.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelMicroSdCard.Name = "labelMicroSdCard"
        Me.labelMicroSdCard.Size = New System.Drawing.Size(81, 19)
        Me.labelMicroSdCard.TabIndex = 15
        Me.labelMicroSdCard.Text = "MetroLabel1"
        '
        'MetroPanel1
        '
        Me.MetroPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MetroPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MetroPanel1.Controls.Add(Me.checkboxInsertBookInformation)
        Me.MetroPanel1.Controls.Add(Me.labelExportStyle)
        Me.MetroPanel1.Controls.Add(Me.buttonExport)
        Me.MetroPanel1.Controls.Add(Me.checkboxHighlightOriginalMark)
        Me.MetroPanel1.Controls.Add(Me.checkboxCreateSeparateFileForEachBook)
        Me.MetroPanel1.Controls.Add(Me.checkboxExpandMarkedText)
        Me.MetroPanel1.Controls.Add(Me.checkboxCreateInlineImages)
        Me.MetroPanel1.Controls.Add(Me.labelExpandMarkedText_By)
        Me.MetroPanel1.Controls.Add(Me.comboboxExportStyle)
        Me.MetroPanel1.Controls.Add(Me.labelExpandMarkedText_Characters)
        Me.MetroPanel1.Controls.Add(Me.textboxCharactersToExpandMarkedTextBy)
        Me.MetroPanel1.HorizontalScrollbarBarColor = True
        Me.MetroPanel1.HorizontalScrollbarHighlightOnWheel = False
        Me.MetroPanel1.HorizontalScrollbarSize = 8
        Me.MetroPanel1.Location = New System.Drawing.Point(490, 204)
        Me.MetroPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.MetroPanel1.Name = "MetroPanel1"
        Me.MetroPanel1.Size = New System.Drawing.Size(225, 228)
        Me.MetroPanel1.TabIndex = 3
        Me.MetroPanel1.VerticalScrollbarBarColor = True
        Me.MetroPanel1.VerticalScrollbarHighlightOnWheel = False
        Me.MetroPanel1.VerticalScrollbarSize = 8
        '
        'checkboxInsertBookInformation
        '
        Me.checkboxInsertBookInformation.AutoSize = True
        Me.checkboxInsertBookInformation.Checked = True
        Me.checkboxInsertBookInformation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkboxInsertBookInformation.Location = New System.Drawing.Point(18, 60)
        Me.checkboxInsertBookInformation.Margin = New System.Windows.Forms.Padding(2)
        Me.checkboxInsertBookInformation.Name = "checkboxInsertBookInformation"
        Me.checkboxInsertBookInformation.Size = New System.Drawing.Size(113, 15)
        Me.checkboxInsertBookInformation.TabIndex = 3
        Me.checkboxInsertBookInformation.Text = "MetroCheckBox1"
        Me.checkboxInsertBookInformation.UseSelectable = True
        '
        'labelExportStyle
        '
        Me.labelExportStyle.AutoSize = True
        Me.labelExportStyle.Enabled = False
        Me.labelExportStyle.FontSize = MetroFramework.MetroLabelSize.Small
        Me.labelExportStyle.FontWeight = MetroFramework.MetroLabelWeight.Regular
        Me.labelExportStyle.Location = New System.Drawing.Point(6, 14)
        Me.labelExportStyle.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.labelExportStyle.Name = "labelExportStyle"
        Me.labelExportStyle.Size = New System.Drawing.Size(73, 15)
        Me.labelExportStyle.TabIndex = 15
        Me.labelExportStyle.Text = "MetroLabel1"
        '
        'buttonExport
        '
        Me.buttonExport.Location = New System.Drawing.Point(18, 176)
        Me.buttonExport.Name = "buttonExport"
        Me.buttonExport.Size = New System.Drawing.Size(183, 23)
        Me.buttonExport.TabIndex = 8
        Me.buttonExport.Text = "MetroButton1"
        Me.buttonExport.UseSelectable = True
        '
        'checkboxHighlightOriginalMark
        '
        Me.checkboxHighlightOriginalMark.AutoSize = True
        Me.checkboxHighlightOriginalMark.Checked = True
        Me.checkboxHighlightOriginalMark.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkboxHighlightOriginalMark.Location = New System.Drawing.Point(20, 148)
        Me.checkboxHighlightOriginalMark.Margin = New System.Windows.Forms.Padding(2)
        Me.checkboxHighlightOriginalMark.Name = "checkboxHighlightOriginalMark"
        Me.checkboxHighlightOriginalMark.Size = New System.Drawing.Size(113, 15)
        Me.checkboxHighlightOriginalMark.TabIndex = 7
        Me.checkboxHighlightOriginalMark.Text = "MetroCheckBox1"
        Me.checkboxHighlightOriginalMark.UseSelectable = True
        '
        'checkboxCreateSeparateFileForEachBook
        '
        Me.checkboxCreateSeparateFileForEachBook.AutoSize = True
        Me.checkboxCreateSeparateFileForEachBook.Checked = True
        Me.checkboxCreateSeparateFileForEachBook.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkboxCreateSeparateFileForEachBook.Location = New System.Drawing.Point(2, 41)
        Me.checkboxCreateSeparateFileForEachBook.Margin = New System.Windows.Forms.Padding(2)
        Me.checkboxCreateSeparateFileForEachBook.Name = "checkboxCreateSeparateFileForEachBook"
        Me.checkboxCreateSeparateFileForEachBook.Size = New System.Drawing.Size(113, 15)
        Me.checkboxCreateSeparateFileForEachBook.TabIndex = 2
        Me.checkboxCreateSeparateFileForEachBook.Text = "MetroCheckBox1"
        Me.checkboxCreateSeparateFileForEachBook.UseSelectable = True
        '
        'checkboxExpandMarkedText
        '
        Me.checkboxExpandMarkedText.AutoSize = True
        Me.checkboxExpandMarkedText.Checked = True
        Me.checkboxExpandMarkedText.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkboxExpandMarkedText.Location = New System.Drawing.Point(2, 101)
        Me.checkboxExpandMarkedText.Margin = New System.Windows.Forms.Padding(2)
        Me.checkboxExpandMarkedText.Name = "checkboxExpandMarkedText"
        Me.checkboxExpandMarkedText.Size = New System.Drawing.Size(113, 15)
        Me.checkboxExpandMarkedText.TabIndex = 5
        Me.checkboxExpandMarkedText.Text = "MetroCheckBox1"
        Me.checkboxExpandMarkedText.UseSelectable = True
        '
        'checkboxCreateInlineImages
        '
        Me.checkboxCreateInlineImages.AutoSize = True
        Me.checkboxCreateInlineImages.Checked = True
        Me.checkboxCreateInlineImages.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkboxCreateInlineImages.Location = New System.Drawing.Point(2, 80)
        Me.checkboxCreateInlineImages.Margin = New System.Windows.Forms.Padding(2)
        Me.checkboxCreateInlineImages.Name = "checkboxCreateInlineImages"
        Me.checkboxCreateInlineImages.Size = New System.Drawing.Size(113, 15)
        Me.checkboxCreateInlineImages.TabIndex = 4
        Me.checkboxCreateInlineImages.Text = "MetroCheckBox1"
        Me.checkboxCreateInlineImages.UseSelectable = True
        '
        'MetroLabel1
        '
        Me.MetroLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MetroLabel1.AutoSize = True
        Me.MetroLabel1.Location = New System.Drawing.Point(497, 195)
        Me.MetroLabel1.Name = "MetroLabel1"
        Me.MetroLabel1.Size = New System.Drawing.Size(86, 19)
        Me.MetroLabel1.TabIndex = 15
        Me.MetroLabel1.Text = "HTML Export"
        '
        'linklabelAbout
        '
        Me.linklabelAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.linklabelAbout.Location = New System.Drawing.Point(640, 435)
        Me.linklabelAbout.Name = "linklabelAbout"
        Me.linklabelAbout.Size = New System.Drawing.Size(75, 23)
        Me.linklabelAbout.TabIndex = 5
        Me.linklabelAbout.Text = "About"
        Me.linklabelAbout.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.linklabelAbout.UseSelectable = True
        '
        'buttonReload
        '
        Me.buttonReload.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonReload.Location = New System.Drawing.Point(640, 175)
        Me.buttonReload.Margin = New System.Windows.Forms.Padding(2)
        Me.buttonReload.Name = "buttonReload"
        Me.buttonReload.Size = New System.Drawing.Size(71, 22)
        Me.buttonReload.TabIndex = 2
        Me.buttonReload.Text = "MetroButton1"
        Me.buttonReload.UseSelectable = True
        '
        'ButtonDelete
        '
        Me.ButtonDelete.Location = New System.Drawing.Point(267, 437)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.Size = New System.Drawing.Size(183, 23)
        Me.ButtonDelete.TabIndex = 16
        Me.ButtonDelete.Text = "MetroButton1"
        Me.ButtonDelete.UseSelectable = True
        '
        'ToxAnnotationsExporterGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(729, 467)
        Me.Controls.Add(Me.ButtonDelete)
        Me.Controls.Add(Me.buttonReload)
        Me.Controls.Add(Me.linklabelAbout)
        Me.Controls.Add(Me.MetroLabel1)
        Me.Controls.Add(Me.MetroPanel1)
        Me.Controls.Add(Me.labelMicroSdCard)
        Me.Controls.Add(Me.labelInternalMemory)
        Me.Controls.Add(Me.checkboxHideBooksWithoutAnnotations)
        Me.Controls.Add(Me.listviewBooks)
        Me.Controls.Add(Me.comboboxMicroSdCard)
        Me.Controls.Add(Me.comboboxInternalMemory)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MinimumSize = New System.Drawing.Size(729, 461)
        Me.Name = "ToxAnnotationsExporterGUI"
        Me.Padding = New System.Windows.Forms.Padding(15, 60, 15, 16)
        Me.Style = MetroFramework.MetroColorStyle.Green
        Me.StyleManager = Me.msmMain
        Me.Text = "Annotation Exporter"
        CType(Me.msmMain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.listviewBooks, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MetroPanel1.ResumeLayout(False)
        Me.MetroPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents msmMain As MetroFramework.Components.MetroStyleManager
    Friend WithEvents comboboxExportStyle As MetroFramework.Controls.MetroComboBox
    Friend WithEvents comboboxMicroSdCard As MetroFramework.Controls.MetroComboBox
    Friend WithEvents comboboxInternalMemory As MetroFramework.Controls.MetroComboBox
    Friend WithEvents textboxCharactersToExpandMarkedTextBy As MetroFramework.Controls.MetroTextBox
    Friend WithEvents labelExpandMarkedText_Characters As MetroFramework.Controls.MetroLabel
    Friend WithEvents labelExpandMarkedText_By As MetroFramework.Controls.MetroLabel
    Friend WithEvents listviewBooks As BrightIdeasSoftware.ObjectListView
    Friend WithEvents columnAuthor As BrightIdeasSoftware.OLVColumn
    Friend WithEvents columnTitle As BrightIdeasSoftware.OLVColumn
    Friend WithEvents checkboxHideBooksWithoutAnnotations As MetroFramework.Controls.MetroCheckBox
    Friend WithEvents labelInternalMemory As MetroFramework.Controls.MetroLabel
    Friend WithEvents labelMicroSdCard As MetroFramework.Controls.MetroLabel
    Friend WithEvents MetroPanel1 As MetroFramework.Controls.MetroPanel
    Friend WithEvents checkboxCreateInlineImages As MetroFramework.Controls.MetroCheckBox
    Friend WithEvents checkboxExpandMarkedText As MetroFramework.Controls.MetroCheckBox
    Friend WithEvents checkboxCreateSeparateFileForEachBook As MetroFramework.Controls.MetroCheckBox
    Friend WithEvents checkboxHighlightOriginalMark As MetroFramework.Controls.MetroCheckBox
    Friend WithEvents buttonExport As MetroFramework.Controls.MetroButton
    Friend WithEvents linklabelAbout As MetroFramework.Controls.MetroLink
    Friend WithEvents MetroLabel1 As MetroFramework.Controls.MetroLabel
    Friend WithEvents labelExportStyle As MetroFramework.Controls.MetroLabel
    Friend WithEvents buttonReload As MetroFramework.Controls.MetroButton
    Friend WithEvents checkboxInsertBookInformation As MetroFramework.Controls.MetroCheckBox
    Friend WithEvents ButtonDelete As MetroFramework.Controls.MetroButton
End Class
