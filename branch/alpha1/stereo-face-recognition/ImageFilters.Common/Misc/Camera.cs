using Emgu.CV;
using Emgu.CV.Structure;

namespace ImageFilters.Common.Misc
{
    public class Camera
    {
        public Image<Bgr, byte> Image { get; set; }

        public FaceRegion2D[] FaceRegions { get; set; }
    }
}