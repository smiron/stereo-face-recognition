using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;

namespace FaceDetection.Misc
{
    public class StereoCalibrationOptions
    {
        public ExtrinsicCameraParameters ExtrinsicCameraParameters
        {
            get;
            set;
        }

        public Matrix<double> FoundamentalMatrix
        {
            get;
            set;
        }

        public Matrix<double> EssentialMatrix
        {
            get;
            set;
        }
    }
}
