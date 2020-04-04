using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
using WindowsInput;
using WindowsInput.Native;
using MouseButton = System.Windows.Input.MouseButton;

namespace jewDCC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("token.txt"))
                TokenField.Text = File.ReadAllText("token.txt");
            else
                TokenField.Text = "";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void MinimizeButton(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void AboutButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This program is made by jewdev#1337\nI hope you like it~!\n(ﾉ◕ヮ◕)ﾉ*:･ﾟ✧",
                "About",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TypeComboBox.Text) || string.IsNullOrEmpty(NameField.Text) || string.IsNullOrEmpty(TokenField.Text))
            {
                MessageBox.Show("Please fill the fields!",
                    "Error!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                string game;
                if (TypeComboBox.Text.StartsWith("L"))
                    game = "leagueoflegends";
                else if (TypeComboBox.Text.StartsWith("S"))
                    game = "skype";
                else
                    game = "battlenet";

                string url = $"https://discordapp.com/api/v6/users/@me/connections/{game}/{new Random().Next(100000, 1000000)}";
                string data = "{\"name\": \"" + NameField.Text + "\",\n\"visibility\": 1\n}";

                try
                {
                    var request = (HttpWebRequest) WebRequest.Create(url);
                    request.Method = "PUT";
                    request.Headers.Add("authorization", TokenField.Text);
                    request.ContentType = "application/json";
                    byte[] byteArray = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    MessageBox.Show(
                            $"Successfully added a connection!\nName: {NameField.Text}\nType: {TypeComboBox.Text}\n(P.S: If you want to remove it go to Settings > Connections and just remove it)",
                            "Success!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                    if ((bool)SaveTokenCheckBox.IsChecked)
                        File.WriteAllText("token.txt", TokenField.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Beep boop... Error!\n{exception}",
                        "Error!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    throw;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NameField.Focus();
            new InputSimulator().Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.OEM_PERIOD);
        }

        private void TokenField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TokenField.Text.Contains("\""))
                TokenField.Text = TokenField.Text.Replace("\"", "");

            if (string.IsNullOrEmpty(TypeComboBox.Text) || string.IsNullOrEmpty(NameField.Text) ||
                string.IsNullOrEmpty(TokenField.Text))
            {
                SaveTokenCheckBox.IsEnabled = false;
                AddConnectionButton.IsEnabled = false;
            }
            else
            {
                SaveTokenCheckBox.IsEnabled = true;
                AddConnectionButton.IsEnabled = true;
            }
        }


        private void NameField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TypeComboBox.Text) || string.IsNullOrEmpty(NameField.Text) ||
                string.IsNullOrEmpty(TokenField.Text))
            {
                SaveTokenCheckBox.IsEnabled = false;
                AddConnectionButton.IsEnabled = false;
            }
            else
            {
                SaveTokenCheckBox.IsEnabled = true;
                AddConnectionButton.IsEnabled = true;
            }
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TypeComboBox.Text) || string.IsNullOrEmpty(NameField.Text) ||
                string.IsNullOrEmpty(TokenField.Text))
            {
                SaveTokenCheckBox.IsEnabled = false;
                AddConnectionButton.IsEnabled = false;
            }
            else
            {
                SaveTokenCheckBox.IsEnabled = true;
                AddConnectionButton.IsEnabled = true;
            }
        }
    }
}
