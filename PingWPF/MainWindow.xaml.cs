using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyPingLibrary;

namespace PingWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PingSender pingSender;
        private bool pingCancellation = false;

        public int PingTimeout { get; set; } = 1000;
        public byte[] PingBuffer { get; set; } = null;
        public int Ttl { get; set; } = 128;

        public MainWindow()
        {
            InitializeComponent();
            pingSender = new PingSender();
        }

        private async void PingButton_Click(object sender, RoutedEventArgs e)
        {
            DisableButton(PingButton);
            EnableButton(StopButton);
            Results.IsHitTestVisible = false;
            Results.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Results.Text = "";
            while (pingCancellation == false)
            {
                Results.Text += await pingSender.SendRequestAsync(Address.Text,
                    PingTimeout, PingBuffer, Ttl);
                if (Results.LineCount > 51)
                {
                    int indexOfSecondResult = Results.Text.IndexOf(" Ответ", 10);
                    Results.Text = Results.Text.Substring(indexOfSecondResult);
                }
                Results.ScrollToEnd();
                Statistics.Text = await pingSender.GetPingStatisticsAsync();
            }
            Results.IsHitTestVisible = true;
            Results.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            pingCancellation = false;
            EnableButton(PingButton);
            pingSender.ClearStatistics();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            DisableButton(StopButton);
            pingCancellation = true;
        }

        private void EnableButton(Button button)
        {
            button.IsEnabled = true;
            button.Visibility = Visibility.Visible;
            button.IsDefault = true;
        }

        private void DisableButton(Button button)
        {
            button.IsDefault = false;
            button.IsEnabled = false;
            button.Visibility = Visibility.Hidden;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow
            {
                Owner = this
            };
            settingsWindow.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow
            {
                Owner = this
            };
            aboutWindow.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
