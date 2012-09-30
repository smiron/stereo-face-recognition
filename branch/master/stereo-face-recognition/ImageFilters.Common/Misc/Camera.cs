using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageFilters.Common;

namespace ImageFilters.Common.Misc
{
    public class Camera
    {
        public Image<Bgr, byte> Image { get; set; }

        public FaceRegion2D[] FaceRegions { get; set; }
    }
}
