using Emgu.CV.GPU;
using ImageFilters.Gpu.Filters;

namespace ImageFilters.Filters
{
    public class FaceFilterGpu : FacePartFilterGpuBase
    {
        #region Fields

        private static FaceFilterGpu _instance;
        private GpuCascadeClassifier _cascadeClassifier;

        #endregion

        #region Properties

        public static FaceFilterGpu Instance
        {
            get { return _instance ?? (_instance = new FaceFilterGpu()); }
        }

        protected override GpuCascadeClassifier CascadeClassifier
        {
            get { return _cascadeClassifier ?? (_cascadeClassifier = new GpuCascadeClassifier("HaarCascades\\haarcascade_frontalface_alt.xml")); }
        }

        #endregion

        #region Instance

        private FaceFilterGpu()
        {
        }

        #endregion
    }
}