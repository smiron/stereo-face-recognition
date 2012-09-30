using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;

namespace Main.Common.Misc
{
    public class Face3D
    {
        public MCvPoint3D64f? Location { get; set; }

        public MCvPoint3D64f? LeftEye { get; set; }

        public MCvPoint3D64f? RightEye { get; set; }

        public MCvPoint3D64f? Mouth { get; set; }
    }
}
