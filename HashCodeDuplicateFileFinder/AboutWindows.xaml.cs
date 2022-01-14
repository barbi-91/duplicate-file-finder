using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace HashCodeDuplicateFileFinder
{
    /// <summary>
    /// Interaction logic for AboutWindows.xaml
    /// </summary>
    public partial class AboutWindows : Window
    {
        public AboutWindows()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(Key_PreviewKeyDown);
        }

        #region Event handlers
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathHashCodeDuplicateFileFinder = System.Reflection.Assembly.GetExecutingAssembly().Location;
                lblVersionHCDFF.Content = "HashCodeDuplicateFileFinder " + FileVersionInfo.GetVersionInfo(pathHashCodeDuplicateFileFinder).ProductVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception message: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LnkRepository_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception message: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LnkDocumentation_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder codeBaseUri = new UriBuilder(codeBase);
                string codeBasePath = Uri.UnescapeDataString(codeBaseUri.Path);
                string documentationFilePath = Path.Combine(Path.GetDirectoryName(codeBasePath), "HashCodeDuplicateFileFinderDocumentation.txt");

                if (File.Exists(documentationFilePath))
                {
                    Process.Start(new ProcessStartInfo(documentationFilePath));
                    e.Handled = true;
                }
                else
                    MessageBox.Show("HashCodeDuplicateFileFinderDocumentation.txt in application folder.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception message: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Key_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception message: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
