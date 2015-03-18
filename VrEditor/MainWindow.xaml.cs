using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace VrEditor
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class MainWindow : Window
    {

        public Scene CurrentScene
        {
            get;
            set;
        }

        public Hotspot CurrentHotspot
        {
            get;
            set;
        }

        public ObservableCollection<HotspotVM> HotspotVMs
        {
            get;
            set;
        }

        public MainWindow()
        {
            InitializeComponent();

            HotspotVMs = new ObservableCollection<HotspotVM>();

            Scene testScene = new Scene("Test");
            CurrentScene = testScene;

            CurrentScene.Hotspots.CollectionChanged += Hotspots_CollectionChanged;

            Hotspot testHotspot = new Hotspot("New hotspot");
            CircleShape shape = new CircleShape();
            shape.Center.X = 3148.0f;
            shape.Center.Y = 1361.0f;
            shape.Radius = 186.0f / 2.0f;
            testHotspot.Shape = shape;

            testScene.Hotspots.Add(testHotspot);


            DataContext = this;
        }

        private void RemoveVM(Hotspot hotspot)
        {
            foreach (HotspotVM vm in HotspotVMs)
            {
                if (vm.Hotspot == hotspot)
                {
                    HotspotVMs.Remove(vm);
                    break;
                }
            }
        }

        private void AddVM(Hotspot hotspot)
        {
            HotspotVM vm = new HotspotVM();
            vm.Hotspot = hotspot;
            HotspotVMs.Add(vm);
        }

        void Hotspots_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                foreach (Hotspot removed in e.OldItems)
                {
                    RemoveVM(removed);
                }
            }
        
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {}
            {
                foreach (Hotspot added in e.NewItems)
                {
                    AddVM(added);
                }
            }
            
        }

       

        private void bDeleteHotspot_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentHotspot != null)
            {
                CurrentScene.Hotspots.Remove(CurrentHotspot);
            }
        }

        private void bCreateHotspot_Click(object sender, RoutedEventArgs e)
        {
            Hotspot hotspot = new Hotspot("New Hotspot");
            CircleShape shape = new CircleShape();
            shape.Radius = 100;
            shape.Center = new Vector2(50, 50);
            hotspot.Shape = shape;


            CurrentScene.Hotspots.Add(hotspot);

            HotspotVM vm = new HotspotVM();
            vm.Hotspot = hotspot;
            HotspotVMs.Add(vm);
        }

        private void ParseScene()
        {
            HotspotVMs.Clear();
            foreach (Hotspot hotspot in CurrentScene.Hotspots)
            {
                AddVM(hotspot);
            }
        }

        private void mLoad_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Scene));
            using (TextReader reader = new StreamReader(@"C:\khaviar\game.xml"))
            {
                CurrentScene = serializer.Deserialize(reader) as Scene;
                ParseScene();
            } 
        }

        private void mSave_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Scene));
            using (TextWriter writer = new StreamWriter(@"C:\khaviar\game.xml"))
            {
                serializer.Serialize(writer, CurrentScene);
            } 
        }

      

        
 

        private Point startDrag;

        private bool isDragging;
        private bool waitForDrag;


        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            var ellipse = (FrameworkElement)sender;
            var hotspotVM = (HotspotVM) ellipse.DataContext;

            CurrentHotspot = hotspotVM.Hotspot;

            ellipse.CaptureMouse();
            startDrag = e.GetPosition(itemsControl);

            waitForDrag = true;

            e.Handled = true;
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
               
                Point curMouseDownPoint = e.GetPosition(itemsControl);
                var dragDelta = curMouseDownPoint - startDrag;

                startDrag = curMouseDownPoint;

                var ellipse = (FrameworkElement)sender;
                var hotspotVM = (HotspotVM)ellipse.DataContext;

                hotspotVM.X += (float) dragDelta.X;
                hotspotVM.Y += (float) dragDelta.Y;
                
            }
            else if (waitForDrag)
            {
                
                Point curMouseDownPoint = e.GetPosition(itemsControl);
                var dragDelta = curMouseDownPoint - startDrag;
                
                if ((dragDelta.X > SystemParameters.MinimumHorizontalDragDistance) || 
                    (dragDelta.Y > SystemParameters.MinimumVerticalDragDistance))
                {

                    isDragging = true;
                    waitForDrag = false;
                }

                

                e.Handled = true;
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var ellipse = (FrameworkElement)sender;
            ellipse.ReleaseMouseCapture();

            e.Handled = true;

            isDragging = false;
            waitForDrag = false;
        }

        


        

        

       
    }
}
