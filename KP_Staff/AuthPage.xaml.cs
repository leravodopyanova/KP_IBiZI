// AuthPage.cs
using KP.Actions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        RegAndAuth regAndAuth;
        public RegPage()
        {
            InitializeComponent();
            regAndAuth = new RegAndAuth();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void btnAuthPage_Click(object sender, RoutedEventArgs e)
        {
            CryptPassword crypt = new CryptPassword();
            string log = logAuth.Text.ToString();
            string pass = passAuth.Password.ToString();

            MessageBox.Show(regAndAuth.CheckUserSalt(log));
            pass = crypt.Compute(pass, regAndAuth.CheckUserSalt(log));
            MessageBox.Show(crypt.Compare(regAndAuth.CheckhashPsss(log), pass).ToString());

            if (crypt.Compare(regAndAuth.CheckhashPsss(log), pass) && regAndAuth.AuthUser(log, pass) == 1)
            {
                gridAuth1.Children.Clear();
                Frame mainFrame = new Frame();
                gridFull.Children.Add(mainFrame);           
                mainFrame.NavigationService.Navigate(new Uri("/WorkerPage.xaml", UriKind.Relative));
            }
            else if (crypt.Compare(regAndAuth.CheckhashPsss(log), pass) && (regAndAuth.AuthUser(log, pass) == 2))
            {
                gridAuth1.Children.Clear();
                Frame mainFrame = new Frame();
                gridFull.Children.Add(mainFrame);
                mainFrame.NavigationService.Navigate(new Uri("/ChallengerPage.xaml", UriKind.Relative));
            }
            else if (crypt.Compare(regAndAuth.CheckhashPsss(log), pass) == false && regAndAuth.AuthUser(log, pass) == 0)
            {
                System.Windows.MessageBox.Show("Пользователь с таким логином и паролем не существует. Введите другие учетные данные или перейдите к Регистрации.");
                auth.Children.Clear();
                Frame mainFrame = new Frame();
                gridFull.Children.Add(mainFrame);
                mainFrame.NavigationService.Navigate(new Uri("/AuthPage.xaml", UriKind.Relative));
            }
        }
    }
}
