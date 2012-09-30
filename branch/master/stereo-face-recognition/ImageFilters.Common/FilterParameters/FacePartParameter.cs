using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageFilters.Common.FilterParameters
{
    public class FacePartParameter : HaarParameter
    {
        public Region2D Region
        {
            get;
            set;
        }
    }
}
