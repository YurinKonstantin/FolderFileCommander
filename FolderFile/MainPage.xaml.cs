using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
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
            InitializeTreeView();
        
        }
        FileAndFolderViewer fileAndFolderViewer { get; set; }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
           
        }

        private async void List1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
           
            ListView listView = (ListView)sender;
            if (listView != null)
            {


                ClassListStroce classListStroce = (ClassListStroce)listView.SelectedItem;

                // StorageFolderQueryResult queryResult =
                //  classListStroce.storageFolder.CreateFolderQuery(CommonFolderQuery.GroupByMonth);
                if (classListStroce != null)
                { 
                    if (classListStroce.FlagFolde)
                    {
                        


                        IReadOnlyList<StorageFolder> folderList =
                            await classListStroce.StorageFolder.GetFoldersAsync();
                        IReadOnlyList<StorageFile> fileList =
                          await classListStroce.StorageFolder.GetFilesAsync();

                        fileAndFolderViewer.storageFolderFirst = classListStroce.StorageFolder;
                        fileAndFolderViewer.ListCol.Clear();
                        foreach (StorageFolder FlFolder1 in folderList)
                        {
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true });
                        }
                        foreach (StorageFile FlFolder1 in fileList)
                        {
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false });
                        }

                    }
                    else
                    {

                    }
                }
               
            }

        }

        private void InitializeTreeView()
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
            var node = args.InvokedItem as TreeViewNode;
            if (node.Content is IStorageItem item)
            {
                
               // FileNameTextBlock.Text = item.Name;
                FilePathTextBlock.Text = item.Path;
               // TreeDepthTextBlock.Text = node.Depth.ToString();

                if (node.Content is StorageFolder folder)
                {
                    list1.SelectedItem = null;
                    
                    fileAndFolderViewer.ListCol.Clear();
                    fileAndFolderViewer.storageFolderFirst = folder;
                    IReadOnlyList<StorageFolder> folderList =
                    await folder.GetFoldersAsync();
                    IReadOnlyList<StorageFile> fileList =
                           await folder.GetFilesAsync();
                    if (fileList.Count != 0 || folderList.Count != 0)
                    {
                        

                        foreach (StorageFolder d in folderList)
                        {
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = d, FlagFolde = true });
                        }

                        foreach (StorageFile file in fileList)
                        {
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = file, FlagFolde = false });
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
            ListView listView = list1;
            if (listView != null)
            {

                if (fileAndFolderViewer.ListCol.Count != 0)
                {

                    // StorageFolderQueryResult queryResult =
                    //  classListStroce.storageFolder.CreateFolderQuery(CommonFolderQuery.GroupByMonth);
                    
                        StorageFolder storageFolder = await fileAndFolderViewer.storageFolderFirst.GetParentAsync();
                     
                        IReadOnlyList<StorageFolder> folderList =
                           await storageFolder.GetFoldersAsync();
                        IReadOnlyList<StorageFile> fileList =
                          await storageFolder.GetFilesAsync();

                        fileAndFolderViewer.storageFolderFirst = storageFolder;
                        fileAndFolderViewer.ListCol.Clear();
                        foreach (StorageFolder FlFolder1 in folderList)
                        {
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true });
                        }
                        foreach (StorageFile FlFolder1 in fileList)
                        {
                            fileAndFolderViewer.ListCol.Add(new ClassListStroce() { storageFile = FlFolder1, FlagFolde = false });
                        }

                }

            }
        }
    }
}
