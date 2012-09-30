using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.GPU;
using System.Drawing;
using Emgu.CV.Structure;

using ImageFilters.Gpu.Filters;

namespace ImageFilters.Filters
{
    public class MouthFilterGpu : FacePartFilterGpuBase
    {
        #region Fields

        private static MouthFilterGpu _instance;
        private GpuCascadeClassifier _cascadeClassifier;

        #endregion

        #region Properties

        public static MouthFilterGpu Instance
        {
            get
            {
                return _instance ?? (_instance = new MouthFilterGpu());
            }
        }

        protected override GpuCascadeClassifier CascadeClassifier
        {
            get
            {
                return _cascadeClassifier ?? (_cascadeClassifier = new GpuCascadeClassifier("HaarCascades\\haarcascade_mcs_mouth.xml"));
            }
        }

        #endregion

        #region Instance

        private MouthFilterGpu()
        {
        }

        #endregion
    }
}
