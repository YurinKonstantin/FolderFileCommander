using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public StorageFolder storageFolderFirst { get; set; }

        
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
        bool simofor = false;
      

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
    
        public List<StorageFolder> ListUSB = new List<StorageFolder>();

        public async void Nazad()
        {
            if (simofor == false)
            {
                simofor = true;
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
                                IReadOnlyList<StorageFolder> folderList = await storageFolder.GetFoldersAsync();
                                IReadOnlyList<StorageFile> fileList = await storageFolder.GetFilesAsync();
                                ListCol.Clear();

                                storageFolderFirst = storageFolder;
                                foreach (StorageFolder FlFolder1 in folderList)
                                {

                                    if (Path == (await FlFolder1.GetParentAsync()).Path)
                                    {
                                        var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                        ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail, Type = Type });
                                    }
                                }

                                foreach (StorageFile FlFolder1 in fileList)
                                {
                                    if (Path == (await FlFolder1.GetParentAsync()).Path)
                                    {
                                        var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                        ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail1, Type = Type });
                                    }
                                }
                            }
                            else
                            {
                                ListCol.Clear();

                                InitializeDataGridView();
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
                        simofor = false;

                    }




                }
                catch (Exception ex)
                {
                    simofor = false;
                }
                simofor = false;
            }

        }
       public ObservableCollection<String> listDostyp = new ObservableCollection<string>();
        public bool dr = false;
        public async void InitializeDataGridView()
        {
           
            listDostyp.Clear();
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            try
            {

                ListCol.Clear();
              storageFolderFirst = null;
                try
                {


                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Desktop);

                    var thumbnailddes =await  storageFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
               
                    ListCol.Add(new ClassListStroce() { StorageFolder = storageFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnailddes });
                    


                }
                catch(Exception ex)
                {
                 
                }
                try
                {
                    StorageFolder storageFolderDownloads = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Downloads);
                    var thumbnailddesDownloads = await storageFolderDownloads.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                    
                    ListCol.Add(new ClassListStroce() { StorageFolder = storageFolderDownloads, FlagFolde = true, Type = Type, ThumbnailMode= thumbnailddesDownloads });
                   
                }
                catch(Exception ex)
                {
                    
                }
                StorageFolder DocumentFolder = KnownFolders.DocumentsLibrary;
                var thumbnaild = await DocumentFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.ResizeThumbnail);
               

                ListCol.Add(new ClassListStroce() { StorageFolder = DocumentFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnaild });
               

                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                var thumbnail = await picturesFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
               
                ListCol.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnail });
               

                // Get Music library.
                StorageFolder musicFolder = KnownFolders.MusicLibrary;
                 var thumbnail1 = await picturesFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
           
                ListCol.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnail1 });
               


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
                        var thumbnail11 = await picturesFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                        ListCol.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail11, Type = Type });

                    }

                }
                try
                {
                    var items = Windows.Storage.UserDataPaths.GetDefault();
                   
                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(items.Profile);
                    var d = await storageFolder.GetFoldersAsync();
                   
                    foreach (StorageFolder FlFolder1 in d)
                    {
                        if(FlFolder1.DisplayName=="OneDrive")
                        {
                            var thumbnailr = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                            

                            ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode= thumbnailr });
                           


                        }
                       
                    }

                    
                }
                catch (Exception ex)
                {

                }
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    try
                    {


                        if (drive.DriveType == DriveType.Fixed && drive.DriveType != DriveType.Removable)
                        {
                            StorageFolder FolderL = await StorageFolder.GetFolderFromPathAsync(drive.Name);
                           
                            var thumbnailL = await FolderL.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                           
                            var clas = new ClassListStroce() { StorageFolder = FolderL, FlagFolde = true, ThumbnailMode= thumbnailL };
                            if (Type == "Maximum")
                            {
                                clas.Type = "Drive";

                            }
                            else
                            {
                                clas.Type = "DriveMiddle";

                            }
                            try
                            {
                                clas.Spase();
                            }
                            catch (Exception ex)
                            {

                            }

                            ListCol.Add(clas);
                           

                        }
                    }
                    catch(Exception)
                    {

                    }
                }


                StorageFolder Folder = KnownFolders.RemovableDevices;
                try
                {


                   //IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                    //foreach (StorageFolder FlFolder1 in folderList)
                    {
                       // thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                       // ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type = Type });
                    }
                }
                catch(Exception )
                {

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


                            IReadOnlyList<StorageFolder> folderList = await storageFolder.GetFoldersAsync();
                            IReadOnlyList<StorageFile> fileList = await storageFolder.GetFilesAsync();
                           
                            storageFolderFirst = storageFolder;
                            ListCol.Clear();
                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                                var thumbnail1 = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                       
                                ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode=thumbnail1 });
                       
                            }
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                                var thumbnail1 = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                                ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, Type = Type, ThumbnailMode= thumbnail1 });
                        
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
            try
            {
                Debug.WriteLine(storageFile.Path); if (storageFile.DisplayType == ".exe")
                {

                    Process.Start(storageFile.Path);
                }
                else
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
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
        public async void Next()
        {
            if (simofor == false)
            {
                try
                {


                    simofor = true;
                    if (classListStroceSelect != null)
                    {
                        if (classListStroceSelect.ElementAt(0).FlagFolde)
                        {


                            Path = classListStroceSelect.ElementAt(0).StorageFolder.Path;
                           
                            IReadOnlyList<StorageFolder> folderList = await classListStroceSelect.ElementAt(0).StorageFolder.GetFoldersAsync();
                            IReadOnlyList<StorageFile> fileList = await classListStroceSelect.ElementAt(0).StorageFolder.GetFilesAsync();
                          

                            storageFolderFirst = classListStroceSelect.ElementAt(0).StorageFolder;
                            ListCol.Clear();

                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                                 var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                             
                                ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode=thumbnail });
                              
                               
                            }
                  
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                                  var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                             
                                ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, Type = Type, ThumbnailMode= thumbnail });
                     


                            }
                            //_DataGrid.SelectedIndex = 0;
                            classListStroceSelect = null;

                        }
                        else
                        {
                            await ActivateFile(classListStroceSelect.ElementAt(0).storageFile);
                        }
                    }
                    simofor = false;
                }
                catch(Exception ex)
                {
                    simofor = false;
                }
            }

        }
        public async void FavoritSpeed()
        {
            if (simofor == false)
            {
                try
                {
                    var items = Windows.Storage.UserDataPaths.GetDefault();
                    Path = items.Profile;
                    StorageFolder storageFolder =await StorageFolder.GetFolderFromPathAsync(items.Profile);
                    var d = await storageFolder.GetFoldersAsync();
                    ListCol.Clear();
                    foreach (StorageFolder FlFolder1 in d)
                    {
                        var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                        
                        ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode= thumbnail });
                       


                    }
                    var d1 = await storageFolder.GetFilesAsync();
                    foreach (StorageFile FlFolder1 in d1)
                    {

                        var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                     
                        ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, Type = Type, ThumbnailMode= thumbnail });
                      


                    }

                }
                catch (Exception ex)
                {
                    simofor = false;
                    Debug.WriteLine(ex.ToString());
                }
            }

        }
     public async void Sorting(string tipselect)//tipselect UpABC, DownABC, UpDate, DownDate
        {
            List<ClassListStroce> classListStroces = new List<ClassListStroce>();
            List<ClassListStroce> classListStroces1 = new List<ClassListStroce>();

            if (tipselect== "UpABC")
            {
                classListStroces1 = (from FolderInfo in ListCol
                            orderby FolderInfo.FileAndFildeName 
                            select FolderInfo).ToList();
            }
            if (tipselect == "DownABC")
            {
                classListStroces1 = (from FolderInfo in ListCol
                                     orderby FolderInfo.FileAndFildeName descending
                                     select FolderInfo).ToList();
            }

            foreach (var d in classListStroces1)
            {
                classListStroces.Add(d);
            }
          
           ListCol.Clear();
            
            foreach (var d in classListStroces)
            {
                // fileAndFolderViewer.ListCol.Add(d);
                if (d.FlagFolde)
                {
                    var thumbnail1 = await d.StorageFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                    ListCol.Add(new ClassListStroce() { StorageFolder = d.StorageFolder, FlagFolde = true, Type = d.Type, ThumbnailMode = thumbnail1 });

                }
                else
                {
                    var thumbnail1 = await d.storageFile.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                   ListCol.Add(new ClassListStroce() { storageFile = d.storageFile, FlagFolde = false, Type = d.Type, ThumbnailMode = thumbnail1 });

                }

            }
     
          
        }
    }
}
