using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VrEditor
{
    public class CircleShape: HotspotShape, INotifyPropertyChanged
    {

        public Vector2 Center
        {
            get;
            set;
        }

        public float Radius
        {
            get;
            set;
        }

        public CircleShape(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public CircleShape()
            : this(new Vector2(0, 0), 0)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
