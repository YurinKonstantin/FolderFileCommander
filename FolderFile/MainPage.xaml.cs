using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Grid;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace FolderFile
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
         
           this.fileAndFolderViewer = new FileAndFolderViewer();
            this.fileAndFolderViewer2 = new FileAndFolderViewer();


            dataTransferManager = DataTransferManager.GetForCurrentView();
       
            
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
           
        }

        private DataTransferManager dataTransferManager;
      
        FileAndFolderViewer fileAndFolderViewer { get; set; }
        FileAndFolderViewer fileAndFolderViewer2 { get; set; }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            bool tryOk = false;
            try
            {
                StorageFolder documentFolder = KnownFolders.DocumentsLibrary;
                tryOk = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                //resourceLoader.GetString("MessageTiletDostup");
               // MessageDialog messageDialog = new MessageDialog(resourceLoader.GetString("MessageContentDostup"), resourceLoader.GetString("MessageTiletDostup"));
               // await messageDialog.ShowAsync();
                ContentDialogDostup contentDialogDostup = new ContentDialogDostup() { Title = resourceLoader.GetString("MessageTiletDostup") };
                contentDialogDostup.TextD(resourceLoader.GetString("MessageContentDostup"));
               await contentDialogDostup.ShowAsync();
                App.Current.Exit();
            }
            
            
           
            if (tryOk)
            {

             
                InitializeTreeView(sampleTreeView);
                InitializeTreeView(sampleTreeView1);
                InitializeDataGridView();
                


            }
            try
            {


                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                String localValue = localSettings.Values["ViewSet"] as string;
                if(localValue== "Max")
                {
                    FileAndFolderViewer.Type = "Maximum";
                }
                if (localValue == "Min")
                {
                    FileAndFolderViewer.Type = "Middle";
                }


                
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
        public async Task addLocation()
        {
            try
            {


                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                folderPicker.FileTypeFilter.Add("*");

                Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null)
                {
                    // Application now has read/write access to all contents in the picked folder
                    // (including other sub-folder contents)
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(folder.DisplayName, folder);

                }
                else
                {

                }
                fileAndFolderViewer.ListCol.Clear();
                sampleTreeView.RootNodes.Clear();
                sampleTreeView1.RootNodes.Clear();
                InitializeTreeView(sampleTreeView);
                InitializeTreeView(sampleTreeView1);
                InitializeDataGridView();
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
     
       
        List<String> listDostyp = new List<string>();
     
        private async void InitializeTreeView(TreeView treeView)
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            try
            {

                treeView.RootNodes.Clear();
                StorageFolder documentFolder = KnownFolders.DocumentsLibrary;
                TreeViewNode pictureNode1 = new TreeViewNode();
                var thumbnaild = await documentFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 600, ThumbnailOptions.ResizeThumbnail);
                pictureNode1.Content = new ClassListStroce() { StorageFolder = documentFolder, FlagFolde = true, ThumbnailMode= thumbnaild };
                pictureNode1.IsExpanded = false;

                pictureNode1.HasUnrealizedChildren = true;

                treeView.RootNodes.Add(pictureNode1);
                FillTreeNode(pictureNode1);


                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                TreeViewNode pictureNode = new TreeViewNode();
                var thumbnaild1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem);
                pictureNode.Content = new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode= thumbnaild1 };
                pictureNode.IsExpanded = false;

                pictureNode.HasUnrealizedChildren = true;
                treeView.RootNodes.Add(pictureNode);
                FillTreeNode(pictureNode);
                // Get Music library.
                StorageFolder musicFolder = KnownFolders.MusicLibrary;
                TreeViewNode musicNode = new TreeViewNode();
                var thumbnaild2 = await musicFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 600, ThumbnailOptions.UseCurrentScale);
                musicNode.Content = new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode= thumbnaild2 }; 
                musicNode.IsExpanded = false;
                musicNode.HasUnrealizedChildren = true;
                treeView.RootNodes.Add(musicNode);
                FillTreeNode(musicNode);
                var storageItemAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
                if (storageItemAccessList.Entries.Count != 0)
                {


                    foreach (Windows.Storage.AccessCache.AccessListEntry entry in storageItemAccessList.Entries)
                    {
                        string mruToken = entry.Token;
                        string mruMetadata = entry.Metadata;
                        Windows.Storage.IStorageItem item = await storageItemAccessList.GetItemAsync(mruToken);
                        StorageFolder ss = await storageItemAccessList.GetFolderAsync(mruToken);
                        TreeViewNode AssecNode = new TreeViewNode();
                        var thumbnaild3 = await ss.GetThumbnailAsync(ThumbnailMode.SingleItem);
                        AssecNode.Content =  new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode= thumbnaild3 };
                        AssecNode.IsExpanded = false;
                        AssecNode.HasUnrealizedChildren = true;
                        treeView.RootNodes.Add(AssecNode);

                        FillTreeNode(AssecNode);
                    }
                }


                StorageFolder Folder = KnownFolders.RemovableDevices;
                IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                foreach (StorageFolder FlFolder1 in folderList)
                {

                    TreeViewNode FolderNode = new TreeViewNode();
                    var thumbnaild3 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem);
                    FolderNode.Content = new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode= thumbnaild3 }; 
                    FolderNode.IsExpanded = false;
                    FolderNode.HasUnrealizedChildren = true;

                    treeView.RootNodes.Add(FolderNode);
                    FillTreeNode(FolderNode);
                }
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    
                    if (drive.DriveType == DriveType.Fixed)
                    {
                        TreeViewNode FolderNode = new TreeViewNode();
                        StorageFolder storageFolderL = await StorageFolder.GetFolderFromPathAsync(drive.Name);
                        var thumbnaild4 = await storageFolderL.GetThumbnailAsync(ThumbnailMode.SingleItem);
                        FolderNode.Content = new ClassListStroce() { StorageFolder = storageFolderL, FlagFolde = true, ThumbnailMode= thumbnaild4 };
                        FolderNode.IsExpanded = false;
                        FolderNode.HasUnrealizedChildren = true;
                        treeView.RootNodes.Add(FolderNode);

                    }

                }
            }
            catch(Exception ex )
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
        private async void InitializeDataGridView()
        {
            try
            {


                fileAndFolderViewer.InitializeDataGridView();
                fileAndFolderViewer2.InitializeDataGridView();
                
                TimeSpan period = TimeSpan.FromSeconds(2);

                ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                {

                    newUsB();

                }, period);

            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
        

      
        private async void InitializeDataGridView21(ObservableCollection<ClassListStroce> classListStroces, StorageFolder storageFolderFirst, string path)
        {
            
            try
            {

                classListStroces.Clear();
                storageFolderFirst = null;
                StorageFolder DocumentFolder = KnownFolders.DocumentsLibrary;
                var thumbnaild = await DocumentFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 200);
                classListStroces.Add(new ClassListStroce() { StorageFolder = DocumentFolder, FlagFolde = true, ThumbnailMode = thumbnaild });

                StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                var thumbnail = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 300);
                classListStroces.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnail });

                // Get Music library.
                StorageFolder musicFolder = KnownFolders.MusicLibrary;
                var thumbnail1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 400);
                classListStroces.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnail1 });

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
                        classListStroces.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail11 });

                    }

                }

                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.DriveType == DriveType.Fixed)
                    {
                        StorageFolder FolderL = await StorageFolder.GetFolderFromPathAsync(drive.Name);

                        var thumbnailL = await FolderL.GetThumbnailAsync(ThumbnailMode.SingleItem, 50);
                        classListStroces.Add(new ClassListStroce() { StorageFolder = FolderL, FlagFolde = true, ThumbnailMode = thumbnailL });
                    }
                }


                StorageFolder Folder = KnownFolders.RemovableDevices;

                IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                foreach (StorageFolder FlFolder1 in folderList)
                {
                    thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                    classListStroces.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1 });
                }
                path = String.Empty;



            }
            catch (UnauthorizedAccessException ex)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                ContentDialogDostup contentDialogDostup = new ContentDialogDostup() { Title = resourceLoader.GetString("MessageTiletDostup") };
                contentDialogDostup.TextD(resourceLoader.GetString("MessageContentDostup"));
                await contentDialogDostup.ShowAsync();
                App.Current.Exit();
            }


        }

        private async void FillTreeNode(TreeViewNode node)
        {
            // Get the contents of the folder represented by the current tree node.
            // Add each item as a new child node of the node that's being expanded.

            // Only process the node if it's a folder and has unrealized children.
            ClassListStroce folder = null;
            if (node.Content is ClassListStroce && node.HasUnrealizedChildren == true)
            {
                folder = node.Content as ClassListStroce;
            }
            else
            {
                // The node isn't a folder, or it's already been filled.
                return;
            }

            IReadOnlyList<IStorageItem> itemsList = await folder.StorageFolder.GetFoldersAsync();

            if (itemsList.Count == 0)
            {
                // The item is a folder, but it's empty. Leave HasUnrealizedChildren = true so
                // that the chevron appears, but don't try to process children that aren't there.
                return;
            }

            foreach (var item in itemsList)
            {
                var newNode = new TreeViewNode();
                var ss = item as StorageFolder;
                var thumbnaild3 = await ss.GetThumbnailAsync(ThumbnailMode.SingleItem);
                var it = new ClassListStroce() { FlagFolde=true, StorageFolder= item as StorageFolder, ThumbnailMode= thumbnaild3 };
                newNode.Content = it;

                if (item is StorageFolder)
                {
                    // If the item is a folder, set HasUnrealizedChildren to true. 
                    // This makes the collapsed chevron show up.
                    newNode.HasUnrealizedChildren = true;
                }
                else
                {
                    // Item is StorageFile. No processing needed for this scenario.
                }
                node.Children.Add(newNode);
            }
            // Children were just added to this node, so set HasUnrealizedChildren to false.
            node.HasUnrealizedChildren = false;
        }
  
        private void SampleTreeView_Expanding(TreeView sender, TreeViewExpandingEventArgs args)
        {
            if (args.Node.HasUnrealizedChildren)
            {
                
                FillTreeNode(args.Node);
            }
        }
   
        private void SampleTreeView_Collapsed(TreeView sender, TreeViewCollapsedEventArgs args)
        {
            args.Node.Children.Clear();
            args.Node.HasUnrealizedChildren = true;
        }
       
        private async void SampleTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            TreeView treeView = (TreeView)sender;
               var node = args.InvokedItem as TreeViewNode;
            if (node.Content is ClassListStroce item)
            {
                
               // FileNameTextBlock.Text = item.Name;
               // FilePathTextBlock.Text = item.Path+" "+ node.Depth.ToString();
               // TreeDepthTextBlock.Text = node.Depth.ToString();

                if (node.Content is ClassListStroce folder)
                {
                    
                        _DataGrid.SelectedItem = null;

                        fileAndFolderViewer.ListCol.Clear();
                        fileAndFolderViewer.ListColName.Clear();
                        fileAndFolderViewer.storageFolderFirst = folder.StorageFolder;
                    fileAndFolderViewer.Path = folder.StorageFolder.Path;
                        IReadOnlyList<StorageFolder> folderList = await folder.StorageFolder.GetFoldersAsync();
                        IReadOnlyList<StorageFile> fileList = await folder.StorageFolder.GetFilesAsync();
                        if (fileList.Count != 0 || folderList.Count != 0)
                        {


                            foreach (StorageFolder d in folderList)
                            {
                                var thumbnail1 = await d.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                                fileAndFolderViewer.ListColName.Add(d.DisplayName);
                            }

                            foreach (StorageFile file in fileList)
                            {
                                var thumbnail1 = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = file, FlagFolde = false, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                                fileAndFolderViewer.ListColName.Add(file.DisplayName);
                            }
                        
                    
                    }
                        node.IsExpanded = !node.IsExpanded;
                }
            }
        }
        private async void SampleTreeView_ItemInvoked1(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            TreeView treeView = (TreeView)sender;
            var node = args.InvokedItem as TreeViewNode;
            if (node.Content is ClassListStroce item)
            {

                // FileNameTextBlock.Text = item.Name;
                // FilePathTextBlock.Text = item.Path+" "+ node.Depth.ToString();
                // TreeDepthTextBlock.Text = node.Depth.ToString();

                if (node.Content is ClassListStroce folder)
                {

                    _DataGrid.SelectedItem = null;

                    fileAndFolderViewer2.ListCol.Clear();
                    fileAndFolderViewer2.ListColName.Clear();
                    fileAndFolderViewer2.storageFolderFirst = folder.StorageFolder;
                    fileAndFolderViewer2.Path = folder.StorageFolder.Path;
                    IReadOnlyList<StorageFolder> folderList = await folder.StorageFolder.GetFoldersAsync();
                    IReadOnlyList<StorageFile> fileList = await folder.StorageFolder.GetFilesAsync();
                    if (fileList.Count != 0 || folderList.Count != 0)
                    {


                        foreach (StorageFolder d in folderList)
                        {
                            var thumbnail1 = await d.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                            fileAndFolderViewer2.ListCol.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                            fileAndFolderViewer2.ListColName.Add(d.DisplayName);
                        }

                        foreach (StorageFile file in fileList)
                        {
                            var thumbnail1 = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                            fileAndFolderViewer2.ListCol.Add(new ClassListStroce() { storageFile = file, FlagFolde = false, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                            fileAndFolderViewer2.ListColName.Add(file.DisplayName);
                        }


                    }
                    node.IsExpanded = !node.IsExpanded;
                }
            }
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Split.IsPaneOpen = !Split.IsPaneOpen;
        }

        /// <summary>
        /// Кнопка назад для первой панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                fileAndFolderViewer.Nazad();
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
      
        /// <summary>
        /// Кнопка назад для второй панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButtonNazad_Click_1(object sender, RoutedEventArgs e)
        {



            fileAndFolderViewer2.Nazad();

        }

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            await addLocation();
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Remove(listDop.SelectedItem.ToString());
            fileAndFolderViewer.ListCol.Clear();
            fileAndFolderViewer2.ListCol.Clear();
            sampleTreeView.RootNodes.Clear();
            sampleTreeView1.RootNodes.Clear();
            InitializeTreeView(sampleTreeView);
            InitializeTreeView(sampleTreeView1);
            InitializeDataGridView();
         
        }

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }


        int x = -1;

        private async void List1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var tag = (ListView)sender;
            
            FileAndFolderViewer.viewFocusTag = tag.Tag.ToString();
          //  FileAndFolderViewer.classListStroceSelect = (List<ClassListStroce>)tag.SelectedItems;
          if (FileAndFolderViewer.classListStroceSelect!=null)
            {
                FileAndFolderViewer.classListStroceSelect.Clear();
            }
            else
            {
                FileAndFolderViewer.classListStroceSelect = new List<ClassListStroce>();
            }
            foreach(var d in tag.SelectedItems)
            {
                FileAndFolderViewer.classListStroceSelect.Add(d as ClassListStroce);
            }
           if (FileAndFolderViewer.viewFocusTag == "twy")
            {
              
               
                ClassListStroce classListStroce = (ClassListStroce)tag.SelectedItem;
                shareList = classListStroce;
                if (fileAndFolderViewer2.storageFolderFirst != null)
                {
                  fileAndFolderViewer2.Path = fileAndFolderViewer2.storageFolderFirst.Path;
                }
                if (classListStroce != null)
                {
                    if (classListStroce.FlagFolde)
                    {
                        ShareBut.IsEnabled = false;
                    }
                    else
                    {
                        
                     
                       ShareBut.IsEnabled = true;

                        

                    }
                }
               
            }
           else
            {
               
               
                ClassListStroce classListStroce = (ClassListStroce)tag.SelectedItem;
                shareList = classListStroce;
                if (fileAndFolderViewer.storageFolderFirst != null)
                {
   fileAndFolderViewer.Path = fileAndFolderViewer.storageFolderFirst.Path;
                }
                if (classListStroce != null)
                {


                    if (classListStroce.FlagFolde)
                    {
                        ShareBut.IsEnabled = false;
                    }
                    else
                    {
                        
                    ShareBut.IsEnabled = true;


                    }
                }

            }

            
            
        }
      
        public async Task ActivateFile(StorageFile storageFile)
        {
            try
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
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
        private async void List1_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {


                ListView listView = (ListView)sender;
                if (listView.Tag.ToString() == "One")
                {
                    fileAndFolderViewer.Next();
                }
                else
                {
                    fileAndFolderViewer2.Next();

                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Split1.IsPaneOpen = !Split1.IsPaneOpen;
        }

        private async void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {

            try
            {

                foreach (var vs in FileAndFolderViewer.classListStroceSelect)
                {


                    // var vs = FileAndFolderViewer.classListStroceSelect;
                  

                        if (vs.FlagFolde != true)
                        {
                            await ActivateFile(vs.storageFile);
                        }

                    
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void AppBarButton_Click_5(object sender, RoutedEventArgs e)
        {
          //  if (focusOne == true)
            {
                _DataGrid.SelectAll();
                
            }
           // else
            {
                _DataGrid2.SelectAll();
            }

        }
        private void suggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestBox autoSuggestBox = (AutoSuggestBox)sender;
            if (args.CheckCurrent())
            {
               
                var term = autoSuggestBox.Text.ToLower();
                var results = fileAndFolderViewer.ListColName.Where(i => i.ToLower().Contains(term)).ToList();
               
              
                autoSuggestBox.ItemsSource = results;
               
            }
        }
        private void suggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var term = args.QueryText.ToLower();
            AutoSuggestBox autoSuggestBox = (AutoSuggestBox)sender;
            var results = fileAndFolderViewer.ListColName.Where(i => i.ToLower().Contains(term)).ToList();
            autoSuggestBox.ItemsSource = results;
            autoSuggestBox.IsSuggestionListOpen = true;
        }

        private async void suggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestBox autoSuggestBox = (AutoSuggestBox)sender;
            string str= args.SelectedItem as string;
            foreach (var st in fileAndFolderViewer.ListCol)
            {
                if(st.FileAndFildeName==str)
                {
                    if(st.FlagFolde)
                    {
                       



                            IReadOnlyList<StorageFolder> folderList = await st.StorageFolder.GetFoldersAsync();
                            IReadOnlyList<StorageFile> fileList = await st.StorageFolder.GetFilesAsync();

                            fileAndFolderViewer.storageFolderFirst = st.StorageFolder;
                            fileAndFolderViewer.ListCol.Clear();
                            fileAndFolderViewer.ListColName.Clear();
                            foreach (StorageFolder FlFolder1 in folderList)
                            {
                                var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail, Type = FileAndFolderViewer.Type });
                                fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                            }
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                                var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail, Type = FileAndFolderViewer.Type });
                                fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                            }
                       
                    }
                    else
                    {
                        await ActivateFile(st.storageFile);
                       
                    }
                    autoSuggestBox.Text = String.Empty;
                    break;

                }
            }
          //  textBlock1.Text = args.SelectedItem as string;
        }

        private async void _DataGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            allContactsMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            var a = ((FrameworkElement)e.OriginalSource).DataContext as ClassListStroce;
            if(a!=null)
            {
                var content = a.FileAndFildeName;
                Copy.Tag = a;
                open.Tag = a;
                OpenAs.Tag = a;
                Perem.Tag = a;
                Paste.Tag = a;
                Delete.Tag = a;
                Share.Tag = a;
                Inf.Tag = a;
                
            }
           

        }
        DataPackage dataPackage = new DataPackage();
        private async  void Copi_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
                ClassListStroce cc = (ClassListStroce)secondItem.Tag;
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                List<IStorageItem> list = new List<IStorageItem>();
                if (cc.FlagFolde)
                {
                    list.Add(cc.StorageFolder);
                }
                else
                {
                    list.Add(cc.storageFile);
                }
                dataPackage.SetStorageItems(list);

                Clipboard.SetContent(dataPackage);
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
                ClassListStroce cc = (ClassListStroce)secondItem.Tag;
                if (cc.FlagFolde != true)
                {

                    await ActivateFile(cc.storageFile);
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
       
        public async void Upgreid( )
        {
            
              
           
               
            fileAndFolderViewer.Upgreid();
            fileAndFolderViewer2.Upgreid();


        }
        private async void AppBarButton_Click_6(object sender, RoutedEventArgs e)
        {
            if (FileAndFolderViewer.viewFocusTag == "One")
            {
              
                        ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование" };
                        contentDialogProcecc.Copy(FileAndFolderViewer.classListStroceSelect, fileAndFolderViewer2.storageFolderFirst);
                        var x = await contentDialogProcecc.ShowAsync();
                fileAndFolderViewer2.Upgreid();

            }
            else
            {
             
                      
                            

                        ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование" };
                        contentDialogProcecc.Copy(FileAndFolderViewer.classListStroceSelect, fileAndFolderViewer.storageFolderFirst);
                        var x = await contentDialogProcecc.ShowAsync();
                fileAndFolderViewer.Upgreid();





            }
          //  Upgreid();

        }

        private async void AppBarButton_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                foreach (var vs in FileAndFolderViewer.classListStroceSelect)
                {
                    if (vs != null)
                    {


                        if (vs.FlagFolde != true)
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFile") };
                            //contentDialogProcecc.DeleteFile(vs.storageFile);
                            // var x = await contentDialogProcecc.ShowAsync();
                            await vs.storageFile.DeleteAsync();
                            // await vs.storageFile.CopyAsync(fileAndFolderViewer.storageFolderFirst1);
                        }
                        else
                        {

                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFolder") };
                            contentDialogProcecc.DeleteFolder(vs.StorageFolder);
                            var x = await contentDialogProcecc.ShowAsync();

                        }
                    }

                }
                Upgreid();

            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }


        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
                ClassListStroce cc = (ClassListStroce)secondItem.Tag;
                if (cc.FlagFolde != true)
                {
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFile") };
                    contentDialogProcecc.DeleteFile(cc.storageFile);
                    var x = await contentDialogProcecc.ShowAsync();

                }
                else
                {

                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFolder") };
                    contentDialogProcecc.DeleteFolder(cc.StorageFolder);
                    var x = await contentDialogProcecc.ShowAsync();

                }
                Upgreid();
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void Paste_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                DataPackageView dataPackageView = Clipboard.GetContent();

                if (dataPackageView.Contains(StandardDataFormats.StorageItems))
                {
                    bool filder = true;
                    if (FileAndFolderViewer.viewFocusTag == "One")
                    {
                        var storageI = await dataPackageView.GetStorageItemsAsync();
                        foreach (var dd in storageI)
                        {

                        }
                        try
                        {
                            StorageFolder storageFolder = (StorageFolder)storageI.ElementAt(0);
                            filder = true;
                        }
                        catch
                        {
                            filder = false;
                        }
                        if (!filder)
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование файла" };
                            contentDialogProcecc.CopyFile((StorageFile)storageI.ElementAt(0), fileAndFolderViewer.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();

                        }
                        else
                        {
                            //  perebor_updates(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                            // CopyDir(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование папки" };
                            contentDialogProcecc.CopyFolder((StorageFolder)storageI.ElementAt(0), fileAndFolderViewer2.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();


                        }
                        fileAndFolderViewer2.Upgreid();
                    }
                    else
                    {
                        var storageI = await dataPackageView.GetStorageItemsAsync();
                        foreach (var dd in storageI)
                        {

                        }
                        try
                        {
                            StorageFolder storageFolder = (StorageFolder)storageI.ElementAt(0);
                            filder = true;
                        }
                        catch
                        {
                            filder = false;
                        }

                        if (!filder)
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TilePasteFile") };
                            contentDialogProcecc.CopyFile((StorageFile)storageI.ElementAt(0), fileAndFolderViewer2.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();


                        }
                        else
                        {
                            //  perebor_updates(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                            // CopyDir(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TilePasteFolder") };
                            contentDialogProcecc.CopyFolder((StorageFolder)storageI.ElementAt(0), fileAndFolderViewer2.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();


                        }
                        fileAndFolderViewer.Upgreid();
                    }

                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                if (FileAndFolderViewer.viewFocusTag == "One")
                {

                    await fileAndFolderViewer.storageFolderFirst.CreateFolderAsync(resourceLoader.GetString("TextNewFolder"), CreationCollisionOption.GenerateUniqueName);
                    fileAndFolderViewer.Upgreid();
                }
                else
                {


                    await fileAndFolderViewer2.storageFolderFirst.CreateFolderAsync(resourceLoader.GetString("TextNewFolder"), CreationCollisionOption.GenerateUniqueName);
                    fileAndFolderViewer2.Upgreid();

                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
               
        }

        private async void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                if (FileAndFolderViewer.viewFocusTag == "One")
                {
                    ContentDialogNewFile contentDialogNewFile = new ContentDialogNewFile() { };
                    contentDialogNewFile.storageFolder = fileAndFolderViewer.storageFolderFirst;
                    await contentDialogNewFile.ShowAsync();
                    fileAndFolderViewer.Upgreid();
                }
                else
                {
                    ContentDialogNewFile contentDialogNewFile = new ContentDialogNewFile() { };
                    contentDialogNewFile.storageFolder = fileAndFolderViewer2.storageFolderFirst;
                    await contentDialogNewFile.ShowAsync();
                    fileAndFolderViewer2.Upgreid();
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void AppBarButton_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {


                
                    foreach (var vs in FileAndFolderViewer.classListStroceSelect)
                    {
                        if (vs != null)
                        {

                            ContentDialogRemove contentDialogNewFile = new ContentDialogRemove() { };
                            contentDialogNewFile.storageFolder = vs.StorageFolder;
                            if (vs.FlagFolde)
                            {
                                contentDialogNewFile.folder = vs.FlagFolde;
                                contentDialogNewFile.storageFolder = vs.StorageFolder;
                                await contentDialogNewFile.ShowAsync();
                            }
                            else
                            {
                                contentDialogNewFile.folder = vs.FlagFolde;
                                contentDialogNewFile.storageFile = vs.storageFile;
                                await contentDialogNewFile.ShowAsync();
                            }
                            Upgreid();
                        }
                    }
                
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
          
        }

        private async void Inf_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
                ClassListStroce cc = (ClassListStroce)secondItem.Tag;
                if (cc.FlagFolde)
                {
                    StringBuilder fileProperties = new StringBuilder();
                    fileProperties.AppendLine(resourceLoader.GetString("NameText") + ": " + cc.StorageFolder.Name);
                    fileProperties.AppendLine(resourceLoader.GetString("TipText") + ": " + cc.StorageFolder.DisplayType);
                    fileProperties.AppendLine(resourceLoader.GetString("PathText") + ": " + cc.StorageFolder.Path);
                    fileProperties.AppendLine(resourceLoader.GetString("CreatText") + ": " + cc.StorageFolder.DateCreated.ToString());
                    // Get file's basic properties.
                    Windows.Storage.FileProperties.BasicProperties basicProperties =
                        await cc.StorageFolder.GetBasicPropertiesAsync();

                    fileProperties.AppendLine(resourceLoader.GetString("IzmenText") + ": " + basicProperties.DateModified);
                    MessageDialog messageDialog = new MessageDialog(fileProperties.ToString(), resourceLoader.GetString("InfoText"));
                    await messageDialog.ShowAsync();

                }
                else
                {
                    StringBuilder fileProperties = new StringBuilder();
                    fileProperties.AppendLine(resourceLoader.GetString("NameText") + ": " + cc.storageFile.Name);
                    fileProperties.AppendLine(resourceLoader.GetString("TipText") + ": " + cc.storageFile.DisplayType);
                    fileProperties.AppendLine(resourceLoader.GetString("PathText") + ": " + cc.storageFile.Path);
                    fileProperties.AppendLine(resourceLoader.GetString("CreatText") + ": " + cc.storageFile.DateCreated.ToString());
                    // Get file's basic properties.
                    Windows.Storage.FileProperties.BasicProperties basicProperties =
                        await cc.storageFile.GetBasicPropertiesAsync();
                    string fileSize = string.Format("{0:n0}", basicProperties.Size);
                    fileProperties.AppendLine(resourceLoader.GetString("SizeText") + ": " + fileSize + " байт(ов)");
                    fileProperties.AppendLine(resourceLoader.GetString("IzmenText") + ": " + basicProperties.DateModified);
                    MessageDialog messageDialog = new MessageDialog(fileProperties.ToString(), resourceLoader.GetString("InfoText"));
                    await messageDialog.ShowAsync();
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private  void AppBarButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            
          
            fileAndFolderViewer.InitializeDataGridView();
        }
        private void AppBarButton_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            fileAndFolderViewer2.InitializeDataGridView();
        }

        private async void AppBarButton_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {


            
        
       
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void OpenAs_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
            ClassListStroce cc = (ClassListStroce)secondItem.Tag;
            if (cc.FlagFolde != true)
            {
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = true;

                // Launch the retrieved file
                bool success = await Windows.System.Launcher.LaunchFileAsync(cc.storageFile, options);
                if (success)
                {
                    // File launched
                }
                else
                {

                }
               
            }
        }

        public async void newUsB()
        {
            try
            {


                StorageFolder Folder = KnownFolders.RemovableDevices;
                IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                foreach (StorageFolder FlFolder1 in folderList)
                {
                    var f = (from d in fileAndFolderViewer.ListUSB where d.DisplayName == FlFolder1.DisplayName select d).Count();
                    if (f == 0)
                    {

                        await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                  async () =>
                  {
                      if (fileAndFolderViewer.Path == String.Empty)
                      {
                          var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                          fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                      }
                      if (fileAndFolderViewer2.Path == String.Empty)
                      {
                          var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                          fileAndFolderViewer2.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                      }
                      fileAndFolderViewer.ListUSB.Add(FlFolder1);
                      TreeViewNode pictureNode1 = new TreeViewNode();
                      pictureNode1.Content = FlFolder1;
                      pictureNode1.IsExpanded = false;

                      pictureNode1.HasUnrealizedChildren = true;

                      sampleTreeView1.RootNodes.Add(pictureNode1);
                      FillTreeNode(pictureNode1);

                      TreeViewNode pictureNode2 = new TreeViewNode();
                      pictureNode2.Content = FlFolder1;
                      pictureNode2.IsExpanded = false;

                      pictureNode2.HasUnrealizedChildren = true;

                      sampleTreeView.RootNodes.Add(pictureNode2);
                      FillTreeNode(pictureNode2);
                  //добавить в древовидное окружение и на панели
                  // fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1 });

              });
                    }

                }
                if (fileAndFolderViewer.ListUSB.Count > 0)
                {

                    try
                    {


                        foreach (var FlFolder1 in fileAndFolderViewer.ListUSB)
                        {
                            var f = (from d in folderList where d.DisplayName == FlFolder1.DisplayName select d).Count();
                            if (f == 0)
                            {
                                fileAndFolderViewer.ListUSB.Remove(FlFolder1);
                                await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                          async () =>
                          {
                              if (fileAndFolderViewer.Path == String.Empty)
                              {
                              //InitializeDataGridView1();
                              fileAndFolderViewer.InitializeDataGridView();
                              }
                              if (fileAndFolderViewer2.Path == String.Empty)
                              {
                                  fileAndFolderViewer2.InitializeDataGridView();
                              }
                              InitializeTreeView(sampleTreeView1);
                              InitializeTreeView(sampleTreeView);



                          });
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        await new MessageDialog(ex.Message).ShowAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
      
        private void AppBarButton_Click_10(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();

        }
        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequestDeferral deferral = args.Request.GetDeferral();

            // MyLogger has SendLogs method but calling this in Button_Click above will not work and the Share
            // dialog will just open and close.  Moving it down to this event solves the issue.

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                DataRequest request = args.Request;
                
                   
                    if (shareList != null)
                    {
                        if (shareList.FlagFolde != true)
                        {
                           
                            try
                            {


                                StorageFile myFile = shareList.storageFile;
                                request.Data.Properties.Title = "Share My File";
                                request.Data.Properties.Description = string.Format("Share log file {0}.", myFile.DisplayName);

                                List<IStorageItem> myStorageItems = new List<IStorageItem>() { myFile };
                                request.Data.SetStorageItems(myStorageItems);
                            
                                deferral.Complete();
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine(ex.ToString());
                                request.FailWithDisplayText(ex.ToString());
                              
                                
                              
                            }

                        }
                       
                    }
                
            
               
            });

        }
        ClassListStroce shareList;
        private async void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {


                MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
                shareList = (ClassListStroce)secondItem.Tag;
              //  if (shareList != null)
                {


                    if (shareList.FlagFolde != true)
                    {


                        DataTransferManager.ShowShareUI();


                    }
                }
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.ToString());
               await messageDialog.ShowAsync();

            }
            
        }
        private async void BGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && sampleTreeView!=null)
            {
                if (rb.Tag.ToString() == "Max")
                {
                    FileAndFolderViewer.Type = "Maximum";
                }
                if (rb.Tag.ToString() == "Min")
                {
                    FileAndFolderViewer.Type = "Middle";
                }
            
                
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["ViewSet"] = rb.Tag.ToString();
                // Upgreid();
                fileAndFolderViewer.Upgreid();

                fileAndFolderViewer2.Upgreid();
                
                
              
              

            }
           
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabView tabView = (TabView)sender;
            if(tabView.SelectedIndex==1)
            {
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                String localValue = localSettings.Values["ViewSet"] as string;
                if(localValue=="Max")
                {
                    chec1.IsChecked = true;
                }
                else
                {
                    chec2.IsChecked = true;
                }
            }
        }

        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TextBox textBox = (TextBox)sender;
                if(textBox!=null && textBox.Text!=null)
                {
                    try
                    {
                        

                        StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(textBox.Text);

                     
                        if(FileAndFolderViewer.classListStroceSelect==null)
                        {
                            FileAndFolderViewer.classListStroceSelect = new List<ClassListStroce>();
                        }
                        else
                        {
                            FileAndFolderViewer.classListStroceSelect.Clear();
                        }
                    
                        FileAndFolderViewer.classListStroceSelect.Add(new ClassListStroce() { StorageFolder = folder, FlagFolde = true, Type = FileAndFolderViewer.Type });
                        fileAndFolderViewer.Next();
                    }
                    catch(Exception ex)
                    {
                        await new MessageDialog(ex.Message).ShowAsync();
                    }
                }
            }
        }

        private async void TextBox_KeyDown_1(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TextBox textBox = (TextBox)sender;
                if (textBox != null && textBox.Text != null)
                {
                    try
                    {


                        StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(textBox.Text);


                        if (FileAndFolderViewer.classListStroceSelect == null)
                        {
                            FileAndFolderViewer.classListStroceSelect = new List<ClassListStroce>();
                        }
                        else
                        {
                            FileAndFolderViewer.classListStroceSelect.Clear();
                        }

                        FileAndFolderViewer.classListStroceSelect.Add(new ClassListStroce() { StorageFolder = folder, FlagFolde = true, Type = FileAndFolderViewer.Type });
                        fileAndFolderViewer2.Next();
                    }
                    catch (Exception ex)
                    {
                        await new MessageDialog(ex.Message).ShowAsync();
                    }
                }
            }
        }
  

       
    
        private void UnorganizedListView_OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
          
        }
        private async void UnorganizedListView_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {


            try
            {


             //   ClassListStroce cc = FileAndFolderViewer.classListStroceSelect.ElementAt(0);
                var cc = (ClassListStroce)e.Items.ElementAt(0);

                if (cc.FlagFolde != true)
                {


                    StorageFile myFile = cc.storageFile;
                    List<IStorageItem> myStorageItems = new List<IStorageItem>() { myFile };
                    e.Data.SetStorageItems(myStorageItems);
                    e.Data.RequestedOperation = DataPackageOperation.Copy;

                }
                else
                {
                    StorageFolder myFile = cc.StorageFolder;
                    List<IStorageItem> myStorageItems = new List<IStorageItem>() { myFile };
                    e.Data.SetStorageItems(myStorageItems);
                    e.Data.RequestedOperation = DataPackageOperation.Copy;
                }


            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }


           

            
            
           
        }
      
        private async void UnorganizedListView_OnDrop(object sender, DragEventArgs e)
        {
            try
            {


                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {


                    foreach (IStorageItem item in await e.DataView.GetStorageItemsAsync())
                    {
                        if (item.IsOfType(StorageItemTypes.Folder))
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование папки" };
                            contentDialogProcecc.CopyFolder(item as StorageFolder, fileAndFolderViewer.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();

                            fileAndFolderViewer.Upgreid();
                            // tabInstance.instanceInteraction.CloneDirectoryAsync((item as StorageFolder).Path, tabInstance.instanceViewModel.Universal.path, (item as StorageFolder).DisplayName);
                        }
                        else
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование файла" };
                            contentDialogProcecc.CopyFile(item as StorageFile, fileAndFolderViewer.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();


                            fileAndFolderViewer.Upgreid();
                            // await (item as StorageFile).CopyAsync(await StorageFolder.GetFolderFromPathAsync(tabInstance.instanceViewModel.Universal.path));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
        private async void UnorganizedListView_OnDrop1(object sender, DragEventArgs e)
        {
            try
            {


                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {


                    foreach (IStorageItem item in await e.DataView.GetStorageItemsAsync())
                    {
                        if (item.IsOfType(StorageItemTypes.Folder))
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование папки" };
                            contentDialogProcecc.CopyFolder(item as StorageFolder, fileAndFolderViewer2.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();

                            fileAndFolderViewer2.Upgreid();
                            // tabInstance.instanceInteraction.CloneDirectoryAsync((item as StorageFolder).Path, tabInstance.instanceViewModel.Universal.path, (item as StorageFolder).DisplayName);
                        }
                        else
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование файла" };
                            contentDialogProcecc.CopyFile(item as StorageFile, fileAndFolderViewer2.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();


                            fileAndFolderViewer2.Upgreid();
                            // await (item as StorageFile).CopyAsync(await StorageFolder.GetFolderFromPathAsync(tabInstance.instanceViewModel.Universal.path));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

    }
  
}
