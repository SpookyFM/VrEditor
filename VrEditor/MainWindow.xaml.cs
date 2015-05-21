using Microsoft.Win32;
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

using System.Xml.Serialization;

using System.Web.Script.Serialization;

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

        public Game CurrentGame
        {
            get;
            set;
        }

        public Hotspot CurrentHotspot
        {
            get;
            set;
        }

        public Asset CurrentAsset
        {
            get;
            set;
        }

        public InventoryItem CurrentItem
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

            CurrentGame = new Game();
            CurrentGame.Name = "New Game";
            

            Scene testScene = new Scene("New Scene");
            CurrentScene = testScene;
            CurrentGame.StartScene = testScene;
            CurrentGame.Scenes.Add(testScene);


            DataContext = this;

            // For development: Load the default file

            
            String filename = "C:/khaviar/BlocksFromHeaven/Project/project.xml";
            _currentProjectFile = filename;

            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            using (TextReader reader = new StreamReader(filename))
            {
                CurrentGame = serializer.Deserialize(reader) as Game;
                if (CurrentGame.StartScene != null)
                    CurrentGame.StartScene = CurrentGame.GetScene(CurrentGame.StartScene.Name);
                ParseScene();
            } 

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
            if (CurrentScene == null) return;
            Hotspot hotspot = new Hotspot("New Hotspot");
            hotspot.IsEnabled = true;
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
            if (CurrentScene == null) return;
            foreach (Hotspot hotspot in CurrentScene.Hotspots)
            {
                AddVM(hotspot);
            }
        }

        private String _currentProjectFile;

        private void mLoad_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScene == null) return;

            String filename = String.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Game project (*.xml)|*.xml";
            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                filename = dialog.FileName;
                _currentProjectFile = filename;
            }
            else
            {
                return;
            }
            
            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            using (TextReader reader = new StreamReader(filename))
            {
                CurrentGame = serializer.Deserialize(reader) as Game;
                CurrentGame.StartScene = CurrentGame.GetScene(CurrentGame.StartScene.Name);
                ParseScene();
            } 
        }

        private void mSave_Click(object sender, RoutedEventArgs e)
        {
            ExportGameXml(SerializationMode.SerializeEditor, false);
        }

      

        
 

        private Point startDrag;
        private float startDistance;
        private float startRadius;

        private bool isDragging;
        private bool waitForDrag;
        private bool isResizing;




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
            ellipse.Focus();
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
            else if (isResizing) {
                var ellipse = (FrameworkElement)sender;
                var hotspotVM = (HotspotVM)ellipse.DataContext;
                
                Point curMouseDownPoint = e.GetPosition(itemsControl);
                Vector2 centerV = (hotspotVM.Hotspot.Shape as CircleShape).Center;

                var resizeDelta = curMouseDownPoint - new Point(centerV.X, centerV.Y);

                float resizeLength = (float) resizeDelta.Length;
                float newDiameter = (resizeLength / startDistance) * startRadius;

                hotspotVM.Width = newDiameter;
                
            } else if (waitForDrag)
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
            isResizing = false;
            waitForDrag = false;
        }

        private void itemsControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                if (isDragging)
                {
                    // We want to stop dragging and start resizing
                    
                    // Save the size and distance we started out with
                    var ellipse = (FrameworkElement)sender;
                    var hotspotVM = (HotspotVM)ellipse.DataContext;

                    Point curMouseDownPoint = Mouse.GetPosition(itemsControl);
                    Vector2 centerV = (hotspotVM.Hotspot.Shape as CircleShape).Center;

                    var resizeDelta = curMouseDownPoint - new Point(centerV.X, centerV.Y);

                    startDistance = (float)resizeDelta.Length;


                    startRadius = (hotspotVM.Hotspot.Shape as CircleShape).Radius;
                    
                    isDragging = false;
                    isResizing = true;
                    e.Handled = true;
                }
            }
        }

        private void itemsControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                if (isResizing)
                {
                    // Go back to dragging?
                    isDragging = true;
                    isResizing = false;
                    startDrag = Mouse.GetPosition(itemsControl);
                    e.Handled = true;
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Relaod the hotspots
            ParseScene();
        }

        private void bDeleteScene_Click(object sender, RoutedEventArgs e)
        {
            CurrentGame.Scenes.Remove(CurrentScene);
        }

        private void bCreateScene_Click(object sender, RoutedEventArgs e)
        {
            Scene scene = new Scene("New scene");
            CurrentGame.Scenes.Add(scene);
        }

        private void bLoadBackground_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScene == null) return;
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                CurrentScene.BackgroundImage = dialog.FileName;
            }
        }

        private void mClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bCreateAsset_Click(object sender, RoutedEventArgs e)
        {
            Asset asset = new Asset();
            asset.Name = "New Asset";
            CurrentGame.Assets.Add(asset);
        }

        private void bDeleteAsset_Click(object sender, RoutedEventArgs e)
        {
            CurrentGame.Assets.Remove(CurrentAsset);
        }

        private void bEditAsset_Click(object sender, RoutedEventArgs e)
        {
            AssetEditor editor = new AssetEditor();
            editor.DataContext = CurrentAsset;
            editor.ShowDialog();
        }

        private void mExportProjectKha_Click(object sender, RoutedEventArgs e)
        {
            String filename = String.Empty;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Project.kha (*.kha)|*.kha";

            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                filename = dialog.FileName;
            }
            else
            {
                return;
            }

            KhaExporter exporter = new KhaExporter();
            String folder = Path.GetDirectoryName(filename);
            exporter.BuildProject(CurrentGame.Assets, folder);
            exporter.AddAssets(CurrentGame, folder);
            exporter.SaveTo(filename);
            
        }

        private void mExportGameXml_Click(object sender, RoutedEventArgs e)
        {
            ExportGameXml(SerializationMode.SerializeGame);
        }

        private void ExportGameXml(SerializationMode mode, bool saveAs = true)
        {
            ImageFileHolder.SerializationMode = mode;

            String filename = String.Empty;
            if (saveAs)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Game project (*.xml)|*.xml";

                bool? result = dialog.ShowDialog();
                if (result.GetValueOrDefault())
                {
                    filename = dialog.FileName;
                }
                else
                {
                    return;
                }
            }
            else
            {
                filename = _currentProjectFile;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, CurrentGame);
            } 
        }

        private void mSaveAs_Click(object sender, RoutedEventArgs e)
        {
            ExportGameXml(SerializationMode.SerializeEditor);
        }

        private void mExportQuick_Click(object sender, RoutedEventArgs e)
        {
            String projectKha = "C:\\khaviar\\BlocksFromHeaven\\project.kha";
            // Export project.kha
            KhaExporter exporter = new KhaExporter();
            String folder = Path.GetDirectoryName(projectKha);
            exporter.BuildProject(CurrentGame.Assets, folder);
            exporter.AddAssets(CurrentGame, folder);
            exporter.SaveTo(projectKha);

            // Export game.xml
            String gameXml = "C:\\khaviar\\BlocksFromHeaven\\Assets\\game.xml";
            ImageFileHolder.SerializationMode = SerializationMode.SerializeGame;
            XmlSerializer serializer = new XmlSerializer(typeof(Game));
            using (TextWriter writer = new StreamWriter(gameXml))
            {
                serializer.Serialize(writer, CurrentGame);
            } 
        }

        private void bDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentGame.InventoryItems.Remove(CurrentItem);
        }

        private void bCreateItem_Click(object sender, RoutedEventArgs e)
        {
            InventoryItem item = new InventoryItem();
            item.Name = "New item";
            CurrentGame.InventoryItems.Add(item);
        }

        private void bEditItem_Click(object sender, RoutedEventArgs e)
        {
            ItemEditor editor = new ItemEditor();
            editor.DataContext = CurrentItem;
            editor.ShowDialog();
        }

        private void GroupBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
        }

        private void GroupBox_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            string[] files = (string[]) data.GetData("FileNameW");

            foreach (string file in files)
            {
                AddAsset(file);
            }
            


        }

        private void AddAsset(String filename)
        {
            // Check if the asset is already there
            foreach (Asset cur in CurrentGame.Assets)
            {
                if (cur.File == filename)
                {
                    return;
                }
            }

            Asset asset = new Asset();
            asset.Name = "New Asset";
            AssetEditor.FromFile(filename, asset);
            CurrentGame.Assets.Add(asset);
        }

        private void bLoadBackgroundRight_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScene == null) return;
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result.GetValueOrDefault())
            {
                CurrentScene.BackgroundImageRight = dialog.FileName;
            }
        }

        


        

        

       
    }
}
