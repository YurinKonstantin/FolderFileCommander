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

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace FolderFile
{
    public sealed partial class ExceptionDialog : ContentDialog
    {
       public string message;
        string stackTrace;
        string offendingMethod;
        public ExceptionDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
          
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }

        private void ExpandMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            // If technical info is collapsed
            if (CollapseIcon.Visibility == Visibility.Collapsed)
            {
                ExpandIcon.Visibility = Visibility.Collapsed;
                CollapseIcon.Visibility = Visibility.Visible;
                TechnicalInformation.Visibility = Visibility.Visible;

            }
            else // if technical info is expanded
            {
                ExpandIcon.Visibility = Visibility.Visible;
                CollapseIcon.Visibility = Visibility.Collapsed;
                TechnicalInformation.Visibility = Visibility.Collapsed;
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
