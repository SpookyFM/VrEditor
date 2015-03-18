using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VrEditor
{
    public class Vector2: INotifyPropertyChanged
    {
        [XmlAttribute]
        public float X
        {
            get;
            set;
        }
        
        [XmlAttribute]
        public float Y
        {
            get;
            set;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
