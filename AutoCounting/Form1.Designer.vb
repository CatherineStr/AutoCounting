<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainForm
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.areaH_nud = New System.Windows.Forms.NumericUpDown()
        Me.areaW_nud = New System.Windows.Forms.NumericUpDown()
        Me.y1_nud = New System.Windows.Forms.NumericUpDown()
        Me.x1_nud = New System.Windows.Forms.NumericUpDown()
        Me.settings_btn = New System.Windows.Forms.Button()
        Me.Stop_btn = New System.Windows.Forms.Button()
        Me.break_btn = New System.Windows.Forms.Button()
        Me.start_btn = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Source_pb = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.difference_pb = New System.Windows.Forms.PictureBox()
        Me.Panel2.SuspendLayout()
        CType(Me.areaH_nud, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.areaW_nud, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.y1_nud, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.x1_nud, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.Source_pb, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.difference_pb, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 490)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1160, 150)
        Me.Panel1.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.areaH_nud)
        Me.Panel2.Controls.Add(Me.areaW_nud)
        Me.Panel2.Controls.Add(Me.y1_nud)
        Me.Panel2.Controls.Add(Me.x1_nud)
        Me.Panel2.Controls.Add(Me.settings_btn)
        Me.Panel2.Controls.Add(Me.Stop_btn)
        Me.Panel2.Controls.Add(Me.break_btn)
        Me.Panel2.Controls.Add(Me.start_btn)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1160, 51)
        Me.Panel2.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(854, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Высота обл."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(779, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Ширина обл."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(679, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "y1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(603, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(18, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "x1"
        '
        'areaH_nud
        '
        Me.areaH_nud.Location = New System.Drawing.Point(855, 25)
        Me.areaH_nud.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.areaH_nud.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.areaH_nud.Name = "areaH_nud"
        Me.areaH_nud.Size = New System.Drawing.Size(68, 20)
        Me.areaH_nud.TabIndex = 8
        Me.areaH_nud.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'areaW_nud
        '
        Me.areaW_nud.Location = New System.Drawing.Point(781, 25)
        Me.areaW_nud.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.areaW_nud.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.areaW_nud.Name = "areaW_nud"
        Me.areaW_nud.Size = New System.Drawing.Size(68, 20)
        Me.areaW_nud.TabIndex = 7
        Me.areaW_nud.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'y1_nud
        '
        Me.y1_nud.Location = New System.Drawing.Point(657, 25)
        Me.y1_nud.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.y1_nud.Name = "y1_nud"
        Me.y1_nud.Size = New System.Drawing.Size(68, 20)
        Me.y1_nud.TabIndex = 6
        Me.y1_nud.Value = New Decimal(New Integer() {180, 0, 0, 0})
        '
        'x1_nud
        '
        Me.x1_nud.Location = New System.Drawing.Point(583, 26)
        Me.x1_nud.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.x1_nud.Name = "x1_nud"
        Me.x1_nud.Size = New System.Drawing.Size(68, 20)
        Me.x1_nud.TabIndex = 5
        Me.x1_nud.Value = New Decimal(New Integer() {180, 0, 0, 0})
        '
        'settings_btn
        '
        Me.settings_btn.Dock = System.Windows.Forms.DockStyle.Right
        Me.settings_btn.Location = New System.Drawing.Point(1085, 0)
        Me.settings_btn.Name = "settings_btn"
        Me.settings_btn.Size = New System.Drawing.Size(75, 51)
        Me.settings_btn.TabIndex = 4
        Me.settings_btn.Text = "Настройки"
        Me.settings_btn.UseVisualStyleBackColor = True
        '
        'Stop_btn
        '
        Me.Stop_btn.Dock = System.Windows.Forms.DockStyle.Left
        Me.Stop_btn.Location = New System.Drawing.Point(150, 0)
        Me.Stop_btn.Name = "Stop_btn"
        Me.Stop_btn.Size = New System.Drawing.Size(75, 51)
        Me.Stop_btn.TabIndex = 2
        Me.Stop_btn.Text = "Стоп"
        Me.Stop_btn.UseVisualStyleBackColor = True
        '
        'break_btn
        '
        Me.break_btn.Dock = System.Windows.Forms.DockStyle.Left
        Me.break_btn.Location = New System.Drawing.Point(75, 0)
        Me.break_btn.Name = "break_btn"
        Me.break_btn.Size = New System.Drawing.Size(75, 51)
        Me.break_btn.TabIndex = 1
        Me.break_btn.Text = "Сброс"
        Me.break_btn.UseVisualStyleBackColor = True
        '
        'start_btn
        '
        Me.start_btn.Dock = System.Windows.Forms.DockStyle.Left
        Me.start_btn.Location = New System.Drawing.Point(0, 0)
        Me.start_btn.Name = "start_btn"
        Me.start_btn.Size = New System.Drawing.Size(75, 51)
        Me.start_btn.TabIndex = 3
        Me.start_btn.Text = "Старт"
        Me.start_btn.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 51)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.difference_pb)
        Me.SplitContainer1.Size = New System.Drawing.Size(1160, 439)
        Me.SplitContainer1.SplitterDistance = 579
        Me.SplitContainer1.TabIndex = 4
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.Source_pb)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.PictureBox2)
        Me.SplitContainer2.Size = New System.Drawing.Size(579, 439)
        Me.SplitContainer2.SplitterDistance = 217
        Me.SplitContainer2.TabIndex = 1
        '
        'Source_pb
        '
        Me.Source_pb.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Source_pb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Source_pb.Location = New System.Drawing.Point(0, 0)
        Me.Source_pb.Name = "Source_pb"
        Me.Source_pb.Size = New System.Drawing.Size(579, 217)
        Me.Source_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Source_pb.TabIndex = 1
        Me.Source_pb.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.PictureBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox2.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(579, 218)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'difference_pb
        '
        Me.difference_pb.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.difference_pb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.difference_pb.Location = New System.Drawing.Point(0, 0)
        Me.difference_pb.Name = "difference_pb"
        Me.difference_pb.Size = New System.Drawing.Size(577, 439)
        Me.difference_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.difference_pb.TabIndex = 1
        Me.difference_pb.TabStop = False
        '
        'mainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1160, 640)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.IsMdiContainer = True
        Me.Name = "mainForm"
        Me.Text = "AutoCounter"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.areaH_nud, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.areaW_nud, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.y1_nud, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.x1_nud, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.Source_pb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.difference_pb, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents break_btn As System.Windows.Forms.Button
    Friend WithEvents start_btn As System.Windows.Forms.Button
    Friend WithEvents Stop_btn As System.Windows.Forms.Button
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Source_pb As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents difference_pb As System.Windows.Forms.PictureBox
    Friend WithEvents settings_btn As System.Windows.Forms.Button
    Friend WithEvents x1_nud As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents areaH_nud As System.Windows.Forms.NumericUpDown
    Friend WithEvents areaW_nud As System.Windows.Forms.NumericUpDown
    Friend WithEvents y1_nud As System.Windows.Forms.NumericUpDown

End Class
