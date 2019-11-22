using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FolderFile
{
    public class ExplorerItemTemplateSelectorT : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate MusicItemTemplate { get; set; }
        public DataTemplate PictureItemTemplate { get; set; }
        public DataTemplate MusicFolderTemplate { get; set; }
        public DataTemplate PictureFolderTemplate { get; set; }
    
        protected override DataTemplate SelectTemplateCore(object item)
        {
            Debug.WriteLine("start");
            var node = (TreeViewNode)item;

            if (node.Content is StorageFolder)
            {
                Debug.WriteLine("Folder");
                var content = node.Content as StorageFolder;
                if (content.DisplayName.StartsWith("Pictures"))
                {
                    Debug.WriteLine("Folder1");
                    return PictureFolderTemplate;
                }

                if (content.DisplayName.StartsWith("Музыка"))
                {
                    Debug.WriteLine("Folder2");
                    return MusicFolderTemplate;
                }

                return MusicFolderTemplate;
            }
            else
            {


                if (node.Content is StorageFile)
                {
                    var content = node.Content as StorageFile;
                    if (content.ContentType.StartsWith("image")) return PictureItemTemplate;
                    if (content.ContentType.StartsWith("audio")) return MusicItemTemplate;

                }
            }
            // return DefaultTemplate;
            return PictureFolderTemplate;
        }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
