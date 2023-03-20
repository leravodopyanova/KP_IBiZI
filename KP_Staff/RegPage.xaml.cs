// RegPage.xaml.cs
using KP.Actions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        RegAndAuth regAndAuth;
        string log;
        string pass;
        public AuthPage()
        {
            InitializeComponent();
            regAndAuth = new RegAndAuth();
        }
       
        private void btnRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            CryptPassword crypt = new CryptPassword();
            string role = "";
            log = loginReg.Text.ToString();
            pass = passwordReg.Password.ToString();
            //pass = crypt.Compute(pass);
            string salt = crypt.GenerateSalt();
            pass = crypt.Compute(pass,salt);
            NavigationService n = NavigationService.GetNavigationService(this);
            NewWorkerPage newWorkerPage = new NewWorkerPage(log, pass, salt);
            NewChallengerPage newChallengerPage = new NewChallengerPage(log, pass, salt);

            if (workerRole.IsChecked == true)
            {
                role = "Работник";
            }
            if(challengerRole.IsChecked == true)
            {
                role = "Претендент";
            }

            if (regAndAuth.AddUser(role, log, pass, salt) == true)
            {                
                g1.Children.Clear();
                Frame mainFrame = new Frame();
                g2.Children.Add(mainFrame);

                if (role == "Работник")
                {
                    mainFrame.NavigationService.Navigate(newWorkerPage, UriKind.Relative);
                }

                if (role == "Претендент")
                {
                    mainFrame.NavigationService.Navigate(newChallengerPage, UriKind.Relative);
                }
            }
        }
    }
}
