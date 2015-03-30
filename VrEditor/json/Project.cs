using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrEditor.json
{
    public class Project
    {
        public string format = "1";

        public Game game;

        public List<Asset> assets = new List<Asset>();

        public List<Room> rooms = new List<Room>();
    }
}
