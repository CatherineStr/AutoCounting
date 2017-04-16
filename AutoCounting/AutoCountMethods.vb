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

    'manual initializing variables and constants 
    Private _coeffBG As Single = 95                 'Процент от старого фона, который берётся для формирования обновлённого фона
    Private _diffThreshold As Byte = 50             'Порог разницы пикселей, при превышении которого новый пиксель устанавливается белым
    'Private _skipFrames As Integer = 0              'количество фреймов, которые надо пропустить после инкремента машин
    Private _diffPercThreshold As Single = 0.5      'порог в диапазоне процентов разницы, при превышении которого производится инкремент количества машин
    'Private _minDiff As Single = Single.MaxValue
    Private _fifoSize As Integer = 10               'размер fifo-буфера, в котором хранятся последние значения процентов разницы, при которых был произведён инкремент машин

    Private _maxDiffs As Queue(Of Single)
    Private _avgMaxDiff As Single = Single.MinValue

    Private _curFrame As UInteger = 0
    Private _numberCars As Integer = 0
    Private _diffPercent As Single = 0
    Private _stopFlag As Boolean = False
    Private _doesCarApproach As Boolean = False
    Private _startX As Integer = 0
    Private _startY As Integer = 0
    Private _areaWidth As Integer = 0
    Private _areaHeight As Integer = 0


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

    Public Property StartX As Integer
        Get
            Return _startX
        End Get
        Set(value As Integer)
            _startX = value
        End Set
    End Property

    Public Property StartY As Integer
        Get
            Return _startY
        End Get
        Set(value As Integer)
            _startY = value
        End Set
    End Property

    Public Property AreaWidth As Integer
        Get
            Return _areaWidth
        End Get
        Set(value As Integer)
            _areaWidth = value
        End Set
    End Property

    Public Property AreaHeight As Integer
        Get
            Return _areaHeight
        End Get
        Set(value As Integer)
            _areaHeight = value
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
        _maxDiffs = New Queue(Of Single)()
    End Sub

    Public Sub getFirstFrame()
        _curFrame = 0
        _numberCars = 0
        _avgMaxDiff = Single.MinValue
        _maxDiffs.Clear()

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
        _sourceGrayM = New GrayMatrix(_sourceGrayM.ResizeMatrixHalf(_sourceGrayM.Gray))
        _bgGrayM = _sourceGrayM
        Dim tmpSourceBitmap = New DisplayBitmap(_sourceGrayM.ToRGBMatrix.ToBitmap())
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
        _sourceGrayM = New GrayMatrix(_sourceGrayM.ResizeMatrixHalf(_sourceGrayM.Gray))
        SourceBitmap = New DisplayBitmap(_sourceGrayM.ToRGBMatrix.ToBitmap)
        Dim sw = New Stopwatch()
        _coeffBG = CInt(_settingsStorageRoot.FindSetting("coeffBG").ValueAsString())
        _diffThreshold = CInt(_settingsStorageRoot.FindSetting("diffThreshold").ValueAsString())
        sw.Start()

        
        handleFrame()
        sw.Stop()
        Dim approaching As String = ""
        If _doesCarApproach Then
            approaching = "   приближается автомобиль "
        End If
        Logger.AddInformation("кадр обработан за " + sw.Elapsed.ToString() + "   процент отличий " + Math.Round(100 * _diffPercent, 3).ToString() + "   средняя максимальная разница " + Math.Round(_avgMaxDiff * 100, 3).ToString() + "    количество авто " + _numberCars.ToString() + approaching)
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
        getFirstFrame()
    End Sub

    Public Sub handleFrame()
        If IsNothing(SourceBitmap) Then Return
        If IsNothing(bgBitmap) Then bgBitmap = SourceBitmap
        If IsNothing(_bgGrayM) Then _bgGrayM = _sourceGrayM
        Dim diffGrayM As GrayMatrix = _sourceGrayM

        Dim bgGray = _bgGrayM.Gray
        StartX = Math.Max(StartX, 0)
        StartY = Math.Max(StartY, 0)
        Dim endX = Math.Min(diffGrayM.Width - 1, StartX + AreaWidth - 1)
        Dim endY = Math.Min(diffGrayM.Height - 1, StartY + AreaHeight - 1)

        Dim oldDiffPercent = _diffPercent
        _diffPercent = 0

        For y As Integer = 0 To diffGrayM.Height - 1
            For x As Integer = 0 To diffGrayM.Width - 1

                Dim newVal = CInt(_bgGrayM.Gray(x, y) * _coeffBG / 100 + _sourceGrayM.Gray(x, y) * (1 - _coeffBG / 100))
                _bgGrayM.Gray(x, y) = CByte(Math.Min(Math.Max(newVal, Byte.MinValue), Byte.MaxValue))

                If x >= StartX And x <= endX And y >= StartY And y <= endY Then
                    newVal = Math.Abs(CInt(diffGrayM.Gray(x, y)) - CInt(bgGray(x, y)))
                    diffGrayM.Gray(x, y) = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)
                    _diffPercent += IIf(newVal > _diffThreshold, 1, 0)
                End If
            Next x
        Next y

        _diffPercent /= (AreaWidth * AreaHeight)
        If _diffPercent > _avgMaxDiff * _diffPercThreshold Then
            If _doesCarApproach Then
                If Math.Round(oldDiffPercent + (oldDiffPercent - _avgMaxDiff) * 100 * 2 / 3) > Math.Round(_diffPercent * 100) Then
                    _doesCarApproach = False
                    _numberCars += 1
                    _maxDiffs.Enqueue(_diffPercent)
                    '_maxDiffs.TrimToSize()
                    If (_maxDiffs.Count > _fifoSize) Then
                        _maxDiffs.Dequeue()
                    End If
                    _avgMaxDiff = _maxDiffs.Average()
                End If
            Else
                _doesCarApproach = True
            End If
        Else
            If _doesCarApproach Then
                _doesCarApproach = False
                _numberCars += 1
                _maxDiffs.Enqueue(_diffPercent)
                If (_maxDiffs.Count > _fifoSize) Then
                    _maxDiffs.Dequeue()
                End If
                _avgMaxDiff = _maxDiffs.Average()
            End If
        End If
        bgBitmap = New DisplayBitmap(_bgGrayM.ToRGBMatrix.ToBitmap())
        DiffBitmap = New DisplayBitmap(diffGrayM.ToRGBMatrix.ToBitmap())
    End Sub

End Class
