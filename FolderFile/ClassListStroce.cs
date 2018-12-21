﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FolderFile
{
   /// <summary>
   /// Содежит информацию о папке или файле
   /// </summary>
   public class ClassListStroce
    {
        StorageFolder storageFolder;
       public StorageFolder StorageFolder
        {
            get
            {
                return storageFolder;
            }
            set
            {
                storageFolder = value;

            }
        }
   
        public  StorageFile storageFile { get; set; }
        public bool FlagFolde { get; set; }
        public string FileAndFildeName
        {
            get
            {
                if (FlagFolde)
                {
                    return storageFolder.DisplayName;
                }
                else
                    return storageFile.DisplayName;
            }
        }
        public string FileAndFildeTip
        {
            get
            {
                if (FlagFolde)
                {
                    return storageFolder.DisplayType;
                }
                else
                    return storageFile.DisplayType;
            }
        }
        public string FileAndFildeData
        {
            get
            {
                if (FlagFolde)
                {
                    return storageFolder.DateCreated.ToString();
                }
                else
                    return storageFile.DateCreated.ToString();
            }
        }

    }
}