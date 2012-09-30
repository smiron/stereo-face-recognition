using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common.Interfaces;
using Emgu.CV.Structure;

namespace ImageFilters.Cpu.Filters
{
    public class ResizeFilterCpu : IFilter<IImage, IImage, ResizeParameter>
    {
        #region Fields

        private static ResizeFilterCpu _instance;

        #endregion

        #region Properties

        public static ResizeFilterCpu Instance
        {
            get
            {
                return _instance ?? (_instance = new ResizeFilterCpu());
            }
        }

        #endregion

        #region Methods

        public IImage GetResult(IImage source, ResizeParameter parameter)
        {
            if (source is Image<Gray, byte> == false)
            {
                return null;
            }

            return ((Image<Gray, byte>)source).Resize(parameter.NewWidth, parameter.NewHeight, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
        }

        #endregion

        #region Instance

        private ResizeFilterCpu()
        {
        }

        #endregion
    }
}
