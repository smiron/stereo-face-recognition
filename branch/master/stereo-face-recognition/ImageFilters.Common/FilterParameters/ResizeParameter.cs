using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageFilters.Common.FilterParameters
{
    public class ResizeParameter : ParameterBase
    {
        public int NewWidth { get; set; }

        public int NewHeight { get; set; }
    }
}
