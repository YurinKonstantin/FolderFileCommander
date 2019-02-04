using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ContentDialogProcecc : ContentDialog
    {
        public ContentDialogProcecc()
        {
            this.InitializeComponent();
       

        }
        string dei = String.Empty;
    
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
          //  args.Cancel = true;
            this.Hide();
        }
        public async void DeleteFolder(StorageFolder storageFolder)
        {
            TextInfo.Text = "Удаление " + storageFolder.DisplayName;
            await storageFolder.DeleteAsync();
            this.Hide();

        }
        public async void DeleteFile(StorageFile storageFile)
        {
            TextInfo.Text = "Удаление " + storageFile.DisplayName;
            await storageFile.DeleteAsync();
            this.Hide();

        }
        public async void CopyFolder(StorageFolder begin_dir, StorageFolder end_dir)
        {
           await CopyDir(begin_dir, end_dir);
            this.Hide();
        }
        public async void CopyFile(StorageFile begin_dir, StorageFolder end_dir)
        {
            await begin_dir.CopyAsync(end_dir);
           
            this.Hide();
        }
        async Task CopyDir(StorageFolder FromDir, StorageFolder ToDir)
        {
            // Directory.CreateDirectory(ToDir);
            
            StorageFolder end_dir1 = await ToDir.CreateFolderAsync(FromDir.DisplayName);
            TextInfo.Text = "Копирование " + end_dir1.Path;
            IReadOnlyList<StorageFile> FileList =
                              await FromDir.GetFilesAsync();
            foreach (var file in FileList)
            {
                TextInfo.Text = "Копирование " + file.Path;
                await file.CopyAsync(end_dir1);

            }
            IReadOnlyList<StorageFolder> folderList =
                         await FromDir.GetFoldersAsync();
            foreach (StorageFolder dir in folderList)
            {
               await CopyDir(dir, end_dir1);
            }
        }
        private async void perebor_updates(StorageFolder begin_dir, StorageFolder end_dir)
        {
            //Берём нашу исходную папку
            StorageFolder end_dir1 = await end_dir.CreateFolderAsync(begin_dir.DisplayName);
            StorageFolder end_dir2 = end_dir1;
            IReadOnlyList<StorageFolder> folderList =
                           await begin_dir.GetFoldersAsync();
            //Перебираем все внутренние папки
            foreach (StorageFolder dir in folderList)
            {
                TextInfo.Text = "Копирование " + dir.DisplayName;
             
                //Проверяем - если директории не существует, то создаём;
                if (await end_dir1.TryGetItemAsync(dir.Name) == null)
                {
                    end_dir2 = await end_dir1.CreateFolderAsync(dir.Name);
                    //  Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                }
               
                IReadOnlyList<StorageFolder> folderList1 =
                          await dir.GetFoldersAsync();
             
                //Рекурсия (перебираем вложенные папки и делаем для них то-же самое).
               
                    foreach (var ss in folderList1)
                    {

                        perebor_updates(ss, end_dir2);
                    }
                
            }

            //Перебираем файлики в папке источнике.
            IReadOnlyList<StorageFile> FileList =
                           await begin_dir.GetFilesAsync();
              foreach (var file in FileList)
              {

               await file.CopyAsync(end_dir1);

              }


           
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            args.Cancel = true;
            this.Hide();
        }
    }
}
