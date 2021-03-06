﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace FolderFile
{
    /// <summary>
    /// Содежит информацию о папке или файле
    /// </summary>
    public class ClassListStroce : INotifyPropertyChanged
    {
        public ClassListStroce(string path, string displauName, bool folderFlag, string tip, string data, string type)
        {
            Path = path;
            FlagFolde = folderFlag;
            FileAndFildeName = displauName;
            FileAndFildeTip = tip;
            Type = type;
            FileAndFildeData = data;


        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
      
        StorageItemThumbnail thumbnailMode;

        public StorageItemThumbnail ThumbnailMode
        {
            get
            {

                return thumbnailMode;


            }
            set
            {

                thumbnailMode = value;
                this.OnPropertyChanged();
            }
        }
   
      
    
        public bool FlagFolde { get; set; }
        public string Path { get; set; }
        public string FileAndFildeName { get; set; }
        public string FileAndFildeTip { get; set; }

   
        public string FileAndFildeData { get; set; }
     
        public string Type { get; set; }
        public async void Spase()
        {
            
                string ff = String.Empty;
                try
                {

                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Path);

                    const String k_freeSpace = "System.FreeSpace";
                    const String k_totalSpace = "System.Capacity";
                    var props = await storageFolder.Properties.RetrievePropertiesAsync(new string[] { k_freeSpace, k_totalSpace });
                double svoboda =Convert.ToDouble(props[k_freeSpace])/1000000000.0;
               
                double vsego = Convert.ToDouble(props[k_totalSpace]) / 1000000000.0;
                TotalSpace = vsego;
                FreeSpace = vsego - svoboda;
                ff = "FreeSpace: " + svoboda.ToString("0.000") + " "; 
                    ff += "Capacity:  " + vsego.ToString("0.000")+" GB"; ;

                ColorsSize(FreeSpace, TotalSpace);
                }
                catch(Exception)
                {

                }
           
            SpaseSize = ff;


            
        }
        public void ColorsSize(double free, double total)
        {
            double pros = free / (total / 100);
            if(pros<=20)
            {
                brush=new SolidColorBrush(Colors.Blue);
            }
            if(pros>80)
            {
                brush = new SolidColorBrush(Colors.Yellow);
            }
            if(pros>90)
            {
                brush = new SolidColorBrush(Colors.Red);
            }
        }
        Brush _brush = new SolidColorBrush(Colors.Blue);
        public Brush brush 
        { 
            get
            {
                return _brush;
            }
            set
            {
                _brush = value;
                this.OnPropertyChanged();
            }
        }
        public double freeSpace = 0;
        public double FreeSpace
        {
            get
            {
                return freeSpace;
            }
            set
            {
                freeSpace = value;
                this.OnPropertyChanged();

            }
        }
        public double totalSpace = 0;
        public double TotalSpace
        {
            get
            {
                return totalSpace;
            }
            set
            {
                totalSpace = value;
                this.OnPropertyChanged();

            }
        }

        string spaseSize="0";
        public  string SpaseSize 
        { 
            get
            {
                return spaseSize;
            }
            set
            {
                spaseSize =value;
                this.OnPropertyChanged();

            }
        }
        public Convedrt convedrt
        {
            get
            {
                return new Convedrt() { path=Path, fold= FlagFolde, isciz=ick};
            }
        }


        public bool ick { get; set; }
    }
    public class Convedrt
    {
        public string path { get; set; }
        public bool fold { get; set; }
        public bool isciz { get; set; }
     
        

    }
}
