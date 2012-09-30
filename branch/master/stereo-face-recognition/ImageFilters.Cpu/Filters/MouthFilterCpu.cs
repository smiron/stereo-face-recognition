using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;

using Emgu.CV;
using ImageFilters.Cpu.Filters;

namespace ImageFilters.Filters
{
    public class MouthFilterCpu : FacePartFilterCpuBase
    {
        #region Fields

        private static MouthFilterCpu _instance;
        private HaarCascade _cascadeClassifier;

        #endregion

        #region Properties

        public static MouthFilterCpu Instance
        {
            get
            {
                return _instance ?? (_instance = new MouthFilterCpu());
            }
        }

        protected override HaarCascade CascadeClassifier
        {
            get
            {
                return _cascadeClassifier ?? (_cascadeClassifier = new HaarCascade("HaarCascades\\haarcascade_mcs_mouth.xml"));
            }
        }

        #endregion

        #region Instance

        private MouthFilterCpu()
        {
        }

        #endregion
    }
}
