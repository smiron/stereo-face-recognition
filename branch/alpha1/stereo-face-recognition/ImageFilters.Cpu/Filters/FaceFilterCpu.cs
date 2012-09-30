using Emgu.CV;
using ImageFilters.Cpu.Filters;

namespace ImageFilters.Filters
{
    public class FaceFilterCpu : FacePartFilterCpuBase
    {
        #region Fields

        private static FaceFilterCpu _instance;
        private HaarCascade _cascadeClassifier;

        #endregion

        #region Properties

        public static FaceFilterCpu Instance
        {
            get { return _instance ?? (_instance = new FaceFilterCpu()); }
        }

        protected override HaarCascade CascadeClassifier
        {
            get { return _cascadeClassifier ?? (_cascadeClassifier = new HaarCascade("HaarCascades\\haarcascade_frontalface_alt.xml")); }
        }

        #endregion

        #region Instance

        private FaceFilterCpu()
        {
        }

        #endregion
    }
}