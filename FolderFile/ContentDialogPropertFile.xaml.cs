using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ContentDialogPropertFile : ContentDialog
    {
        public  ContentDialogPropertFile()
        {
            this.InitializeComponent();
            
        }

        public async void TextProperty(ClassListStroce cc)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            
            
            
       

        
   
            // Get file's basic properties.
            Windows.Storage.FileProperties.BasicProperties basicProperties =
                await (await StorageFile.GetFileFromPathAsync(cc.Path)).GetBasicPropertiesAsync();
         
            
            string fileSize = string.Format("{0:n0}", basicProperties.Size);
         
       
        Windows.Storage.FileAttributes folderAttributes = (await StorageFile.GetFileFromPathAsync(cc.Path)).Attributes;
           
            if ((folderAttributes & Windows.Storage.FileAttributes.ReadOnly) == Windows.Storage.FileAttributes.ReadOnly)
                Debug.WriteLine("The item is read-only.");

            if ((folderAttributes & Windows.Storage.FileAttributes.Directory) == Windows.Storage.FileAttributes.Directory)
                Debug.WriteLine("The item is a folder.");

            if ((folderAttributes & Windows.Storage.FileAttributes.Archive) == Windows.Storage.FileAttributes.Archive)
                Debug.WriteLine("The item is archived.");

            if ((folderAttributes & Windows.Storage.FileAttributes.Temporary) == Windows.Storage.FileAttributes.Temporary)
                Debug.WriteLine("The item is temporary.");


            textName.Text = resourceLoader.GetString("NameText") + ": " + (await StorageFile.GetFileFromPathAsync(cc.Path)).Name;
            var thumbnaildstorageFolder = await (await StorageFile.GetFileFromPathAsync(cc.Path)).GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 40, ThumbnailOptions.UseCurrentScale);
            // MessageDialog messageDialog = new MessageDialog(fileProperties.ToString(), resourceLoader.GetString("InfoText"));
            //await messageDialog.ShowAsync();
            this.Title = resourceLoader.GetString("InfoText");
            BitmapImage image = null;
                StorageItemThumbnail thumbnail = (StorageItemThumbnail)thumbnaildstorageFolder;
                image = new BitmapImage();
                image.SetSource(thumbnail);
            img.Source = image;
       


            textTip.Text = resourceLoader.GetString("TipText") + ": ";
            textTip1.Text = (await StorageFile.GetFileFromPathAsync(cc.Path)).DisplayType;

            textpath.Text = resourceLoader.GetString("PathText") + ": ";
            textpath1.Text = (await StorageFile.GetFileFromPathAsync(cc.Path)).Path;

            textcreate.Text = resourceLoader.GetString("CreatText") + ": ";
            textcreate1.Text = (await StorageFile.GetFileFromPathAsync(cc.Path)).DateCreated.ToString();
            textizm.Text = resourceLoader.GetString("IzmenText") + ": ";
            textizm1.Text = basicProperties.DateModified.ToLocalTime().ToString();

            textsize.Text = resourceLoader.GetString("SizeText") + ": ";
            textsize1.Text = GetFileSize(basicProperties);

        }
        public string GetFileSize(BasicProperties file)
        {
            try
            {
                double sizeinbytes = file.Size;
                double sizeinkbytes = Math.Round((sizeinbytes / 1024));
                double sizeinmbytes = Math.Round((sizeinkbytes / 1024));
                double sizeingbytes = Math.Round((sizeinmbytes / 1024));
                if (sizeingbytes > 1)
                    return string.Format("{0:n0} GB", sizeingbytes)+" ("+ string.Format("{0:n0} B", sizeinbytes)+")"; //размер в гигабайтах
                else if (sizeinmbytes > 1)
                    return string.Format("{0:n0} MB", sizeinmbytes) + " (" + string.Format("{0:n0} B", sizeinbytes) + ")"; //возвращает размер в мегабайтах, если размер файла менее одного гигабайта
                else if (sizeinkbytes > 1)
                    return string.Format("{0:n0} KB", sizeinkbytes) + " (" + string.Format("{0:n0} B", sizeinbytes) + ")"; //возвращает размер в килобайтах, если размер файла менее одного мегабайта
                else
                    return string.Format("{0:n0} B", sizeinbytes); //возвращает размер в байтах, если размер файла менее одного килобайта
            }
            catch { return "Ошибка получения размера файла"; } //перехват ошибок и возврат сообщения об ошибке
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
