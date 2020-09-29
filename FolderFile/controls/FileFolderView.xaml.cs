using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
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

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace FolderFile.controls
{
    public sealed partial class FileFolderView : UserControl
    {
        public FileFolderView()
        {
            Debug.WriteLine("r0");
            this.InitializeComponent();
            FrameworkElement root = (FrameworkElement)Window.Current.Content;

            Debug.WriteLine("r1");
            this.fileAndFolderViewer = new FileAndFolderViewer();
            Debug.WriteLine("r1");


            InitializeTreeView();
            Debug.WriteLine("r3");




            dataTransferManager = DataTransferManager.GetForCurrentView();


            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }


        public void iniz(string s)
        {
            if(s=="L")
            {
                butright.Visibility = Visibility.Collapsed;
                butleft.Visibility = Visibility.Visible;
                Split.PanePlacement = SplitViewPanePlacement.Left;
            }
            else
            {
                butright.Visibility = Visibility.Visible;
                butleft.Visibility = Visibility.Collapsed;
                Split.PanePlacement = SplitViewPanePlacement.Right;
            }
        }
        private DataTransferManager dataTransferManager;

       public FileAndFolderViewer fileAndFolderViewer { get; set; }
        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                TextBox textBox = (TextBox)sender;
                if (textBox != null && textBox.Text != null)
                {
                    try
                    {
                        StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(textBox.Text);
                        if (fileAndFolderViewer.classListStroceSelect == null)
                        {
                            fileAndFolderViewer.classListStroceSelect = new List<ClassListStroce>();
                        }
                        else
                        {
                            fileAndFolderViewer.classListStroceSelect.Clear();
                        }

                        fileAndFolderViewer.classListStroceSelect.Add(new ClassListStroce() { StorageFolder = folder, FlagFolde = true, Type = FileAndFolderViewer.Type });
                        fileAndFolderViewer.Next();
                    }
                    catch (Exception ex)
                    {
                        await new MessageDialog(ex.Message).ShowAsync();
                    }
                }
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
                var thumbnaild3 = await ss.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 32, ThumbnailOptions.ResizeThumbnail);
                var it = new ClassListStroce() { FlagFolde = true, StorageFolder = item as StorageFolder, ThumbnailMode = thumbnaild3 };
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
                            //var thumbnail1 = await d.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                            // fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true, Type = FileAndFolderViewer.Type });
                            fileAndFolderViewer.ListColName.Add(d.DisplayName);
                        }

                        foreach (StorageFile file in fileList)
                        {
                            var thumbnail1 = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale);
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = file, FlagFolde = false, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                            fileAndFolderViewer.ListColName.Add(file.DisplayName);
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
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }


        public async void InitializeTreeView()
        {
            sampleTreeView.RootNodes.Clear();
            TreeViewNode pictureNodestorageFolder = new TreeViewNode();

            // A TreeView can have more than 1 root node. The Pictures library
            // and the Music library will each be a root node in the tree.
            // Get Pictures library.
            try
            {

                try
                {


                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Desktop);

                    var thumbnaildstorageFolder = await storageFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.ResizeThumbnail);
                    pictureNodestorageFolder.Content = new ClassListStroce() { StorageFolder = storageFolder, FlagFolde = true, ThumbnailMode = thumbnaildstorageFolder };
                    pictureNodestorageFolder.IsExpanded = false;
                    pictureNodestorageFolder.HasUnrealizedChildren = true;
                    sampleTreeView.RootNodes.Add(pictureNodestorageFolder);
                    FillTreeNode(pictureNodestorageFolder);
                }
                catch (Exception)
                {

                }


                try
                {


                    StorageFolder storageFolderDownloads = await StorageFolder.GetFolderFromPathAsync(Windows.Storage.UserDataPaths.GetDefault().Downloads);
                    TreeViewNode pictureNodestorageFolderDownloads = new TreeViewNode();
                    var thumbnaildstorageFolderDownloads = await storageFolderDownloads.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.ResizeThumbnail);
                    pictureNodestorageFolderDownloads.Content = new ClassListStroce() { StorageFolder = storageFolderDownloads, FlagFolde = true, ThumbnailMode = thumbnaildstorageFolderDownloads };
                    pictureNodestorageFolderDownloads.IsExpanded = false;

                    pictureNodestorageFolderDownloads.HasUnrealizedChildren = true;

                    sampleTreeView.RootNodes.Add(pictureNodestorageFolderDownloads);
                    FillTreeNode(pictureNodestorageFolder);
                }
                catch (Exception)
                {

                }



                try
                {


                    StorageFolder documentFolder = KnownFolders.DocumentsLibrary;
                    TreeViewNode pictureNode1 = new TreeViewNode();
                    var thumbnaild = await documentFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.ResizeThumbnail);
                    pictureNode1.Content = new ClassListStroce() { StorageFolder = documentFolder, FlagFolde = true, ThumbnailMode = thumbnaild };
                    pictureNode1.IsExpanded = false;

                    pictureNode1.HasUnrealizedChildren = true;

                    sampleTreeView.RootNodes.Add(pictureNode1);
                    FillTreeNode(pictureNode1);
                }
                catch (Exception)
                {

                }
                try
                {


                    StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
                    TreeViewNode pictureNode = new TreeViewNode();
                    var thumbnaild1 = await picturesFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.ResizeThumbnail);
                    pictureNode.Content = new ClassListStroce() { StorageFolder = picturesFolder, FlagFolde = true, ThumbnailMode = thumbnaild1 };
                    pictureNode.IsExpanded = false;

                    pictureNode.HasUnrealizedChildren = true;
                    sampleTreeView.RootNodes.Add(pictureNode);
                    FillTreeNode(pictureNode);
                }
                catch (Exception)
                {

                }
                try
                {


                    // Get Music library.
                    StorageFolder musicFolder = KnownFolders.MusicLibrary;
                    TreeViewNode musicNode = new TreeViewNode();
                    var thumbnaild2 = await musicFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.UseCurrentScale);
                    musicNode.Content = new ClassListStroce() { StorageFolder = musicFolder, FlagFolde = true, ThumbnailMode = thumbnaild2 };
                    musicNode.IsExpanded = false;
                    musicNode.HasUnrealizedChildren = true;
                    sampleTreeView.RootNodes.Add(musicNode);
                    FillTreeNode(musicNode);
                }
                catch (Exception)
                {

                }

                try
                {


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
                            var thumbnaild3 = await ss.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.UseCurrentScale);
                            AssecNode.Content = new ClassListStroce() { StorageFolder = ss, FlagFolde = true, ThumbnailMode = thumbnaild3 };
                            AssecNode.IsExpanded = false;
                            AssecNode.HasUnrealizedChildren = true;
                            sampleTreeView.RootNodes.Add(AssecNode);

                            FillTreeNode(AssecNode);
                        }
                    }
                }
                catch (Exception)
                {

                }
                try
                {
                    var items = Windows.Storage.UserDataPaths.GetDefault();

                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(items.Profile);
                    var d = await storageFolder.GetFoldersAsync();

                    foreach (StorageFolder FlFolder1 in d)
                    {

                        if (FlFolder1.DisplayName == "OneDrive")
                        {


                            TreeViewNode pictureNode1 = new TreeViewNode();
                            var thumbnaild = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.ResizeThumbnail);
                            pictureNode1.Content = new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnaild };
                            pictureNode1.IsExpanded = false;

                            pictureNode1.HasUnrealizedChildren = true;

                            sampleTreeView.RootNodes.Add(pictureNode1);
                            FillTreeNode(pictureNode1);
                        }

                    }


                }
                catch (Exception ex)
                {


                }
                try
                {


                    DriveInfo[] drives = DriveInfo.GetDrives();

                    foreach (DriveInfo drive in drives)
                    {
                        try

                        {


                            if (drive.DriveType == DriveType.Fixed && drive.DriveType != DriveType.Removable)
                            {
                                TreeViewNode FolderNode = new TreeViewNode();
                                StorageFolder storageFolderL = await StorageFolder.GetFolderFromPathAsync(drive.Name);
                                var thumbnaild4 = await storageFolderL.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.UseCurrentScale);
                                FolderNode.Content = new ClassListStroce() { StorageFolder = storageFolderL, FlagFolde = true, ThumbnailMode = thumbnaild4 };
                                FolderNode.IsExpanded = false;
                                FolderNode.HasUnrealizedChildren = true;
                                sampleTreeView.RootNodes.Add(FolderNode);


                            }
                        }
                        catch (Exception)
                        {

                        }

                    }
                }
                catch (Exception)
                {

                }
                try
                {

                    // await new MessageDialog("RemovableDevices").ShowAsync();
                    StorageFolder Folder = KnownFolders.RemovableDevices;
                    IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                    foreach (StorageFolder FlFolder1 in folderList)
                    {

                        TreeViewNode FolderNode = new TreeViewNode();
                        var thumbnaild3 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 24, ThumbnailOptions.UseCurrentScale);
                        FolderNode.Content = new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnaild3 };
                        FolderNode.IsExpanded = false;
                        FolderNode.HasUnrealizedChildren = true;

                        sampleTreeView.RootNodes.Add(FolderNode);
                        FillTreeNode(FolderNode);
                        // Debug.WriteLine("s:" + Folder.Name );

                        //  await new MessageDialog("s:" + Folder.).ShowAsync();
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

      

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }


        int x = -1;

        private async void List1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {try
            {


                var tag = (ListView)sender;
               
                //  FileAndFolderViewer.viewFocusTag = tag.Tag.ToString();
               //fileAndFolderViewer.classListStroceSelect.Clear();
             //   foreach (ClassListStroce d in tag.SelectedItems)
                {
                   // fileAndFolderViewer.classListStroceSelect.Add(d);
                }
               // fileAndFolderViewer.classListStroceSelect = (List<ClassListStroce>)tag.SelectedItems;
                if (fileAndFolderViewer.classListStroceSelect != null)
                {
                    fileAndFolderViewer.classListStroceSelect.Clear();
                }
                else
                {
                    fileAndFolderViewer.classListStroceSelect = new List<ClassListStroce>();
                }
                foreach (var d in tag.SelectedItems)
                {
                    fileAndFolderViewer.classListStroceSelect.Add(d as ClassListStroce);
                }




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

                        //ShareBut.IsEnabled = false;
                    }
                    else
                    {

                        //ShareBut.IsEnabled = true;


                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
        private async void List1_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {

                    ListView listView = (ListView)sender;
               
                    fileAndFolderViewer.Next();
                
              
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
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
            string str = args.SelectedItem as string;
            foreach (var st in fileAndFolderViewer.ListCol)
            {
                if (st.FileAndFildeName == str)
                {
                    if (st.FlagFolde)
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
            if (a != null)
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
        private async void Copi_Click(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }

        public async void Upgreid()
        {

            fileAndFolderViewer.Upgreid();
      

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
                    contentDialogProcecc.Delete(fileAndFolderViewer.classListStroceSelect);
                    var x = await contentDialogProcecc.ShowAsync();

                }
                else
                {
                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFile") };
                    contentDialogProcecc.Delete(fileAndFolderViewer.classListStroceSelect);
                    var x = await contentDialogProcecc.ShowAsync();
                }
                Upgreid();
            }
            catch (Exception ex)
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
                            contentDialogProcecc.CopyFolder((StorageFolder)storageI.ElementAt(0), fileAndFolderViewer.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();


                        }
                    Upgreid();
                     
                    
               

                }
            }
            catch (Exception ex)
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
                if (secondItem != null)
                {



                    ClassListStroce cc = (ClassListStroce)secondItem.Tag;
                    if (cc.FlagFolde)
                    {
                        ContentDialogPropertFolder contentDialogPropertFolder = new ContentDialogPropertFolder();
                        contentDialogPropertFolder.TextProperty(cc);
                        await contentDialogPropertFolder.ShowAsync();

                        // StringBuilder fileProperties = new StringBuilder();
                        // fileProperties.AppendLine(resourceLoader.GetString("NameText") + ": " + cc.StorageFolder.Name);
                        // fileProperties.AppendLine(resourceLoader.GetString("TipText") + ": " + cc.StorageFolder.DisplayType);
                        // fileProperties.AppendLine(resourceLoader.GetString("PathText") + ": " + cc.StorageFolder.Path);
                        // fileProperties.AppendLine(resourceLoader.GetString("CreatText") + ": " + cc.StorageFolder.DateCreated.ToString());
                        // Get file's basic properties.
                        // Windows.Storage.FileProperties.BasicProperties basicProperties =
                        //   await cc.StorageFolder.GetBasicPropertiesAsync();

                        // fileProperties.AppendLine(resourceLoader.GetString("IzmenText") + ": " + basicProperties.DateModified);
                        //  MessageDialog messageDialog = new MessageDialog(fileProperties.ToString(), resourceLoader.GetString("InfoText"));
                        // await messageDialog.ShowAsync();

                    }
                    else
                    {
                        ContentDialogPropertFile contentDialogPropertFile = new ContentDialogPropertFile();
                        contentDialogPropertFile.TextProperty(cc);
                        await contentDialogPropertFile.ShowAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void AppBarButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            fileAndFolderViewer.InitializeDataGridView();
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
                        catch (Exception ex)
                        {
                            //Debug.WriteLine(ex.ToString());
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
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.ToString());
                await messageDialog.ShowAsync();

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
                ListView listView = (ListView)sender;
                //Debug.WriteLine(listView.Tag.ToString());
                //   ClassListStroce cc = FileAndFolderViewer.classListStroceSelect.ElementAt(0);
                var cc = (ClassListStroce)e.Items.ElementAt(0);
                if (cc.FlagFolde != true)
                {

                    StorageFile myFile = cc.storageFile;
                    List<IStorageItem> myStorageItems = new List<IStorageItem>() { myFile };
                    e.Data.SetStorageItems(myStorageItems);
                    e.Data.RequestedOperation = DataPackageOperation.Copy;
                    e.Data.Properties.Description = this.Name;
                    Debug.WriteLine(this.Name);
                }
                else
                {
                    StorageFolder myFile = cc.StorageFolder;
                    List<IStorageItem> myStorageItems = new List<IStorageItem>() { myFile };
                    e.Data.SetStorageItems(myStorageItems);
                    e.Data.RequestedOperation = DataPackageOperation.Copy;
                    e.Data.Properties.Description = this.Name;
                    Debug.WriteLine(this.Name);
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }

        private async void UnorganizedListView_OnDrop(object sender, DragEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                if (e.DataView.Contains(StandardDataFormats.StorageItems) && this.Name != e.Data.Properties.Description)
                {
                    foreach (IStorageItem item in await e.DataView.GetStorageItemsAsync())
                    {
                        if (item.IsOfType(StorageItemTypes.Folder))
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TextCopi") };
                            contentDialogProcecc.CopyFolder(item as StorageFolder, fileAndFolderViewer.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();
                            fileAndFolderViewer.Upgreid();
                            // tabInstance.instanceInteraction.CloneDirectoryAsync((item as StorageFolder).Path, tabInstance.instanceViewModel.Universal.path, (item as StorageFolder).DisplayName);
                        }
                        else
                        {
                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TextCopi") };
                            contentDialogProcecc.CopyFile(item as StorageFile, fileAndFolderViewer.storageFolderFirst);
                            var x = await contentDialogProcecc.ShowAsync();
                            fileAndFolderViewer.Upgreid();
                            // await (item as StorageFile).CopyAsync(await StorageFolder.GetFolderFromPathAsync(tabInstance.instanceViewModel.Universal.path));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }


        private void AppBarButton_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            fileAndFolderViewer.FavoritSpeed();
        }

   
        public void selectAll()
        {
            _DataGrid.SelectAll();

        }

        private static bool IsCtrlKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }
        private async void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {

            if (IsCtrlKeyPressed())
            {
                switch (e.Key)
                {
                    //case VirtualKey.P: DemoMovie.Play(); break;
                    case VirtualKey.A: _DataGrid.SelectAll(); break;
                        //case VirtualKey.S: DemoMovie.Stop(); break;
                }
            }
            else
            {


                if (e.Key == VirtualKey.Back)
                {
                    fileAndFolderViewer.Nazad();
                }
                if (e.Key == VirtualKey.Enter)
                {
                    fileAndFolderViewer.Next();
                }
                if (e.Key == VirtualKey.Delete)
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                    ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFile") };
                    contentDialogProcecc.Delete(fileAndFolderViewer.classListStroceSelect);
                    var x = await contentDialogProcecc.ShowAsync();

                    fileAndFolderViewer.Upgreid();
                }

            }

        }



        private void Split_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (IsCtrlKeyPressed())
            {
                switch (e.Key)
                {
                    //case VirtualKey.P: DemoMovie.Play(); break;
                    case VirtualKey.A: _DataGrid.SelectAll(); break;
                        //case VirtualKey.S: DemoMovie.Stop(); break;
                }
            }
            else
            {

            }
        }



        private async void AppBarButton_Tapped_5(object sender, TappedRoutedEventArgs e)
        {
            test();

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
        private void test()
        {
            var watch = Stopwatch.StartNew();
            var path = @"C:\gameUnity";
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
                    if (((System.IO.FileAttributes)findData.dwFileAttributes & System.IO.FileAttributes.Directory) != System.IO.FileAttributes.Directory)
                    {
                        // do something with it
                        var fn = findData.cFileName;
                        ++count;
                    }
                } while (FindNextFile(hFile, out findData));

                FindClose(hFile);
            }
            Debug.WriteLine("count " + count + ", ellapsed=" + watch.ElapsedMilliseconds);
        }
        private void MenuFlyoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = (MenuFlyoutItem)sender;
            Debug.WriteLine(menuFlyoutItem.Tag.ToString());
            fileAndFolderViewer.Sorting(menuFlyoutItem.Tag.ToString());

        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = (MenuFlyoutItem)sender;

            fileAndFolderViewer.Sorting(menuFlyoutItem.Tag.ToString());
        }
        public void Focs(bool tr)
        {
            if(tr)
            {
            

            }
            else
            {
                _DataGrid.SelectedIndex = -1;
            }
       
            //buttonStyle.TargetType = typeof(Button);
        }
      

    }
}
