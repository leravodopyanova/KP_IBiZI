// MainWindow.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Handler(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
                
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            GridFull.Children.Clear();
            Frame mainFrame = new Frame();
            GridFull.Children.Add(mainFrame);
            mainFrame.NavigationService.Navigate(new Uri("/AuthPage.xaml", UriKind.Relative));
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            GridFull.Children.Clear();
            Frame mainFrame = new Frame();
            GridFull.Children.Add(mainFrame);
            mainFrame.NavigationService.Navigate(new Uri("/RegPage.xaml", UriKind.Relative));
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/leravodopyanova");
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            GridFull.Children.Clear();
        }
    }
}
