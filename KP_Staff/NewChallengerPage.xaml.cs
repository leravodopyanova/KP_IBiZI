// NewChallengerPage.xaml.cs
using KP.Actions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для NewChallengerPage.xaml
    /// </summary>
    public partial class NewChallengerPage : Page
    {
        string log;
        string pass;
        string salt;
        RegAndAuth reg_auth = new RegAndAuth();

        public NewChallengerPage(string _log, string _pass, string _salt)
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
       
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            string login = log;
            string password = pass;
            string middlename = middlenameCh.Text.ToString();
            string phone = phoneNumCh.Text.ToString();
            string surname = surnameCh.Text.ToString();
            string email = emailCh.Text;
            string C_name = nameCh.Text;
            string education = educationCh.Text;
            DateTime birth = new DateTime();
            DateTime start = new DateTime();
            start = dateofbirth.SelectedDate.Value;
            birth = dateofbirth.SelectedDate.Value;
            string address = addressCh.Text;
            string salt = this.salt;
            int c = 0;

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
                    phoneNumCh.Text = "";
                }

                if (email.Contains("@") && email.Contains("."))
                {
                    c = c + 1;
                }
                else
                {
                    MessageBox.Show("Адрес электронной почты должен содержать символы @ и(.) точки \n Шаблон адреса: adres@mymail.com");
                    emailCh.Text = "";
                }
            }

            if (c == 2)
            {
                if (reg_auth.AddUserInfo("Претендент", login, password, C_name, middlename, surname, education, birth, 0, 0, start, email, phone, address, salt) == true)
                {
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
