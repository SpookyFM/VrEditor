using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VrEditor
{
    [ImplementPropertyChanged]
    public class Scene
    {
        
        [XmlAttribute]
        public String Name
        {
            get;
            set;
        }

        public ImageFileHolder BackgroundImage
        {
            get;
            set;
        }

        public ImageFileHolder BackgroundImageRight
        {
            get;
            set;
        }

        public String OnEnter
        {
            get;
            set;
        }

        public ObservableCollection<Hotspot> Hotspots
        {
            get;
            set;
        }

        public Scene(String name): this()
        {
            Name = name;
        }

        public Scene()
        {
            Hotspots = new ObservableCollection<Hotspot>();
        }

        public override string ToString()
        {
            return Name;
        }



    }
}
