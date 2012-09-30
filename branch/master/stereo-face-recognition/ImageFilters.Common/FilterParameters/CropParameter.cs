using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageFilters.Common;

namespace ImageFilters.Common.FilterParameters
{
    public class CropParameter : ParameterBase
    {
        public Region2D Region { get; set; }
    }
}
