using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrEditor.json
{
    public class Room
    {

        public string id;

        public string name = "blocks";

        public List<string> neighbours = new List<string>();

        public List<string> assets = new List<string>();
    }
}
