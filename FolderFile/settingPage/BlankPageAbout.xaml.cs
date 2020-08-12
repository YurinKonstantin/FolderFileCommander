using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
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
    public sealed partial class BlankPageAbout : Page
    {
        public BlankPageAbout()
        {
            this.InitializeComponent();
            var version = Package.Current.Id.Version;
            VersionNumber.Text = string.Format($"Version: {version.Major}.{version.Minor}.{version.Build}.{version.Revision}");
        }
        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(@"https://github.com/YurinKonstantin/FolderFileCommander/issues/new/choose"));
        }

        private async void OpenLogLocationButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?ProductId=9PHH2VDQWBG7"));
        }

        private async void ListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?ProductId=9PHH2VDQWBG7"));
        }
    }
}
