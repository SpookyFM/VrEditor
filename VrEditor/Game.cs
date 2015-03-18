using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VrEditor
{
    [ImplementPropertyChanged]
    public class Game
    {
        [XmlAttribute]
        public String Name
        {
            get;
            set;
        }

        public Scene StartScene
        {
            get;
            set;
        }

        public ObservableCollection<Scene> Scenes
        {
            get;
            set;
        }

        public Game()
        {
            Scenes = new ObservableCollection<Scene>();
        }


        public Scene GetScene(String name)
        {
            foreach (Scene scene in Scenes)
            {
                if (scene.Name == name) return scene;
            }
            return null;
        }

        public Game(String name)
            : this()
        {
            Name = name;
        }
    }
}
