using OfficeOpenXml;
using MAC.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using MAC.Models.Value;
using MAC.ViewModels;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services;
using MAC.ViewModels.Services.SerialPort;

namespace MAC.Models
{
    public class ScResultItem : BaseVm
    {
        private readonly FlukeSerialPort _fluke;
        private readonly CommutatorSerialPort _comm;
        private readonly ScSerialPort _sc;
        private readonly int _numberSc;
        private readonly string _startTestStringDateTime;
        private readonly MainSettingsModel _mainSettingsModel;
        private readonly ActiveSettingsModel _activeSettingsModel;
        private MeasurementsData _measurementsData;

        public Action SetIsWaitCancelFalse;


        private readonly Dictionary<string, string> _channelName = new Dictionary<string, string>
        {
            {"CH0", "t возд"},
            {"CH1", "t дор"},
            {"CH2", "t п/п"},
            {"CH3", "Влаж"},
            {"CH5", "ДНВ"},
            {"CH6", "ДВС"}
        };


        /// <summary>
        /// Примерное время для считывания одного значения
        /// Необходимо для расчета времени при использовании среднего значения.
        /// Которое снимает несколько значение за одно тестирование на выбранных характеристиках Fluke.
        /// </summary>
        const int TimeGetValue = 10;

        /// <summary>
        /// Время калибровки на каждую точку при калибровке новых версий кс в секундах.
        /// </summary>
        private const int TimeOutCalibrationNewSc = 60;

        /// <summary>
        /// Имя заданное лаборантом
        /// </summary>
        public string NameSc { get; set; }

        /// <summary>
        /// Считываемая версия Контролера
        /// </summary>
        public Version VersionSc { get; set; }


        /// <summary>
        /// Свойство для отслеживания действия при отмене задачи теста.
        /// </summary>
        public TypeCancelTask TypeCancelTask { get; set; }

        public Channel CurrentChannel { get; set; } = Channel.None;

        public ObservableCollection<IScValue> Ch0 { get; set; }
        public ObservableCollection<IScValue> Ch1 { get; set; }
        public ObservableCollection<IScValue> Ch2 { get; set; }
        public ObservableCollection<IScValue> Ch3 { get; set; }
        public ObservableCollection<IScValue> Ch5 { get; set; }
        public ObservableCollection<IScValue> Ch6 { get; set; }

        /// <summary>
        /// Максимальное кол-во измерений для данного теста
        /// </summary>
        public int CountActiveMeasurements { get; set; }

        /// <summary>
        /// Кол-во пройденных измерени
        /// </summary>
        public int CurrentActiveMeasurement { get; set; }

        /// <summary>
        /// Примерное время на весь тест.
        /// </summary>
        public TimeSpan TimeLeftOnAllMeasurements { get; set; }

        /// <summary>
        /// MessageProgressBar состоит из двух частей. Описания текущего действия и примерного времени.
        /// </summary>
        public string CurrentActTest { get; set; }

        /// <summary>
        /// Сообщение высвечивающее действия происходящие в логике.
        /// </summary>
        public string MessageProgressBar => $"{CurrentActTest}  -  Осталось {TimeLeftOnAllMeasurements}";

        public string MessageCurrentAndMaxCountMeasurements =>
            $"{CurrentActiveMeasurement} / {CountActiveMeasurements}";

        /// <summary>
        /// Показатель, проверяется ли сейчас КС
        /// </summary>
        public bool IsCheckedNow { get; set; }


        /// <summary>
        /// Конструкстор при создании итемов КС
        /// </summary>
        /// <param name="fluke"></param>
        /// <param name="comm"></param>
        /// <param name="sc"></param>
        /// <param name="mainSettingsModel"></param>
        /// <param name="activeSettings"></param>
        /// <param name="typeCancelTask"></param>
        /// <param name="setIsWaitCancelFalse"></param>
        public ScResultItem(FlukeSerialPort fluke, CommutatorSerialPort comm, ComConnectItem sc,
            MainSettingsModel mainSettingsModel,
            ActiveSettingsModel activeSettings, TypeCancelTask typeCancelTask, Action setIsWaitCancelFalse)
        {
            SetIsWaitCancelFalse = setIsWaitCancelFalse;
            _activeSettingsModel = activeSettings;
            TypeCancelTask = typeCancelTask;
            _mainSettingsModel = mainSettingsModel;
            _startTestStringDateTime = $"_{DateTime.Now:dd.MM.yyyy_HH-mm-ss}";
            _numberSc = sc.Number;
            NameSc = sc.Name;
            _fluke = fluke;
            _comm = comm;
            _sc = new ScSerialPort(sc);


            #region Заполнение коллекций CH

            Ch0 = new ObservableCollection<IScValue>
            {
                new OhmValueSc(80, activeSettings.Ch0Ohm80),
                new OhmValueSc(90, activeSettings.Ch0Ohm90),
                new OhmValueSc(100, activeSettings.Ch0Ohm100),
                new OhmValueSc(115, activeSettings.Ch0Ohm115),
                new OhmValueSc(130, activeSettings.Ch0Ohm130),
                new OhmValueSc(140, activeSettings.Ch0Ohm140)
            };
            Ch1 = new ObservableCollection<IScValue>
            {
                new OhmValueSc(80, activeSettings.Ch1Ohm80),
                new OhmValueSc(90, activeSettings.Ch1Ohm90),
                new OhmValueSc(100, activeSettings.Ch1Ohm100),
                new OhmValueSc(115, activeSettings.Ch1Ohm115),
                new OhmValueSc(130, activeSettings.Ch1Ohm130),
                new OhmValueSc(140, activeSettings.Ch1Ohm140)
            };
            Ch2 = new ObservableCollection<IScValue>
            {
                new OhmValueSc(80, activeSettings.Ch2Ohm80),
                new OhmValueSc(90, activeSettings.Ch2Ohm90),
                new OhmValueSc(100, activeSettings.Ch2Ohm100),
                new OhmValueSc(115, activeSettings.Ch2Ohm115),
                new OhmValueSc(130, activeSettings.Ch2Ohm130),
                new OhmValueSc(140, activeSettings.Ch2Ohm140)
            };
            Ch3 = new ObservableCollection<IScValue>
            {
                new VoltValueSc(1, activeSettings.Ch3V1),
                new VoltValueSc(2, activeSettings.Ch3V2),
                new VoltValueSc(3, activeSettings.Ch3V3),
                new VoltValueSc(4, activeSettings.Ch3V4),
                new VoltValueSc(5, activeSettings.Ch3V5)
            };
            Ch5 = new ObservableCollection<IScValue>
            {
                new VoltValueSc(1, activeSettings.Ch5V1),
                new VoltValueSc(2, activeSettings.Ch5V2),
                new VoltValueSc(3, activeSettings.Ch5V3),
                new VoltValueSc(4, activeSettings.Ch5V4),
                new VoltValueSc(5, activeSettings.Ch5V5)
            };
            Ch6 = new ObservableCollection<IScValue>
            {
                new HzValueSc(5, activeSettings.Ch6Hz5),
                new HzValueSc(25, activeSettings.Ch6Hz25),
                new HzValueSc(50, activeSettings.Ch6Hz50),
                new HzValueSc(75, activeSettings.Ch6Hz75),
                new HzValueSc(100, activeSettings.Ch6Hz100)
            };

            #endregion

            SetErrorValue();
            SetCountAndTimeMeasurements(activeSettings, mainSettingsModel);
        }

        /// <summary>
        /// Расчет времени тестирования Sc
        /// </summary>
        /// <param name="activeSettings"></param>
        /// <param name="mainSettingsModel"></param>
        private void SetCountAndTimeMeasurements(ActiveSettingsModel activeSettings,
            MainSettingsModel mainSettingsModel)
        {
            var timeOhm = TimeSpan.FromSeconds(mainSettingsModel.TimeOutOhm);
            var timeV = TimeSpan.FromSeconds(mainSettingsModel.TimeOutV);
            var timeHz = TimeSpan.FromSeconds(mainSettingsModel.TimeOutHz);

            TimeLeftOnAllMeasurements = new TimeSpan();
            CurrentActiveMeasurement = 0;
            CountActiveMeasurements = 0;

            void UpdateMeasurements(bool activeMeasurement, TypeMeasurement typeMeasurement)
            {
                if (!activeMeasurement) return;

                CountActiveMeasurements++;
                switch (typeMeasurement)
                {
                    case TypeMeasurement.Ohm:
                        if (_mainSettingsModel.IsUseAverageValue)
                        {
                            TimeLeftOnAllMeasurements +=
                                TimeSpan.FromSeconds(timeOhm.TotalSeconds +
                                                     TimeGetValue * _mainSettingsModel.CountAverageValue);
                        }
                        else
                            TimeLeftOnAllMeasurements +=
                                TimeSpan.FromSeconds(timeOhm.TotalSeconds);

                        break;
                    case TypeMeasurement.V:
                        if (_mainSettingsModel.IsUseAverageValue)
                            TimeLeftOnAllMeasurements +=
                                TimeSpan.FromSeconds(timeV.TotalSeconds +
                                                     TimeGetValue * _mainSettingsModel.CountAverageValue);
                        else
                            TimeLeftOnAllMeasurements += timeV;
                        break;
                    case TypeMeasurement.Hz:
                        if (_mainSettingsModel.IsUseAverageValue)
                            TimeLeftOnAllMeasurements +=
                                TimeSpan.FromSeconds(timeHz.TotalSeconds +
                                                     TimeGetValue * _mainSettingsModel.CountAverageValue);
                        else
                            TimeLeftOnAllMeasurements += timeHz;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(typeMeasurement), typeMeasurement, null);
                }
            }


            CountActiveMeasurements = 0;
            TimeLeftOnAllMeasurements = new TimeSpan();

            UpdateMeasurements(activeSettings.Ch0Ohm80, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm90, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm100, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm115, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm130, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm140, TypeMeasurement.Ohm);

            UpdateMeasurements(activeSettings.Ch1Ohm80, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm90, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm100, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm115, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm130, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm140, TypeMeasurement.Ohm);

            UpdateMeasurements(activeSettings.Ch2Ohm80, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm90, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm100, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm115, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm130, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm140, TypeMeasurement.Ohm);

            UpdateMeasurements(activeSettings.Ch3V1, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch3V2, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch3V3, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch3V4, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch3V5, TypeMeasurement.V);

            UpdateMeasurements(activeSettings.Ch5V1, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch5V2, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch5V3, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch5V4, TypeMeasurement.V);
            UpdateMeasurements(activeSettings.Ch5V5, TypeMeasurement.V);

            UpdateMeasurements(activeSettings.Ch6Hz5, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz25, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz50, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz75, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz100, TypeMeasurement.Hz);
        }


        /// <summary>
        /// Заполняю итемы значений, значениями погрешности.
        /// </summary>
        private void SetErrorValue()
        {
            var channelName = $"Sc{_numberSc}";

            foreach (var itemCh in Ch0)
                itemCh.ErrorValue = (decimal) Settings.Default[$"{channelName}Ch0Error"];

            foreach (var itemCh in Ch1)
                itemCh.ErrorValue = (decimal) Settings.Default[$"{channelName}Ch1Error"];

            foreach (var itemCh in Ch2)
                itemCh.ErrorValue = (decimal) Settings.Default[$"{channelName}Ch2Error"];

            foreach (var itemCh in Ch3)
                itemCh.ErrorValue = (decimal) Settings.Default[$"{channelName}Ch3Error"];

            foreach (var itemCh in Ch5)
                itemCh.ErrorValue = (decimal) Settings.Default[$"{channelName}Ch5Error"];
        }


        #region Const Channel 

        //Для Comm 1,2,3,4,5,6 
        //Для   Sc 0,1,2,3,5,6 соответственно коммутатору

        private const int ChannelCh0 = 1;
        private const int ChannelCh1 = 2;
        private const int ChannelCh2 = 3;
        private const int ChannelCh3 = 4;
        private const int ChannelCh5 = 5;
        private const int ChannelCh6 = 6;

        #endregion

        #region Коллекции значений каналов

        public ObservableCollection<int> ChOhm { get; set; } = new ObservableCollection<int>
        {
            80, 90, 100, 115, 130, 140
        };

        public ObservableCollection<int> ChV { get; set; } = new ObservableCollection<int>
        {
            1, 2, 3, 4, 5
        };

        public ObservableCollection<int> ChHz { get; set; } = new ObservableCollection<int>
        {
            5, 25, 50, 75, 100
        };

        #endregion

        public void MainStartMeasurements(CancellationTokenSource ctSource , MeasurementsData measurementsData)
        {
            _measurementsData = measurementsData;

            _sc.OpenSerialPort();
            _fluke.OpenFlukePort();
            _comm.OpenCommPort();

            var (scVersion, version) = _sc.GetVersionSc();

            VersionSc = version;

            var folderPath = Path.Combine(_mainSettingsModel.FullLogPath, $"{NameSc}{_startTestStringDateTime}");

            switch (scVersion)
            {
                case ScVersion.Old:
                    StartMeasurementsOldVersion(ctSource, folderPath);
                    break;
                case ScVersion.New:
                    StartMeasurementNewVersion(ctSource, folderPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            try
            {
                CreateMeasurement(folderPath);
            }
            catch (Exception e)
            {
                GlobalLog.Log.Debug(e, e.Message);
            }


            #region Fluke Close

            try
            {
                _fluke.Close();
            }
            catch
            {
                //ignore
            }

            #endregion

            #region Comm Close

            try
            {
                _comm.Close();
            }
            catch
            {
                //ignore
            }

            #endregion

            #region SC Close

            try
            {
                _sc.Close();
            }
            catch
            {
                //ignore
            }

            #endregion
        }

        public void StartMeasurementsOldVersion(CancellationTokenSource ctSource, string folderPath)
        {
            IsCheckedNow = true;

            _comm.OnSignalController(_numberSc);

            var ch0IsActiveCollection = Ch0.Select(item => item.IsActive).ToList();
            var ch1IsActiveCollection = Ch1.Select(item => item.IsActive).ToList();
            var ch2IsActiveCollection = Ch2.Select(item => item.IsActive).ToList();

            #region Добавляю время калибровки к общему времени

            if (ch0IsActiveCollection.Contains(true))
            {
                if (_mainSettingsModel.IsUseAverageValue)
                {
                    TimeLeftOnAllMeasurements +=
                        TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm +
                                             TimeGetValue * _mainSettingsModel.CountAverageValue);
                }
                else
                    TimeLeftOnAllMeasurements +=
                        TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm);
            }

            if (ch1IsActiveCollection.Contains(true))
            {
                if (_mainSettingsModel.IsUseAverageValue)
                {
                    TimeLeftOnAllMeasurements +=
                        TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm +
                                             TimeGetValue * _mainSettingsModel.CountAverageValue);
                }
                else
                    TimeLeftOnAllMeasurements +=
                        TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm);
            }

            if (ch2IsActiveCollection.Contains(true))
            {
                if (_mainSettingsModel.IsUseAverageValue)
                {
                    TimeLeftOnAllMeasurements +=
                        TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm +
                                             TimeGetValue * _mainSettingsModel.CountAverageValue);
                }
                else
                    TimeLeftOnAllMeasurements +=
                        TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm);
            }

            #endregion

            //Создаем папку для данной КС
            Directory.CreateDirectory(folderPath);

            //CH0 Ohm
            if (ch0IsActiveCollection.Contains(true))
            {
                try
                {
                    //Если калибровка выключена или если калибровка прошла = true
                    var resCalibrationOhm0 =
                        !_mainSettingsModel.IsOnCalibration || CalibrationOhm(ChannelCh0, ctSource);

                    if (IsCancellationRequested(ctSource)) return;

                    if (resCalibrationOhm0)
                    {
                        ChannelMeasurements(ChannelCh0, Ch0, ctSource, Channel.Ch0);
                        if (IsCancellationRequested(ctSource)) return;
                    }
                }
                catch (Exception e)
                {
                    GlobalLog.Log.Debug(e, e.Message);
                }
                finally
                {
                    CreateLogFile(folderPath, ChannelCh0);
                }
            }

            //CH1 Ohm
            if (ch1IsActiveCollection.Contains(true))
            {
                try
                {
                    var resCalibrationOhm1 =
                        !_mainSettingsModel.IsOnCalibration || CalibrationOhm(ChannelCh1, ctSource);

                    if (IsCancellationRequested(ctSource)) return;

                    if (resCalibrationOhm1)
                    {
                        ChannelMeasurements(ChannelCh1, Ch1, ctSource, Channel.Ch1);
                        if (IsCancellationRequested(ctSource)) return;
                    }
                }
                catch (Exception e)
                {
                    GlobalLog.Log.Debug(e, e.Message);
                }
                finally
                {
                    CreateLogFile(folderPath, ChannelCh1);
                }
            }

            //CH2 Ohm
            if (ch2IsActiveCollection.Contains(true))
            {
                try
                {
                    var resCalibrationOhm2 =
                        !_mainSettingsModel.IsOnCalibration || CalibrationOhm(ChannelCh2, ctSource);

                    if (IsCancellationRequested(ctSource)) return;

                    if (resCalibrationOhm2)
                    {
                        ChannelMeasurements(ChannelCh2, Ch2, ctSource, Channel.Ch2);
                        if (IsCancellationRequested(ctSource)) return;
                    }
                }
                catch (Exception e)
                {
                    GlobalLog.Log.Debug(e, e.Message);
                }
                finally
                {
                    CreateLogFile(folderPath, ChannelCh2);
                }
            }

            //CH3 Volt
            try
            {
                ChannelMeasurements(ChannelCh3, Ch3, ctSource, Channel.Ch3);
            }
            catch (Exception e)
            {
                GlobalLog.Log.Debug(e, e.Message);
            }
            finally
            {
                CreateLogFile(folderPath, ChannelCh3);
            }

            if (IsCancellationRequested(ctSource)) return;

            //CH5 Volt
            try
            {
                ChannelMeasurements(ChannelCh5, Ch5, ctSource, Channel.Ch5);
            }
            catch (Exception e)
            {
                GlobalLog.Log.Debug(e, e.Message);
            }
            finally
            {
                CreateLogFile(folderPath, ChannelCh5);
            }

            if (IsCancellationRequested(ctSource)) return;

            //CH6 Hz
            try
            {
                ChannelMeasurements(ChannelCh6, Ch6, ctSource, Channel.Ch6);
            }
            catch (Exception e)
            {
                GlobalLog.Log.Debug(e, e.Message);
            }
            finally
            {
                CreateLogFile(folderPath, ChannelCh6);
            }

            if (IsCancellationRequested(ctSource)) return;

            CurrentChannel = Channel.None;

            IsCheckedNow = false;
        }

        /// <summary>
        /// Запуст теста для новой версии кс
        /// </summary>
        /// <param name="ctSource"></param>
        /// <param name="folderPath"></param>
        public void StartMeasurementNewVersion(CancellationTokenSource ctSource, string folderPath)
        {
            IsCheckedNow = true;

            //NameSc = _sc.GetSerialNumberSc(_mainSettingsModel.ScVersion);

            _comm.OnSignalController(_numberSc);

            var ch0IsActiveCollection = Ch0.Select(item => item.IsActive).ToList();
            var ch1IsActiveCollection = Ch1.Select(item => item.IsActive).ToList();
            var ch2IsActiveCollection = Ch2.Select(item => item.IsActive).ToList();

            #region Добавляю время калибровки к общему времени

            if (ch0IsActiveCollection.Contains(true))
            {
                TimeLeftOnAllMeasurements += TimeSpan.FromSeconds(TimeOutCalibrationNewSc * 2);
            }

            if (ch1IsActiveCollection.Contains(true))
            {
                TimeLeftOnAllMeasurements += TimeSpan.FromSeconds(TimeOutCalibrationNewSc * 2);
            }

            if (ch2IsActiveCollection.Contains(true))
            {
                TimeLeftOnAllMeasurements += TimeSpan.FromSeconds(TimeOutCalibrationNewSc * 2);
            }

            #endregion

            //Создаем папку для данной КС
            Directory.CreateDirectory(folderPath);

            //CH0 Ohm
            if (ch0IsActiveCollection.Contains(true))
            {
                try
                {
                    if (_mainSettingsModel.IsOnCalibration)
                    {
                        CalibrationOhmChannelScNewVersion(ChannelCh0, ctSource);
                    }

                    if (IsCancellationRequested(ctSource)) return;
                    ChannelMeasurements(ChannelCh0, Ch0, ctSource, Channel.Ch0);
                }
                finally
                {
                    CreateLogFile(folderPath, ChannelCh0);
                }

                if (IsCancellationRequested(ctSource)) return;
            }

            //CH1 Ohm
            if (ch1IsActiveCollection.Contains(true))
            {
                try
                {
                    if (_mainSettingsModel.IsOnCalibration)
                    {
                        CalibrationOhmChannelScNewVersion(ChannelCh1, ctSource);
                    }

                    if (IsCancellationRequested(ctSource)) return;
                    ChannelMeasurements(ChannelCh1, Ch1, ctSource, Channel.Ch1);
                }
                finally
                {
                    CreateLogFile(folderPath, ChannelCh1);
                }

                if (IsCancellationRequested(ctSource)) return;
            }

            //CH2 Ohm
            if (ch2IsActiveCollection.Contains(true))
            {
                try
                {
                    if (_mainSettingsModel.IsOnCalibration)
                    {
                        CalibrationOhmChannelScNewVersion(ChannelCh2, ctSource);
                    }

                    if (IsCancellationRequested(ctSource)) return;
                    ChannelMeasurements(ChannelCh2, Ch2, ctSource, Channel.Ch2);
                }
                finally
                {
                    CreateLogFile(folderPath, ChannelCh2);
                }

                if (IsCancellationRequested(ctSource)) return;
            }

            //CH3 Volt
            try
            {
                ChannelMeasurements(ChannelCh3, Ch3, ctSource, Channel.Ch3);
            }
            finally
            {
                CreateLogFile(folderPath, ChannelCh3);
            }

            if (IsCancellationRequested(ctSource)) return;

            //CH5 Volt
            try
            {
                ChannelMeasurements(ChannelCh5, Ch5, ctSource, Channel.Ch5);
            }
            finally
            {
                CreateLogFile(folderPath, ChannelCh5);
            }

            if (IsCancellationRequested(ctSource)) return;

            //CH6 Hz
            try
            {
                ChannelMeasurements(ChannelCh6, Ch6, ctSource, Channel.Ch6);
            }
            finally
            {
                CreateLogFile(folderPath, ChannelCh6);
            }

            if (IsCancellationRequested(ctSource)) return;

            CurrentChannel = Channel.None;

            IsCheckedNow = false;
        }

        /// <summary>
        /// Калибровка для новой версии кс
        /// </summary>
        private void CalibrationOhmChannelScNewVersion(int channel, CancellationTokenSource ctSource)
        {
            var nameChannel = channel < 5
                ? _channelName[$"CH{channel - 1}"]
                : _channelName[$"CH{channel}"];

            CurrentActTest = $"Калибровка {nameChannel}";


            _comm.OnSignalController(_numberSc);
            _comm.OnСhannel(channel);

            _sc.OpenSession();
            _sc.Send($"fcalt {channel - 1}");
            _sc.SendWithOutN("y");

            _fluke.FlukeOff();
            _fluke.SetOhmValue(80);

            var timeOutSleep = TimeOutCalibrationNewSc;
            while (timeOutSleep != 0)
            {
                if (IsCancellationRequested(ctSource))
                    return;

                timeOutSleep -= 1;
                TimeLeftOnAllMeasurements -= TimeSpan.FromSeconds(1);
                Thread.Sleep(1000);
            }

            _sc.SendWithOutN("80");
            _sc.SendEnter();

            _fluke.FlukeOff();
            _fluke.SetOhmValue(120);

            timeOutSleep = TimeOutCalibrationNewSc;
            while (timeOutSleep != 0)
            {
                if (IsCancellationRequested(ctSource))
                    return;

                timeOutSleep -= 1;
                TimeLeftOnAllMeasurements -= TimeSpan.FromSeconds(1);
                Thread.Sleep(1000);
            }

            _sc.SendWithOutN("120");
            _sc.SendWithOutN("\r\n");
        }

        /// <summary>
        /// Запуск тестирования указанного канала 
        /// </summary>
        /// <param name="channel">Индекс проверяемого канала</param>
        /// <param name="ch">Заполняемая коллекция значений</param>
        /// <param name="ctSource"></param>
        /// <param name="currentChannel"></param>
        private void ChannelMeasurements(int channel, IEnumerable<IScValue> ch, CancellationTokenSource ctSource,
            Channel currentChannel)
        {
            var isStartChannelMeasurement = true;

            CurrentChannel = currentChannel;

            foreach (var itemCh in ch)
            {
                if (!itemCh.IsActive) continue;


                var nameChannel = channel < 5
                    ? _channelName[$"CH{channel - 1}"]
                    : _channelName[$"CH{channel}"];

                CurrentActTest = $"{nameChannel} : {itemCh.ValueMeasurement} {itemCh.TypeMeasurement}";

                itemCh.IsСheckedNow = true;


                if (isStartChannelMeasurement)
                {
                    _fluke.FlukeOff();
                }

                _comm.OnСhannel(channel);

                itemCh.SetFlukeSettings(_fluke , isStartChannelMeasurement);

                isStartChannelMeasurement = false;

                if (IsCancellationRequested(ctSource)) return;
                var value = _mainSettingsModel.IsUseAverageValue
                    ? SignalControllerAverageDataRead(channel, ctSource)
                    : SignalControllerDataRead(channel, ctSource);

                if (value == null)
                    return;


                //Переводим милиВольт в Вольты, если тестируются вольтажные каналы
                if (itemCh.TypeMeasurement == TypeMeasurement.V)
                {
                    value /= 1000;
                    value = decimal.Round((decimal) value, 3);
                }


                //Отклонение от нормы
                var differenceValue = Convert.ToDecimal(value) - itemCh.ValueMeasurement;

                itemCh.ResultValue = value;

                //Проверяю валидность результата
                itemCh.IsValidResult = itemCh.CheckedValidationDifferenceValue(differenceValue);

                itemCh.IsVerified = true;

                itemCh.IsСheckedNow = false;

                CurrentActiveMeasurement++;

                if (_mainSettingsModel.IsUseAverageValue)
                    TimeLeftOnAllMeasurements -= TimeSpan.FromSeconds(
                        GetTimeOnTypeMeasurements(itemCh.TypeMeasurement) +
                        TimeGetValue * _mainSettingsModel.CountAverageValue);
                else
                    TimeLeftOnAllMeasurements -=
                        TimeSpan.FromSeconds(GetTimeOnTypeMeasurements(itemCh.TypeMeasurement));
            }

            CurrentChannel = Channel.None;
        }

        #region Методы для перезапуска Sc

        /// <summary>
        /// Метод, что бы очистить все записаные значения ResultValue в колекциях всех каналов.
        /// Необходимо при перезапуске тестирования КС
        /// </summary>
        public void ClearAllResultsValue()
        {
            ClearChannelResultsValue(Ch0);
            ClearChannelResultsValue(Ch1);
            ClearChannelResultsValue(Ch2);
            ClearChannelResultsValue(Ch3);
            ClearChannelResultsValue(Ch5);
            ClearChannelResultsValue(Ch6);

            CurrentActiveMeasurement = 0;
            SetCountAndTimeMeasurements(_activeSettingsModel, _mainSettingsModel);
        }

        private void ClearChannelResultsValue(IEnumerable<IScValue> ch)
        {
            foreach (var scValue in ch)
            {
                scValue.IsVerified = false;
                scValue.IsСheckedNow = false;
                scValue.ResultValue = null;
            }
        }

        #endregion

        /// <summary>
        /// Возвращает время измерения, взависимости от его типа
        /// </summary>
        /// <param name="typeMeasurement"></param>
        /// <returns></returns>
        private int GetTimeOnTypeMeasurements(TypeMeasurement typeMeasurement)
        {
            switch (typeMeasurement)
            {
                case TypeMeasurement.Ohm:
                    return _mainSettingsModel.TimeOutOhm;
                case TypeMeasurement.V:
                    return _mainSettingsModel.TimeOutV;
                case TypeMeasurement.Hz:
                    return _mainSettingsModel.TimeOutHz;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeMeasurement), typeMeasurement, null);
            }
        }

        /// <summary>
        /// Калибровка необходимая для тестировая Ohm каналов
        /// </summary>
        /// <param name="channel">Индекс тестируемого канала</param>
        /// <param name="ctSource"></param>
        /// <returns>Результат каллибровки</returns>
        private bool CalibrationOhm(int channel, CancellationTokenSource ctSource)
        {
            var countAttempt = 0;
            CurrentActTest = channel < 5
                ? $"Калибровка CH{channel - 1}"
                : $"Калибровка CH{channel}";

            while (true)
            {
                if (countAttempt++ == 3) return false;

                //Выставляю на fluke  нужное значение и включаю его
                _fluke.Send("OUT 100 OHM;OPER");
                _comm.OnСhannel(channel);

                _sc.OpenSession();
                _sc.StartTest();

                if (IsCancellationRequested(ctSource)) return false;


                var timeOutOhm = _mainSettingsModel.TimeOutOhm * 1000;

                while (timeOutOhm != 0)
                {
                    if (IsCancellationRequested(ctSource)) return false;
                    timeOutOhm -= 1000;
                    Thread.Sleep(1000);
                }

                _sc.StopTest();
                _sc.CloseSession();


                if (_mainSettingsModel.IsUseAverageValue)
                {
                    var averageValue = SignalControllerAverageDataRead(channel, ctSource, true);
                    if (averageValue >= 99.99m && averageValue <= 100.01m)
                    {
                        TimeLeftOnAllMeasurements -= TimeSpan.FromSeconds(
                            GetTimeOnTypeMeasurements(TypeMeasurement.Ohm) +
                            TimeGetValue * _mainSettingsModel.CountAverageValue);
                        return true;
                    }
                }
                else if (!_mainSettingsModel.IsUseAverageValue)
                {
                    var value = SignalControllerDataRead(channel, ctSource, true);
                    if (value == 100m)
                    {
                        TimeLeftOnAllMeasurements -= TimeSpan.FromSeconds(_mainSettingsModel.TimeOutOhm);
                        return true;
                    }
                }
            }
        }

        #region Чтения значения с КС и его обработка

        /// <summary>
        /// Чтение одного значения с КС
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ctSource"></param>
        /// <param name="isCalibration"></param>
        /// <returns></returns>
        private decimal? SignalControllerDataRead(int channel, CancellationTokenSource ctSource,
            bool isCalibration = false)
        {
            //Выбранный канал , для КС это 0-1-2 . Для Коммутатора 1-2-3 соответсвенно
            var channelSignalController = channel < 5
                ? channel - 1
                : channel;


            _sc.OpenSession();
            _sc.StartTest();

            //Выбор времени теста по каналу
            switch (channelSignalController)
            {
                case 0:
                case 1:
                case 2:
                    //Если это каллибровка
                    if (isCalibration)
                    {
                        _sc.StopTest();
                        Thread.Sleep(200);
                        _sc.Send($"Cal t{channelSignalController}");
                    }

                    var timeOutOhm = _mainSettingsModel.TimeOutOhm * 1000;

                    while (timeOutOhm != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutOhm -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                case 3:
                case 5:

                    var timeOutV = _mainSettingsModel.TimeOutV * 1000;

                    while (timeOutV != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutV -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                case 6:

                    var timeOutHz = _mainSettingsModel.TimeOutHz * 1000;

                    while (timeOutHz != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutHz -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                default:
                    throw new ArgumentException();
            }


            _sc.StopTest();
            _sc.CloseSession();

            var currentData = _sc.GetCurrentData();
            //Удаляю все пробелы, во измежание проблем.
            //Так как КС может добавить мусорные пробелы при перепадах напряжения.
            currentData = currentData.Replace(" ", "");

            var firstIndex = currentData.LastIndexOf("CH0", StringComparison.Ordinal);
            var lastIndex = currentData.LastIndexOf("[Hz]", StringComparison.Ordinal);

            string data = string.Empty;

            try
            {
                data = currentData.Substring(firstIndex, lastIndex - firstIndex + "[Hz]".Length);
            }
            catch (Exception e)
            {
                GlobalLog.Log.Debug(e, e.Message);
            }

            //Получаю значение по выбранному каналу.
            var clearData = DataClear(data, channelSignalController);

            var value = decimal.Parse(clearData, CultureInfo.InvariantCulture);


            return value;
        }

        /// <summary>
        /// Чтения нескольких(кол-во определяется в настройках пользователем) значений с КС
        /// и получение среднего.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ctSource"></param>
        /// <param name="isCalibration"></param>
        /// <returns></returns>
        private decimal? SignalControllerAverageDataRead(int channel, CancellationTokenSource ctSource,
            bool isCalibration = false)
        {
            var collectionDataDecimal = new List<decimal>();
            var countAverage = _mainSettingsModel.CountAverageValue;

            //Выбранный канал , для КС это 0-1-2 . Для Коммутатора 1-2-3 соответсвенно
            var channelSignalController = channel < 5
                ? channel - 1
                : channel;


            _sc.OpenSession();
            _sc.StartTest();

            //Выбор времени теста по каналу
            switch (channelSignalController)
            {
                case 0:
                case 1:
                case 2:
                    //Если это каллибровка
                    if (isCalibration)
                    {
                        _sc.Send($"Cal t{channelSignalController}");
                    }

                    var timeOutOhm =
                        (_mainSettingsModel.TimeOutOhm + _mainSettingsModel.CountAverageValue * TimeGetValue) * 1000;

                    while (timeOutOhm != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutOhm -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                case 3:
                case 5:

                    var timeOutV = (_mainSettingsModel.TimeOutV + _mainSettingsModel.CountAverageValue * TimeGetValue) *
                                   1000;

                    while (timeOutV != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutV -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                case 6:
                    var timeOutHz =
                        (_mainSettingsModel.TimeOutHz + _mainSettingsModel.CountAverageValue * TimeGetValue) *
                        1000;

                    while (timeOutHz != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutHz -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                default:
                    throw new ArgumentException();
            }


            _sc.StopTest();
            _sc.CloseSession();

            var currentData = _sc.GetCurrentData();

            currentData = currentData.Replace(" ", "");


            var delimiters = new[] {"\r\n\r\n"};
            var collectionDataString = currentData.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();


            //Удаляю два последних итема, в которых содержится завершение сессии кс
            collectionDataString.Remove(collectionDataString.Last());
            collectionDataString.Remove(collectionDataString.Last());
            //получаю CountAverageValue последних тестов
            while (collectionDataString.Count > _mainSettingsModel.CountAverageValue)
            {
                collectionDataString.RemoveAt(0);
            }


            foreach (string item in collectionDataString)
            {
                var firstIndex = item.LastIndexOf("CH0", StringComparison.Ordinal);
                var lastIndex = item.LastIndexOf("[Hz]", StringComparison.Ordinal);

                var data = item.Substring(firstIndex, lastIndex - firstIndex + "[Hz]".Length);

                //Получаю значение по выбранному каналу.
                var clearData = DataClear(data, channelSignalController);

                var value = decimal.Parse(clearData, CultureInfo.InvariantCulture);
                collectionDataDecimal.Add(value);
            }

            var valueAverage = collectionDataDecimal.Sum() / countAverage;
            valueAverage = decimal.Round(valueAverage, 2);


            return valueAverage;
        }


        public static IEnumerable<string> SplitAndKeep(string s, params string[] delims)
        {
            var rows = new List<string>() {s};
            foreach (var delim in delims) //delimiter counter
            {
                for (var i = 0; i < rows.Count; i++) //row counter
                {
                    var index = rows[i].IndexOf(delim, StringComparison.Ordinal);
                    if (index > -1
                        && rows[i].Length > index + 1)
                    {
                        var leftPart = rows[i].Substring(0, index + delim.Length);
                        var rightPart = rows[i].Substring(index + delim.Length);
                        rows[i] = leftPart;
                        rows.Insert(i + 1, rightPart);
                    }
                }
            }

            return rows;
        }


        /// <summary>
        /// Получение после выполнения SignalControllerDataRead, возвращает значения по выбранному каналу.
        /// </summary>
        /// <returns>Значения теста по выбранному каналу.</returns>
        private string DataClear(string data, int channel)
        {
            //Каналов всего 7. И канал CH4  не используется в тестах.
            //Так что все невалидные индексы канала приводят к ошибке.
            if (channel > 7 || channel == 4)
                throw new ArgumentException();

            //channel - 1 , так как отсчет идет с 0.
            var ch = string.Concat("CH", $"{channel}");
            var chFirstIndex = data.IndexOf(ch, StringComparison.Ordinal);
            var clearData = data.Substring(chFirstIndex);
            //У канала  CH6  нет \r\n , так как это последняя строка
            if (channel != 6)
            {
                var chLastIndex = clearData.IndexOf("\r\n", StringComparison.Ordinal);
                clearData = clearData.Substring(0, chLastIndex);
            }


            switch (channel)
            {
                case 0:
                case 1:
                case 2:
                    return SubstringValue("[avg]", "[ohm]", clearData);
                case 3:
                    return SubstringValue("[avg]", "[mV]", clearData);
                case 5:
                    return SubstringValue("[raw]", "[mV]", clearData);
                case 6:
                    return SubstringValue("[raw]", "[Hz]", clearData);
                default:
                    throw new ArgumentException();
            }
        }

        private string SubstringValue(string startText, string endText, string data)
        {
            var firstIndex = data.IndexOf(startText, StringComparison.Ordinal) + startText.Length;
            var lastIndex = data.IndexOf(endText, StringComparison.Ordinal);
            return data.Substring(firstIndex, lastIndex - firstIndex);
        }

        #endregion

        /// <summary>
        /// Проверка на запрос отмены
        /// </summary>
        /// <param name="ctSource"></param>
        /// <returns></returns>
        private bool IsCancellationRequested(CancellationTokenSource ctSource)
        {
            var token = ctSource.Token;

            if (token.IsCancellationRequested)
            {
                IsCheckedNow = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Освобождение ресурсов портов
        /// </summary>
        public void StopTest()
        {
            #region Fluke Close

            try
            {
                _fluke.Close();
            }
            catch
            {
                //ignore
            }

            #endregion

            #region Comm Close

            try
            {
                _comm.Close();
            }
            catch
            {
                //ignore
            }

            #endregion

            #region SC Close

            try
            {
                _sc.Close();
            }
            catch
            {
                //ignore
            }

            #endregion
        }


        #region Write Log and Measurement Result

        private void CreateMeasurement(string folderPath)
        {
            CreateMeasurementCsv(folderPath);
            CreateMeasurementXlsx(folderPath);
        }

        private string ConvertResultForXlsx(decimal? value)
        {
            return value == null ? string.Empty : Convert.ToString(value);
        }

        private void CreateMeasurementXlsx(string folderPath)
        {
            var pathExampleXlsxFile = @"Resources\test_cs_protocol.xlsx";

            while (true)
            {
                if (File.Exists(pathExampleXlsxFile))
                    break;

                var errorMessage =
                    "Отсутствует файл образец test_cs_protocol.xlsx  " +
                    "Пожалуйста поместите файл и повторите попытку(ОК). Или нажмите отмена для пропуска создания .xlsx";

                var mb = MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OKCancel);

                //Если было нажато ОК
                if (mb == MessageBoxResult.OK)
                    continue;

                //Если была нажата отмена
                return;
            }

            using (var package = new ExcelPackage(new FileInfo(pathExampleXlsxFile)))
            {
                var ws = package.Workbook.Worksheets.First();

                ws.Cells[14, 6].Value = NameSc;

                ws.Cells[55 , 22].Value = $"{DateTime.Now:dd.MM.yyyy}";

                ws.Cells[55, 16].Value = _measurementsData.Verifier;




                //Температура
                ws.Cells[25, 5].Value = _measurementsData.Temperature;
                //Влажность
                ws.Cells[26, 5].Value = _measurementsData.Humidity;
                //Атмосферное давление
                ws.Cells[27, 5].Value = _measurementsData.Pressure;
                //Напряжение , константа
                ws.Cells[28, 5].Value = _measurementsData.Voltage;
                //Частота , константа
                ws.Cells[29, 5].Value = _measurementsData.Frequency;

                #region Set Result Value

                #region Ch0

                ws.Cells[8, 13].Value = ConvertResultForXlsx(Ch0[0].ResultValue);
                ws.Cells[9, 13].Value = ConvertResultForXlsx(Ch0[1].ResultValue);
                ws.Cells[10, 13].Value = ConvertResultForXlsx(Ch0[2].ResultValue);
                ws.Cells[11, 13].Value = ConvertResultForXlsx(Ch0[3].ResultValue);
                ws.Cells[12, 13].Value = ConvertResultForXlsx(Ch0[4].ResultValue);
                ws.Cells[13, 13].Value = ConvertResultForXlsx(Ch0[5].ResultValue);

                #endregion

                #region Ch1

                ws.Cells[8, 16].Value = ConvertResultForXlsx(Ch1[0].ResultValue);
                ws.Cells[9, 16].Value = ConvertResultForXlsx(Ch1[1].ResultValue);
                ws.Cells[10, 16].Value = ConvertResultForXlsx(Ch1[2].ResultValue);
                ws.Cells[11, 16].Value = ConvertResultForXlsx(Ch1[3].ResultValue);
                ws.Cells[12, 16].Value = ConvertResultForXlsx(Ch1[4].ResultValue);
                ws.Cells[13, 16].Value = ConvertResultForXlsx(Ch1[5].ResultValue);

                #endregion

                #region Ch2

                ws.Cells[8, 19].Value = ConvertResultForXlsx(Ch2[0].ResultValue);
                ws.Cells[9, 19].Value = ConvertResultForXlsx(Ch2[1].ResultValue);
                ws.Cells[10, 19].Value = ConvertResultForXlsx(Ch2[2].ResultValue);
                ws.Cells[11, 19].Value = ConvertResultForXlsx(Ch2[3].ResultValue);
                ws.Cells[12, 19].Value = ConvertResultForXlsx(Ch2[4].ResultValue);
                ws.Cells[13, 19].Value = ConvertResultForXlsx(Ch2[5].ResultValue);

                #endregion

                #region Ch3

                ws.Cells[17, 14].Value = ConvertResultForXlsx(Ch3[0].ResultValue);
                ws.Cells[18, 14].Value = ConvertResultForXlsx(Ch3[1].ResultValue);
                ws.Cells[19, 14].Value = ConvertResultForXlsx(Ch3[2].ResultValue);
                ws.Cells[20, 14].Value = ConvertResultForXlsx(Ch3[3].ResultValue);
                ws.Cells[21, 14].Value = ConvertResultForXlsx(Ch3[4].ResultValue);

                #endregion

                #region Ch5

                ws.Cells[17, 17].Value = ConvertResultForXlsx(Ch5[0].ResultValue);
                ws.Cells[18, 17].Value = ConvertResultForXlsx(Ch5[1].ResultValue);
                ws.Cells[19, 17].Value = ConvertResultForXlsx(Ch5[2].ResultValue);
                ws.Cells[20, 17].Value = ConvertResultForXlsx(Ch5[3].ResultValue);
                ws.Cells[21, 17].Value = ConvertResultForXlsx(Ch5[4].ResultValue);

                #endregion

                #region Ch6

                ws.Cells[25, 16].Value = ConvertResultForXlsx(Ch6[0].ResultValue);
                ws.Cells[26, 16].Value = ConvertResultForXlsx(Ch6[1].ResultValue);
                ws.Cells[27, 16].Value = ConvertResultForXlsx(Ch6[2].ResultValue);
                ws.Cells[28, 16].Value = ConvertResultForXlsx(Ch6[3].ResultValue);
                ws.Cells[29, 16].Value = ConvertResultForXlsx(Ch6[4].ResultValue);

                #endregion

                #endregion

                var xlsxCombinePath = Path.Combine(folderPath, $"{NameSc}.xlsx");

                package.SaveAs(new FileInfo(xlsxCombinePath));
            }
        }


        private void CreateMeasurementCsv(string folderPath)
        {
            var csvContent = new StringBuilder();

            #region Create Data

            csvContent.AppendLine("OHM;R[0];R[1];R[2];V;V[3];V[5];Hz;Hz[6]");
            csvContent.AppendLine("80;" +
                                  Ch0[0].ResultValue + ";" +
                                  Ch1[0].ResultValue + ";" +
                                  Ch2[0].ResultValue + ";1;" +
                                  Ch3[0].ResultValue + ";" +
                                  Ch5[0].ResultValue + ";5;" +
                                  Ch6[0].ResultValue);
            csvContent.AppendLine("90;" +
                                  Ch0[1].ResultValue + ";" +
                                  Ch1[1].ResultValue + ";" +
                                  Ch2[1].ResultValue + ";2;" +
                                  Ch3[1].ResultValue + ";" +
                                  Ch5[1].ResultValue + ";25;" +
                                  Ch6[1].ResultValue);
            csvContent.AppendLine("100;" +
                                  Ch0[2].ResultValue + ";" +
                                  Ch1[2].ResultValue + ";" +
                                  Ch2[2].ResultValue + ";3;" +
                                  Ch3[2].ResultValue + ";" +
                                  Ch5[2].ResultValue + ";50;" +
                                  Ch6[2].ResultValue);
            csvContent.AppendLine("115;" +
                                  Ch0[3].ResultValue + ";" +
                                  Ch1[3].ResultValue + ";" +
                                  Ch2[3].ResultValue + ";4;" +
                                  Ch3[3].ResultValue + ";" +
                                  Ch5[3].ResultValue + ";75;" +
                                  Ch6[3].ResultValue);
            csvContent.AppendLine("130;" +
                                  Ch0[4].ResultValue + ";" +
                                  Ch1[4].ResultValue + ";" +
                                  Ch2[4].ResultValue + ";5;" +
                                  Ch3[4].ResultValue + ";" +
                                  Ch5[4].ResultValue + ";100;" +
                                  Ch6[4].ResultValue);
            csvContent.AppendLine("140;" +
                                  Ch0[5].ResultValue + ";" +
                                  Ch1[5].ResultValue + ";" +
                                  Ch2[5].ResultValue);

            #endregion

            var csvPath = Path.Combine(folderPath, $"{NameSc}.csv");
            if (File.Exists(csvPath)) File.Delete(csvPath);
            File.AppendAllText(csvPath, csvContent.ToString());
        }

        /// <summary>
        /// Метод для создания файла лога.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="channel"></param>
        private void CreateLogFile(string folderPath, int channel)
        {
            var content = _sc.GetFullData();

            var fileName = channel < 5
                ? $"CH{channel - 1}.txt"
                : $"CH{channel}.txt";

            var folderLogPath = Path.Combine(folderPath, "Log");

            var path = Path.Combine(folderLogPath, fileName);

            //Папка для логов, если она еще не создана
            if (!File.Exists(folderLogPath))
                Directory.CreateDirectory(folderLogPath);
            File.WriteAllText(path, content);
            _sc.ClearFullData();
        }

        #endregion
    }

    /// <summary>
    /// Енум каналов
    /// </summary>
    public enum Channel
    {
        None = 0,
        Ch0 = 1,
        Ch1 = 2,
        Ch2 = 3,
        Ch3 = 4,

        //4-ой не существует. Гениальная задумка изобретателя ксочной хреновины. 
        Ch5 = 5,
        Ch6 = 6
    }

    /// <summary>
    /// Enum на будущее, если потребуется поддержка следующих версий
    /// Для распределения действий алгоритма для кс разных версий.
    /// </summary>
    public enum ScVersion
    {
        Old,
        New
    }
}