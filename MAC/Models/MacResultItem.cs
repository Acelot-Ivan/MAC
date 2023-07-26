using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MAC.Models.Value;
using MAC.Properties;
using MAC.ViewModels;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services;
using MAC.ViewModels.Services.SerialPort;
using OfficeOpenXml;

namespace MAC.Models
{
    public class MacResultItem : BaseVm
    {
        private readonly FlukeSerialPort _fluke;
        private readonly CommutatorSerialPort _comm;
        private readonly MacSerialPort _mac;
        private readonly int _numberMac;
        private readonly string _startTestStringDateTime;
        private readonly MainSettingsModel _mainSettingsModel;
        private readonly ActiveSettingsModel _activeSettingsModel;
        private MeasurementsData _measurementsData;

        public Action SetIsWaitCancelFalse;

        private readonly Dictionary<string, string> _channelName = new Dictionary<string, string>
        {
            {"X1", "t возд"},
            {"X2", "t дор"},
            {"X3", "t п/п"},
            {"X4", "Влаж"},
            {"X5", "ДНВ"},
            {"X6", "ДВС"}
        };

        /// <summary>
        /// Примерное время для считывания одного значения
        /// Необходимо для расчета времени при использовании среднего значения.
        /// Которое снимает несколько значение за одно тестирование на выбранных характеристиках Fluke.
        /// </summary>
        const int TimeGetValue = 10;

        /// <summary>
        /// Время калибровки на каждую точку при калибровке новых версий mac в секундах.
        /// </summary>
        private const int TimeOutCalibrationNewSc = 60;

        /// <summary>
        /// Имя заданное лаборантом
        /// </summary>
        public string NameMac { get; set; }

        /// <summary>
        /// Считываемая версия Контролера
        /// </summary>
        public Version VersionMac { get; set; }

        /// <summary>
        /// Свойство для отслеживания действия при отмене задачи теста.
        /// </summary>
        public TypeCancelTask TypeCancelTask { get; set; }

        public Channel CurrentChannel { get; set; } = Channel.None;

        public ObservableCollection<IMacValue> Ch1 { get; set; }
        public ObservableCollection<IMacValue> Ch2 { get; set; }
        public ObservableCollection<IMacValue> Ch3 { get; set; }
        public ObservableCollection<IMacValue> Ch4 { get; set; }
        public ObservableCollection<IMacValue> Ch5 { get; set; }
        public ObservableCollection<IMacValue> Ch6 { get; set; }

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
        /// Показатель, проверяется ли сейчас Mac
        /// </summary>
        public bool IsCheckedNow { get; set; }

        /// <summary>
        /// Конструкстор при создании итемов МАС
        /// </summary>
        /// <param name="fluke"></param>
        /// <param name="comm"></param>
        /// <param name="sc"></param>
        /// <param name="mainSettingsModel"></param>
        /// <param name="activeSettings"></param>
        /// <param name="typeCancelTask"></param>
        /// <param name="setIsWaitCancelFalse"></param>
        public MacResultItem(FlukeSerialPort fluke, CommutatorSerialPort comm, ComConnectItem mac,
            MainSettingsModel mainSettingsModel,
            ActiveSettingsModel activeSettings, TypeCancelTask typeCancelTask, Action setIsWaitCancelFalse)
        {
            SetIsWaitCancelFalse = setIsWaitCancelFalse;
            _activeSettingsModel = activeSettings;
            TypeCancelTask = typeCancelTask;
            _mainSettingsModel = mainSettingsModel;
            _startTestStringDateTime = $"_{DateTime.Now:dd.MM.yyyy_HH-mm-ss}";
            _numberMac = mac.Number;
            NameMac = mac.Name;
            _fluke = fluke;
            _comm = comm;
            _mac = new MacSerialPort(mac);


            #region Заполнение коллекций CH

            Ch1 = new ObservableCollection<IMacValue>
            {
                new OhmValueMac(30, activeSettings.Ch0Ohm1),
                new OhmValueMac(85, activeSettings.Ch0Ohm2),
                new OhmValueMac(110, activeSettings.Ch0Ohm3),
                new OhmValueMac(155, activeSettings.Ch0Ohm4),
                new OhmValueMac(190, activeSettings.Ch0Ohm5),
            };
            Ch2 = new ObservableCollection<IMacValue>
            {
                new OhmValueMac(30, activeSettings.Ch1Ohm1),
                new OhmValueMac(85, activeSettings.Ch1Ohm2),
                new OhmValueMac(110, activeSettings.Ch1Ohm3),
                new OhmValueMac(155, activeSettings.Ch1Ohm4),
                new OhmValueMac(190, activeSettings.Ch1Ohm5),
            };
            Ch3 = new ObservableCollection<IMacValue>
            {
                new OhmValueMac(30, activeSettings.Ch2Ohm1),
                new OhmValueMac(85, activeSettings.Ch2Ohm2),
                new OhmValueMac(110, activeSettings.Ch2Ohm3),
                new OhmValueMac(155, activeSettings.Ch2Ohm4),
                new OhmValueMac(190, activeSettings.Ch2Ohm5),
            };
            Ch4 = new ObservableCollection<IMacValue>
            {
                new VoltValue(0.345m, activeSettings.Ch3V1),
                new VoltValue(1.325m, activeSettings.Ch3V2),
                new VoltValue(2.550m, activeSettings.Ch3V3),
                new VoltValue(3.775m, activeSettings.Ch3V4),
                new VoltValue(4.775m, activeSettings.Ch3V5)
            };
            Ch5 = new ObservableCollection<IMacValue>
            {
                new VoltValue(0.345m, activeSettings.Ch5V1),
                new VoltValue(1.325m, activeSettings.Ch5V2),
                new VoltValue(2.550m, activeSettings.Ch5V3),
                new VoltValue(3.775m, activeSettings.Ch5V4),
                new VoltValue(4.775m, activeSettings.Ch5V5)
            };
            Ch6 = new ObservableCollection<IMacValue>
            {
                new HzValueMac(50, activeSettings.Ch6Hz1),
                new HzValueMac(250, activeSettings.Ch6Hz2),
                new HzValueMac(500, activeSettings.Ch6Hz3),
                new HzValueMac(750, activeSettings.Ch6Hz4),
                new HzValueMac(1000, activeSettings.Ch6Hz5)
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

            UpdateMeasurements(activeSettings.Ch0Ohm1, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm2, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm3, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm4, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch0Ohm5, TypeMeasurement.Ohm);

            UpdateMeasurements(activeSettings.Ch1Ohm1, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm2, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm3, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm4, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch1Ohm5, TypeMeasurement.Ohm);

            UpdateMeasurements(activeSettings.Ch2Ohm1, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm2, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm3, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm4, TypeMeasurement.Ohm);
            UpdateMeasurements(activeSettings.Ch2Ohm5, TypeMeasurement.Ohm);

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

            UpdateMeasurements(activeSettings.Ch6Hz1, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz2, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz3, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz4, TypeMeasurement.Hz);
            UpdateMeasurements(activeSettings.Ch6Hz5, TypeMeasurement.Hz);
        }

        /// <summary>
        /// Заполняю итемы значений, значениями погрешности.
        /// </summary>
        private void SetErrorValue()
        {
            var channelName = $"Sc{_numberMac}";

            foreach (var itemCh in Ch1)
                itemCh.ErrorValue = (decimal)Settings.Default[$"{channelName}Ch0Error"];

            foreach (var itemCh in Ch2)
                itemCh.ErrorValue = (decimal)Settings.Default[$"{channelName}Ch1Error"];

            foreach (var itemCh in Ch3)
                itemCh.ErrorValue = (decimal)Settings.Default[$"{channelName}Ch2Error"];

            foreach (var itemCh in Ch4)
                itemCh.ErrorValue = (decimal)Settings.Default[$"{channelName}Ch3Error"];

            foreach (var itemCh in Ch5)
                itemCh.ErrorValue = (decimal)Settings.Default[$"{channelName}Ch5Error"];
        }

        #region Const Channel 

        //Для Comm 1,2,3,4,5,6 
        //Для   Mac 0,1,2,3,5,6 соответственно коммутатору

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
            30, 85, 110, 155, 190
        };

        public ObservableCollection<decimal> ChV { get; set; } = new ObservableCollection<decimal>
        {
            0.345m, 1.325m, 2.550m, 3.775m, 4.755m
        };

        public ObservableCollection<int> ChHz { get; set; } = new ObservableCollection<int>
        {
            50, 250, 500, 750, 1000
        };

        #endregion

        public void MainStartMeasurements(CancellationTokenSource ctSource, MeasurementsData measurementsData)
        {
            _measurementsData = measurementsData;

            _fluke.OpenFlukePort();
            _comm.OpenCommPort();

            _comm.OnPowerIndex(_numberMac);

            _mac.OpenSerialPort();

            var (macVersion, version) = _mac.GetVersionMac();

            VersionMac = version;

            var folderPath = Path.Combine(_mainSettingsModel.FullLogPath, $"{NameMac}{_startTestStringDateTime}");

            StartMeasurementNewVersion(ctSource, folderPath);

            try
            {
                //CreateMeasurement(folderPath);
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
                _mac.Close();
            }
            catch
            {
                //ignore
            }

            #endregion
        }

        /// <summary>
        /// Запуст теста Mac
        /// </summary>
        /// <param name="ctSource"></param>
        /// <param name="folderPath"></param>
        public void StartMeasurementNewVersion(CancellationTokenSource ctSource, string folderPath)
        {
            IsCheckedNow = true;

            var ch0IsActiveCollection = Ch1.Select(item => item.IsActive).ToList();
            var ch1IsActiveCollection = Ch2.Select(item => item.IsActive).ToList();
            var ch2IsActiveCollection = Ch3.Select(item => item.IsActive).ToList();

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

            //Создаем папку для данной MAC
            Directory.CreateDirectory(folderPath);

            //CH0 Ohm
            if (ch0IsActiveCollection.Contains(true))
            {
                try
                {
                    if (_mainSettingsModel.IsOnCalibration)
                    {
                        CalibrationOhmChannelMac(ChannelCh0, ctSource);
                    }

                    if (IsCancellationRequested(ctSource)) return;
                    ChannelMeasurements(ChannelCh0, Ch1, ctSource, Channel.Ch0);
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
                        CalibrationOhmChannelMac(ChannelCh1, ctSource);
                    }

                    if (IsCancellationRequested(ctSource)) return;
                    ChannelMeasurements(ChannelCh1, Ch2, ctSource, Channel.Ch1);
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
                        CalibrationOhmChannelMac(ChannelCh2, ctSource);
                    }

                    if (IsCancellationRequested(ctSource)) return;
                    ChannelMeasurements(ChannelCh2, Ch3, ctSource, Channel.Ch2);
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
                ChannelMeasurements(ChannelCh3, Ch4, ctSource, Channel.Ch3);
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
        /// Калибровка Mac
        /// </summary>
        private void CalibrationOhmChannelNewVersion(int channel, CancellationTokenSource ctSource)
        {
            var nameChannel = channel < 5
                ? _channelName[$"CH{channel - 1}"]
                : _channelName[$"CH{channel}"];

            CurrentActTest = $"Калибровка {nameChannel}";


            _comm.OnPowerIndex(_numberMac);
            _comm.OnСhannel(channel);

            _mac.OpenSession();
            _mac.Send($"fcalt {channel - 1} 200");
            _mac.SendWithOutN("y");

            _fluke.FlukeOff();
            _fluke.SetOhmValueCalibration();

            var timeOutSleep = TimeOutCalibrationNewSc;
            while (timeOutSleep != 0)
            {
                if (IsCancellationRequested(ctSource))
                    return;

                timeOutSleep -= 1;
                TimeLeftOnAllMeasurements -= TimeSpan.FromSeconds(1);
                Thread.Sleep(1000);
            }

            _mac.SendWithOutN("80");
            _mac.SendEnter();

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

            _mac.SendWithOutN("120");
            _mac.SendWithOutN("\r\n");
        }

        private void CalibrationOhmChannelMac(int channel, CancellationTokenSource ctSource)
        {
            var nameChannel = _channelName[$"X{channel}"];

            CurrentActTest = $"Калибровка {nameChannel}";


            _fluke.SetOhmValueCalibration();

            _comm.OnPowerIndex(_numberMac);
            _comm.OnСhannel(channel);

            _mac.OpenSession();
            _mac.SendWithOutN("init def");
            
            //check  "DO YOU WANT TO PERFORM REINITIALIZATION (Y/N)? "
            _mac.SendWithOutN("y");

            _mac.SendWithOutN("test");

            Thread.Sleep(30000);

            _mac.SendWithOutN(" ");

            _mac.SendWithOutN($"fcal {channel} 200");

            _mac.SendWithOutN("y");

            var x = _mac.GetCurrentData();
        }

        /// <summary>
        /// Запуск тестирования указанного канала 
        /// </summary>
        /// <param name="channel">Индекс проверяемого канала</param>
        /// <param name="ch">Заполняемая коллекция значений</param>
        /// <param name="ctSource"></param>
        /// <param name="currentChannel"></param>
        private void ChannelMeasurements(int channel, IEnumerable<IMacValue> ch, CancellationTokenSource ctSource,
            Channel currentChannel)
        {
            var isStartChannelMeasurement = true;

            CurrentChannel = currentChannel;

            foreach (var itemCh in ch)
            {
                if (!itemCh.IsActive) continue;


                var nameChannel = _channelName[$"X{channel}"];

                CurrentActTest = $"{nameChannel} : {itemCh.ValueMeasurement} {itemCh.TypeMeasurement}";

                itemCh.IsСheckedNow = true;


                if (isStartChannelMeasurement)
                {
                    _fluke.FlukeOff();
                }

                _comm.OnСhannel(channel);

                itemCh.SetFlukeSettings(_fluke, isStartChannelMeasurement);

                isStartChannelMeasurement = false;

                if (IsCancellationRequested(ctSource)) return;
                var value = _mainSettingsModel.IsUseAverageValue
                    ? AverageDataRead(channel, ctSource)
                    : DataRead(channel, ctSource);

                if (value == null)
                    return;


                //Переводим милиВольт в Вольты, если тестируются вольтажные каналы
                if (itemCh.TypeMeasurement == TypeMeasurement.V)
                {
                    value /= 1000;
                    value = decimal.Round((decimal)value, 3);
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
        /// Необходимо при перезапуске тестирования Mac
        /// </summary>
        public void ClearAllResultsValue()
        {
            ClearChannelResultsValue(Ch1);
            ClearChannelResultsValue(Ch2);
            ClearChannelResultsValue(Ch3);
            ClearChannelResultsValue(Ch4);
            ClearChannelResultsValue(Ch5);
            ClearChannelResultsValue(Ch6);

            CurrentActiveMeasurement = 0;
            SetCountAndTimeMeasurements(_activeSettingsModel, _mainSettingsModel);
        }

        private void ClearChannelResultsValue(IEnumerable<IMacValue> ch)
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

        #region Чтения значения с Mac и его обработка

        /// <summary>
        /// Чтение одного значения с Mac
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ctSource"></param>
        /// <param name="isCalibration"></param>
        /// <returns></returns>
        private decimal? DataRead(int channel, CancellationTokenSource ctSource)
        {
            _mac.OpenSession();
            _mac.StartTest();

            //Выбор времени теста по каналу
            switch (channel)
            {
                case 1:
                case 2:
                case 3:
                    var timeOutOhm = _mainSettingsModel.TimeOutOhm * 1000;

                    while (timeOutOhm != 0)
                    {
                        if (IsCancellationRequested(ctSource))
                            return null;
                        timeOutOhm -= 1000;
                        Thread.Sleep(1000);
                    }

                    break;
                case 4:
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


            _mac.StopTest();
            _mac.CloseSession();

            var currentData = _mac.GetCurrentData();
            //Удаляю все пробелы, во измежание проблем.
            //Так как МАС может добавить мусорные пробелы при перепадах напряжения.
            currentData = currentData.Replace(" ", "");

            var firstIndex = currentData.LastIndexOf("X1", StringComparison.Ordinal);
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
            var clearData = DataClear(data, channel);

            var value = decimal.Parse(clearData, CultureInfo.InvariantCulture);


            return value;
        }

        /// <summary>
        /// Чтения нескольких(кол-во определяется в настройках пользователем) значений с Mac
        /// и получение среднего.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ctSource"></param>
        /// <param name="isCalibration"></param>
        /// <returns></returns>
        private decimal? AverageDataRead(int channel, CancellationTokenSource ctSource)
        {
            var collectionDataDecimal = new List<decimal>();
            var countAverage = _mainSettingsModel.CountAverageValue;

            _mac.OpenSession();
            _mac.StartTest();

            //Выбор времени теста по каналу
            switch (channel)
            {
                case 1:
                case 2:
                case 3:
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
                case 4:
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


            _mac.StopTest();
            _mac.CloseSession();

            var currentData = _mac.GetCurrentData();

            currentData = currentData.Replace(" ", "");


            var delimiters = new[] { "\r\n\r\n" };
            var collectionDataString = currentData.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();


            //Удаляю два последних итема, в которых содержится завершение сессии МАС
            collectionDataString.Remove(collectionDataString.Last());
            collectionDataString.Remove(collectionDataString.Last());
            //получаю CountAverageValue последних тестов
            while (collectionDataString.Count > _mainSettingsModel.CountAverageValue)
            {
                collectionDataString.RemoveAt(0);
            }


            foreach (string item in collectionDataString)
            {
                var firstIndex = item.LastIndexOf("X1", StringComparison.Ordinal);
                var lastIndex = item.LastIndexOf("[Hz]", StringComparison.Ordinal);

                var data = item.Substring(firstIndex, lastIndex - firstIndex + "[Hz]".Length);

                //Получаю значение по выбранному каналу.
                var clearData = DataClear(data, channel);

                var value = decimal.Parse(clearData, CultureInfo.InvariantCulture);
                collectionDataDecimal.Add(value);
            }

            var valueAverage = collectionDataDecimal.Sum() / countAverage;
            valueAverage = decimal.Round(valueAverage, 2);


            return valueAverage;
        }

        public static IEnumerable<string> SplitAndKeep(string s, params string[] delims)
        {
            var rows = new List<string>() { s };
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
        /// Получение после выполнения DataRead, возвращает значения по выбранному каналу.
        /// </summary>
        /// <returns>Значения теста по выбранному каналу.</returns>
        private string DataClear(string data, int channel)
        {
            //Каналов всего 7. И канал CH4  не используется в тестах.
            //Так что все невалидные индексы канала приводят к ошибке.
            if (channel > 7)
                throw new ArgumentException();

            //channel - 1 , так как отсчет идет с 0.
            var ch = string.Concat("X", $"{channel}");
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
                case 1:
                case 2:
                case 3:
                    return SubstringValue("[avg]", "[ohm]", clearData);
                case 4:
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
                _mac.Close();
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

                ws.Cells[14, 6].Value = NameMac;

                ws.Cells[55, 22].Value = $"{DateTime.Now:dd.MM.yyyy}";

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

                #region Ch1

                ws.Cells[8, 13].Value = ConvertResultForXlsx(Ch1[0].ResultValue);
                ws.Cells[9, 13].Value = ConvertResultForXlsx(Ch1[1].ResultValue);
                ws.Cells[10, 13].Value = ConvertResultForXlsx(Ch1[2].ResultValue);
                ws.Cells[11, 13].Value = ConvertResultForXlsx(Ch1[3].ResultValue);
                ws.Cells[12, 13].Value = ConvertResultForXlsx(Ch1[4].ResultValue);
                ws.Cells[13, 13].Value = ConvertResultForXlsx(Ch1[5].ResultValue);

                #endregion

                #region Ch2

                ws.Cells[8, 16].Value = ConvertResultForXlsx(Ch2[0].ResultValue);
                ws.Cells[9, 16].Value = ConvertResultForXlsx(Ch2[1].ResultValue);
                ws.Cells[10, 16].Value = ConvertResultForXlsx(Ch2[2].ResultValue);
                ws.Cells[11, 16].Value = ConvertResultForXlsx(Ch2[3].ResultValue);
                ws.Cells[12, 16].Value = ConvertResultForXlsx(Ch2[4].ResultValue);
                ws.Cells[13, 16].Value = ConvertResultForXlsx(Ch2[5].ResultValue);

                #endregion

                #region Ch3

                ws.Cells[8, 19].Value = ConvertResultForXlsx(Ch3[0].ResultValue);
                ws.Cells[9, 19].Value = ConvertResultForXlsx(Ch3[1].ResultValue);
                ws.Cells[10, 19].Value = ConvertResultForXlsx(Ch3[2].ResultValue);
                ws.Cells[11, 19].Value = ConvertResultForXlsx(Ch3[3].ResultValue);
                ws.Cells[12, 19].Value = ConvertResultForXlsx(Ch3[4].ResultValue);
                ws.Cells[13, 19].Value = ConvertResultForXlsx(Ch3[5].ResultValue);

                #endregion

                #region Ch4

                ws.Cells[17, 14].Value = ConvertResultForXlsx(Ch4[0].ResultValue);
                ws.Cells[18, 14].Value = ConvertResultForXlsx(Ch4[1].ResultValue);
                ws.Cells[19, 14].Value = ConvertResultForXlsx(Ch4[2].ResultValue);
                ws.Cells[20, 14].Value = ConvertResultForXlsx(Ch4[3].ResultValue);
                ws.Cells[21, 14].Value = ConvertResultForXlsx(Ch4[4].ResultValue);

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

                var xlsxCombinePath = Path.Combine(folderPath, $"{NameMac}.xlsx");

                package.SaveAs(new FileInfo(xlsxCombinePath));
            }
        }


        private void CreateMeasurementCsv(string folderPath)
        {
            var csvContent = new StringBuilder();

            #region Create Data

            csvContent.AppendLine("OHM;R[0];R[1];R[2];V;V[3];V[5];Hz;Hz[6]");
            csvContent.AppendLine("80;" +
                                  Ch1[0].ResultValue + ";" +
                                  Ch2[0].ResultValue + ";" +
                                  Ch3[0].ResultValue + ";1;" +
                                  Ch4[0].ResultValue + ";" +
                                  Ch5[0].ResultValue + ";5;" +
                                  Ch6[0].ResultValue);
            csvContent.AppendLine("90;" +
                                  Ch1[1].ResultValue + ";" +
                                  Ch2[1].ResultValue + ";" +
                                  Ch3[1].ResultValue + ";2;" +
                                  Ch4[1].ResultValue + ";" +
                                  Ch5[1].ResultValue + ";25;" +
                                  Ch6[1].ResultValue);
            csvContent.AppendLine("100;" +
                                  Ch1[2].ResultValue + ";" +
                                  Ch2[2].ResultValue + ";" +
                                  Ch3[2].ResultValue + ";3;" +
                                  Ch4[2].ResultValue + ";" +
                                  Ch5[2].ResultValue + ";50;" +
                                  Ch6[2].ResultValue);
            csvContent.AppendLine("115;" +
                                  Ch1[3].ResultValue + ";" +
                                  Ch2[3].ResultValue + ";" +
                                  Ch3[3].ResultValue + ";4;" +
                                  Ch4[3].ResultValue + ";" +
                                  Ch5[3].ResultValue + ";75;" +
                                  Ch6[3].ResultValue);
            csvContent.AppendLine("130;" +
                                  Ch1[4].ResultValue + ";" +
                                  Ch2[4].ResultValue + ";" +
                                  Ch3[4].ResultValue + ";5;" +
                                  Ch4[4].ResultValue + ";" +
                                  Ch5[4].ResultValue + ";100;" +
                                  Ch6[4].ResultValue);
            csvContent.AppendLine("140;" +
                                  Ch1[5].ResultValue + ";" +
                                  Ch2[5].ResultValue + ";" +
                                  Ch3[5].ResultValue);

            #endregion

            var csvPath = Path.Combine(folderPath, $"{NameMac}.csv");
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
            var content = _mac.GetFullData();

            var fileName = channel < 5
                ? $"CH{channel - 1}.txt"
                : $"CH{channel}.txt";

            var folderLogPath = Path.Combine(folderPath, "Log");

            var path = Path.Combine(folderLogPath, fileName);

            //Папка для логов, если она еще не создана
            if (!File.Exists(folderLogPath))
                Directory.CreateDirectory(folderLogPath);
            File.WriteAllText(path, content);
            _mac.ClearFullData();
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
    /// Для распределения действий алгоритма для МАС разных версий.
    /// </summary>
    public enum MacVersion
    {
        New,
        Old
    }
}
