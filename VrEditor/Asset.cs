using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrEditor
{
    [ImplementPropertyChanged]
    public class Asset
    {
        public string Id
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public string File
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
