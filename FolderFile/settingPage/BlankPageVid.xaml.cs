using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FolderFile.settingPage
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPageVid : Page
    {
        public BlankPageVid()
        {
            this.InitializeComponent(); List<string> _themeval = new List<string>();
            _themeval.Add("Maximum");
            _themeval.Add("Minimum");

            DateFormatChooser.ItemsSource = _themeval;
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            String localValue = localSettings.Values["ViewSet"] as string;
            if (localValue == "Max")
            {
                DateFormatChooser.SelectedIndex = 0;

            }
            if (localValue == "Min")
            {
                DateFormatChooser.SelectedIndex = 1;
            }

            DateFormatChooser.Loaded += (s, e) =>
            {
                DateFormatChooser.SelectionChanged += (s1, e1) =>
                {
                    var themeComboBox = s1 as ComboBox;
                    
                    switch (DateFormatChooser.SelectedIndex)
                    {
                        case 0:
                            FileAndFolderViewer.Type = "Maximum";
                            localSettings.Values["ViewSet"] = "Max";
                            Debug.WriteLine("Max");
                            break;

                        case 1:
                            FileAndFolderViewer.Type = "Middle";
                            Debug.WriteLine("Min");
                            localSettings.Values["ViewSet"] = "Min";
                            break;

                        
                    }
                   
                   
                };
            };
        }
   
               




         





            
    }
}
