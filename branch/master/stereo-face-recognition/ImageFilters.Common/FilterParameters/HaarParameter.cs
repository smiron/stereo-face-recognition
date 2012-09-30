using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageFilters.Common.FilterParameters
{
    public class HaarParameter : ParameterBase
    {
        public int MinWidth { get; set; }

        public int MinHeight { get; set; }
    }
}
