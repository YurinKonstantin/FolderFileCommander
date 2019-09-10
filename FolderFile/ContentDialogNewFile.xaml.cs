using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FolderFile
{
    public sealed partial class ContentDialogNewFile : ContentDialog
    {
        public ContentDialogNewFile()
        {
            this.InitializeComponent();
        }
        public StorageFolder storageFolder;
        public StorageFile storageFile;
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {


                string tip = String.Empty;
                if (tipF.Text.Contains('.'))
                {
                    tip = tipF.Text;
                }
                else
                {
                    tip = "." + tipF.Text;
                }
                await storageFolder.CreateFileAsync(nameF.Text + tip, CreationCollisionOption.GenerateUniqueName);
            }
            catch(Exception ex)
            {
                MessageDialog b = new MessageDialog("Произошла ошибка при создание файла. Файл не создан!", "Ошибка");
               await b.ShowAsync();
            }
            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            this.Hide();
        }
      
       
    }
}
