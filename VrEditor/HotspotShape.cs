﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VrEditor
{
    [XmlInclude(typeof(CircleShape))]
    public abstract class HotspotShape
    {
    }
}
