﻿Imports Bwl.Framework
Imports System.IO
Imports Bwl.Imaging
Imports System.ComponentModel

Public Class AutoCountMethods
    Private _logger As New Logger
    Private _settingsStorageRoot As New SettingsStorageRoot
    Private _defaultDir As String
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
    Private _minDiff As Single = 0
    Private _maxDiff As Single = 1
    Private _edgeThreshold

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

    Public Property defaultDir As String
        Get
            Return _defaultDir
        End Get
        Set(value As String)
            _defaultDir = value
        End Set
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
        '_settingsStorageRoot.CreateStringSetting("defaultDirectory", Application.StartupPath + "\18_08_2015_13_50_58_2__", "Директория по умолчанию", "Директория, из которой загружаются изображения")
        '_settingsStorageRoot.CreateIntegerSetting("coeffBG", 95, "Процент фона", "Процент от старого фона, который берётся для формирования обновлённого фона")
        _settingsStorageRoot.CreateIntegerSetting("diffThreshold", 50, "Порог разницы", "Порог разницы пикселей, при превышении которого новый пиксель устанавливается белым")
        _settingsStorageRoot.CreateDoubleSetting("fastSpeedChange", 0.05, "Скорость быстрого изменения фона", "Коэффициент, на который умножается каждый пиксель нового кадра для быстрого обновления фона")
        _settingsStorageRoot.CreateDoubleSetting("slowSpeedChange", 0.005, "Скорость медленного изменения фона", "Коэффициент, на который умножается каждый пиксель нового кадра для медленного обновления фона")
        _settingsStorageRoot.CreateIntegerSetting("generalizationRange", 20, "Степень генерализации", "Длина стороны квадратных блоков, которые выделяются на изображении при генерализации")
        _settingsStorageRoot.CreateIntegerSetting("edgeThreshold", 128, "Порог контуров изображения", "(0-255) Применяется при обнаружении границ для бинаризации изображения, полученного при применении оператора Собеля.")
        '_files = Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg") 'Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg")
    End Sub

    Public Sub getFirstFrame()
        _curFrame = 0
        _numberCars = 0

        _files = Directory.GetFiles(_defaultDir, "*.jpg")
        Dim source As Bitmap
        Try
            source = New Bitmap(_files(_curFrame))
        Catch
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

    Public Sub nextFrame(mode As Integer)
        If (_curFrame = (_files.Length - 1)) Then Return
        _curFrame += 1
        Dim source As Bitmap
        Try
            source = New Bitmap(_files(_curFrame))
        Catch
            _files = Directory.GetFiles(_defaultDir, "*.jpg")
            _curFrame = 0
            If (_curFrame = (_files.Length - 1)) Then Return
            source = New Bitmap(_files(_curFrame))
        End Try
        _sourceGrayM = BitmapConverter.BitmapToGrayMatrix(source)
        _sourceGrayM = New GrayMatrix(_sourceGrayM.ResizeMatrixHalf(_sourceGrayM.Gray))
        SourceBitmap = New DisplayBitmap(_sourceGrayM.ToRGBMatrix.ToBitmap)
        Dim sw = New Stopwatch()
        '_coeffBG = CInt(_settingsStorageRoot.FindSetting("coeffBG").ValueAsString())
        _diffThreshold = CInt(_settingsStorageRoot.FindSetting("diffThreshold").ValueAsString())
        _fastChange = CSng(_settingsStorageRoot.FindSetting("fastSpeedChange").ValueAsString())
        _slowChange = CSng(_settingsStorageRoot.FindSetting("slowSpeedChange").ValueAsString())
        _generalizationRange = CInt(_settingsStorageRoot.FindSetting("generalizationRange").ValueAsString())
        _edgeThreshold = CInt(_settingsStorageRoot.FindSetting("edgeThreshold").ValueAsString())
        sw.Start()
        Select Case mode
            Case 0
                handleFrame()
            Case 1
                handleFrameEdges()
            Case 2
                handleFrameGener()
        End Select


        sw.Stop()
        Dim approaching As String = ""
        If _doesCarApproach Then
            approaching = "   приближается автомобиль "
        End If
        Logger.AddInformation("кадр " + _curFrame.ToString() + " обработан за " + sw.Elapsed.ToString() + "   процент отличий " + Math.Round(100 * _diffPercent, 3).ToString() + "   maxDiff " + Math.Round(_maxDiff * 100, 3).ToString() + "   minDiff " + Math.Round(_minDiff * 100, 3).ToString() + "    количество авто " + _numberCars.ToString() + approaching)
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

    Public Sub start(mode As Integer)
        '_files = Directory.GetFiles(_settingsStorageRoot.FindSetting("defaultDirectory").ValueAsString(), "*.jpg")

        getFirstFrame()
        For i As Integer = _curFrame To _files.Length
            If (StopFlag) Then
                i -= 1
                Continue For
            End If
            nextFrame(mode)
        Next i
    End Sub

    Public Sub break()
        getFirstFrame()
    End Sub

    Private _fastChange = 0.05
    Private _slowChange = 0.005
    'Private _diffThresholdVariant As Byte = 127
    Private _numbFramesForBGLearning = CInt(_fastChange / _slowChange)
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
                Dim bgPixVal = _bgGrayM.Gray(x, y)
                Dim sPixVal = _sourceGrayM.Gray(x, y)
                'Dim newVal = CInt(bgPixVal * _coeffBG / 100 + sPixVal * (1 - _coeffBG / 100))
                Dim newVal = 0
                If (Math.Abs(CInt(sPixVal) - CInt(bgPixVal)) > _diffThreshold) And
                    (_numbFramesForBGLearning = 0) Then
                    newVal = CInt(bgPixVal * (1 - _slowChange) + sPixVal * _slowChange)
                Else
                    newVal = CInt(bgPixVal * (1 - _fastChange) + sPixVal * _fastChange)
                    If (_numbFramesForBGLearning > 0) Then _numbFramesForBGLearning -= 1
                End If
                _bgGrayM.Gray(x, y) = CByte(Math.Min(Math.Max(newVal, Byte.MinValue), Byte.MaxValue))

                If x >= StartX And x <= endX And y >= StartY And y <= endY Then
                    newVal = Math.Abs(CInt(sPixVal) - CInt(bgPixVal))
                    diffGrayM.Gray(x, y) = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)
                    _diffPercent += IIf(newVal > _diffThreshold, 1, 0)
                End If
            Next x
        Next y

        _diffPercent /= (AreaWidth * AreaHeight)
        If _diffPercent > (_maxDiff - _minDiff) / 2 Then
            _maxDiff = _diffPercent * _fastChange + _maxDiff * (1 - _fastChange)
            _minDiff = _diffPercent * _slowChange + _minDiff * (1 - _slowChange)
        Else
            _maxDiff = _diffPercent * _slowChange + _maxDiff * (1 - _slowChange)
            _minDiff = _diffPercent * _fastChange + _minDiff * (1 - _fastChange)
        End If
        If _diffPercent > (_maxDiff - _minDiff) / 2 Then
            If _doesCarApproach Then
            Else
                _doesCarApproach = True
            End If
        Else
            If _doesCarApproach Then
                _doesCarApproach = False
                _numberCars += 1
            End If
        End If
        bgBitmap = New DisplayBitmap(_bgGrayM.ToRGBMatrix.ToBitmap())
        DiffBitmap = New DisplayBitmap(diffGrayM.ToRGBMatrix.ToBitmap())
    End Sub

    Public Sub handleFrameEdges()
        If IsNothing(SourceBitmap) Then Return
        If IsNothing(bgBitmap) Then bgBitmap = SourceBitmap
        If IsNothing(_bgGrayM) Then _bgGrayM = _sourceGrayM
        Dim diffGrayM As GrayMatrix = _sourceGrayM

        Dim sourceGrM = _sourceGrayM.Gray
        Dim edges = detectEdges(sourceGrM)
        Dim edgeGrayM = New GrayMatrix(edges)

        'Dim bgGray = _bgGrayM.Gray
        'StartX = Math.Max(StartX, 0)
        'StartY = Math.Max(StartY, 0)
        'Dim endX = Math.Min(diffGrayM.Width - 1, StartX + AreaWidth - 1)
        'Dim endY = Math.Min(diffGrayM.Height - 1, StartY + AreaHeight - 1)

        'Dim oldDiffPercent = _diffPercent
        '_diffPercent = 0

        'For y As Integer = 0 To diffGrayM.Height - 1
        '    For x As Integer = 0 To diffGrayM.Width - 1
        '        Dim bgPixVal = _bgGrayM.Gray(x, y)
        '        Dim sPixVal = _sourceGrayM.Gray(x, y)
        '        'Dim newVal = CInt(bgPixVal * _coeffBG / 100 + sPixVal * (1 - _coeffBG / 100))
        '        Dim newVal = 0
        '        'IIf(Math.Abs(CInt(sPixVal) - CInt(bgPixVal)) > (_maxDiff - _minDiff) / 2,
        '        '    newVal = CInt(bgPixVal * (1 - _slowChange) + sPixVal * _slowChange),
        '        '    newVal = CInt(bgPixVal * (1 - _fastChange) + sP(_maxDiff - _minDiff) / 2ixVal * _fastChange))
        '        If (Math.Abs(CInt(sPixVal) - CInt(bgPixVal)) > _diffThreshold) And
        '            (_numbFramesForBGLearning = 0) Then
        '            newVal = CInt(bgPixVal * (1 - _slowChange) + sPixVal * _slowChange)
        '        Else
        '            newVal = CInt(bgPixVal * (1 - _fastChange) + sPixVal * _fastChange)
        '            If (_numbFramesForBGLearning > 0) Then _numbFramesForBGLearning -= 1
        '        End If
        '        _bgGrayM.Gray(x, y) = CByte(Math.Min(Math.Max(newVal, Byte.MinValue), Byte.MaxValue))

        '        'If x >= StartX And x <= endX And y >= StartY And y <= endY Then
        '        '    newVal = Math.Abs(CInt(sPixVal) - CInt(bgPixVal))
        '        '    diffGrayM.Gray(x, y) = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)
        '        '    _diffPercent += IIf(newVal > _diffThreshold, 1, 0)
        '        'End If
        '    Next x
        'Next y

        '_diffPercent /= (AreaWidth * AreaHeight)
        'If _diffPercent > (_maxDiff - _minDiff) / 2 Then
        '    _maxDiff = _diffPercent * _fastChange + _maxDiff * (1 - _fastChange)
        '    _minDiff = _diffPercent * _slowChange + _minDiff * (1 - _slowChange)
        'Else
        '    _maxDiff = _diffPercent * _slowChange + _maxDiff * (1 - _slowChange)
        '    _minDiff = _diffPercent * _fastChange + _minDiff * (1 - _fastChange)
        'End If
        'If _diffPercent > (_maxDiff - _minDiff) / 2 Then
        '    If _doesCarApproach Then
        '    Else
        '        _doesCarApproach = True
        '    End If
        'Else
        '    If _doesCarApproach Then
        '        _doesCarApproach = False
        '        _numberCars += 1
        '    End If
        'End If
        'bgBitmap = New DisplayBitmap(_bgGrayM.ToRGBMatrix.ToBitmap())
        'DiffBitmap = New DisplayBitmap(diffGrayM.ToRGBMatrix.ToBitmap())
        DiffBitmap = New DisplayBitmap(edgeGrayM.ToRGBMatrix.ToBitmap())
    End Sub

    Public Function detectEdges(imageMatrix As Byte(,)) As Byte(,)
        Dim grad1 = New Single(2, 2) {{-1, -2, -1}, {0, 0, 0}, {1, 2, 1}}
        Dim grad2 = New Single(2, 2) {{-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1}}
        'Dim gradX = applyConvolutionMatrix(imageMatrix, grad1)
        'Dim gradY = applyConvolutionMatrix(imageMatrix, grad2)
        Dim pixAreaWidth = 1

        Dim result As Byte(,) = New Byte(imageMatrix.GetLength(0) - 1, imageMatrix.GetLength(1) - 1) {}
        Dim len0 = imageMatrix.GetLength(0)
        Dim len1 = imageMatrix.GetLength(1)
        For y As Integer = 0 To imageMatrix.GetLength(1) - 1
            For x As Integer = 0 To imageMatrix.GetLength(0) - 1
                Dim newPix As Single = 0, gradX As Single = 0, gradY As Single = 0

                For _y As Integer = Math.Max(0, y - pixAreaWidth) To Math.Min(y + pixAreaWidth, imageMatrix.GetLength(1) - 1)
                    For _x As Integer = Math.Max(0, x - pixAreaWidth) To Math.Min(x + pixAreaWidth, imageMatrix.GetLength(0) - 1)
                        gradX += grad1(x - _x + pixAreaWidth, y - _y + pixAreaWidth) * imageMatrix(_x, _y)
                        gradY += grad2(x - _x + pixAreaWidth, y - _y + pixAreaWidth) * imageMatrix(_x, _y)
                    Next _x
                Next _y
                newPix = Math.Sqrt(Math.Pow(gradX, 2) + Math.Pow(gradY, 2))
                If newPix < _edgeThreshold Then
                    newPix = 0
                Else
                    newPix = Byte.MaxValue
                End If
                result(x, y) = CByte(Math.Max(Math.Min(newPix, CSng(Byte.MaxValue)), CSng(Byte.MinValue)))

            Next x
        Next y
        Return result
    End Function

    Private _generalizationRange As Integer = 30
    Public Sub handleFrameGener()
        If IsNothing(SourceBitmap) Then Return
        If IsNothing(bgBitmap) Then bgBitmap = SourceBitmap
        If IsNothing(_bgGrayM) Then _bgGrayM = _sourceGrayM
        Dim diffGrayM As GrayMatrix = _sourceGrayM
        Dim generDiffGrayM As Byte(,) = New Byte(diffGrayM.Width - 1, diffGrayM.Height - 1) {}

        Dim bgGray = _bgGrayM.Gray
        StartX = Math.Max(StartX, 0)
        StartY = Math.Max(StartY, 0)
        Dim endX = Math.Min(diffGrayM.Width - 1, StartX + AreaWidth - 1)
        Dim endY = Math.Min(diffGrayM.Height - 1, StartY + AreaHeight - 1)

        Dim oldDiffPercent = _diffPercent
        _diffPercent = 0

        For y As Integer = 0 To diffGrayM.Height - 1
            For x As Integer = 0 To diffGrayM.Width - 1
                Dim bgPixVal = _bgGrayM.Gray(x, y)
                Dim sPixVal = _sourceGrayM.Gray(x, y)
                'Dim newVal = CInt(bgPixVal * _coeffBG / 100 + sPixVal * (1 - _coeffBG / 100))
                Dim newVal = 0
                If (Math.Abs(CInt(sPixVal) - CInt(bgPixVal)) > _diffThreshold) And
                    (_numbFramesForBGLearning = 0) Then
                    newVal = CInt(bgPixVal * (1 - _slowChange) + sPixVal * _slowChange)
                Else
                    newVal = CInt(bgPixVal * (1 - _fastChange) + sPixVal * _fastChange)
                    If (_numbFramesForBGLearning > 0) Then _numbFramesForBGLearning -= 1
                End If
                _bgGrayM.Gray(x, y) = CByte(Math.Min(Math.Max(newVal, Byte.MinValue), Byte.MaxValue))

                newVal = Math.Abs(CInt(sPixVal) - CInt(bgPixVal))
                newVal = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)
                diffGrayM.Gray(x, y) = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)

                If (newVal = Byte.MaxValue) And
                    (y - _generalizationRange >= 0) And
                    (x - _generalizationRange >= 0) Then
                    Dim isAreaFullWhite = True
                    For _y As Integer = y - _generalizationRange To y
                        For _x As Integer = x - _generalizationRange To x
                            If diffGrayM.Gray(_x, _y) = 0 Then
                                isAreaFullWhite = False
                                Exit For
                            End If
                        Next _x
                        If Not isAreaFullWhite Then
                            Exit For
                        End If
                    Next _y
                    If isAreaFullWhite Then
                        For _y As Integer = y - _generalizationRange To y
                            For _x As Integer = x - _generalizationRange To x
                                If Not (generDiffGrayM(_x, _y) = Byte.MaxValue) Then
                                    generDiffGrayM(_x, _y) = Byte.MaxValue
                                End If
                            Next _x
                        Next _y
                    End If
                End If

                'If x >= StartX And x <= endX And y >= StartY And y <= endY Then
                '    newVal = Math.Abs(CInt(sPixVal) - CInt(bgPixVal))
                '    diffGrayM.Gray(x, y) = IIf(newVal > _diffThreshold, Byte.MaxValue, Byte.MinValue)
                '    _diffPercent += IIf(newVal > _diffThreshold, 1, 0)
                'End If
            Next x
        Next y

        _diffPercent /= (AreaWidth * AreaHeight)
        If _diffPercent > (_maxDiff - _minDiff) / 2 Then
            _maxDiff = _diffPercent * _fastChange + _maxDiff * (1 - _fastChange)
            _minDiff = _diffPercent * _slowChange + _minDiff * (1 - _slowChange)
        Else
            _maxDiff = _diffPercent * _slowChange + _maxDiff * (1 - _slowChange)
            _minDiff = _diffPercent * _fastChange + _minDiff * (1 - _fastChange)
        End If
        bgBitmap = New DisplayBitmap(_bgGrayM.ToRGBMatrix.ToBitmap())
        'DiffBitmap = New DisplayBitmap(diffGrayM.ToRGBMatrix.ToBitmap())
        DiffBitmap = New DisplayBitmap((New GrayMatrix(generDiffGrayM)).ToRGBMatrix.ToBitmap())
    End Sub
End Class
