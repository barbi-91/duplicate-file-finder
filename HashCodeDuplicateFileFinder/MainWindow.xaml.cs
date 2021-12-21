using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.XPath;


using Microsoft.WindowsAPICodePack.Dialogs;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

using MenuItem = System.Windows.Forms.MenuItem;

namespace HashCodeDuplicateFileFinder

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Fields
        private const string ERROR = "Error";
        private const string INFORMATION = "Information";
        private const string WARNING = "Warning";

        private int _countFiles;
        private int _fileIdenticalSizeAndHashes = 0;
        private int _filesCount;
        private string[] _allPaths;
        private bool _isTrue;
        private List<FileHashes> _filesListInfo = new List<FileHashes>();
        private List<FilesGroupedByHash> _data = new List<FilesGroupedByHash>();
        //dictionary fill list with size (unique or identical hashes), key is size
        private Dictionary<long, List<string>> dictionaryIdenticalSize = new Dictionary<long, List<string>>();
        //dictionary fill list with hashes (unique or identical), key is hash
        private Dictionary<string, List<string>> dictionaryIdenticalHash = new Dictionary<string, List<string>>();
        #endregion fields

        #region Event
        //button1 -select folder 
        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
                openDialog.IsFolderPicker = true;

                if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string path = openDialog.FileName;
                    txtblockPath.Background = Brushes.LightGreen;
                    txtblockPath.Foreground = Brushes.Green;
                    txtblockPath.Text = path;
                    _allPaths = Directory.GetFiles(openDialog.FileName);
                    _filesCount = _allPaths.Length;
                    _isTrue = (_filesCount >= 1);
                    if (_isTrue)
                    {
                        //System.Windows.MessageBox.Show("Folder is selected, ready for calculate files Hash!", "Information", MessageBoxButton.OK);
                        BtnColorReseterAndEnabler(Brushes.Green, Brushes.Red, Brushes.Red, true, false, false);
                    }
                    else
                        System.Windows.MessageBox.Show("Folder is empty, please select another folder to check for duplicates!", "Folder do not contain files", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //button2 -identical hash(identical size) calculate
        private void btnCalculateHash_Click(object sender, RoutedEventArgs e)
        {
            if (_isTrue)
            {
                BtnColorReseterAndEnabler(Brushes.Red, Brushes.Red, Brushes.Green, false, false, true);
                btnDelete.Visibility = Visibility.Visible;
                btnDelete.BorderBrush = Brushes.Green;

                foreach (string pathItem in _allPaths)
                {
                    ////BYTE SIZE
                    FileInfo infoSize = new FileInfo(pathItem);
                    long fileByteSizefromPath = infoSize.Length;
                    
                    //fil dictionary by size (key: file size, value: path)
                    if (dictionaryIdenticalSize.ContainsKey(fileByteSizefromPath))
                    {
                        dictionaryIdenticalSize[fileByteSizefromPath].Add(pathItem);
                    }
                    else
                    {
                        dictionaryIdenticalSize.Add(fileByteSizefromPath, new List<string> { pathItem });
                    }
                }
                //single out pairs with multiple values
                foreach (var pairItem in dictionaryIdenticalSize)
                {
                    int count = pairItem.Value.Count();
                    bool isUniqueSize = count == 1;
                    //elimination all pairs with only one value(path)
                    if (isUniqueSize)
                    {
                        //not to disturb the counter in loop -continue
                        continue;
                    }
                    else
                    {
                        _countFiles = 0;
                        foreach (string pathItem in pairItem.Value)
                        {
                            //////file content Size in bytes for MD5
                            byte[] fileContentBytesSizefromPath = File.ReadAllBytes(pathItem);

                            //file hash from content string
                            string fileHashContentfromPath = CreateMD5(fileContentBytesSizefromPath);

                            //fil dictionary by Hash (key: hash, value: path)
                            if (dictionaryIdenticalHash.ContainsKey(fileHashContentfromPath))
                            {
                                dictionaryIdenticalHash[fileHashContentfromPath].Add(pathItem);
                            }
                            else
                            {
                                dictionaryIdenticalHash.Add(fileHashContentfromPath, new List<string> { pathItem });
                            }
                        }
                    }
                }
                //filter by identical hash
                foreach (var pairItem in dictionaryIdenticalHash)
                {
                    int count = pairItem.Value.Count();
                    bool isUniqueHash = count == 1;
                    //elimination all pairs with only one value(path)
                    if (isUniqueHash)
                    {
                        //dictionaryIdenticalHashidenticalSize.Remove(pairItem.Key); 
                        continue;
                    }
                    else
                    {
                        _countFiles = 0;
                        foreach (string pathItem in pairItem.Value)
                        {
                            ////Byte size for ctor-property long size
                            FileInfo infoSize = new FileInfo(pathItem);
                            long fileByteSizefromPath = infoSize.Length;

                            ////filename
                            string fileNamefromPath = System.IO.Path.GetFileName(pathItem);

                            //////file content Size in bytes for MD5
                            byte[] fileContentBytesSizefromPath = File.ReadAllBytes(pathItem);

                            //file hash from content string
                            string fileHashContentfromPath = CreateMD5(fileContentBytesSizefromPath);

                            //file info last modified
                            DateTime lastModified = File.GetLastWriteTime(pathItem);

                            //////ctor
                            FileHashes individualFile = new FileHashes(fileNamefromPath, fileHashContentfromPath, pathItem, fileByteSizefromPath, lastModified);

                            //group by single property- hash add to listview
                            _filesListInfo.Add(individualFile);

                            //file counter
                            _countFiles += 1;
                        }
                        
                    }
                    _fileIdenticalSizeAndHashes = _fileIdenticalSizeAndHashes + _countFiles;
                    bool HaveMoreThanOne = (_fileIdenticalSizeAndHashes >= 1);
                    if (HaveMoreThanOne)
                        lblFilesCount.Content = string.Concat("Contains: ", _fileIdenticalSizeAndHashes, " duplicate files");
                    else
                        lblFilesCount.Content = string.Concat("Contains: ", _fileIdenticalSizeAndHashes, " duplicate file");
                }

                var _data = GroupBySingleProperty(_filesListInfo);
                lstTableInfo.ItemsSource = _data;

                bool NoFileInFolder = (_fileIdenticalSizeAndHashes == 0);
                if (NoFileInFolder)
                {
                    lblFilesCount.Content = string.Concat("Contains: 0 duplicate file in folder");
                    btnDelete.Visibility = Visibility.Hidden;
                    System.Windows.MessageBox.Show("There is no duplicate file in the folder!");
                    ClearMaster();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Folder is not selected, please select folder!", "Information", MessageBoxButton.OK);
                btnSelectFolder.Focus();
            }
        }

        //button 3 Delete from table and from data list
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<FileHashes> deletingListFH = new List<FileHashes>();
            List<FilesGroupedByHash> deleteGroupbyHash = new List<FilesGroupedByHash>();
            var counterGL = 0;
            var counterDel = 0;

            var _data = GroupBySingleProperty(_filesListInfo);
            foreach (var d in _data)
            {
                foreach (var hgl in d.HeshesGroupListGP)
                {
                    counterGL = d.HeshesGroupListGP.Count;
                    if (hgl.IsChecked)
                    {

                        deletingListFH.Add(hgl);
                        File.Delete(hgl.PathP);
                    }
                }
                counterDel = deletingListFH.Count();
                counterGL = counterGL - counterDel;
                if (counterGL < 2)
                {
                    deleteGroupbyHash.Add(d);
                }
                else
                {
                    foreach (var l in deletingListFH)
                    {
                        d.HeshesGroupListGP.Remove(l);
                    }
                }
                deletingListFH = new List<FileHashes>();
                counterGL = 0;
                counterDel = 0;
            }
            foreach (var dl in deleteGroupbyHash)
{
                _data.Remove(dl);
            }
            lstTableInfo.ItemsSource = null;
            lstTableInfo.ItemsSource = _data;

            deleteGroupbyHash = new List<FilesGroupedByHash>();
            bool isEmptyList = _data.Count < 1;
            System.Windows.MessageBox.Show("Duplicate file/s deleted!", "Information", MessageBoxButton.OK);

            if (isEmptyList)
            {
                btnDelete.Visibility = Visibility.Hidden;
                ClearMaster();
            }
            lblFilesCount.Content = ($"Contains:  {lstTableInfo.Items.Count} group/s with identical content in each group (duplicate files)");
        }
        //button 4 clear table and reset all variables
        private void btnCLearAll_Click(object sender, RoutedEventArgs e)
        {
            ClearMaster();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {

            string copyText = txtblockPath.Text.ToString();
            if (copyText != null)
            {
                System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, (object)copyText);
            }
        }
        #endregion event

        #region method
        //method -file hash calculate
        public string CreateMD5(byte[] ArrayFileContent)
        {
            MD5 md5 = MD5.Create();
            // content byte convert to hash byte
            byte[] NizhashUbaytima = md5.ComputeHash(ArrayFileContent);

            //convert array in hash bayts in hexadecimal X2 string  with Stringbiledr
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < NizhashUbaytima.Length; i++)
            {
                sb.Append(NizhashUbaytima[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void ClearMaster()
        {
            txtblockPath.Text = "";
            txtblockPath.Foreground = Brushes.Purple;
            txtblockPath.Background = Brushes.LavenderBlush;
            lstTableInfo.ItemsSource = null;
            btnSelectFolder.Focus();
            lblFilesCount.Content = "Contains: ";

            _countFiles = 0;
            _fileIdenticalSizeAndHashes = 0;
            _filesCount = 0;
            _allPaths = null;
            _isTrue = false;
            dictionaryIdenticalHash = new Dictionary<string, List<string>>();
            dictionaryIdenticalSize = new Dictionary<long, List<string>>();
            _filesListInfo = new List<FileHashes>();
            _data = new List<FilesGroupedByHash>();

            BtnColorReseterAndEnabler(Brushes.Red, Brushes.Green, Brushes.Red, false, true, false);
            btnDelete.Visibility = Visibility.Hidden;
        }

        private List<FilesGroupedByHash> GroupBySingleProperty(List<FileHashes> filesHashesList)
        {
            List<FilesGroupedByHash> queryHash =
                (from file in filesHashesList
                 group file by file.HashP into newGroup
                 orderby newGroup.Key
                 //select newGroup;
                 select new FilesGroupedByHash(newGroup.Key, newGroup.ToList()))
                .ToList();
            return queryHash;
        }

        ////method button border color reset and enabler
        public void BtnColorReseterAndEnabler(SolidColorBrush colorBtnCalculateHash, SolidColorBrush colorBtnSelectFolder, SolidColorBrush colorBtnClearAll, bool enableBtnCalculateHash, bool enableBtnSelectFolder, bool enableBtnClear)
        {
            btnCalculateHash.BorderBrush = colorBtnCalculateHash;
            btnCalculateHash.IsEnabled = enableBtnCalculateHash;
            btnSelectFolder.BorderBrush = colorBtnSelectFolder;
            btnSelectFolder.IsEnabled = enableBtnSelectFolder;
            btnCLearAll.BorderBrush = colorBtnClearAll;
            btnCLearAll.IsEnabled = enableBtnClear;
        }
        #endregion method
    }
}
