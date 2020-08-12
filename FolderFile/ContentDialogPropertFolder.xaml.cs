using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FolderFile
{
    public sealed partial class ContentDialogPropertFolder : ContentDialog
    {
        public ContentDialogPropertFolder()
        {
            this.InitializeComponent();
        }
        public async void TextProperty(ClassListStroce cc)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();


           




            // Get file's basic properties.
            Windows.Storage.FileProperties.BasicProperties basicProperties =
                await cc.StorageFolder.GetBasicPropertiesAsync();


            string fileSize = string.Format("{0:n0}", basicProperties.Size);


          //  Windows.Storage.FileAttributes folderAttributes = cc.storageFile.Attributes;

           // if ((folderAttributes & Windows.Storage.FileAttributes.ReadOnly) == Windows.Storage.FileAttributes.ReadOnly)
               // Debug.WriteLine("The item is read-only.");

          //  if ((folderAttributes & Windows.Storage.FileAttributes.Directory) == Windows.Storage.FileAttributes.Directory)
               // Debug.WriteLine("The item is a folder.");

          //  if ((folderAttributes & Windows.Storage.FileAttributes.Archive) == Windows.Storage.FileAttributes.Archive)
              //  Debug.WriteLine("The item is archived.");

          //  if ((folderAttributes & Windows.Storage.FileAttributes.Temporary) == Windows.Storage.FileAttributes.Temporary)
             //   Debug.WriteLine("The item is temporary.");


            textName.Text = resourceLoader.GetString("NameText") + ": " + cc.StorageFolder.Name;
            var thumbnaildstorageFolder = await cc.StorageFolder.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 40, ThumbnailOptions.UseCurrentScale);
            // MessageDialog messageDialog = new MessageDialog(fileProperties.ToString(), resourceLoader.GetString("InfoText"));
            //await messageDialog.ShowAsync();
            this.Title = resourceLoader.GetString("InfoText");
            BitmapImage image = null;
            StorageItemThumbnail thumbnail = (StorageItemThumbnail)thumbnaildstorageFolder;
            image = new BitmapImage();
            image.SetSource(thumbnail);
            img.Source = image;



            textTip.Text = resourceLoader.GetString("TipText") + ": ";
            textTip1.Text = cc.StorageFolder.DisplayType;

            textpath.Text = resourceLoader.GetString("PathText") + ": ";
            textpath1.Text = cc.StorageFolder.Path;

            textcreate.Text = resourceLoader.GetString("CreatText") + ": ";
            textcreate1.Text = cc.StorageFolder.DateCreated.ToString();
            textizm.Text = resourceLoader.GetString("IzmenText") + ": ";
            textizm1.Text = basicProperties.DateModified.ToLocalTime().ToString();

           // textsize.Text = resourceLoader.GetString("SizeText") + ": ";
          //  textsize1.Text = GetFileSize(basicProperties);

        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
