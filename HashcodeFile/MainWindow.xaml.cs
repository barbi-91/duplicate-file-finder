using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
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


using Microsoft.WindowsAPICodePack.Dialogs;

using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;


namespace HashcodeFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Fields

        private const string ERROR = "Error";
        private const string INFORMATION = "Information";
        private const string WARNING = "Warning";

        private int filesCount;
        private string[] allPaths;
        private bool isTrue;
        //byte[] fileContentBytesfromPath;

        //private string[] allNames;
        //private List<long> filesInSize;

        #endregion fields


        #region Event

        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
                openDialog.IsFolderPicker = true;

                if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string path = openDialog.FileName;
                    txtblockPath.Background = Brushes.LightGray;
                    txtblockPath.Foreground = Brushes.Green;
                    txtblockPath.Text = path;

                    allPaths = Directory.GetFiles(openDialog.FileName);
                    filesCount = allPaths.Length;
                    isTrue = (filesCount >= 1);
                    if (isTrue)
                    {
                        MessageBox.Show("Folder is selected, ready for calculate files Hash!", "Information", MessageBoxButton.OK);
                        btnCalculateHash.Focus();
                    }
                    else
                        MessageBox.Show("Folder is empty, please select another folder!", "Information", MessageBoxButton.OK);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //button2 -hash calculate
        private void btnCalculateHash_Click(object sender, RoutedEventArgs e)
        {
            if (isTrue)
            {
                foreach (string pathItem in allPaths)
                {
                    //filename
                    string fileNamefromPath = System.IO.Path.GetFileName(pathItem);

                    ////file content Size in bytes for MD5
                    byte[] fileContentBytesSizefromPath = File.ReadAllBytes(pathItem);

                    //VELICINA U BAJTIMA for ctor-property long size
                    FileInfo infoSize = new FileInfo(pathItem);
                    long fileByteSizefromPath = infoSize.Length;

                    //file hash from content string
                    string fileHashContentfromPath = CreateMD5(fileContentBytesSizefromPath);


                    ////ctor
                    FileHash individualFile = new FileHash(fileNamefromPath, fileHashContentfromPath, pathItem, fileByteSizefromPath);
                    ///add item in listview
                    lstTabelInfo.Items.Add(individualFile);

                    if (isTrue)
                        lblFilesCount.Content = string.Concat("Contains: ", filesCount, " files");
                    else
                        lblFilesCount.Content = string.Concat("Contains: ", filesCount, " file");

                }
            }
            else
            {
                MessageBox.Show("Folder is empty, please select another folder!", "Information", MessageBoxButton.OK);
                btnSelectFolder.Focus();
            }
            
        }
        #endregion event
        //sadrzaj u hash, hash u x2
        //hash uzima niz bajtova sadrzaja filea
        public string CreateMD5(byte[] NizsadrzajFilea)
        {
            MD5 md5 = MD5.Create();
            //bajtove sadrzaja pretvara u hash bytove
            byte[] NizhashUbaytima = md5.ComputeHash(NizsadrzajFilea);

            // Konvertiranje Niza u baytima hasha u hexadecimalni X2 string pomocu Stringbiledra
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < NizhashUbaytima.Length; i++)
            {
                sb.Append(NizhashUbaytima[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void ClearMaster()
        {
            txtblockPath.Text = string.Empty;
            lstTabelInfo.Items.Clear();
            btnSelectFolder.Focus();
        }

        private void btnCLearAll_Click(object sender, RoutedEventArgs e)
        {
            ClearMaster();
        }
    }
}

    

    
    //kako radi filename dohvacenje:
    //public void Test(string fileName)
    //{
    //    string path = Path.GetDirectoryName(fileName);
    //    string filename_with_ext = Path.GetFileName(fileName);
    //    string filename_without_ext = Path.GetFileNameWithoutExtension(fileName);
    //    string ext_only = Path.GetExtension(fileName);
    //}