using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FolderFile
{
    public class ListViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Maximum { set; get; }
        public DataTemplate Middle { set; get; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var myItem = item as ClassListStroce;
            if (myItem != null)
            {
                switch (myItem.Type)
                {
                    case "Maximum":
                        return Maximum;
                    case "Middle":
                        return Middle;

                }
            }

            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }





    }
}
