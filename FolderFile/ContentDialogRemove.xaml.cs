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
    public sealed partial class ContentDialogRemove : ContentDialog
    {
        public ContentDialogRemove()
        {
            this.InitializeComponent();
        }
        public StorageFolder storageFolder;
        public StorageFile storageFile;
       public bool folder = true;
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
           if(folder)
            {
                remofFolder();
            }
           else
            {
                remofFile();
            }
            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
       public async void remofFile()
        {
            try
            {
                string tip = String.Empty;
                if (newName.Text.Contains('.'))
                {
                    await storageFile.RenameAsync(newName.Text, NameCollisionOption.GenerateUniqueName);
                }
                else
                {
                    MessageDialog b = new MessageDialog("неверное имя файла!", "Ошибка");
                    await b.ShowAsync();
                }
             
            }
            catch (Exception ex)
            {
                MessageDialog b = new MessageDialog("Произошла ошибка при создание файла. Файл не создан!", "Ошибка");
                await b.ShowAsync();
            }
        }
        public async void remofFolder()
        {
            try
            {
                
                   
               await storageFolder.RenameAsync(newName.Text, NameCollisionOption.GenerateUniqueName);
               
               
                

            }
            catch (Exception ex)
            {
                MessageDialog b = new MessageDialog("Произошла ошибка при создание файла. Файл не создан!", "Ошибка");
                await b.ShowAsync();
            }
        }
    }
}
