﻿using MAC.Models;
using MAC.Properties;
using MAC.ViewModels.Base;
using MAC.ViewModels.Services;
using MAC.ViewModels.Services.SerialPort;
using MAC.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MAC.ViewModels
{
    public class MainWindowVm : BaseVm
    {
        /// <summary>
        /// Используется для передачи ссылки на данные страницы MainSettingsVm.
        /// </summary>
        public MainSettingsModel MainSettingsModel { get; set; } = new MainSettingsModel();

        public ActiveSettingsModel ActiveMeasurements { get; set; } = new ActiveSettingsModel();
        public ObservableCollection<ComConnectItem> AllComConnect { get; set; }
        private ComConnectItem Fluke => AllComConnect[0];
        private ComConnectItem Comm => AllComConnect[1];

        /// <summary>
        /// Свойства для биндинга интерфейса.
        /// Что бы подсвечивать кнопку выбранной страницы, а так же блокировать повторное нажатие на выбор этой страницы
        /// </summary>
        public SelectedPage SelectedPage { get; set; }

        public string Version { get; set; }
            = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Свойство для переключения вкладок при тестировании.
        /// </summary>
        public int SelectIndexTabControl { get; set; }

        public ObservableCollection<MacResultItem> ActiveMacResultItems { get; set; } =
            new ObservableCollection<MacResultItem>();

        private CancellationTokenSource _ctsTask;
        private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Флаг для того что бы отслеживать, запущено ли тестирование в данные момент.
        /// </summary>
        public bool IsActiveTest { get; set; }

        /// <summary>
        /// Флаг для отображения картинки в начале работы с программой.
        /// </summary>
        public bool VisibilityImage { get; set; } = true;

        public double ContentGridHeight { get; set; }
        public double ContentGridWidth { get; set; }
        public double ContentResultGridHeight { get; set; }

        /// <summary>
        /// Свойство для хранения условий поверки
        /// </summary>
        private MeasurementsData _measurementsData = new MeasurementsData();

        /// <summary>
        /// Свойство для контроля наполнения контента Frame
        /// </summary>
        public UserControl FrameContent { get; set; }

        /// <summary>
        /// Флаг отвечающий, будет ли в тесте учитыватся погрешность для полученных значений.
        /// </summary>
        public bool IsErrorActive { get; set; }

        /// <summary>
        /// Флаг для ожидания завершения задачи, после ее отмены.
        /// </summary>
        public bool IsWaitCancel { get; set; }

        public TypeCancelTask TypeCancelTask { get; set; }

        #region RelayCommand

        #region Смена страниц

        public RelayCommand GoOnMainSettingsCommand =>
            new RelayCommand(ChangePageOnMainSettings, o => SelectedPage != SelectedPage.MainSettings);

        public RelayCommand GoOnMeasurementsSettings => new RelayCommand(ChangePageOnMeasurementsSettings,
            o => SelectedPage != SelectedPage.ActiveSettings);

        public RelayCommand GoOnConnectSettings => new RelayCommand(ChangePageOnConnectSettings,
            o => SelectedPage != SelectedPage.ConnectSettings);

        public RelayCommand GoOnMainWindowCommand =>
            new RelayCommand(ChangePageOnMainWindow, o => SelectedPage != SelectedPage.MainWindow);

        #endregion

        public RelayCommand StartTestSignalControllersCommand => new RelayCommand(StartTestMac);
        public RelayCommand PauseTestSignalControllersCommand { get; set; }
        public RelayCommand StopTestSignalControllersCommand => new RelayCommand(StopTest);
        public RelayCommand RestartTestOneSignalControllerCommand => new RelayCommand(RestartTestOneSignalController);
        public RelayCommand RestartTestChannelCommand => new RelayCommand(RestartTestChannel);
        public RelayCommand RestartTestMeasurementCommand => new RelayCommand(RestartTestMeasurement);

        #endregion

        #region Vm Страниц

        private MainSettingsVm MainSettingsVm { get; set; }

        private MeasurementsSettingsVm MeasurementsSettingsVm { get; set; }

        private ConnectSettingsVm ConnectSettingsVm { get; set; }


        #endregion


        public MainWindowVm()
        {
            SelectedPage = SelectedPage.MainWindow;

            // MAC 2-6 пока отключены, пока не коммутатор не сможет с ними работать
            AllComConnect = new ObservableCollection<ComConnectItem>
            {
                new ComConnectItem(MainConst.NameTypeFluke, 0, Settings.Default.FlukeComPort),
                new ComConnectItem(MainConst.NameTypeComm, 0, Settings.Default.CommComPort),
                new ComConnectItem(MainConst.NameTypeMac, 1, Settings.Default.Mac1ComPort),
                new ComConnectItem(MainConst.NameTypeMac, 2, Settings.Default.Mac2ComPort),
                new ComConnectItem(MainConst.NameTypeMac, 3, Settings.Default.Mac3ComPort),
                new ComConnectItem(MainConst.NameTypeMac, 4, Settings.Default.Mac4ComPort),
                new ComConnectItem(MainConst.NameTypeMac, 5, Settings.Default.Mac5ComPort),
                new ComConnectItem(MainConst.NameTypeMac, 6, Settings.Default.Mac6ComPort)
            };

            MainSettingsVm = new MainSettingsVm(MainSettingsModel, ContentGridHeight, ContentGridWidth, IsActiveTest);
            ConnectSettingsVm = new ConnectSettingsVm(AllComConnect, ContentGridHeight, ContentGridWidth, IsActiveTest);
            MeasurementsSettingsVm =
                new MeasurementsSettingsVm(ActiveMeasurements, ContentGridHeight, ContentGridWidth, IsActiveTest);
        }



        #region Методы смены страниц

        private void ChangePageOnMainSettings()
        {
            FrameContent = new MainSettings(MainSettingsVm);
            SelectedPage = SelectedPage.MainSettings;
        }

        private void ChangePageOnMeasurementsSettings()
        {
            FrameContent = new MeasurementsSettings(MeasurementsSettingsVm);
            SelectedPage = SelectedPage.ActiveSettings;
        }


        private void ChangePageOnConnectSettings()
        {
            FrameContent = new ConnectSettings(ConnectSettingsVm);
            SelectedPage = SelectedPage.ConnectSettings;
        }

        private void ChangePageOnMainWindow()
        {
            FrameContent = null;
            SelectedPage = SelectedPage.MainWindow;
        }

        #endregion

        private void StartTestMac()
        {
            if (CheckValidationFlukeAndCommutator()) return;

            //Создаю класс подключения Fluke and Comm
            var fluke = new FlukeSerialPort(Fluke);
            var comm = new CommutatorSerialPort(Comm);

            //Открываем меню выбора доступных МАС. И выставляем активные для теста.
            if (SetActiveSignalController(comm)) return;

            if (OpenMeasurementsData()) return;

            //Проверка наличия пути сохранения для логов
            CheckPathLog();

            _ctsTask = new CancellationTokenSource();
            var token = _ctsTask.Token;
            //Начало теста

            Task.Run(async () => await Task.Run(() =>
            {
                StartTest(fluke, comm);
            }), token);

        }

        private bool OpenMeasurementsData()
        {
            var setMeasurementsDataVm = new SetMeasurementsDataVm(ref _measurementsData);
            var setMeasurementsData = new SetMeasurementsData(setMeasurementsDataVm);
            setMeasurementsData.ShowDialog();
            var isContinue = setMeasurementsData.ViewModel.IsContinue;
            setMeasurementsData.Close();

            return !isContinue;
        }

        private void SetIsWaitCancelFalse()
        {
            IsWaitCancel = false;
        }

        // ReSharper disable once UnusedMember.Local
        private bool SetActiveSignalController(CommutatorSerialPort comm)
        {
            var selectActiveSignalController =
                new SelectActiveMac(new SelectActiveMacVm(AllComConnect.Skip(2), comm));
            selectActiveSignalController.ShowDialog();
            var isContinue = selectActiveSignalController.ViewModels.IsContinue;
            selectActiveSignalController.Close();
            return !isContinue;
        }

        private void StartTest(FlukeSerialPort fluke, CommutatorSerialPort comm)
        {
            //Получаю список выставленных МАС
            var activeMacResultItems = AllComConnect.Skip(2).Where(item => item.IsActiveTest)
                .Select(item =>
                    new MacResultItem(fluke, comm, item, MainSettingsModel, ActiveMeasurements, TypeCancelTask,
                        SetIsWaitCancelFalse));


            _dispatcher.Invoke(() =>
            {
                //Чистим старый список(если он есть)
                ActiveMacResultItems.Clear();

                ActiveMacResultItems = new ObservableCollection<MacResultItem>(activeMacResultItems);
            });

            IsActiveTest = true;

            if (IsCancellationRequested(_ctsTask)) return;


            for (var i = 0; i < ActiveMacResultItems.Count; i++)
            {
                SelectIndexTabControl = i;
                var macResultItem = ActiveMacResultItems[i];
                try
                {
                    VisibilityImage = false;
                    macResultItem.MainStartMeasurements(_ctsTask, _measurementsData);
                }
                catch (Exception e)
                {
                    macResultItem.IsCheckedNow = false;
                    GlobalLog.Log.Debug(e, e.Message);
                }

                macResultItem.StopTest();

                if (IsCancellationRequested(_ctsTask))
                {
                    IsWaitCancel = false;


                    if (TypeCancelTask == TypeCancelTask.StopTest)
                    {
                        break;
                    }

                    if (TypeCancelTask == TypeCancelTask.RestartSc)
                    {
                        _ctsTask = new CancellationTokenSource();
                        macResultItem.ClearAllResultsValue();
                        i--;
                    }
                }
            }


            //...По окончанию теста, обнулить выбранные МАС.
            foreach (var item in AllComConnect.Skip(2))
                item.IsActiveTest = false;

            IsActiveTest = false;
        }

        #region Набор методов для команд остановки теста, рестарта МАС, канала или измерения

        private void StopTest()
        {
            CtcCancel(TypeCancelTask.StopTest);
        }

        private void RestartTestOneSignalController()
        {
            CtcCancel(TypeCancelTask.RestartSc);
        }

        private void RestartTestChannel(object obj)
        {
            var scItem = obj as MacResultItem;
            CtcCancel(TypeCancelTask.RestartChannel, scItem);
        }

        private void RestartTestMeasurement(object obj)
        {
            var scItem = obj as MacResultItem;
            CtcCancel(TypeCancelTask.RestartMeasurement, scItem);
        }

        private void CtcCancel(TypeCancelTask typeCancelTask, MacResultItem scResultItem = null)
        {
            if (typeCancelTask == TypeCancelTask.RestartSc || typeCancelTask == TypeCancelTask.StopTest)
            {
                TypeCancelTask = typeCancelTask;
                IsWaitCancel = true;
                _ctsTask.Cancel();
            }
            else
            {
                if (scResultItem != null)
                {
                    TypeCancelTask = typeCancelTask;
                    IsWaitCancel = true;
                    scResultItem.TypeCancelTask = typeCancelTask;
                    _ctsTask.Cancel();
                }
                else
                {
                    MessageBox.Show("Ошибка перезапуска тестирования канала!");
                }
            }
        }

        #endregion

        #region Проверка подключенных Fluke and Commutator

        // ReSharper disable once UnusedMember.Local
        private bool CheckValidationFlukeAndCommutator()
        {
            //1.Проверка валидности Fluke Connect
            var checkFlukeConnect = CheckFlukeConnect();
            if (!checkFlukeConnect) return true;

            //2.Проверка валидности Commutator Connect
            var checkCommutatorConnect = CheckCommutatorConnect();
            if (!checkCommutatorConnect) return true;
            return false;
        }

        private bool CheckCommutatorConnect()
        {
            var commutator = AllComConnect[1];
            commutator.CheckComConnect();

            if (commutator.CheckedResult)
                return true;

            var typeError = commutator.ErrorConnect?.GetType();
            if (typeError != null && typeError.Name == new ArgumentException().GetType().Name)
            {
                MessageBox.Show($"Невалидное значение com port для {MainConst.NameTypeComm}");
            }
            else
            {
                MessageBox.Show($"Ошибка подключения {MainConst.NameTypeComm}");
                if (commutator.ErrorConnect != null)
#pragma warning disable Serilog004 // Constant MessageTemplate verifier
                    GlobalLog.Log.Debug(commutator.ErrorConnect, commutator.ErrorConnect.Message);
#pragma warning restore Serilog004 // Constant MessageTemplate verifier
            }

            return false;
        }

        private bool CheckFlukeConnect()
        {
            var fluke = AllComConnect[0];
            fluke.CheckComConnect();

            if (fluke.CheckedResult)
                return true;

            var typeError = fluke.ErrorConnect?.GetType();
            if (typeError != null && typeError.Name == new ArgumentException().GetType().Name)
            {
                MessageBox.Show($"Невалидное значение com port для {MainConst.NameTypeFluke}");
            }
            else
            {
                MessageBox.Show($"Ошибка подключения {MainConst.NameTypeFluke}");
#pragma warning disable Serilog004 // Constant MessageTemplate verifier
                if (fluke.ErrorConnect != null) GlobalLog.Log.Debug(fluke.ErrorConnect, fluke.ErrorConnect.Message);
#pragma warning restore Serilog004 // Constant MessageTemplate verifier
            }

            return false;
        }

        #endregion

        #region Проверка путей сохранения логов и результатов тестированя

        // ReSharper disable once UnusedMember.Local
        private void CheckPathLog()
        {
            var pathSaveResult = MainSettingsModel.FullLogPath;
            if (string.IsNullOrEmpty(pathSaveResult) || pathSaveResult.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                SaveLogDialogPath();
        }


        private void SaveLogDialogPath()
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                MainSettingsModel.FullLogPath = folderBrowser.SelectedPath;
            }
        }

        #endregion

        /// <summary>
        /// Проверка запроса на отмену
        /// </summary>
        /// <param name="ctSource"></param>
        /// <returns></returns>
        private bool IsCancellationRequested(CancellationTokenSource ctSource) =>
            ctSource.Token.IsCancellationRequested;
    }

    /// <summary>
    /// enum отвечающий за бинды на выбранную страницу
    /// </summary>
    public enum SelectedPage
    {
        MainWindow = 0,
        ActiveSettings = 1,
        MainSettings = 2,
        ConnectSettings = 3,
    }

    /// <summary>
    /// enum  для привязки к одному токену, нескольких вариантов действий.
    /// </summary>
    public enum TypeCancelTask
    {
        StopTest,
        RestartSc,
        RestartChannel,
        RestartMeasurement
    }
}