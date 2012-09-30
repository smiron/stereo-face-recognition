using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.GPU;
using System.Drawing;
using Emgu.CV.Structure;

using ImageFilters.Gpu.Filters;

namespace ImageFilters.Gpu
{
    public class EyeFilterGpu : FacePartFilterGpuBase
    {
        #region Fields

        protected GpuCascadeClassifier _cascadeClassifier;
        private static EyeFilterGpu _instance;

        #endregion

        #region Properties

        public static EyeFilterGpu Instance
        {
            get
            {
                return _instance ?? (_instance = new EyeFilterGpu());
            }
        }

        protected override GpuCascadeClassifier CascadeClassifier
        {
            get
            {
                return _cascadeClassifier ?? (_cascadeClassifier = new GpuCascadeClassifier("HaarCascades\\haarcascade_eye.xml"));
            }
        }

        #endregion

        #region Instance

        private EyeFilterGpu()
        {
        }

        #endregion
    }
}
