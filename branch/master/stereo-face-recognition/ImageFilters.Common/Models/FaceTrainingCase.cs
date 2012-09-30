using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ImageFilters.Common.Models
{
    public class FaceTrainingCase
    {
        public string Label { get; private set; }

        public Image<Gray, byte> Image { get; private set; }

        public FaceTrainingCase(string label, Image<Gray, byte> image)
        {
            Label = label;
            Image = image;
        }
    }
}
