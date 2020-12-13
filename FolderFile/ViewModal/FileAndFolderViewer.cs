using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
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
      
        public ObservableCollection<ClassListStroce> ListCol
        { 
            get
            {
                return this._ListCol;
            }

            set
            {
                _ListCol = value;
              
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

      /*  public async void Nazad()
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
                                        // var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                        //ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail, Type = Type });
                                        ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, true, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), FileAndFolderViewer.Type));// { StorageFolder = FlFolder1, FlagFolde = true, Type = Type });
                                    }
                                }

                                foreach (StorageFile FlFolder1 in fileList)
                                {
                                    if (Path == (await FlFolder1.GetParentAsync()).Path)
                                    {
                                        var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                        ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, false, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), FileAndFolderViewer.Type) { ThumbnailMode = thumbnail1 });// { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail1, Type = Type });
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

        }*/
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
             
                try
                {


                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Desktop);

                    //var thumbnailddes =await  storageFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                    ListCol.Add(new ClassListStroce(storageFolder.Path, storageFolder.DisplayName, true, storageFolder.DisplayType, storageFolder.DateCreated.ToString(), Type){ ick = true });// { StorageFolder = storageFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnailddes });
                   // ListCol.Add(new ClassListStroce() { StorageFolder = storageFolder, FlagFolde = true, Type = Type});



                }
                catch(Exception ex)
                {
                 
                }
                try
                {
                    StorageFolder storageFolderDownloads = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Downloads);
                    //var thumbnailddesDownloads = await storageFolderDownloads.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                    ListCol.Add(new ClassListStroce(storageFolderDownloads.Path, storageFolderDownloads.DisplayName, true, storageFolderDownloads.DisplayType, storageFolderDownloads.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = storageFolderDownloads, FlagFolde = true, Type = Type, ThumbnailMode= thumbnailddesDownloads });
                   
                }
                catch(Exception ex)
                {
                    
                }
               // StorageFolder DocumentFolder = KnownFolders.DocumentsLibrary;
                StorageFolder DocumentFolder= await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Documents);
               // var thumbnaild = await DocumentFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.ResizeThumbnail);
                //Debug.WriteLine(KnownFolders.DocumentsLibrary.Path);

                ListCol.Add(new ClassListStroce(DocumentFolder.Path, DocumentFolder.DisplayName, true, DocumentFolder.DisplayType, DocumentFolder.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = DocumentFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnaild });
               

                StorageFolder picturesFolder = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Pictures);
                //var thumbnail = await picturesFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                ListCol.Add(new ClassListStroce(picturesFolder.Path, picturesFolder.DisplayName, true, picturesFolder.DisplayType, picturesFolder.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = picturesFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnail });
               

                // Get Music library.
                StorageFolder musicFolder = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Music); ;
                // var thumbnail1 = await picturesFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                ListCol.Add(new ClassListStroce(musicFolder.Path, musicFolder.DisplayName, true, musicFolder.DisplayType, musicFolder.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = musicFolder, FlagFolde = true, Type = Type, ThumbnailMode= thumbnail1 });
               


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
                        //var thumbnail11 = await picturesFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                        ListCol.Add(new ClassListStroce(ss.Path, ss.DisplayName, true, ss.DisplayType, ss.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail11, Type = Type });

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
                            //var thumbnailr = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);


                            ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, true, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode= thumbnailr });
                           


                        }
                       
                    }

                    
                }
                catch (Exception ex)
                {

                }
                var f = GetInternalDrives();
                foreach (var d in f)
                {
                    Debug.WriteLine(d.DisplayName);
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

                            var clas = new ClassListStroce(FolderL.Path, FolderL.DisplayName, true, FolderL.DisplayType, FolderL.DateCreated.ToString(), Type) { ick = true, ThumbnailMode= thumbnailL };// { StorageFolder = FolderL, FlagFolde = true, ThumbnailMode= thumbnailL };
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
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
           

        }
        public List<StorageFolder> GetInternalDrives()
        {
            string driveLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int driveLettersLen = driveLetters.Length;
            string removableDriveLetters = "";
            string driveLetter;

            List<StorageFolder> drives = new List<StorageFolder>();
            StorageFolder removableDevices = KnownFolders.RemovableDevices;
            IReadOnlyList<StorageFolder> folders = Task.Run<IReadOnlyList<StorageFolder>>(async () => await removableDevices.GetFoldersAsync()).Result;

            foreach (StorageFolder removableDevice in folders)
            {
                if (string.IsNullOrEmpty(removableDevice.Path)) continue;
                driveLetter = removableDevice.Path.Substring(0, 1).ToUpper();
                if (driveLetters.IndexOf(driveLetter) > -1) removableDriveLetters += driveLetter;
            }

            for (int curDrive = 0; curDrive < driveLettersLen; curDrive++)
            {
                driveLetter = driveLetters.Substring(curDrive, 1);
                if (removableDriveLetters.IndexOf(driveLetter) > -1) continue;

                try
                {
                    StorageFolder drive = Task.Run<StorageFolder>(async () => await StorageFolder.GetFolderFromPathAsync(driveLetter + ":")).Result;
                    drives.Add(drive);
                }
                catch (System.AggregateException) { }

            }
            return drives;
        }
        public async void Upgreid()
        {
                    try
                    {
                       
                     
                        


                    IReadOnlyList<StorageFolder> folderList =await (await StorageFolder.GetFolderFromPathAsync(Path)).GetFoldersAsync();// storageFolder.GetFoldersAsync();
                            IReadOnlyList<StorageFile> fileList = await (await StorageFolder.GetFolderFromPathAsync(Path)).GetFilesAsync();
                           
                            //storageFolderFirst = storageFolder;
                            ListCol.Clear();
                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                        //var thumbnail1 = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                        // ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode=thumbnail1 });
                        ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, true, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), FileAndFolderViewer.Type) { ick = true });// { StorageFolder = FlFolder1, FlagFolde = true, Type = Type });

                    }
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                               // var thumbnail1 = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                        ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, false, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), FileAndFolderViewer.Type) { ick = true });// { storageFile = FlFolder1, FlagFolde = false, Type = Type, ThumbnailMode= thumbnail1 });
                        
                            }
                        
                      
                
                    }
                    catch (Exception ex)
                    {
                     
                 
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
        public async void Next(ClassListStroce classListStroce)
        {
            if (simofor == false)
            {
                try
                {
                    VicProgres = Visibility.Visible;
                    ListCol.Clear();
                    simofor = true;
                   
                        if (classListStroce.FlagFolde)
                        {

                        var watch = Stopwatch.StartNew();
                        watch.Start();
                        Path = classListStroce.Path;
                           
                            IReadOnlyList<StorageFolder> folderList = await (await StorageFolder.GetFolderFromPathAsync(classListStroce.Path)).GetFoldersAsync();
                        winProgres = folderList.Count();
                        
                        // IReadOnlyList<StorageFile> fileList = await (await StorageFolder.GetFolderFromPathAsync(classListStroce.Path)).GetFilesAsync();
                        //var fileList1 = await classListStroceSelect.ElementAt(0).StorageFolder.GetItemsAsync();

              
                           

                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                                // var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                                ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, true, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), Type) { ick = true});// { StorageFolder = FlFolder1, FlagFolde = true, Type = Type });
                                                                                                                                                                                           //ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode = thumbnail });
                            

                            }
                        stepProgres = folderList.Count();


                        IReadOnlyList<StorageFile> fileList = await (await StorageFolder.GetFolderFromPathAsync(classListStroce.Path)).GetFilesAsync();
                        winProgres = winProgres+ fileList.Count();


                        foreach (StorageFile FlFolder1 in fileList)
                            {
                            // var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                            ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, false, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), Type){ick=true  });// { storageFile = FlFolder1, FlagFolde = false, Type = Type, ThumbnailMode= thumbnail });
                           

                        }
                        stepProgres += fileList.Count();
                        watch.Stop();
                            Debug.WriteLine(watch.ElapsedMilliseconds.ToString());
                            //_DataGrid.SelectedIndex = 0;
                            classListStroceSelect = null;

                        }
                        else
                        {
                            await ActivateFile(await StorageFile.GetFileFromPathAsync(classListStroce.Path));
                        }
                   
                    simofor = false;
                 
                }
                catch(Exception ex)
                {
                    simofor = false;
                }
                finally
                {
                    simofor = false;
                    VicProgres = Visibility.Collapsed;
                }
            }

        }
        int winProgres = 0;
        public int WinProgres
        {
            get
            {
                return winProgres;
            }
            set
            {
                winProgres = value;
                this.OnPropertyChanged();
            }
        }
        int stepProgres = 0;
        public int StepProgres
        {
            get
            {
                return stepProgres;
            }
            set
            {
                stepProgres = value;
                this.OnPropertyChanged();
            }
        }
        Visibility vicProgres=Visibility.Collapsed;
        public Visibility VicProgres
        {
            get
            {
                return vicProgres;
            }
            set
            {
             
                vicProgres = value;
                this.OnPropertyChanged();
            }
        }
        public async void FavoritSpeed()
        {
            
            if (simofor == false)
            {
                try
                {
                    ListCol.Clear();
                    var items = Windows.Storage.UserDataPaths.GetDefault();
                    Path = items.Profile;
                  
                    StorageFolder storageFolder =await StorageFolder.GetFolderFromPathAsync(items.Profile);
                    var d = await storageFolder.GetFoldersAsync();
                    
                    foreach (StorageFolder FlFolder1 in d)
                    {
                        // var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                        //ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, Type = Type, ThumbnailMode= thumbnail });
                        ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, true, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), Type) { ick = true });// { StorageFolder = FlFolder1, FlagFolde = true, Type = Type});



                    }
                    var d1 = await storageFolder.GetFilesAsync();
                    foreach (StorageFile FlFolder1 in d1)
                    {

                        //var thumbnail = await FlFolder1.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                        ListCol.Add(new ClassListStroce(FlFolder1.Path, FlFolder1.DisplayName, false, FlFolder1.DisplayType, FlFolder1.DateCreated.ToString(), Type) { ick = true });// { storageFile = FlFolder1, FlagFolde = false, Type = Type, ThumbnailMode= thumbnail });
                      


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
                    // var thumbnail1 = await d.StorageFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);

                    ListCol.Add(d);// { StorageFolder = d.StorageFolder, FlagFolde = true, Type = d.Type, ThumbnailMode = thumbnail1 });

                }
                else
                {
                    // var thumbnail1 = await d.storageFile.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                    ListCol.Add(d);// { storageFile = d.storageFile, FlagFolde = false, Type = d.Type, ThumbnailMode = thumbnail1 });

                }

            }
     
          
        }
        string countElements;
        public string CountElements
        {
            get
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                return resourceLoader.GetString("elementsText")+" "+ _ListCol.Count().ToString();
              
            }
           

        }
        public string CountFolders
        {
            get
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                var s = (from d in _ListCol where d.FlagFolde == true select d).Count();
                return resourceLoader.GetString("FoldersText") + " " + s.ToString();

            }



        }
        public string Countfiles
        {
            get
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                var s = (from d in _ListCol where d.FlagFolde == false select d).Count();
                return resourceLoader.GetString("FilesText") + " " + s.ToString();

            }


        }





        public enum FINDEX_INFO_LEVELS
        {
            FindExInfoStandard = 0,
            FindExInfoBasic = 1
        }

        public enum FINDEX_SEARCH_OPS
        {
            FindExSearchNameMatch = 0,
            FindExSearchLimitToDirectories = 1,
            FindExSearchLimitToDevices = 2
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("api-ms-win-core-file-fromapp-l1-1-0.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstFileExFromApp(
            string lpFileName,
            FINDEX_INFO_LEVELS fInfoLevelId,
            out WIN32_FIND_DATA lpFindFileData,
            FINDEX_SEARCH_OPS fSearchOp,
            IntPtr lpSearchFilter,
            int dwAdditionalFlags);

        public const int FIND_FIRST_EX_CASE_SENSITIVE = 1;
        public const int FIND_FIRST_EX_LARGE_FETCH = 2;

        [DllImport("api-ms-win-core-file-l1-1-0.dll", CharSet = CharSet.Unicode)]
        static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("api-ms-win-core-file-l1-1-0.dll")]
        static extern bool FindClose(IntPtr hFindFile);
        private void NextFind()
        {
            var watch = Stopwatch.StartNew();
            var path = @"C:\";
            WIN32_FIND_DATA findData;
            FINDEX_INFO_LEVELS findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoStandard;
            int additionalFlags = 0;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                findInfoLevel = FINDEX_INFO_LEVELS.FindExInfoBasic;
                additionalFlags = FIND_FIRST_EX_LARGE_FETCH;
            }

            IntPtr hFile = FindFirstFileExFromApp(path + "\\*.*", findInfoLevel, out findData, FINDEX_SEARCH_OPS.FindExSearchNameMatch, IntPtr.Zero,
                                                  additionalFlags);
            var count = 0;
            if (hFile.ToInt64() != -1)
            {
                do
                {
                    if (((System.IO.FileAttributes)findData.dwFileAttributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.System)
                    {
                        // do something with it
                        var fn = findData.cFileName;
                        Debug.WriteLine(fn);
                        ++count;
                    }
                } while (FindNextFile(hFile, out findData));

                FindClose(hFile);
            }
            Debug.WriteLine("count " + count + ", ellapsed=" + watch.ElapsedMilliseconds);
        }
    }
}
