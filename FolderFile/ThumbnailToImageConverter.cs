using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace FolderFile
{
   public class ThumbnailToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage image=new BitmapImage();
            image.UriSource = new Uri("ms-appx:///Assets/FolderIcon.png", UriKind.RelativeOrAbsolute);
            Convedrt convedrt = value as Convedrt;
           
            if (convedrt != null && !convedrt.isciz)
            {
                
                if (convedrt.fold)
                {
                    image.UriSource = new Uri("ms-appx:///Assets/FolderIcon.png", UriKind.RelativeOrAbsolute);
                }
                else
                {
                    image.UriSource = new Uri("ms-appx:///Assets/FolderIcon.png", UriKind.RelativeOrAbsolute);
                }
            }
            else
            {

                if (convedrt != null && !convedrt.fold)
                {
                    
                    var storageFile = (StorageFile.GetFileFromPathAsync(convedrt.path)).AsTask();
                    Task.WhenAll(storageFile);
                    StorageFile d = storageFile.Result;
                    
                    var thumbnail = d.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale).AsTask().Result;
                    image = new BitmapImage();
                    image.SetSource(thumbnail);
                }
                if (convedrt != null && convedrt.fold)
                {
                   
                    var storageFile = (StorageFolder.GetFolderFromPathAsync(convedrt.path)).AsTask();
                    Task.WhenAll(storageFile);
                    StorageFolder d = storageFile.Result;
                    
                    var thumbnail = d.GetScaledImageAsThumbnailAsync(ThumbnailMode.SingleItem, 60, ThumbnailOptions.UseCurrentScale).AsTask().Result;
                    image = new BitmapImage();
                    image.SetSource(thumbnail);
                }
            }
          //  if (convedrt != null && (convedrt.Types == "Drive" || convedrt.Types == "DriveMiddle"))
           // {
               // Debug.WriteLine(convedrt.path + "\n" + convedrt.fold.ToString() + "\n" + convedrt.Types + "\n" + convedrt.isciz.ToString());
              //  StorageItemThumbnail thumbnail = convedrt.ThumbnailMode2;
             
               // image = new BitmapImage();
               // image.SetSource(thumbnail);
          //  }
            return (image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
