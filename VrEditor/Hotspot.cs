using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace VrEditor
{
    [ImplementPropertyChanged]
    public class Hotspot: INotifyPropertyChanged {

        [XmlAttribute]
        public String Name
        {
            get;
            set;
        }

        [XmlAttribute]
        public Boolean IsEnabled
        {
            get;
            set;
        }

        public HotspotShape Shape
        {
            get;
            set;
        }

        public String OnExamine
        {
            get;
            set;
        }

        public String OnGaze
        {
            get;
            set;
        }

        public String OnUse
        {
            get;
            set;
        }

        public String OnLook
        {
            get;
            set;
        }

        public String OnUseInventory
        {
            get;
            set;
        }

        public String OnTalkTo
        {
            get;
            set;
        }

        

        public Hotspot(String name) {
            Name = name;
        }

        public Hotspot()
        {
            OnExamine = String.Empty;
            OnUse = String.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
