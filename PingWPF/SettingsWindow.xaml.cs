using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PingWPF
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow mainWindow = null;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Owner as MainWindow;
            if (mainWindow != null)
            {
                PingTimeout.Text = mainWindow.PingTimeout.ToString();
                if (mainWindow.PingBuffer == null)
                {
                    PingBuffer.Text = "32";
                }
                else
                {
                    PingBuffer.Text = mainWindow.PingBuffer.Length.ToString();
                }
                Ttl.Text = mainWindow.Ttl.ToString();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                int pingTimeout, pingBufferLength, ttl;
                if (int.TryParse(PingTimeout.Text, out pingTimeout))
                {
                    mainWindow.PingTimeout = pingTimeout;
                }
                else
                {
                    MessageBox.Show("Значение максимального времени ожидания ответа " +
                        "на запрос ping было введено неправильно.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    PingTimeout.Text = mainWindow.PingTimeout.ToString();
                    return;
                }
                if (int.TryParse(PingBuffer.Text, out pingBufferLength))
                {
                    byte[] pingBuffer = new byte[pingBufferLength];
                    mainWindow.PingBuffer = pingBuffer;
                }
                else
                {
                    MessageBox.Show("Значение размера буфера для запроса ping " +
                        "было введено неправильно.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mainWindow.PingBuffer == null)
                    {
                        PingBuffer.Text = "32";
                    }
                    else
                    {
                        PingBuffer.Text = mainWindow.PingBuffer.Length.ToString();
                    }
                    return;
                }
                if (int.TryParse(Ttl.Text, out ttl))
                {
                    if (ttl > 0 && ttl < 256)
                    {
                        mainWindow.Ttl = ttl;
                    }
                    else
                    {
                        MessageBox.Show("Значение максимального числа узлов маршрутизации " +
                            "для пакета ping (TTL) было введено неправильно.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        Ttl.Text = mainWindow.Ttl.ToString();
                        return;
                    }
                }
            }
            Close();
        }

        private void Preview_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }
    }
}
