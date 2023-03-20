// WorkerPage.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для WorkerPage.xaml
    /// </summary>
    public partial class WorkerPage : Page
    {
        public static PersonnelEntitiesUs workersEntities { get; set; }
        public static ObservableCollection<Staff> ListStaff { get; set; }
        public static ObservableCollection<Challenger> ListChallenger { get; set; }
        public static ObservableCollection<Salary> ListSalary { get; set; }
        public static ObservableCollection<Position> ListPosition { get; set; }
        Staff _staff;
        private bool isDirty = true;

        public WorkerPage()
        {
            InitializeComponent();
            workersEntities = new PersonnelEntitiesUs();
            ListStaff = new ObservableCollection<Staff>();
            ListChallenger = new ObservableCollection<Challenger>();
            ListSalary = new ObservableCollection<Salary>();
            ListPosition = new ObservableCollection<Position>();
            _staff = new Staff();
            SetUnvisibilityFind(true);
        }        

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/leravodopyanova");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e){ }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetStaff();
            DataGrid.SelectedIndex = 0;
        }

        private void GetStaff()
        {
            var queryEmployee = from staff in workersEntities.Staffs orderby staff.surname select staff;
            foreach (Staff st in queryEmployee)
            {
                ListStaff.Add(st);
            }

            DataGrid.ItemsSource = ListStaff;
        }

        private void cmbPayee_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Positions
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "C_name";
            cmb.SelectedValuePath = "id_p";
        }

        private void cmbPayee1_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Positions
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "oklad";
            cmb.SelectedValuePath = "id_p";
        }

        private void cmbPayee2_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Positions
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "stavka";
            cmb.SelectedValuePath = "id_p";
        }

        private void cmbPayee3_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Salaries
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "sizeofsalary";
            cmb.SelectedValuePath = "id_sal";
        }

        private void cmbPayee4_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Salaries
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "period";
            cmb.SelectedValuePath = "id_sal";
        }

        private void cmbPayee6_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Premiums
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "sizeofpremium";
            cmb.SelectedValuePath = "id_prem";
        }

        private void cmbPayee7_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Premiums
                      select k;
            cmb.ItemsSource = res.ToList();
            cmb.DisplayMemberPath = "C_name";
            cmb.SelectedValuePath = "id_prem";
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteStaff();
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
            Staff staff = workersEntities.Staffs.Create();
            staff.middlename = "не задано";
            staff.phone = "не задано";
            staff.number_p = 0;
            staff.series_p = 0;
            staff.surname = "не задано";
            staff.email = "не задано";
            staff.C_name = "не задано";
            staff.id_p = 5;
            staff.id_prem = 4;
            staff.id_sal = 9;

            try
            {
                workersEntities.Staffs.Add(staff);
                ListStaff.Add(staff);
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
            Staff staff = DataGrid.SelectedItem as Staff;
            if (staff != null)
            {
                workersEntities.Staffs.Remove(staff);
                DataGrid.SelectedIndex =
                DataGrid.SelectedIndex == 0 ? 1 : DataGrid.SelectedIndex - 1;
                ListStaff.Remove(staff);
                workersEntities.SaveChanges();
            }
            else
            {
                System.Windows.MessageBox.Show("Выберите строку для удаления");
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
            RewriteStaff();
            DataGrid.IsReadOnly = false;
            isDirty = false;
            SetUnvisibilityFind(true);
        }

        private void SetUnvisibilityFind(bool st)
        {
            if(st == true)
            {
                ButtonFindBirth.IsEnabled = false;
                ButtonFindEdu.IsEnabled = false;
                ButtonFindPosition.IsEnabled = false;
                ButtonFindSalary.IsEnabled = false;
                ButtonFindStart.IsEnabled = false;
                ButtonFindSurname.IsEnabled = false;
                btnFindAll.IsEnabled = false;
            }
            else
            {
                ButtonFindBirth.IsEnabled = true;
                ButtonFindEdu.IsEnabled = true;
                ButtonFindPosition.IsEnabled = true;
                ButtonFindSalary.IsEnabled = true;
                ButtonFindStart.IsEnabled = true;
                ButtonFindSurname.IsEnabled = true;
                btnFindAll.IsEnabled = true;
            }           
        }

        private void RewriteStaff()
        {
            workersEntities = new PersonnelEntitiesUs();
            ListStaff.Clear();
            GetStaff();
        }

        private void ButtonFindSalary_Click(object sender, RoutedEventArgs e)
        {
            int salary = int.Parse(salaryFrom.Text);
            int id = 9;
            ListStaff.Clear();
            ListSalary.Clear();
            Staff st = DataGrid.SelectedItem as Staff;
            Salary sal = new Salary();
            var querySalary = from s in workersEntities.Salaries
                              where s.sizeofsalary >= salary
                              select s;
            foreach (Salary s in querySalary)
            {
                id = s.id_sal;
                var queryStaff = from staff in workersEntities.Staffs
                                 where staff.id_sal == id
                                 select staff;

                foreach (Staff staff in queryStaff)
                {
                    ListStaff.Add(staff);
                }               
            }
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудник с зарплатой >= \n" + salary + "\n не найден", "Предупреждение", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
        }

        private void ButtonFindSurname_Click(object sender, RoutedEventArgs e)
        {
            var tbx = sender as System.Windows.Controls.TextBox;
            var surname = surnameWorker.Text;
            workersEntities = new PersonnelEntitiesUs();
            ListStaff.Clear();

            var queryportsmen = from staff in workersEntities.Staffs
                                where staff.surname.Contains(surname)
                                select staff;
            foreach (Staff s in queryportsmen)
            {
                ListStaff.Add(s);
            }
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудники с фамилией \n" + surname + "\n не найдены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonFindEdu_Click(object sender, RoutedEventArgs e)
        {
            string education = educationWorker.Text.ToString();
            workersEntities = new PersonnelEntitiesUs();
            ListStaff.Clear();

            var queryportsmen = from staff in workersEntities.Staffs
                                where staff.education.Contains(education)
                                select staff;
            foreach (Staff s in queryportsmen)
            {
                ListStaff.Add(s);
            }
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудники с образованием \n" + education + "\n не найдены", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonFindBirth_Click(object sender, RoutedEventArgs e)
        {
            DateTime? birth = dateofbirth.SelectedDate;
            workersEntities = new PersonnelEntitiesUs();
            ListStaff.Clear();

            var queryportsmen = from staff in workersEntities.Staffs
                                where staff.dateofbirth >= birth
                                select staff;
            foreach (Staff s in queryportsmen)
            {
                ListStaff.Add(s);
            }
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудники с датой рождения от \n" + birth + "\n не найдены", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ButtonFindPosition_Click(object sender, RoutedEventArgs e)
        {
            string position = positionWorker.Text.ToString();
            int id = 5;
            ListStaff.Clear();
            ListPosition.Clear();
            Staff st = DataGrid.SelectedItem as Staff;
            Position sal = new Position();
            var query = from s in workersEntities.Positions
                              where s.C_name.Contains(position)
                              select s;
            foreach (Position p in query)
            {
                id = p.id_p;
                var queryStaff = from staff in workersEntities.Staffs
                                 where staff.id_p == id
                                 select staff;

                foreach (Staff staff in queryStaff)
                {
                    ListStaff.Add(staff);
                }
            }
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудник с должностью \n" + position + "\n не найден", "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }

        private void ButtonFindStart_Click(object sender, RoutedEventArgs e)
        {
            DateTime? start = dateofstartjob.SelectedDate;
            workersEntities = new PersonnelEntitiesUs();
            ListStaff.Clear();

            var queryportsmen = from staff in workersEntities.Staffs
                                where staff.dateofstartjob >= start                               
                                select staff;
            foreach (Staff s in queryportsmen)
            {
                ListStaff.Add(s);
            }
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудники с датой начала работы от \n" + start + "\n не найдены", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _staff = DataGrid.SelectedItem as Staff;
        }

        private void btnFindAll_Click(object sender, RoutedEventArgs e)
        {
            int salary = int.Parse(salaryFrom.Text);
            int id1 = 9, id2 = 5;
            string surname = surnameWorker.Text;
            string education = educationWorker.Text.ToString();
            string position = positionWorker.Text.ToString();
            Staff st = DataGrid.SelectedItem as Staff;
            Salary sal = new Salary();
            Position pos = new Position();
            workersEntities = new PersonnelEntitiesUs();
            ListStaff.Clear();
            ListSalary.Clear();
            ListPosition.Clear();
            DateTime? birth = dateofbirth.SelectedDate;
            DateTime? start = dateofstartjob.SelectedDate;

            var querySalary = from s in workersEntities.Salaries
                              where s.sizeofsalary >= salary
                              select s;
            var queryPos = from s in workersEntities.Positions
                        where s.C_name.Contains(position)
                        select s;
            var queryStaff = from staff in workersEntities.Staffs select staff;
            foreach (Position p in queryPos)
            {
                id2 = p.id_p;

                foreach (Salary s in querySalary)
                {
                    id1 = s.id_sal; 
                    queryStaff = from staff in workersEntities.Staffs
                                 where staff.id_p == id2 && staff.id_sal == id1
                                 && staff.surname.Contains(surname) && staff.education.Contains(education)
                                 && staff.dateofbirth >= birth
                                 && staff.dateofstartjob >= start
                                 select staff;
                }              
                foreach (Staff staff in queryStaff)
                {
                    ListStaff.Add(staff);
                }
            }
            
            if (ListStaff.Count > 0)
            {
                DataGrid.ItemsSource = ListStaff;
            }
            else
                System.Windows.MessageBox.Show("Сотрудник по данным параметрам не найден", "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
        }

        private void positionWorker_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox cmb1 = (System.Windows.Controls.ComboBox)sender;
            var res = from k in workersEntities.Positions
                      select k;
            cmb1.ItemsSource = res.ToList();
            cmb1.DisplayMemberPath = "C_name";
            cmb1.SelectedValuePath = "id_p";
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _staff = DataGrid.SelectedItem as Staff;
        }

        private void btnWord_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            DataGrid.SelectAllCells();
            DataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, DataGrid);
            DataGrid.UnselectAllCells();
            String result = (string)System.Windows.Clipboard.GetData(System.Windows.DataFormats.Dif);
            File.AppendAllText("D:\\workersSaves_" + rnd.Next().ToString() + ".csv", result, UnicodeEncoding.UTF8);
            System.Windows.MessageBox.Show("Файл сохранен, проверьте сохранение в D:\\");
        }
    }
}
