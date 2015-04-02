using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VrEditor
{
    [ImplementPropertyChanged]
    public class InventoryItem
    {
        
        [XmlAttribute]
        public String Name
        {
            get;
            set;
        }

        public ImageFileHolder Image
        {
            get;
            set;
        }

        public ImageFileHolder ActiveImage
        {
            get;
            set;
        }

        public String OnExamine
        {
            get;
            set;
        }

    }
}
