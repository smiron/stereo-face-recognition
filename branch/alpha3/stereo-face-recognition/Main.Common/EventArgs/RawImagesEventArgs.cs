using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Misc
{
    public class RawImagesEventArgs : EventArgs
    {
        public Image<Gray, byte> Left
        {
            get;
            private set;
        }

        public Image<Gray, byte> Right
        {
            get;
            private set;
        }

        public RawImagesEventArgs(Image<Gray, byte> left, Image<Gray, byte> right)
        {
            Left = left;
            Right = right;
        }
    }
}
