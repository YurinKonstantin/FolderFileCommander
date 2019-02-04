using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Grid;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
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
            


        }
        bool focusOne=true;
        FileAndFolderViewer fileAndFolderViewer { get; set; }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
           
           var storageItemAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
            if(storageItemAccessList.Entries.Count==0)
            {
                ContentDialog subscribeDialog = new ContentDialog
                {
                    Title = "Перед тем как начать давайте добавим расположения для работы",
                    Content = "Приложение будет иметь доступ к указанным папкам и содержимому. Вы так же можете вдальнейшем добавлять или удалять расположения. Вы так же имеете изначально доступ к папкам  с музыкой и изображением",
                    CloseButtonText = "Не сейчас",
                    PrimaryButtonText = "Добавить",
                    
                    DefaultButton = ContentDialogButton.Primary
                };

                ContentDialogResult result = await subscribeDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                    folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                    folderPicker.FileTypeFilter.Add("*");

                    Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                    if (folder != null)
                    {
                        // Application now has read/write access to all contents in the picked folder
                        // (including other sub-folder contents)
                        Windows.Storage.AccessCache.StorageApplicationPermissions.
                        FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                      
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                    // Do nothing.
                }
                //  foreach (Windows.Storage.AccessCache.AccessListEntry entry in storageItemAccessList.Entries)
                // {
                //    string mruToken = entry.Token;
                //    string mruMetadata = entry.Metadata;
                // Windows.Storage.IStorageItem item = await storageItemAccessList.GetItemAsync(mruToken);
                // The type of item will tell you whether it's a file or a folder.

                //}
            }
            InitializeTreeView();
            InitializeTreeView1();
            InitializeDataGridView();
        }
        public async Task addLocation()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(folder.DisplayName, folder);

            }
            else
            {

            }
            fileAndFolderViewer.ListCol.Clear();
            sampleTreeView.RootNodes.Clear();
            InitializeTreeView();
            InitializeDataGridView();

        }
     
       
        List<String> listDostyp = new List<string>();
        private async void InitializeTreeView1()
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            TreeViewNode pictureNode = new TreeViewNode();
            pictureNode.Content = picturesFolder;
            pictureNode.IsExpanded = false;

            pictureNode.HasUnrealizedChildren = true;
           
            sampleTreeView1.RootNodes.Add(pictureNode);
            FillTreeNode1(pictureNode);






            // Get Music library.
            StorageFolder musicFolder = KnownFolders.MusicLibrary;
            TreeViewNode musicNode = new TreeViewNode();
            musicNode.Content = musicFolder;
            musicNode.IsExpanded = false;
            musicNode.HasUnrealizedChildren = true;
      
            sampleTreeView1.RootNodes.Add(musicNode);
            FillTreeNode1(musicNode);


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
                    AssecNode.Content = ss;
                    AssecNode.IsExpanded = false;
                    AssecNode.HasUnrealizedChildren = true;
                
                    sampleTreeView1.RootNodes.Add(AssecNode);
                    FillTreeNode1(AssecNode);
                }
            }
        }
        private async void InitializeTreeView()
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            TreeViewNode pictureNode = new TreeViewNode();
            pictureNode.Content = picturesFolder;
            pictureNode.IsExpanded = false;
           
            pictureNode.HasUnrealizedChildren = true;
            sampleTreeView.RootNodes.Add(pictureNode);
        
            FillTreeNode(pictureNode);


           


          
            // Get Music library.
            StorageFolder musicFolder = KnownFolders.MusicLibrary;
            TreeViewNode musicNode = new TreeViewNode();
            musicNode.Content = musicFolder;
            musicNode.IsExpanded = false;
            musicNode.HasUnrealizedChildren = true;
            sampleTreeView.RootNodes.Add(musicNode);
         
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
                    AssecNode.Content = ss;
                    AssecNode.IsExpanded = false;
                    AssecNode.HasUnrealizedChildren = true;
                    sampleTreeView.RootNodes.Add(AssecNode);
               
                    FillTreeNode(AssecNode);
                }
            }
        }
        private async void InitializeDataGridView()
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            var thumbnail = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnail });
            fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnail });



            // Get Music library.
            StorageFolder musicFolder = KnownFolders.MusicLibrary;
            var thumbnail1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnail1 });
            fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnail1 });

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
                    var thumbnail11 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                    fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail11 });
                    fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail11 });
                }
            }

        }
        private async void InitializeDataGridView1()
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            var thumbnail11 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnail11 });
          



            // Get Music library.
            StorageFolder musicFolder = KnownFolders.MusicLibrary;
            var thumbnail = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnail });
       

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
                    var thumbnail1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                    fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail1 });
               
                }
            }

        }

        private async void InitializeDataGridView2()
        {
            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            var thumbnail11 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
            fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnail11 });



            // Get Music library.
            StorageFolder musicFolder = KnownFolders.MusicLibrary;
            var thumbnail = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
            fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnail });

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
                    var thumbnail1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                    fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnail1 });
                }
            }

        }

        private async void FillTreeNode(TreeViewNode node)
        {
            // Get the contents of the folder represented by the current tree node.
            // Add each item as a new child node of the node that's being expanded.

            // Only process the node if it's a folder and has unrealized children.
            StorageFolder folder = null;
            if (node.Content is StorageFolder && node.HasUnrealizedChildren == true)
            {
                folder = node.Content as StorageFolder;
            }
            else
            {
                // The node isn't a folder, or it's already been filled.
                return;
            }

            IReadOnlyList<IStorageItem> itemsList = await folder.GetFoldersAsync();

            if (itemsList.Count == 0)
            {
                // The item is a folder, but it's empty. Leave HasUnrealizedChildren = true so
                // that the chevron appears, but don't try to process children that aren't there.
                return;
            }

            foreach (var item in itemsList)
            {
                var newNode = new TreeViewNode();
                newNode.Content = item;

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
        private async void FillTreeNode1(TreeViewNode node)
        {
            // Get the contents of the folder represented by the current tree node.
            // Add each item as a new child node of the node that's being expanded.

            // Only process the node if it's a folder and has unrealized children.
            StorageFolder folder = null;
            if (node.Content is StorageFolder && node.HasUnrealizedChildren == true)
            {
                folder = node.Content as StorageFolder;
            }
            else
            {
                // The node isn't a folder, or it's already been filled.
                return;
            }

            IReadOnlyList<IStorageItem> itemsList = await folder.GetFoldersAsync();

            if (itemsList.Count == 0)
            {
                // The item is a folder, but it's empty. Leave HasUnrealizedChildren = true so
                // that the chevron appears, but don't try to process children that aren't there.
                return;
            }

            foreach (var item in itemsList)
            {
                var newNode = new TreeViewNode();
                newNode.Content = item;

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
        private void SampleTreeView_Expanding1(TreeView sender, TreeViewExpandingEventArgs args)
        {
            if (args.Node.HasUnrealizedChildren)
            {

                FillTreeNode1(args.Node);
            }
        }
        private void SampleTreeView_Collapsed(TreeView sender, TreeViewCollapsedEventArgs args)
        {
            args.Node.Children.Clear();
            args.Node.HasUnrealizedChildren = true;
        }
        private void SampleTreeView_Collapsed1(TreeView sender, TreeViewCollapsedEventArgs args)
        {
            args.Node.Children.Clear();
            args.Node.HasUnrealizedChildren = true;
        }
        private async void SampleTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            TreeView treeView = (TreeView)sender;
               var node = args.InvokedItem as TreeViewNode;
            if (node.Content is IStorageItem item)
            {
                
               // FileNameTextBlock.Text = item.Name;
               // FilePathTextBlock.Text = item.Path+" "+ node.Depth.ToString();
               // TreeDepthTextBlock.Text = node.Depth.ToString();

                if (node.Content is StorageFolder folder)
                {
                    
                        _DataGrid.SelectedItem = null;

                        fileAndFolderViewer.ListCol.Clear();
                        fileAndFolderViewer.ListColName.Clear();
                        fileAndFolderViewer.storageFolderFirst = folder;

                        IReadOnlyList<StorageFolder> folderList =
                        await folder.GetFoldersAsync();
                        IReadOnlyList<StorageFile> fileList =
                               await folder.GetFilesAsync();
                        if (fileList.Count != 0 || folderList.Count != 0)
                        {


                            foreach (StorageFolder d in folderList)
                            {
                                var thumbnail1 = await d.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true, ThumbnailMode = thumbnail1 });
                                fileAndFolderViewer.ListColName.Add(d.DisplayName);
                            }

                            foreach (StorageFile file in fileList)
                            {
                                var thumbnail1 = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = file, FlagFolde = false, ThumbnailMode = thumbnail1 });
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
            if (node.Content is IStorageItem item)
            {

                // FileNameTextBlock.Text = item.Name;
           //     FilePathTextBlock.Text = item.Path + " " + node.Depth.ToString();
                // TreeDepthTextBlock.Text = node.Depth.ToString();

                if (node.Content is StorageFolder folder)
                {
                    
                        _DataGrid2.SelectedItem = null;

                        fileAndFolderViewer.ListCol1.Clear();
                        fileAndFolderViewer.ListColName2.Clear();
                        fileAndFolderViewer.storageFolderFirst1 = folder;

                        IReadOnlyList<StorageFolder> folderList =
                        await folder.GetFoldersAsync();
                        IReadOnlyList<StorageFile> fileList =
                               await folder.GetFilesAsync();
                        if (fileList.Count != 0 || folderList.Count != 0)
                        {


                            foreach (StorageFolder d in folderList)
                            {
                                var thumbnail1 = await d.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true, ThumbnailMode = thumbnail1 });
                                fileAndFolderViewer.ListColName2.Add(d.DisplayName);
                            }

                            foreach (StorageFile file in fileList)
                            {
                                var thumbnail1 = await file.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { storageFile = file, FlagFolde = false, ThumbnailMode = thumbnail1 });
                                fileAndFolderViewer.ListColName2.Add(file.DisplayName);
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

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            ListView listView;
           
                listView = _DataGrid;
          
          
            if (listView != null)
            {



                // StorageFolderQueryResult queryResult =
                //  classListStroce.storageFolder.CreateFolderQuery(CommonFolderQuery.GroupByMonth);

               

                    try
                    {

                        StorageFolder storageFolder = await fileAndFolderViewer.storageFolderFirst.GetParentAsync();
                        IReadOnlyList<StorageFolder> folderList =
                           await storageFolder.GetFoldersAsync();
                        IReadOnlyList<StorageFile> fileList =
                          await storageFolder.GetFilesAsync();

                        fileAndFolderViewer.storageFolderFirst = storageFolder;
                        fileAndFolderViewer.ListCol.Clear();
                    fileAndFolderViewer.ListColName.Clear();
                    foreach (StorageFolder FlFolder1 in folderList)
                        {
                        var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                        fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail });
                        fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                        }
                        foreach (StorageFile FlFolder1 in fileList)
                        {
                        var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                        fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode= thumbnail1 });
                        fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                    }
                    }
                    catch (Exception ex)
                    {
                        fileAndFolderViewer.ListCol.Clear();
                    fileAndFolderViewer.ListColName.Clear();
                    InitializeDataGridView1();
                    }
              
                

            }
        }

        private async void AppBarButtonNazad_Click_1(object sender, RoutedEventArgs e)
        {
            ListView listView;
          
                listView = _DataGrid2;


            if (listView != null)
            {



                // StorageFolderQueryResult queryResult =
                //  classListStroce.storageFolder.CreateFolderQuery(CommonFolderQuery.GroupByMonth);





                try
                {

                    StorageFolder storageFolder = await fileAndFolderViewer.storageFolderFirst1.GetParentAsync();
                    IReadOnlyList<StorageFolder> folderList =
                       await storageFolder.GetFoldersAsync();
                    IReadOnlyList<StorageFile> fileList =
                      await storageFolder.GetFilesAsync();

                    fileAndFolderViewer.storageFolderFirst1 = storageFolder;
                    fileAndFolderViewer.ListCol1.Clear();
                    foreach (StorageFolder FlFolder1 in folderList)
                    {
                        var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                        fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1 });
                    }
                    foreach (StorageFile FlFolder1 in fileList)
                    {
                        var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                        fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail1 });
                    }
                }
                catch (Exception ex)
                {
                    fileAndFolderViewer.ListCol1.Clear();
                    InitializeDataGridView2();
                }
            }
             


            
        }

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            await addLocation();
        }

        private async void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Remove(listDop.SelectedItem.ToString());
            fileAndFolderViewer.ListCol.Clear();
            fileAndFolderViewer.ListCol1.Clear();
            sampleTreeView.RootNodes.Clear();
            sampleTreeView1.RootNodes.Clear();
            InitializeTreeView();
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
           if(tag.Tag.ToString()== "twy")
            {
                focusOne = false;
                _DataGrid.SelectedItem = null;
                ClassListStroce classListStroce = (ClassListStroce)tag.SelectedItem;
                if (fileAndFolderViewer.storageFolderFirst1 != null)
                    FilePathTextBlock.Text = fileAndFolderViewer.storageFolderFirst1.Path;
            }
           else
            {
                focusOne = true;
                _DataGrid2.SelectedItem = null;
                ClassListStroce classListStroce = (ClassListStroce)tag.SelectedItem;
                if(fileAndFolderViewer.storageFolderFirst != null)
                FilePathTextBlock.Text = fileAndFolderViewer.storageFolderFirst.Path;
            }
            
            
        }
        public async Task ActivateFile(StorageFile storageFile)
        {
            var success = await Windows.System.Launcher.LaunchFileAsync(storageFile);
        }
        private async void List1_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
      ListView listView = (ListView)sender;
            if (listView.Tag.ToString() == "One")
            {
                if (listView.SelectedItem != null)
                {

                    ClassListStroce classListStroce = (ClassListStroce)listView.SelectedItem;


                    if (listView != null)
                    {
                        // StorageFolderQueryResult queryResult =
                        //  classListStroce.storageFolder.CreateFolderQuery(CommonFolderQuery.GroupByMonth);

                        if (classListStroce != null)
                        {
                            if (classListStroce.FlagFolde)
                            {



                                IReadOnlyList<StorageFolder> folderList = await classListStroce.StorageFolder.GetFoldersAsync();
                                IReadOnlyList<StorageFile> fileList = await classListStroce.StorageFolder.GetFilesAsync();

                                fileAndFolderViewer.storageFolderFirst = classListStroce.StorageFolder;
                                fileAndFolderViewer.ListCol.Clear();
                                fileAndFolderViewer.ListColName.Clear();
                                foreach (StorageFolder FlFolder1 in folderList)
                                {
                                    var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                    fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail });
                                    fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                                }
                                foreach (StorageFile FlFolder1 in fileList)
                                {
                                    var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                    fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail });
                                    fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                                }
                                _DataGrid.SelectedIndex = 0;

                            }
                            else
                            {
                                await ActivateFile(classListStroce.storageFile);
                            }
                        }


                    }



                    listView.SelectedItem = null;
                }
            }
            else
            {
                if (listView.SelectedItem != null)
                {

                    ClassListStroce classListStroce = (ClassListStroce)listView.SelectedItem;


                    if (listView != null)
                    {
                        // StorageFolderQueryResult queryResult =
                        //  classListStroce.storageFolder.CreateFolderQuery(CommonFolderQuery.GroupByMonth);

                        if (classListStroce != null)
                        {
                            if (classListStroce.FlagFolde)
                            {


                                IReadOnlyList<StorageFolder> folderList = await classListStroce.StorageFolder.GetFoldersAsync();
                                IReadOnlyList<StorageFile> fileList = await classListStroce.StorageFolder.GetFilesAsync();

                                fileAndFolderViewer.storageFolderFirst1 = classListStroce.StorageFolder;
                                fileAndFolderViewer.ListCol1.Clear();
                                fileAndFolderViewer.ListColName.Clear();
                                foreach (StorageFolder FlFolder1 in folderList)
                                {
                                    var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                    fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail });
                                    fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                                }
                                foreach (StorageFile FlFolder1 in fileList)
                                {
                                    var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                    fileAndFolderViewer.ListCol1.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail });
                                    fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                                }
                                _DataGrid2.SelectedIndex = 0;

                            }
                            else
                            {
                                await ActivateFile(classListStroce.storageFile);
                            }
                        }


                    }



                    listView.SelectedItem = null;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Split1.IsPaneOpen = !Split1.IsPaneOpen;
        }

        private async void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            if(focusOne==true)
            {
                var vs = (ClassListStroce)_DataGrid.SelectedItem;
               if (vs.FlagFolde!=true)
                {
                   
                   await ActivateFile(vs.storageFile);
                }
            }
            else
            {
                var vs = (ClassListStroce)_DataGrid2.SelectedItem;
                if (vs.FlagFolde != true)
                {

                    await ActivateFile(vs.storageFile);
                }
            }
        }

        private void AppBarButton_Click_5(object sender, RoutedEventArgs e)
        {
            if (focusOne == true)
            {
                _DataGrid.SelectAll();
                
            }
            else
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
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail });
                                fileAndFolderViewer.ListColName.Add(FlFolder1.DisplayName);
                            }
                            foreach (StorageFile FlFolder1 in fileList)
                            {
                                var thumbnail = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                                fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false, ThumbnailMode = thumbnail });
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
            var content = a.FileAndFildeName;
            Copy.Tag = a;
            open.Tag = a;
            Perem.Tag = a;
            Paste.Tag = a;
            
            Delete.Tag = a;
            Inf.Tag = a;

        }
        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
            ClassListStroce cc = (ClassListStroce)secondItem.Tag;
            MessageDialog messageDialog =new MessageDialog(cc.FileAndFildeTip.ToString());
         await messageDialog.ShowAsync();
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
            ClassListStroce cc = (ClassListStroce)secondItem.Tag;
            if (cc.FlagFolde != true)
            {

                await ActivateFile(cc.storageFile);
            }

        }


        private async void AppBarButton_Click_6(object sender, RoutedEventArgs e)
        {
            if (focusOne == true)
            {
                var vs = (ClassListStroce)_DataGrid.SelectedItem;
                if (vs.FlagFolde != true)
                {
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование файла" };
                    contentDialogProcecc.CopyFile(vs.storageFile, fileAndFolderViewer.storageFolderFirst1);
                    var x = await contentDialogProcecc.ShowAsync();
                   
                }
                else
                {
                    //  perebor_updates(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                    // CopyDir(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title="Копирование папки"};
                    contentDialogProcecc.CopyFolder(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                 var x=  await contentDialogProcecc.ShowAsync();
                 

                }
            }
            else
            {
                var vs = (ClassListStroce)_DataGrid2.SelectedItem;
                if (vs.FlagFolde != true)
                {
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование файла" };
                    contentDialogProcecc.CopyFile(vs.storageFile, fileAndFolderViewer.storageFolderFirst);
                    var x = await contentDialogProcecc.ShowAsync();
                   
                  
                }
                else
                {
                    //  perebor_updates(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                    // CopyDir(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst1);
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Копирование папки" };
                    contentDialogProcecc.CopyFolder(vs.StorageFolder, fileAndFolderViewer.storageFolderFirst);
                    var x = await contentDialogProcecc.ShowAsync();
                   

                }
            }

        }

        private async void AppBarButton_Click_7(object sender, RoutedEventArgs e)
        {
            if (focusOne == true)
            {
                var vs = (ClassListStroce)_DataGrid.SelectedItem;
                if (vs.FlagFolde != true)
                {
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Удаление файла" };
                    //contentDialogProcecc.DeleteFile(vs.storageFile);
                    // var x = await contentDialogProcecc.ShowAsync();
                    await vs.storageFile.DeleteAsync();
                    // await vs.storageFile.CopyAsync(fileAndFolderViewer.storageFolderFirst1);
                }
                else
                {
                    
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Удаление папки" };
                    contentDialogProcecc.DeleteFolder(vs.StorageFolder);
                    var x = await contentDialogProcecc.ShowAsync();
                    
                }
            }
            else
            {
                var vs = (ClassListStroce)_DataGrid2.SelectedItem;
                if (vs.FlagFolde != true)
                {
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Удаление файла" };
                    //  contentDialogProcecc.DeleteFile(vs.storageFile);
                    // var x = await contentDialogProcecc.ShowAsync();
                   await vs.storageFile.DeleteAsync();
                 //   await vs.storageFile.CopyAsync(fileAndFolderViewer.storageFolderFirst1);
                }
                else
                {

                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Удаление папки" };
                    contentDialogProcecc.DeleteFolder(vs.StorageFolder);
                    var x = await contentDialogProcecc.ShowAsync();

                }
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem secondItem = (MenuFlyoutItem)sender;
            ClassListStroce cc = (ClassListStroce)secondItem.Tag;
            if (cc.FlagFolde != true)
            {
                ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Удаление файла" };
                contentDialogProcecc.DeleteFile(cc.storageFile);
                var x = await contentDialogProcecc.ShowAsync();
           
            }
            else
            {

                ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = "Удаление папки" };
                contentDialogProcecc.DeleteFolder(cc.StorageFolder);
                var x = await contentDialogProcecc.ShowAsync();

            }
        }
    }
}
