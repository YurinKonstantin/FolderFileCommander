using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FolderFile
{
    class FileAndFolderViewer
    {
        ObservableCollection<ClassListStroce> _ListCol = new ObservableCollection<ClassListStroce>();
        public StorageFolder storageFolderFirst { get; set; }
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
    }
}
