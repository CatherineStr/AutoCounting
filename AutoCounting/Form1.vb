Imports System.IO
Imports Bwl.Framework
Imports System.Threading


Public Class mainForm
    Dim lform As LoggerForm
    Private _autoCountMethods As AutoCountMethods
    Private thread As Thread

    Private Sub mainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '_files = Directory.GetFiles(_currentDir, "*.jpg")
        'Source_pb.Image = New Bitmap(_files(_curFrame))

        _autoCountMethods = New AutoCountMethods()
        AddHandler _autoCountMethods.sourceImgChanged, AddressOf sourceImgChanged
        AddHandler _autoCountMethods.bgImgChanged, AddressOf bgImgChanged
        AddHandler _autoCountMethods.diffImgChanged, AddressOf diffImgChanged
        lform = New LoggerForm(_autoCountMethods.Logger)
        lform.MdiParent = Me
        Me.Panel1.Controls.Add(Me.lform)
        lform.Dock = DockStyle.Fill
        'lform.Height = 200
        lform.FormBorderStyle = FormBorderStyle.None
        lform.Show()

        _autoCountMethods.prevFrame()

    End Sub

    Private Sub next_btn_Click(sender As Object, e As EventArgs)
        stopCalculations()
        thread = New Thread(AddressOf _autoCountMethods.nextFrame)
        thread.Start()
        'If (_curFrame = (_files.Length - 1)) Then Return
        '_curFrame+=1
        'Source_pb.Image = New Bitmap(_files(_curFrame))
        'Source_pb.Refresh()
    End Sub


    Private Sub sourceImgChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.Source_pb.Image = _autoCountMethods.SourceBitmap.Bitmap
    End Sub

    Private Sub bgImgChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.PictureBox2.Image = _autoCountMethods.bgBitmap.Bitmap
    End Sub

    Private Sub diffImgChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.difference_pb.Image = _autoCountMethods.DiffBitmap.Bitmap
    End Sub

    Private Sub start_btn_Click(sender As Object, e As EventArgs) Handles start_btn.Click
        If _autoCountMethods.StopFlag Then
            _autoCountMethods.StopFlag = False
            Return
        End If
        If Not stopCalculations() Then Return
        _autoCountMethods.StopFlag = False
        thread = New Thread(AddressOf _autoCountMethods.start)
        thread.Start()
    End Sub

    Public Function stopCalculations() As Boolean
        If thread Is Nothing Then
            Return True
        End If
        If thread.ThreadState <> System.Diagnostics.ThreadState.Terminated And thread.ThreadState <> ThreadState.Aborted And thread.ThreadState <> ThreadState.Stopped And thread.ThreadState <> ThreadState.Suspended Then
            thread.Suspend()
            If MessageBox.Show("Ранее запущенные вычисления не были завершены. Прервать текущие вычисления?", "", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                thread.Resume()
                Return False
            End If
            thread.Resume()
            thread.Abort()
        ElseIf thread.ThreadState = ThreadState.Suspended Then
            thread.Resume()
        End If
        Return True
    End Function

    Private Sub break_btn_Click(sender As Object, e As EventArgs) Handles break_btn.Click
        stopCalculations()
        _autoCountMethods.break()
    End Sub

    Private Sub Stop_btn_Click(sender As Object, e As EventArgs) Handles Stop_btn.Click
        'If thread Is Nothing Then Return
        'If thread.ThreadState <> System.Diagnostics.ThreadState.Terminated And thread.ThreadState <> ThreadState.Aborted And thread.ThreadState <> ThreadState.Stopped And thread.ThreadState <> ThreadState.Suspended Then
        '    thread.Suspend()
        'End If
        _autoCountMethods.StopFlag = True
    End Sub

    Private Sub settings_btn_Click(sender As Object, e As EventArgs) Handles settings_btn.Click
        If Not stopCalculations() Then Return
        _autoCountMethods.showSettings()
    End Sub
End Class
