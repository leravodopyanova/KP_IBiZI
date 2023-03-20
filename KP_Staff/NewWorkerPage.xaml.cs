// NewWorkerPage.xaml.cs
using KP.Actions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для NewWorkerPage.xaml
    /// </summary>
    public partial class NewWorkerPage : Page
    {
        RegAndAuth reg_auth = new RegAndAuth();
        string log;
        string pass;
        string salt;

        public NewWorkerPage(string _log, string _pass, string _salt)
        {
            InitializeComponent();
            log = _log;
            pass = _pass;
            salt = _salt;
        }

        private void dateofbirth_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = dateofbirth.SelectedDate;
            MessageBox.Show(selectedDate.Value.Date.ToShortDateString());
        }

        private void dateofstartjob_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = dateofstartjob.SelectedDate;
            MessageBox.Show(selectedDate.Value.Date.ToShortDateString());
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {        
            string login = log;
            string password = pass;
            string middlename = middlenameWorker.Text.ToString();
            string phone = phoneNum.Text.ToString();
            int number_p = int.Parse(num_p.Text);
            int series_p = int.Parse(seria_p.Text);
            string surname = surnameWorker.Text.ToString();
            string email = emailR.Text;
            string C_name = nameWorker.Text;
            string education = educationWorker.Text;
            DateTime birth = new DateTime();
            DateTime start = new DateTime();
            birth = dateofbirth.SelectedDate.Value;
            start = dateofstartjob.SelectedDate.Value;
            string salt = this.salt;
            string address = "";
            int c = 0;
            MessageBox.Show(salt+ " " + password) ;
            if (phone == "")
            {
                MessageBox.Show("Телефон не задан!");
            }
            else
            {
                if (phone.StartsWith("+7") && phone.Length == 12 && (phone.Contains("1") || phone.Contains("2") || phone.Contains("3") ||
                phone.Contains("4") || phone.Contains("5") || phone.Contains("6") || phone.Contains("7") || phone.Contains("8") ||
                phone.Contains("9") || phone.Contains("0") || phone.Contains("+")))
                {
                    c = 1;                   
                }
                else
                {
                    MessageBox.Show("Номер телефон должен содержать 12 цифр и / или символ + и начинаться на + 7\n Шаблон номера телефона: +79876543210");
                    phoneNum.Text = "";
                }

                if (email.Contains("@") && email.Contains("."))
                {
                    c = c + 1;
                }
                else
                {
                    MessageBox.Show("Адрес электронной почты должен содержать символы @ и(.) точки \n Шаблон адреса: adres@mymail.com");
                    emailR.Text = "";
                }
            }


            if (c == 2)
            {
                if(reg_auth.AddUserInfo("Работник", login, password, C_name, middlename, surname, education, birth, series_p, number_p, start, email, phone, address, salt) == true){
                    MessageBox.Show("Регистрация пройдена успешно. Выполните авторизацию.");
                    grid1.Children.Clear();
                    Frame mainFrame = new Frame();
                    grid1.Children.Add(mainFrame);
                    mainFrame.NavigationService.Navigate(new Uri("/AuthPage.xaml", UriKind.Relative));
                }
                else { MessageBox.Show("Регистрация незавершена."); }
            }
            else { MessageBox.Show("Регистрация незавершена."); }
        }  
    }
}
