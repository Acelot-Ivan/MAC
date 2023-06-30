using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MAC.Models;
using MAC.ViewModels.Base;

namespace MAC.Style.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для ChannelButton.xaml
    /// </summary>
    public partial class ChannelButton : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion


        /// <summary>
        /// Текс на кнопке
        /// </summary>
        public static readonly DependencyProperty TextButtonDependencyProperty =
            DependencyProperty.Register("TextButton", typeof(string), typeof(ChannelButton));


        /// <summary>
        /// Тестируемый канал
        /// </summary>
        public static readonly DependencyProperty CurrentChannelDependencyProperty =
            DependencyProperty.Register("CurrentChannel", typeof(Channel), typeof(ChannelButton),
                new PropertyMetadata(Channel.None, UpdateIsCurrentEquallyDefault));

        /// <summary>
        /// Канал, где используется данный контрл
        /// </summary>
        public static readonly DependencyProperty DefaultChannelDependencyProperty =
            DependencyProperty.Register("DefaultChannel", typeof(Channel), typeof(ChannelButton),
                new PropertyMetadata(Channel.None, UpdateIsCurrentEquallyDefault));

        /// <summary>
        /// Сюда должна быть передана команда рестарта
        /// </summary>
        public static readonly DependencyProperty RestartChannelTestCommandDependencyProperty =
            DependencyProperty.Register("RestartChannelTestCommand", typeof(RelayCommand), typeof(ChannelButton));


        /// <summary>
        /// Текс на кнопке
        /// </summary>
        public string TextButton
        {
            get => (string)GetValue(TextButtonDependencyProperty);
            set => SetValue(TextButtonDependencyProperty, value);
        }

        /// <summary>
        /// Тестируемый канал
        /// </summary>
        public Channel CurrentChannel
        {
            get => (Channel)GetValue(CurrentChannelDependencyProperty);
            set => SetValue(CurrentChannelDependencyProperty, value);
        }

        /// <summary>
        /// Канал, где используется данный контрл
        /// </summary>
        public Channel DefaultChannel
        {
            get => (Channel)GetValue(DefaultChannelDependencyProperty);
            set => SetValue(DefaultChannelDependencyProperty, value);
        }

        /// <summary>
        /// Сюда должна быть передана команда рестарта
        /// </summary>
        public RelayCommand RestartChannelTestCommand
        {
            get => (RelayCommand)GetValue(RestartChannelTestCommandDependencyProperty);
            set => SetValue(RestartChannelTestCommandDependencyProperty, value);
        }

        private bool _isCurrentEquallyDefault;

        public bool IsCurrentEquallyDefault
        {
            get => _isCurrentEquallyDefault;
            set
            {
                _isCurrentEquallyDefault = value;
                OnPropertyChanged(nameof(IsCurrentEquallyDefault));
            }
        }

        private static void UpdateIsCurrentEquallyDefault(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue) return;

            var channelButton = (d as ChannelButton);
            if (channelButton != null)
            {
                channelButton.IsCurrentEquallyDefault =
                    channelButton.CurrentChannel == channelButton.DefaultChannel;
            }
        }

        public ChannelButton()
        {
            InitializeComponent();
        }
    }
}