using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OneCP
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tb_filepath.Text = Properties.Settings.Default.Path;
        }

        private void btn_SetFilePath_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Path = tb_filepath.Text;
            Properties.Settings.Default.Save();
            Close();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Path))
            {
                MessageBoxResult yesno = MessageBox.Show("No path has been set", "Sure", MessageBoxButton.YesNo);
                if (yesno == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
            else { Close(); }
        }
    }
}
