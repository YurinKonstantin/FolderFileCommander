using FolderFile.settingPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class BlankPageSetting : Page
    {
        public BlankPageSetting()
        {
            this.InitializeComponent();
            SettingsPane.SelectedItem = SettingsPane.MenuItems[0];
        }
        private void SettingsPane_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack();
        }
        private void SettingsPane_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                //NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                if(navItemTag== "Appearance")
                {
                    SettingsContentFrame.Navigate(typeof(BlankPageVid));
                }
                if(navItemTag=="About")
                {
                    SettingsContentFrame.Navigate(typeof(BlankPageAbout));
                }
                if (navItemTag == "Files and Folders")
                {
                    SettingsContentFrame.Navigate(typeof(BlankPageFilesFolders));
                }

            }
        }
    }
}
