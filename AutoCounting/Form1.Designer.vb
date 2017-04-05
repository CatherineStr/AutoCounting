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
        Me.previous_btn = New System.Windows.Forms.Button()
        Me.next_btn = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.SelectDir_btn = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Source_pb = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.Source_pb, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.next_btn)
        Me.Panel1.Controls.Add(Me.previous_btn)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 488)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1160, 43)
        Me.Panel1.TabIndex = 1
        '
        'previous_btn
        '
        Me.previous_btn.Dock = System.Windows.Forms.DockStyle.Left
        Me.previous_btn.Location = New System.Drawing.Point(0, 0)
        Me.previous_btn.Name = "previous_btn"
        Me.previous_btn.Size = New System.Drawing.Size(75, 43)
        Me.previous_btn.TabIndex = 0
        Me.previous_btn.Text = "Назад"
        Me.previous_btn.UseVisualStyleBackColor = True
        '
        'next_btn
        '
        Me.next_btn.Dock = System.Windows.Forms.DockStyle.Left
        Me.next_btn.Location = New System.Drawing.Point(75, 0)
        Me.next_btn.Name = "next_btn"
        Me.next_btn.Size = New System.Drawing.Size(75, 43)
        Me.next_btn.TabIndex = 1
        Me.next_btn.Text = "Вперёд"
        Me.next_btn.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.SelectDir_btn)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1160, 43)
        Me.Panel2.TabIndex = 3
        '
        'SelectDir_btn
        '
        Me.SelectDir_btn.Dock = System.Windows.Forms.DockStyle.Left
        Me.SelectDir_btn.Location = New System.Drawing.Point(0, 0)
        Me.SelectDir_btn.Name = "SelectDir_btn"
        Me.SelectDir_btn.Size = New System.Drawing.Size(110, 43)
        Me.SelectDir_btn.TabIndex = 0
        Me.SelectDir_btn.Text = "Выбрать директорию"
        Me.SelectDir_btn.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 43)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Source_pb)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PictureBox2)
        Me.SplitContainer1.Size = New System.Drawing.Size(1160, 445)
        Me.SplitContainer1.SplitterDistance = 580
        Me.SplitContainer1.TabIndex = 4
        '
        'Source_pb
        '
        Me.Source_pb.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Source_pb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Source_pb.Location = New System.Drawing.Point(0, 0)
        Me.Source_pb.Name = "Source_pb"
        Me.Source_pb.Size = New System.Drawing.Size(580, 445)
        Me.Source_pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Source_pb.TabIndex = 0
        Me.Source_pb.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.PictureBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox2.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(576, 445)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'mainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1160, 531)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "mainForm"
        Me.Text = "AutoCounter"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.Source_pb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents next_btn As System.Windows.Forms.Button
    Friend WithEvents previous_btn As System.Windows.Forms.Button
    Friend WithEvents SelectDir_btn As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Source_pb As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox

End Class
