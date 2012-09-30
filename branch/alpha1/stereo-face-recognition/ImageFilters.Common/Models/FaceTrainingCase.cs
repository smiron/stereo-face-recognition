using Emgu.CV;
using Emgu.CV.Structure;

namespace ImageFilters.Common.Models
{
    public class FaceTrainingCase
    {
        public FaceTrainingCase(string label, Image<Gray, byte> image)
        {
            Label = label;
            Image = image;
        }

        public string Label { get; private set; }

        public Image<Gray, byte> Image { get; private set; }
    }
}