using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FolderFile
{
    sealed partial class FileAndFolderViewer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        ObservableCollection<ClassListStroce> _ListCol = new ObservableCollection<ClassListStroce>();
        public StorageFolder storageFolderFirst
        { get; set; }
        ObservableCollection<ClassListStroce> _ListCol2 = new ObservableCollection<ClassListStroce>();
        public StorageFolder storageFolderFirst2 { get; set; }
        public ObservableCollection<ClassListStroce> ListCol
        { get
            {
                return this._ListCol;
            }

            set
            {
                _ListCol = value;
               
            }
        }
        public ObservableCollection<ClassListStroce> ListCol2
        {
            get
            {
                return this._ListCol2;
            }

            set
            {
                _ListCol2 = value;
            }
        }

        ObservableCollection<string> _ListColName = new ObservableCollection<string>();
        public ObservableCollection<string> ListColName
        {
            get
            {
                return this._ListColName;
            }

            set
            {
                _ListColName = value;
            }
        }
    
      
        ObservableCollection<string> _ListColName2 = new ObservableCollection<string>();
        public  ObservableCollection<string> ListColName2
        {
            get
            {
             
                return this._ListColName2;
            }

            set
            {
                _ListColName2 = value;
            }
        }
        string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                this.OnPropertyChanged();
            }
        }
        string path2;
        public string Path2
        {
            get
            {
                return path2;
            }
            set
            {
                path2 = value;
                this.OnPropertyChanged();
            }
        }
        public List<StorageFolder> ListUSB = new List<StorageFolder>();

        public async void Nazad()
        {
            try
            {
                try
                {
                    if (storageFolderFirst != null)
                    {
                        StorageFolder storageFolder = await storageFolderFirst.GetParentAsync();
                        if (storageFolder != null)
                        {


                          Path = storageFolder.Path;



                            IReadOnlyList<StorageFolder> folderList =
                              await storageFolder.GetFoldersAsync();


                            storageFolderFirst = storageFolder;
                           ListCol.Clear();
                            // fileAndFolderViewer.ListColName.Clear();
                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                                var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail, Type = Type });
                                //fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                            }
                            IReadOnlyList<StorageFile> fileList =
                          await storageFolder.GetFilesAsync();
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                                var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail1, Type = Type });
                                // fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                            }
                        }
                        else
                        {
                            ListCol.Clear();
                            // fileAndFolderViewer.ListColName.Clear();
                            // InitializeDataGridView1();
                            InitializeDataGridView();
                        }
                    }
                    else
                    {
                        ListCol.Clear();
                        // fileAndFolderViewer.ListColName.Clear();
                        //  InitializeDataGridView1();
                        InitializeDataGridView();
                    }
                }
                catch (Exception ex)
                {
                 ListCol.Clear();
                    // fileAndFolderViewer.ListColName.Clear();
                    // InitializeDataGridView1();
                }




            }
            catch (Exception ex)
            {

            }
        }
        List<String> listDostyp = new List<string>();
        public async void InitializeDataGridView()
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            try
            {

                ListCol.Clear();
              storageFolderFirst = null;
                StorageFolder DocumentFolder = KnownFolders.DocumentsLibrary;
                var thumbnaild = await DocumentFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 200);
                ListCol.Add(new ClassListStroce() { StorageFolder = DocumentFolder, FlagFolde = true, ThumbnailMode = thumbnaild, Type = Type });

                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                var thumbnail = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 300);
                ListCol.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnail, Type = Type });

                // Get Music library.
                StorageFolder musicFolder = KnownFolders.MusicLibrary;
                var thumbnail1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 400);
               ListCol.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnail1, Type = Type });

                var storageItemAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
                if (storageItemAccessList.Entries.Count != 0)
                {
                    foreach (Windows.Storage.AccessCache.AccessListEntry entry in storageItemAccessList.Entries)
                    {
                        string mruToken = entry.Token;
                        string mruMetadata = entry.Metadata;
                        Windows.Storage.IStorageItem item = await storageItemAccessList.GetItemAsync(mruToken);
                        listDostyp.Add(mruToken);
                        StorageFolder ss = await storageItemAccessList.GetFolderAsync(mruToken);
                        var thumbnail11 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 500);
                        ListCol.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail11, Type = Type });

                    }

                }

                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.DriveType == DriveType.Fixed)
                    {
                        StorageFolder FolderL = await StorageFolder.GetFolderFromPathAsync(drive.Name);

                        var thumbnailL = await FolderL.GetThumbnailAsync(ThumbnailMode.SingleItem, 50);
                       ListCol.Add(new ClassListStroce() { StorageFolder = FolderL, FlagFolde = true, ThumbnailMode = thumbnailL, Type = Type });
                    }
                }


                StorageFolder Folder = KnownFolders.RemovableDevices;

                IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                foreach (StorageFolder FlFolder1 in folderList)
                {
                    thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                    ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type = Type });
                }
               Path = String.Empty;



            }
            catch (UnauthorizedAccessException ex)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                MessageDialog messageDialog = new MessageDialog(resourceLoader.GetString("MessageContentDostup"), resourceLoader.GetString("MessageTiletDostup"));
                await messageDialog.ShowAsync();
                App.Current.Exit();
            }


        }
        public async void Upgreid()
        {
                    try
                    {
                        StorageFolder storageFolder = storageFolderFirst;
                        if (storageFolder != null)
                        {


                            IReadOnlyList<StorageFolder> folderList =
                               await storageFolder.GetFoldersAsync();
                            IReadOnlyList<StorageFile> fileList =
                              await storageFolder.GetFilesAsync();
                           
                            storageFolderFirst = storageFolder;
                            ListCol.Clear();
                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                                var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type= Type });
                            }
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                                var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail1, Type = Type });
                            }
                        }
                        else
                        {
                           ListCol.Clear();
                            InitializeDataGridView();
                        }
                 
                    }
                    catch (Exception ex)
                    {
                     
                     ListCol.Clear();
                        InitializeDataGridView();
                    }
        }
        public async Task ActivateFile(StorageFile storageFile)
        {
            var success = await Windows.System.Launcher.LaunchFileAsync(storageFile);


            if (success)
            {
                // File launched
            }
            else
            {
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = true;

                // Launch the retrieved file
                bool success1 = await Windows.System.Launcher.LaunchFileAsync(storageFile, options);
                if (success1)
                {
                    // File launched
                }
                else
                {

                }
            }
        }
        public async void Next()
        {
            if (classListStroceSelect != null)
            {
                if (classListStroceSelect.ElementAt(0).FlagFolde)
                {


                    Path = classListStroceSelect.ElementAt(0).StorageFolder.Path;
                    IReadOnlyList<StorageFolder> folderList = await FileAndFolderViewer.classListStroceSelect.ElementAt(0).StorageFolder.GetFoldersAsync();
                    IReadOnlyList<StorageFile> fileList = await FileAndFolderViewer.classListStroceSelect.ElementAt(0).StorageFolder.GetFilesAsync();

                    storageFolderFirst = FileAndFolderViewer.classListStroceSelect.ElementAt(0).StorageFolder;
                    ListCol.Clear();
                   
                    foreach (StorageFolder FlFolder1 in folderList)
                    {
                        var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 200);
                        ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail, Type = Type });
                        ListColName.Add(FlFolder1.DisplayName);
                    }
                    foreach (StorageFile FlFolder1 in fileList)
                    {
                        var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 200);
                       ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail, Type = Type });
                        ListColName.Add(FlFolder1.DisplayName);
                    }
                    //_DataGrid.SelectedIndex = 0;
                    FileAndFolderViewer.classListStroceSelect = null;

                }
                else
                {
                    await ActivateFile(classListStroceSelect.ElementAt(0).storageFile);
                }
            }
        }


    }
}
