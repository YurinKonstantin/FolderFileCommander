using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FolderFile
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage1EditFileFolder : Page
    {
        public BlankPage1EditFileFolder()
        {
            this.InitializeComponent();
        }
        CancellationTokenSource tokenSource = new CancellationTokenSource();
       
        public async void Del(List<ClassListStroce> classListStroces)
        {
            var token = tokenSource.Token;
            // await Delete(classListStroces);
            await Task.Run(() => Delete(classListStroces, token), token);
        }
       
        async Task Delete(List<ClassListStroce> classListStroces, CancellationToken ct)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                if (classListStroces != null)
                {


                    foreach (var s in classListStroces)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            
                            ct.ThrowIfCancellationRequested();
                        }
                        if (s.FlagFolde != true)
                        {
                            textOperacia.Text = resourceLoader.GetString("TextCopi") + " " + (await StorageFile.GetFileFromPathAsync(s.Path)).Name + "\n" + (await StorageFile.GetFileFromPathAsync(s.Path)).Path;

                            DeleteFile((await StorageFile.GetFileFromPathAsync(s.Path)));
                        }
                        else
                        {
                            DeleteFolder((await StorageFolder.GetFolderFromPathAsync(s.Path)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            Window.Current.Close();
          //  this.Hide();
        }
        private async void DeleteFolder(StorageFolder storageFolder)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                textpath.Text = resourceLoader.GetString("TileDeleteFolder") + " " + storageFolder.Name + "\n" + storageFolder.Path;


                await storageFolder.DeleteAsync();
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
          //  this.Hide();

        }
        private async void DeleteFile(StorageFile storageFile)
        {
            try
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                textpath.Text = resourceLoader.GetString("TileDeleteFile") + " " + storageFile.Name + "\n" + storageFile.Path;
                await storageFile.DeleteAsync();
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
          //  this.Hide();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
