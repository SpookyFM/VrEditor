using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrEditor
{
    [ImplementPropertyChanged]
    class Game
    {
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
    }
}
