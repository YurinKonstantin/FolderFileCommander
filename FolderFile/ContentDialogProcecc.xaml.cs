using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        public async void Delete(List<ClassListStroce> classListStroces)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                if (classListStroces != null)
                {


                    foreach (var s in classListStroces)
                    {
                        if (s.FlagFolde != true)
                        {
                            TextInfo.Text = resourceLoader.GetString("TextCopi") + " " + s.storageFile.Name + "\n" + s.storageFile.Path;
                          
                             DeleteFile(s.storageFile);
                        }
                        else
                        {
                            DeleteFolder(s.StorageFolder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

            this.Hide();
        }
        private async void DeleteFolder(StorageFolder storageFolder)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                TextInfo.Text = resourceLoader.GetString("TileDeleteFolder") + " " + storageFolder.Name + "\n" + storageFolder.Path;
                await storageFolder.DeleteAsync();
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            this.Hide();

        }
        private async void DeleteFile(StorageFile storageFile)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                TextInfo.Text = resourceLoader.GetString("TileDeleteFile") + " " + storageFile.Name + "\n" + storageFile.Path;
                await storageFile.DeleteAsync();
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            this.Hide();

        }
      public async void CopyFolder(StorageFolder begin_dir, StorageFolder end_dir)
        {
            try
            {


                await CopyDir(begin_dir, end_dir);
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            this.Hide();
        }
       public async void CopyFile(StorageFile begin_fil, StorageFolder end_dir)
        {
            try
            {


                await begin_fil.CopyAsync(end_dir, begin_fil.Name, NameCollisionOption.GenerateUniqueName);
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            this.Hide();
        }
        public async void Copy(List<ClassListStroce> classListStroces, StorageFolder enddir)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                if (classListStroces != null)
                {


                    foreach (var s in classListStroces)
                    {
                        if (s.FlagFolde != true)
                        {
                            TextInfo.Text = resourceLoader.GetString("TextCopi") + " " + s.storageFile.Name+"\n"+ s.storageFile.Path;
                            await s.storageFile.CopyAsync(enddir, s.storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        }
                        else
                        {
                            await CopyDir(s.StorageFolder, enddir);
                        }
                    }
                }
             }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
             }

            this.Hide();
        }
       public async Task CopyDir(StorageFolder FromDir, StorageFolder ToDir)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            // Directory.CreateDirectory(ToDir);
            try
            {


                StorageFolder end_dir1 = await ToDir.CreateFolderAsync(FromDir.DisplayName, CreationCollisionOption.GenerateUniqueName);
                TextInfo.Text = resourceLoader.GetString("TextCopi") +" " + end_dir1.Name+"\n"+ end_dir1.Path;
                IReadOnlyList<StorageFile> FileList =
                                  await FromDir.GetFilesAsync();
                foreach (var file in FileList)
                {
                    TextInfo.Text = resourceLoader.GetString("TextCopi")+" " + end_dir1.Name + "\n" + end_dir1.Path;
                    await file.CopyAsync(end_dir1);

                }
                IReadOnlyList<StorageFolder> folderList =
                             await FromDir.GetFoldersAsync();
                foreach (StorageFolder dir in folderList)
                {
                    await CopyDir(dir, end_dir1);
                }
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
 

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            args.Cancel = true;
            this.Hide();
        }
    }
}
