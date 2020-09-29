using FolderFile.controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Grid;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Services.Store;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
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
            //Windows.ApplicationModel.Resources.Core.ResourceContext.SetGlobalQualifierValue("Language", "it");
            
            this.InitializeComponent();
          
            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = AppSettings.Theme;
            
           

            dataTransferManager = DataTransferManager.GetForCurrentView();
            

             dataTransferManager.DataRequested += DataTransferManager_DataRequested;



        }
        public FileFolderView viewFocusTag { get; set; }
        public FileAndFolderViewer fileAndFolderViewer { get; set; }
        
        
        class AppSettings
        {
            public const ElementTheme DEFAULTTHEME = ElementTheme.Light;
            public const ElementTheme NONDEFLTHEME = ElementTheme.Dark;

            const string KEY_THEME = "appColourMode";
            static ApplicationDataContainer LOCALSETTINGS = ApplicationData.Current.LocalSettings;

            /// <summary>
            /// Gets or sets the current app colour setting from memory (light or dark mode).
            /// </summary>
            public static ElementTheme Theme
            {
                get
                {
                    // Never set: default theme
                    if (LOCALSETTINGS.Values[KEY_THEME] == null)
                    {
                        LOCALSETTINGS.Values[KEY_THEME] = (int)DEFAULTTHEME;
                        return DEFAULTTHEME;
                    }
                    // Previously set to default theme
                    else if ((int)LOCALSETTINGS.Values[KEY_THEME] == (int)DEFAULTTHEME)
                        return DEFAULTTHEME;
                    // Previously set to non-default theme
                    else
                        return NONDEFLTHEME;
                }
                set
                {
                    // Error check
                    if (value == ElementTheme.Default)
                        throw new System.Exception("Only set the theme to light or dark mode!");
                    // Never set
                    else if (LOCALSETTINGS.Values[KEY_THEME] == null)
                        LOCALSETTINGS.Values[KEY_THEME] = (int)value;
                    // No change
                    else if ((int)value == (int)LOCALSETTINGS.Values[KEY_THEME])
                        return;
                    // Change
                    else
                        LOCALSETTINGS.Values[KEY_THEME] = (int)value;
                }
            }
        }
        private DataTransferManager dataTransferManager;
      

   
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            Debug.WriteLine("1");
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            bool tryOk = false;
            try
            {
                StorageFolder documentFolder = KnownFolders.DocumentsLibrary;
                tryOk = true;
               // ViewRight.iniz("R");
            }
            catch (UnauthorizedAccessException ex)
            {
                //resourceLoader.GetString("MessageTiletDostup");
               // MessageDialog messageDialog = new MessageDialog(resourceLoader.GetString("MessageContentDostup"), resourceLoader.GetString("MessageTiletDostup"));
               // await messageDialog.ShowAsync();
                ContentDialogDostup contentDialogDostup = new ContentDialogDostup() { Title = resourceLoader.GetString("MessageTiletDostup") };
                contentDialogDostup.TextD(resourceLoader.GetString("MessageContentDostup"));
               await contentDialogDostup.ShowAsync();
                App.Current.Exit();
            }
            
            
           
            if (tryOk)
            {
                 ViewRight.iniz("R");
               

              
                InitializeDataGridView();
                


            }
            try
            {


                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                String localValue = localSettings.Values["ViewSet"] as string;
                Debug.WriteLine("1"+localValue);
                if(localValue== "Max")
                {
                    FileAndFolderViewer.Type = "Maximum";
                   
                }
                if (localValue == "Min")
                {
                    FileAndFolderViewer.Type = "Middle";
                }


                
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            try
            {
                InitializeLicense();
            }
            catch(Exception ex)
            {

            }
           

        }
        private StoreContext context = null;
        private StoreAppLicense appLicense = null;

        private async void InitializeLicense()
        {
            try
            {


                if (context == null)
                {
                    context = StoreContext.GetDefault();
                    // If your app is a desktop app that uses the Desktop Bridge, you
                    // may need additional code to configure the StoreContext object.
                    // For more info, see https://aka.ms/storecontext-for-desktop.
                }

                appLicense = await context.GetAppLicenseAsync();

                if (appLicense.IsActive)
                {
                    if (appLicense.IsTrial)
                    {
                        int remainingTrialTime = (appLicense.ExpirationDate - DateTime.Now).Days;
                        trial.Visibility = Visibility.Visible;
                        
                        StoreProductResult result = await context.GetStoreProductForCurrentAppAsync();
                        if (result.ExtendedError == null)
                        {
                            if(result.Product.Price.IsOnSale)
                            {
                                PurchasePrice.Text = "Sale " + result.Product.Price.FormattedPrice;
                            }
                            else
                            {
                                PurchasePrice.Text = "Price " + result.Product.Price.FormattedBasePrice;
                            }
                            

                           
                        }
                        //Debug.WriteLine($"This is the trial version. Expiration date: {appLicense.ExpirationDate}");

                        textlic.Text = $"You can use this app for {remainingTrialTime} more days before the trial period ends.";
                        // StoreServicesCustomEventLogger logger = StoreServicesCustomEventLogger.GetDefault();
                        // logger.Log("isTrialEvent");
                        //  textBlock.Text = $"This is the trial version. Expiration date: {appLicense.ExpirationDate}";
                        
                        // Show the features that are available during trial only.
                    }
                    else
                    {
                        textlic.Text = "You have a full license.";
                        appLicense = await context.GetAppLicenseAsync();
                        StoreProductResult result = await context.GetStoreProductForCurrentAppAsync();
                        if (result.ExtendedError == null)
                        {
                           
                                if (result.Product.Price.IsOnSale)
                                {
                                    PurchasePrice.Text = "Sale " + result.Product.Price.FormattedPrice;
                                }
                                else
                                {
                                    PurchasePrice.Text = "Price " + result.Product.Price.FormattedBasePrice;
                                }



                            
                            //PurchasePrice.Text = "Price " + result.Product.Price.FormattedPrice;


                        }
                        trial.Visibility = Visibility.Collapsed;
                        // StoreServicesCustomEventLogger logger = StoreServicesCustomEventLogger.GetDefault();
                        //  logger.Log("NotTrialEvent");

                        // Show the features that are available only with a full license.
                    }
                }
                else
                {
                    trial.Visibility = Visibility.Collapsed;
                    //textlic.Text = "You don't have a license. The trial time can't be determined.";
                    //StoreServicesCustomEventLogger logger = StoreServicesCustomEventLogger.GetDefault();
                    //  logger.Log("NotActiveEvent");
                }

                // Register for the licenced changed event.
                context.OfflineLicensesChanged += context_OfflineLicensesChanged;
            }
            catch (Exception ex)
            {
                trial.Visibility = Visibility.Collapsed;
            }
        }

        private async void context_OfflineLicensesChanged(StoreContext sender, object args)
        {
            // Reload the license.
            try
            {


                appLicense = await context.GetAppLicenseAsync();


                if (appLicense.IsActive)
                {
                    if (appLicense.IsTrial)
                    {
                        Debug.WriteLine($"This is the trial version. Expiration date: {appLicense.ExpirationDate}");
                        //  textBlock.Text = $"This is the trial version. Expiration date: {appLicense.ExpirationDate}";
                        trial.Visibility = Visibility.Visible;
                        // Show the features that are available during trial only.
                    }
                    else
                    {
                        Debug.WriteLine("rff");
                        trial.Visibility = Visibility.Collapsed;
                        // Show the features that are available only with a full license.
                    }
                }
            }
            catch (Exception ex)
            {
                trial.Visibility = Visibility.Collapsed;
            }
        }
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
                fileAndFolderViewer.ListCol.Clear();
                
               
                ViewLeft.InitializeTreeView();
                ViewRight.InitializeTreeView();
               
                
                InitializeDataGridView();
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
     
       
        List<String> listDostyp = new List<string>();
     
       
        private async void InitializeDataGridView()
        {
            try
            {

                ViewLeft.fileAndFolderViewer.InitializeDataGridView();
               
              ViewRight.fileAndFolderViewer.InitializeDataGridView();
              
                TimeSpan period = TimeSpan.FromSeconds(2);

                ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
                {

                    newUsB();

                }, period);

            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }

        }
        

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            await addLocation();
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {


                //Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Remove(listDop.SelectedItem.ToString());
                Upgreid();
                ViewLeft.InitializeTreeView();
                ViewRight.InitializeTreeView();

            }
            catch(Exception ex)
            {

            }
         
        }

        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }


        int x = -1;

     
        public static bool IsAdmin()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(
                WindowsIdentity.GetCurrent());

            return pricipal.IsInRole(
                WindowsBuiltInRole.Administrator);
        }
        public async Task ActivateFile(StorageFile storageFile)
        {
            try
            {
                Debug.WriteLine(storageFile.Path+"\t"+ storageFile.DisplayType);
                if (storageFile.Path.Split('.')[1] == "exe")
                {

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = storageFile.Path;
                    bool admin = IsAdmin();

                    if (!admin)
                    {
                        psi.Verb = "runas";
                    }


                    try
                    {
                        Process.Start(psi);
                    }
                    catch (Exception ex)
                    {

                        if (admin) //админские права уже были, что-то испортилось
                        {
                            
                            //return false;
                        }
                        MessageDialog messageDialog = new MessageDialog(ex.Message);
                        await messageDialog.ShowAsync();
                        //иначе может быть просто нажали отмену в UAC
                    }
                }
                else
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
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }
     


        private async void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {

            try
            {
                

                    foreach (var vs in viewFocusTag.fileAndFolderViewer.classListStroceSelect)
                    {

                        //var vs = FileAndFolderViewer.classListStroceSelect;
                        if (vs.FlagFolde != true)
                        {
                            await ActivateFile(vs.storageFile);
                        }
                    }
                
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void AppBarButton_Click_5(object sender, RoutedEventArgs e)
        {
            viewFocusTag.selectAll();

        }

        DataPackage dataPackage = new DataPackage();
     
        public async void Upgreid( )
        {

            ViewLeft.fileAndFolderViewer.Upgreid();
           ViewRight.fileAndFolderViewer.Upgreid();

        }
        /// <summary>
        /// Копирование файлов и папок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Click_6(object sender, RoutedEventArgs e)
        {
            if (viewFocusTag.Name == ViewFocusName.ViewLeft)
            {
              
                        ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() {  };
                        contentDialogProcecc.Copy(viewFocusTag.fileAndFolderViewer.classListStroceSelect, ViewRight.fileAndFolderViewer.storageFolderFirst);
                        var x = await contentDialogProcecc.ShowAsync();
                ViewRight.fileAndFolderViewer.Upgreid();

            }
            else
            {
             
                      
                            

                        ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { };
                        contentDialogProcecc.Copy(viewFocusTag.fileAndFolderViewer.classListStroceSelect, ViewLeft.fileAndFolderViewer.storageFolderFirst);
                        var x = await contentDialogProcecc.ShowAsync();
                ViewLeft.fileAndFolderViewer.Upgreid();





            }
           // Upgreid();

        }

        /// <summary>
        /// Удаление файлов и папок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {


                             var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                            ContentDialogProcecc contentDialogProcecc = new ContentDialogProcecc() { Title = resourceLoader.GetString("TileDeleteFile") };
                            contentDialogProcecc.Delete(viewFocusTag.fileAndFolderViewer.classListStroceSelect);
                            var x = await contentDialogProcecc.ShowAsync();
                viewFocusTag.fileAndFolderViewer.Upgreid();
                           // Upgreid();

            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }


        }



    

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
               

                    await viewFocusTag.fileAndFolderViewer.storageFolderFirst.CreateFolderAsync(resourceLoader.GetString("TextNewFolder"), CreationCollisionOption.GenerateUniqueName);
                    viewFocusTag.fileAndFolderViewer.Upgreid();
                
              
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
               
        }

        private async void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                
                    ContentDialogNewFile contentDialogNewFile = new ContentDialogNewFile() { };
                    contentDialogNewFile.storageFolder = viewFocusTag.fileAndFolderViewer.storageFolderFirst;
                    await contentDialogNewFile.ShowAsync();
                    viewFocusTag.fileAndFolderViewer.Upgreid();
                
              
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void AppBarButton_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {


                
                    foreach (var vs in viewFocusTag.fileAndFolderViewer.classListStroceSelect)
                    {
                        if (vs != null)
                        {

                            ContentDialogRemove contentDialogNewFile = new ContentDialogRemove() { };
                            contentDialogNewFile.storageFolder = vs.StorageFolder;
                            if (vs.FlagFolde)
                            {
                                contentDialogNewFile.folder = vs.FlagFolde;
                                contentDialogNewFile.storageFolder = vs.StorageFolder;
                                await contentDialogNewFile.ShowAsync();
                            }
                            else
                            {
                                contentDialogNewFile.folder = vs.FlagFolde;
                                contentDialogNewFile.storageFile = vs.storageFile;
                                await contentDialogNewFile.ShowAsync();
                            }
                            Upgreid();
                        }
                    }
                
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
          
        }


        

        private async void AppBarButton_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {


            
        
       
            }
            catch(Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }



        public async void newUsB()
        {
            try
            {
               

                StorageFolder Folder = KnownFolders.RemovableDevices;
                IReadOnlyList<StorageFolder> folderList = await Folder.GetFoldersAsync();
                foreach (StorageFolder FlFolder1 in folderList)
                {
                    var f = (from d in ViewLeft.fileAndFolderViewer.ListUSB where d.DisplayName == FlFolder1.DisplayName select d).Count();
                    if (f == 0)
                    {

                        await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                  async () =>
                  {
                      if (ViewLeft.fileAndFolderViewer.Path == String.Empty)
                      {
                          var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                          ViewLeft.fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                      }
                      if (ViewRight.fileAndFolderViewer.Path == String.Empty)
                      {
                          var thumbnail1 = await FlFolder1.GetThumbnailAsync(ThumbnailMode.SingleItem, 100);
                          ViewRight.fileAndFolderViewer.ListCol.Add(new ClassListStroce() { StorageFolder = FlFolder1, FlagFolde = true, ThumbnailMode = thumbnail1, Type = FileAndFolderViewer.Type });
                      }
                      ViewLeft.fileAndFolderViewer.ListUSB.Add(FlFolder1);
                      ViewRight.InitializeTreeView();
                      ViewLeft.InitializeTreeView();


                  });
                        
                    }

                }
                if (ViewLeft.fileAndFolderViewer.ListUSB.Count > 0)
                {

                    try
                    {


                        foreach (var FlFolder1 in ViewLeft.fileAndFolderViewer.ListUSB)
                        {
                            var f = (from d in folderList where d.DisplayName == FlFolder1.DisplayName select d).Count();
                            if (f == 0)
                            {
                                ViewLeft.fileAndFolderViewer.ListUSB.Remove(FlFolder1);
                                await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                          async () =>
                          {
                              if (ViewLeft.fileAndFolderViewer.Path == String.Empty)
                              {
                                  //InitializeDataGridView1();
                                  ViewLeft.fileAndFolderViewer.InitializeDataGridView();
                              }
                              if (ViewRight.fileAndFolderViewer.Path == String.Empty)
                              {
                                  ViewRight.fileAndFolderViewer.InitializeDataGridView();
                              }


                              ViewRight.InitializeTreeView();
                              ViewLeft.InitializeTreeView();



                          });
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        await new MessageDialog(ex.Message).ShowAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                //await new MessageDialog(ex.Message).ShowAsync();
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
                            catch(Exception ex)
                            {
                                //Debug.WriteLine(ex.ToString());
                                request.FailWithDisplayText(ex.ToString());
                              
                                
                              
                            }

                        }
                       
                    }
                
            
               
            });

        }
        ClassListStroce shareList;
    
        private async void BGRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //RadioButton rb = sender as RadioButton;

           // if (rb != null  )
            {
              //  if (rb.Tag.ToString() == "Max")
                {
                    //FileAndFolderViewer.Type = "Maximum";
                }
               // if (rb.Tag.ToString() == "Min")
                {
                   // FileAndFolderViewer.Type = "Middle";
                }
            
                
             //   ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
               // localSettings.Values["ViewSet"] = rb.Tag.ToString();
              if(ViewLeft !=null && ViewLeft.fileAndFolderViewer !=null)
                {
                 //   ViewLeft.fileAndFolderViewer.Upgreid();
                }
                
                if (ViewRight != null && ViewRight.fileAndFolderViewer != null)
                {
                   // ViewRight.fileAndFolderViewer.Upgreid();
                }
         





            }
           
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabView tabView = (TabView)sender;
            if(tabView.SelectedIndex==1)
            {
                //ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

              //  String localValue = localSettings.Values["ViewSet"] as string;
              //  Debug.WriteLine(localValue);
             //   if(localValue=="Max")
                {
                    //chec1.IsChecked = true;
                }
               // else
                {
                    //chec2.IsChecked = true;
                }
            }
        }

        


        private void AppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.RequestedTheme = ApplicationTheme.Dark;
        }

        private void AppBarToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Application.Current.RequestedTheme = ApplicationTheme.Light;
        }
        private static bool IsCtrlKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }
    

   

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                // StoreServicesCustomEventLogger logger = StoreServicesCustomEventLogger.GetDefault();
                // logger.Log("butByaEvent");
                StoreProductResult productResult = await context.GetStoreProductForCurrentAppAsync();
                if (productResult.ExtendedError != null)
                {
                    // The user may be offline or there might be some other server failure
                    
                  
                    return;
                }
                StorePurchaseResult result = await productResult.Product.RequestPurchaseAsync();
                if (result.ExtendedError != null)
                {
                    MessageDialog messageDialog = new MessageDialog($"Purchase failed: ExtendedError: {result.ExtendedError.Message}");
                    await messageDialog.ShowAsync();

                    return;
                }

                switch (result.Status)
                {
                    case StorePurchaseStatus.AlreadyPurchased:
                        MessageDialog messageDialog = new MessageDialog($"You already bought this app and have a fully-licensed version.");
                        await messageDialog.ShowAsync();
                      
                        break;

                    case StorePurchaseStatus.Succeeded:
                        InitializeLicense();
                        break;

                    case StorePurchaseStatus.NotPurchased:
                     messageDialog = new MessageDialog("Product was not purchased, it may have been canceled.");
                        await messageDialog.ShowAsync();
                  
                        break;

                    case StorePurchaseStatus.NetworkError:
                        messageDialog = new MessageDialog("Product was not purchased due to a Network Error.");
                        await messageDialog.ShowAsync();
                    
                        break;

                    case StorePurchaseStatus.ServerError:
                        messageDialog = new MessageDialog("Product was not purchased due to a Server Error.");
                        await messageDialog.ShowAsync();

                        break;

                    default:
                        messageDialog = new MessageDialog("Product was not purchased due to an Unknown Error.");
                        await messageDialog.ShowAsync();
                      
                        break;
                }
                // await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?ProductId=9PHH2VDQWBG7"));
            }
            catch (Exception ex)
            {

            }
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
            var path = @"C:\";
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
                    if (((System.IO.FileAttributes)findData.dwFileAttributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.System)
                    {
                        // do something with it
                        var fn = findData.cFileName;
                        Debug.WriteLine(fn);
                        ++count;
                    }
                } while (FindNextFile(hFile, out findData));

                FindClose(hFile);
            }
            Debug.WriteLine("count " + count + ", ellapsed=" + watch.ElapsedMilliseconds);
        }

        private void ViewLeft_GotFocus(object sender, RoutedEventArgs e)
        {
            FileFolderView fileFolderView = (FileFolderView)sender;
            viewFocusTag = fileFolderView;
            viewFocusTag.Focs(true);
           if( viewFocusTag.Name == ViewFocusName.ViewLeft)
            {
                ViewRight.Focs(false);
            }
           else
            {
                ViewLeft.Focs(false);
            }


        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPageSetting));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test();
        }
    }
  
}
