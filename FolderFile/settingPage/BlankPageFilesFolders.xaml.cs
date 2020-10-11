using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.AccessCache;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FolderFile.settingPage
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPageFilesFolders : Page
    {
        public BlankPageFilesFolders()
        {
            this.InitializeComponent();
            fileAndFolderViewer = new FileAndFolderViewer();
            fileAndFolderViewer.InitializeDataGridView();
        }
        public FileAndFolderViewer fileAndFolderViewer { get; set; }
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
                //fileAndFolderViewer.ListCol.Clear();


               


             
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            await addLocation();
            fileAndFolderViewer.InitializeDataGridView();
            this.InitializeComponent();
        }
        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        private async void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {

               var d = ((Button)sender).DataContext;
               
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Remove(d.ToString());
                fileAndFolderViewer.InitializeDataGridView();
                this.InitializeComponent();


            }
            catch (Exception ex)
            {

            }

        }
    }
}
