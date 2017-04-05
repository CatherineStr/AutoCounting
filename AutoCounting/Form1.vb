Imports System.IO

Public Class mainForm

    Private _currentDir As String = Application.StartupPath + "\18_08_2015_13_50_58_2__"
    Private _files As String()
    Private _curFrame As UInteger = 0

    Private Sub mainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _files = Directory.GetFiles(_currentDir, "*.jpg")
        Source_pb.Image = New Bitmap(_files(_curFrame))
    End Sub

    Private Sub previous_btn_Click(sender As Object, e As EventArgs) Handles previous_btn.Click
        If (_curFrame = 0) Then Return
        _curFrame -= 1
        Source_pb.Image = New Bitmap(_files(_curFrame))
        Source_pb.Refresh()
    End Sub

    Private Sub next_btn_Click(sender As Object, e As EventArgs) Handles next_btn.Click
        If (_curFrame = (_files.Length - 1)) Then Return
        _curFrame+=1
        Source_pb.Image = New Bitmap(_files(_curFrame))
        Source_pb.Refresh()
    End Sub
End Class
