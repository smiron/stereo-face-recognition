using Emgu.CV.GPU;
using ImageFilters.Gpu.Filters;

namespace ImageFilters.Gpu
{
    public class EyeFilterGpu : FacePartFilterGpuBase
    {
        #region Fields

        private static EyeFilterGpu _instance;
        protected GpuCascadeClassifier _cascadeClassifier;

        #endregion

        #region Properties

        public static EyeFilterGpu Instance
        {
            get { return _instance ?? (_instance = new EyeFilterGpu()); }
        }

        protected override GpuCascadeClassifier CascadeClassifier
        {
            get { return _cascadeClassifier ?? (_cascadeClassifier = new GpuCascadeClassifier("HaarCascades\\haarcascade_eye.xml")); }
        }

        #endregion

        #region Instance

        private EyeFilterGpu()
        {
        }

        #endregion
    }
}