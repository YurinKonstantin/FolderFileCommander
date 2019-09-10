using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace FolderFile
{
    class FileAndFolderViewer: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        ObservableCollection<ClassListStroce> _ListCol = new ObservableCollection<ClassListStroce>();
        public StorageFolder storageFolderFirst { get; set; }
        ObservableCollection<ClassListStroce> _ListCol1 = new ObservableCollection<ClassListStroce>();
        public StorageFolder storageFolderFirst1 { get; set; }
        public ObservableCollection<ClassListStroce> ListCol
        { get
            {
                return this._ListCol;
            }

            set
            {
                _ListCol = value;
            }
        }
        public ObservableCollection<ClassListStroce> ListCol1
        {
            get
            {
                return this._ListCol1;
            }

            set
            {
                _ListCol1 = value;
            }
        }

        ObservableCollection<string> _ListColName = new ObservableCollection<string>();
        public ObservableCollection<string> ListColName
        {
            get
            {
                return this._ListColName;
            }

            set
            {
                _ListColName = value;
            }
        }

        ObservableCollection<string> _ListColName2 = new ObservableCollection<string>();
        public ObservableCollection<string> ListColName2
        {
            get
            {
                return this._ListColName2;
            }

            set
            {
                _ListColName2 = value;
            }
        }
        string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                this.OnPropertyChanged();
            }
        }

       
    }
}
