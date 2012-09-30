using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using ImageFilters.Cpu.Filters;

namespace ImageFilters.Cpu
{
    public class EyeFilterCpu : FacePartFilterCpuBase
    {
        #region Fields

        private static EyeFilterCpu _instance;
        private HaarCascade _cascadeClassifier;

        #endregion

        #region Properties

        public static EyeFilterCpu Instance
        {
            get
            {
                return _instance ?? (_instance = new EyeFilterCpu());
            }
        }

        protected override HaarCascade CascadeClassifier
        {
            get 
            {
                return _cascadeClassifier ?? (_cascadeClassifier = new HaarCascade("HaarCascades\\haarcascade_eye.xml"));
            }
        }

        #endregion

        #region Instance

        private EyeFilterCpu()
        {
        }

        #endregion
    }
}
