Imports Bwl.Framework
Imports System.IO
Imports Bwl.Imaging
Imports System.ComponentModel

Public Class AutoCountMethods
    Private _logger As New Logger
    Private _settingsStorageRoot As New SettingsStorageRoot
    Private _files As String()
    Private _sourceBitmap As DisplayBitmap
    Private _bgBitmap As DisplayBitmap
    Private _diffBitmap As DisplayBitmap
    Private _sourceGrayM As GrayMatrix
    Private _bgGrayM As GrayMatrix

    Private _curFrame As UInteger = 1
    Private _coeffBG As Single = 95
    Private _diffThreshold As Byte = 50

    Private _stopFlag As Boolean = False
    Private _diffPercent As Single = 0

    Public ReadOnly Property Logger As Logger
        Get
            Return _logger
        End Get
    End Property

    Public ReadOnly Property Zoom As Double
        Get
            Return CDbl(_settingsStorageRoot.FindSetting("zoom").ValueAsString())
        End Get
    End Property

    Public Property SourceBitmap As DisplayBitmap
        Get
            Return _sourceBitmap
        End Get
        Set(value As DisplayBitmap)
            _sourceBitmap = value
            OnSourceImgChanged(New PropertyChangedEventArgs("SourceImg"))
        End Set
    End Property

    Public Property StopFlag As Boolean
        Get
            Return _stopFlag
        End Get
        Set(value As Boolean)
            _stopFlag = value
        End Set
    End Property

    Private Sub OnSourceImgChanged(ByVal e As EventArgs)
        Dim handler As EventHandler = sourceImgChangedEvent
        If handler IsNot Nothing Then
            handler(Me, e)
        End If
    End Sub
    Public Event sourceImgChanged As EventHandler

    Public Property bgBitmap As DisplayBitmap
        Get
            Return _bgBitmap
        End Get
        Set(value As DisplayBitmap)
            _bgBitmap = value
            OnBGImgChanged(New PropertyChangedEventArgs("BGImg"))
        End Set
    End Property


    Private Sub OnBGImgChanged(ByVal e As EventArgs)
        Dim handler As EventHandler = bgImgChangedEvent
        If handler IsNot Nothing Then
            handler(Me, e)
        End If
    End Sub
    Public Event bgImgChanged As EventHandler

    Public Property DiffBitmap As DisplayBitmap
        Get
            Return _diffBitmap
        End Get
        Set(value As DisplayBitmap)
            _diffBitmap = value
            OnDiffImgChanged(New PropertyChangedEventArgs("DiffImg"))
        End Set
    End Property


    Private Sub OnDiffImgChanged(ByVal e As EventArgs)
        Dim handler As EventHandler = diffImgChangedEvent
        If handler IsNot Nothing Then
            handler(Me, e)
        End If
    End Sub
    Public Event diffImgChanged As EventHandler

    Public Sub showSettings()
        _settingsStorageRoot.ShowSettingsForm(Nothing)
    End Sub

    Public Sub New()
        _settingsStorageRoot.DefaultWriter = New IniFileSettingsWriter("parameters.ini")
        '_settingsStorageRoot.CreateDoubleSetting("zoom", 0.5)
        _settingsStorageRoot.CreateStringSetting("defaultDirectory", Application.StartupPath + "\18_08_2015_13_50_58_2__", "Директория по умолчанию", "Директория, из которой загружаются изображения")
        _settingsStorageRoot.CreateIntegerSetting("coeffBG", 95, "Процент фона", "Процент от старого фона, который берётся для формирования обновлённого фона")
        _settingsStorageRoot.CreateIntegerSetting("diffThreshold", 50, "Порог разницы", "Порог разницы пикселей, при превышении которого новый пиксель устанавливается белым")
        _files = Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg")
    End Sub

    Public Sub prevFrame()

        If (_curFrame = 0) Then Return
        _curFrame -= 1
        'SourceBitmap = New DisplayBitmap(New Bitmap(_files(_curFrame)))

        Dim source As Bitmap
        Try
            source = New Bitmap(_files(_curFrame))
        Catch
            _files = Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg")
            _curFrame = 0
            If (_curFrame = (_files.Length - 1)) Then Return
            source = New Bitmap(_files(_curFrame))
        End Try
        _sourceGrayM = BitmapConverter.BitmapToGrayMatrix(source)
        _bgGrayM = _sourceGrayM
        'Dim q As GrayMatrix = grayM.ResizeHalf()
        Dim tmpSourceBitmap = New DisplayBitmap(_sourceGrayM.ToRGBMatrix.ToBitmap())
        'tmpSourceBitmap.Resize(CInt(tmpSourceBitmap.Width * Zoom), CInt(tmpSourceBitmap.Height * Zoom))
        SourceBitmap = tmpSourceBitmap
        bgBitmap = tmpSourceBitmap
    End Sub

    Public Sub nextFrame()
        If (_curFrame = (_files.Length - 1)) Then Return
        _curFrame += 1
        Dim source As Bitmap
        Try
            source = New Bitmap(_files(_curFrame))
        Catch
            _files = Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg")
            _curFrame = 0
            If (_curFrame = (_files.Length - 1)) Then Return
            source = New Bitmap(_files(_curFrame))
        End Try
        _sourceGrayM = BitmapConverter.BitmapToGrayMatrix(source)
        SourceBitmap = New DisplayBitmap(_sourceGrayM.ToRGBMatrix.ToBitmap)
        Dim sw = New Stopwatch()
        sw.Start()

        refreshBG()
        calcDifference(_sourceGrayM.Width \ 3, _sourceGrayM.Height \ 3, _sourceGrayM.Width, _sourceGrayM.Height)
        Logger.AddInformation("кадр обработан за " + sw.Elapsed.ToString() + "           процент отличий " + Math.Round(100 * _diffPercent, 3).ToString())
        sw.Stop()
        'SourceBitmap.Resize(CInt(SourceBitmap.Width * Zoom), CInt(SourceBitmap.Height * Zoom))
    End Sub

    Public Sub refreshBG()
        If IsNothing(SourceBitmap) Then Return
        If IsNothing(bgBitmap) Then bgBitmap = SourceBitmap
        If IsNothing(_bgGrayM) Then _bgGrayM = BitmapConverter.BitmapToGrayMatrix(bgBitmap.Bitmap)

        'Logger.AddInformation("Старт обновления пикселей фона...")
        _coeffBG = CInt(_settingsStorageRoot.FindSetting("coeffBG").ValueAsString())
        For y As Integer = 0 To _bgGrayM.Height - 1
            For x As Integer = 0 To _bgGrayM.Width - 1
                Dim newVal = CInt(_bgGrayM.Gray(x, y) * _coeffBG / 100 + _sourceGrayM.Gray(x, y) * (1 - _coeffBG / 100))
                _bgGrayM.Gray(x, y) = CByte(Math.Min(Math.Max(newVal, Byte.MinValue), Byte.MaxValue))
            Next x
        Next y

        bgBitmap = New DisplayBitmap(_bgGrayM.ToRGBMatrix.ToBitmap())
        'Logger.AddInformation("Фон обновлён")
    End Sub

    Public Sub start()
        _files = Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg")
        For i As Integer = _curFrame To _files.Length
            If (StopFlag) Then
                i -= 1
                Continue For
            End If
            nextFrame()
        Next i
    End Sub

    Public Sub break()
        _curFrame = 1
        prevFrame()
    End Sub

    Public Sub calcDifference(start_x As Integer, start_y As Integer, width As Integer, height As Integer)
        Dim diffGrayM As GrayMatrix = _sourceGrayM
        Dim bgGray = _bgGrayM.Gray
        start_x = Math.Max(start_x, 0)
        start_y = Math.Max(start_y, 0)
        width = Math.Min(diffGrayM.Width - 1, start_x + width - 1)
        height = Math.Min(diffGrayM.Height - 1, start_y + height - 1)

        _diffThreshold = CInt(_settingsStorageRoot.FindSetting("diffThreshold").ValueAsString())
        _diffPercent = 0

        For y As Integer = start_y To height
            For x As Integer = start_x To width
                Dim newVal = CByte(Math.Abs(CInt(diffGrayM.Gray(x, y)) - CInt(bgGray(x, y))))
                diffGrayM.Gray(x, y) = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)
                _diffPercent += IIf(newVal > _diffThreshold, 1, 0)
            Next x
        Next y

        _diffPercent /= (width - start_x + 1) * (height - start_y + 1)
        DiffBitmap = New DisplayBitmap(diffGrayM.ToRGBMatrix.ToBitmap())
    End Sub

End Class
