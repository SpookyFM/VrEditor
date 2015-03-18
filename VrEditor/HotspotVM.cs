using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrEditor
{

    [ImplementPropertyChanged]
    public class HotspotVM
    {

        private Hotspot _hotspot;

        private void InitFromHotspot()
        {
            CircleShape shape = (CircleShape)_hotspot.Shape;
            Update = 1.0f;
        }

        public Hotspot Hotspot
        {
            get { return _hotspot; }
            set { _hotspot = value;
                CircleShape shape = (CircleShape)_hotspot.Shape;
                shape.PropertyChanged += _hotspot_PropertyChanged;
                shape.Center.PropertyChanged += _hotspot_PropertyChanged;
                InitFromHotspot();
            }
        }

        void _hotspot_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InitFromHotspot();
        }

        [DoNotCheckEqualityAttribute]
        public float X
        {
            get
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                return (float)(shape.Center.X - shape.Radius * 0.5);
            }
            set
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                shape.Center.X = (float) value + shape.Radius * 0.5f;
            }
        }

        [DoNotCheckEqualityAttribute]
        public float Y
        {
            get
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                return (float)(shape.Center.Y - shape.Radius * 0.5);
            }
            set
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                shape.Center.Y = (float)value + shape.Radius * 0.5f;
            }
        }

        [DoNotCheckEquality]
        [AlsoNotifyFor("Height")]
        public float Width
        {
            get
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                return shape.Radius * 2.0f;
            }
            set
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                shape.Radius = value / 2.0f;
            }
        }

        [DoNotCheckEquality]
        [AlsoNotifyFor("Width")]
        public float Height
        {
            get
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                return shape.Radius * 2.0f;
            }
            set
            {
                CircleShape shape = (CircleShape)_hotspot.Shape;
                shape.Radius = value / 2.0f;
            }
        }

        [DoNotCheckEquality]
        [AlsoNotifyFor("X", "Y", "Width", "Height")]
        public float Update
        {
            get;
            set;
        }

    }
}
