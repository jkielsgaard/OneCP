using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace OneCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings set = new Settings();
        string file;
        bool RunThread = true;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Path)) { set.ShowDialog(); }
            file = Properties.Settings.Default.Path;
            RefreshListbox();

            Thread th = new Thread(() => checkfile());
            th.Start();
        }


        #region Context menu
        private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            if (listb_copy.SelectedItem != null)
            {
                Clipboard.SetDataObject(listb_copy.SelectedItem.ToString());
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            List<string> lines = new List<string>();

            foreach (string line in File.ReadAllLines(file)) { lines.Add(line); }
            if (listb_copy.SelectedItem != null) { lines.RemoveAt(listb_copy.SelectedIndex); }

            File.WriteAllText(file, String.Empty);

            foreach (string line in lines) { File.AppendAllText(file, line + Environment.NewLine); }

            RefreshListbox();
        }
        #endregion


        #region Buttons
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            File.AppendAllText(file, Clipboard.GetText() + Environment.NewLine);
            RefreshListbox();
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            set.Show();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            RunThread = false;
            Application.Current.Shutdown();
        }
        #endregion


        #region Functions
        private void checkfile()
        {
            DateTime LastModified_Old = File.GetLastWriteTime(file);
            DateTime LastModified_New = LastModified_Old;

            while (RunThread)
            {
                WaitStat(10);
                LastModified_New = File.GetLastWriteTime(file);

                if (LastModified_New > LastModified_Old)
                {
                    Dispatcher.Invoke(new Action(() => RefreshListbox()));
                    LastModified_New = LastModified_Old;
                }
            }
        }

        private void WaitStat(int Halfsec)
        {
            int i = 0;

            while (i < Halfsec)
            {
                Thread.Sleep(500);
                i++;
                if (!RunThread) { i = Halfsec; }
            }
        }

        private void RefreshListbox()
        {
            listb_copy.Items.Clear();
            foreach (string line in File.ReadAllLines(file))
            {
                listb_copy.Items.Add(line);
            }
        }
        # endregion
    }
}
