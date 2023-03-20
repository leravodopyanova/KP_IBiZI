// ChallengerPage.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для ChallengerPage.xaml
    /// </summary>
    public partial class ChallengerPage : Page
    {
        public static PersonnelEntitiesUs workersEntities { get; set; }
        public static ObservableCollection<Staff> ListStaff { get; set; }
        public static ObservableCollection<Challenger> ListChallenger { get; set; }
        public static ObservableCollection<Salary> ListSalary { get; set; }
        public static ObservableCollection<Position> ListPosition { get; set; }
        Challenger _chall;
        private bool isDirty = true;

        public ChallengerPage()
        {
            workersEntities = new PersonnelEntitiesUs();
            ListStaff = new ObservableCollection<Staff>();
            ListChallenger = new ObservableCollection<Challenger>();
            ListSalary = new ObservableCollection<Salary>();
            ListPosition = new ObservableCollection<Position>();
            _chall = new Challenger();            
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/leravodopyanova");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) { }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetChallenger();
            DataGrid.SelectedIndex = 0;
            SetUnvisibilityFind(true);
        }

        private void GetChallenger()
        {
            var queryEmployee = from chal in workersEntities.Challengers orderby chal.surname select chal;
            foreach (Challenger ch in queryEmployee)
            {
                ListChallenger.Add(ch);
            }

            DataGrid.ItemsSource = ListChallenger;
        }

        private void cmbPayee_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var res = from k in workersEntities.Positions
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "C_name";
            cmb.SelectedValuePath = "id_p";
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteChallenger();
            DataGrid.IsReadOnly = true;
            isDirty = true;
        }
        private void EditCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGrid.IsReadOnly = false;
            DataGrid.BeginEdit();
            isDirty = false;
        }
        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Challenger challenger = workersEntities.Challengers.Create();
            challenger.middlename = "не задано";
            challenger.phone = "не задано";
            challenger.surname = "не задано";
            challenger.email = "не задано";
            challenger.C_address = "не задано";
            challenger.C_name = "не задано";
            challenger.id_p = 5;

            try
            {
                workersEntities.Challengers.Add(challenger);
                ListChallenger.Add(challenger);
                isDirty = false;
            }
            catch (DataServiceRequestException ex)
            {
                throw new ApplicationException("Ошибка добавления данных" + ex.ToString());
            }
        }
        private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetUnvisibilityFind(false);
        }
        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            workersEntities.SaveChanges();
            isDirty = true;
            DataGrid.IsReadOnly = true;
        }
        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Challenger challenger = DataGrid.SelectedItem as Challenger;
            if (challenger != null)
            {
                workersEntities.Challengers.Remove(challenger);
                DataGrid.SelectedIndex =
                DataGrid.SelectedIndex == 0 ? 1 : DataGrid.SelectedIndex - 1;
                ListChallenger.Remove(challenger);
                workersEntities.SaveChanges();
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления");
            }
        }

        private void EditCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;
        }
        private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !isDirty;
        }

        private void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteChallenger();
            DataGrid.IsReadOnly = false;
            isDirty = false;
            SetUnvisibilityFind(true);
        }

        private void SetUnvisibilityFind(bool st)
        {
            if (st == true)
            {
                ButtonFindBirth.IsEnabled = false;
                ButtonFindEdu.IsEnabled = false;
                ButtonFindPosition.IsEnabled = false;
                ButtonFindSurname.IsEnabled = false;
                btnFindAll.IsEnabled = false;
            }
            else
            {
                ButtonFindBirth.IsEnabled = true;
                ButtonFindEdu.IsEnabled = true;
                ButtonFindPosition.IsEnabled = true;
                ButtonFindSurname.IsEnabled = true;
                btnFindAll.IsEnabled = true;
            }
        }

        private void RewriteChallenger()
        {
            workersEntities = new PersonnelEntitiesUs();
            ListChallenger.Clear();
            GetChallenger();
        }
                
        private void ButtonFindSurname_Click(object sender, RoutedEventArgs e)
        {
            string surname = surnameChallenger.Text;
            workersEntities = new PersonnelEntitiesUs();
            ListChallenger.Clear();

            var query = from chal in workersEntities.Challengers
                                where chal.surname.Contains(surname)
                                select chal;
            foreach (Challenger ch in query)
            {
                ListChallenger.Add(ch);
            }
            if (ListChallenger.Count > 0)
            {
                DataGrid.ItemsSource = ListChallenger;
            }
            else
                MessageBox.Show("Претенденты с фамилией \n" + surname + "\n не найдены", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonFindEdu_Click(object sender, RoutedEventArgs e)
        {
            string education = educationChallenger.Text.ToString();
            workersEntities = new PersonnelEntitiesUs();
            ListChallenger.Clear();

            var query = from chall in workersEntities.Challengers  
                                where chall.education.Contains(education)
                                select chall;
            foreach (Challenger ch in query)
            {
                ListChallenger.Add(ch);
            }
            if (ListChallenger.Count > 0)
            {
                DataGrid.ItemsSource = ListChallenger;
            }
            else
                MessageBox.Show("Претенденты с образованием \n" + education + "\n не найдены", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonFindBirth_Click(object sender, RoutedEventArgs e)
        {
            DateTime? birth = dateofbirth.SelectedDate;
            workersEntities = new PersonnelEntitiesUs();
            ListChallenger.Clear();

            var query = from ch in workersEntities.Challengers
                                where ch.dateofbirth >= birth
                                select ch;
            foreach (Challenger ch in query)
            {
                ListChallenger.Add(ch);
            }
            if (ListChallenger.Count > 0)
            {
                DataGrid.ItemsSource = ListChallenger;
            }
            else
                MessageBox.Show("Претенденты с датой рождения от \n" + birth + "\n не найдены", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonFindPosition_Click(object sender, RoutedEventArgs e)
        {
            string position = positionChallenger.Text.ToString();
            int id = 5;
            ListChallenger.Clear();
            ListPosition.Clear();
            Challenger ch = DataGrid.SelectedItem as Challenger;
            Position pos = new Position();
            var query = from s in workersEntities.Positions
                        where s.C_name.Contains(position)
                        select s;
            foreach (Position p in query)
            {
                id = p.id_p;
                var queryChall = from chall in workersEntities.Challengers
                                 where chall.id_p == id
                                 select chall;

                foreach (Challenger challenger in queryChall)
                {
                    ListChallenger.Add(challenger);
                }
            }
            if (ListChallenger.Count > 0)
            {
                DataGrid.ItemsSource = ListChallenger;
            }
            else
                MessageBox.Show("Претенденты с должностью \n" + position + "\n не найден", "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
     
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _chall = DataGrid.SelectedItem as Challenger;
        }

        private void btnFindAll_Click(object sender, RoutedEventArgs e)
        {
            int id2 = 5;
            string surname = surnameChallenger.Text;
            string education = educationChallenger.Text.ToString();
            string position = positionChallenger.Text.ToString();
            Challenger st = DataGrid.SelectedItem as Challenger;
            Salary sal = new Salary();
            Position pos = new Position();
            workersEntities = new PersonnelEntitiesUs();
            ListChallenger.Clear();
            ListPosition.Clear();
            DateTime? birth = dateofbirth.SelectedDate;
           
            var queryPos = from s in workersEntities.Positions
                           where s.C_name.Contains(position)
                           select s;
            var queryChall = from chall in workersEntities.Challengers select chall;
            foreach (Position p in queryPos)
            {
                id2 = p.id_p;

                queryChall = from chall in workersEntities.Challengers
                             where chall.id_p == id2
                             && chall.surname.Contains(surname) && chall.education.Contains(education)
                             && chall.dateofbirth >= birth
                             select chall;
                foreach (Challenger chall in queryChall)
                {
                    ListChallenger.Add(chall);
                }
            }

            if (ListChallenger.Count > 0)
            {
                DataGrid.ItemsSource = ListChallenger;
            }
            else
                MessageBox.Show("Претенденты по данным параметрам не найдены", "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }
       
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _chall = DataGrid.SelectedItem as Challenger;
        }      

        private void positionChallenger_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb1 = (ComboBox)sender;
            var res = from k in workersEntities.Positions
                      select k;
            cmb1.ItemsSource = res.ToList();
            cmb1.DisplayMemberPath = "C_name";
            cmb1.SelectedValuePath = "id_p";
        }    

        private void Hire_Click(object sender, RoutedEventArgs e)
        {
            btnHire.IsEnabled = true;
        }

        private void btnHire_Click(object sender, RoutedEventArgs e)
        {
            string postname = "";
            Staff staff = workersEntities.Staffs.Create();
            Challenger challenger = DataGrid.SelectedItem as Challenger;
            staff.middlename = challenger.middlename;
            staff.phone = challenger.phone;
            staff.surname = challenger.surname;
            staff.email = challenger.email;
            staff.C_name = challenger.C_name;
            staff.id_p = challenger.id_p;
            var res = from k in workersEntities.Positions
                      where k.id_p == challenger.id_p
                      select k;
            foreach (Position pos in res)
            {
                postname = pos.C_name;
            }

            try
            {
                MailToChallenger(challenger.email, postname);
                workersEntities.Staffs.Add(staff);
                ListStaff.Add(staff);
                workersEntities.SaveChanges();

                workersEntities.Challengers.Remove(challenger);
                ListChallenger.Remove(challenger);
                workersEntities.SaveChanges();

                isDirty = false;
                MessageBox.Show("Претенденту отправлено письмо о результате подачи заявления на работу!");
            }
            catch (DataServiceRequestException ex)
            {
                throw new ApplicationException("Ошибка добавления данных" + ex.ToString());
            }
        }

        private void btnWord_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            DataGrid.SelectAllCells();
            DataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, DataGrid);
            DataGrid.UnselectAllCells();
            String result = (string)System.Windows.Clipboard.GetData(System.Windows.DataFormats.Dif);
            File.AppendAllText("D:\\challengersSave_" + rnd.Next().ToString() + ".csv", result, UnicodeEncoding.UTF8);
            System.Windows.MessageBox.Show("Файл сохранен, проверьте сохранение в D:\\");
        }

        private void MailToChallenger(string email, string post)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("studentlera22@gmail.com", "Valeria V.");
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Результат подачи заявления на работу";
            // текст письма
            m.Body = "<h2>Поздравляем! Вы приняты к нам на работу на должность " + post + "</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("studentlera22@gmail.com", "change2002");
            smtp.EnableSsl = true;
            smtp.Send(m);
            Console.Read();
        }
    }
}
