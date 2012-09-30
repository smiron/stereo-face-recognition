using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using ImageFilters.Common;
using ImageFilters.Common.Interfaces;
using ImageFilters.Common.FilterParameters;

namespace ImageFilters.Cpu.Filters
{
    public class CropFilterCpu : IFilter<IImage, IImage, CropParameter>
    {
        #region Fields

        private static CropFilterCpu _instance;

        #endregion

        #region Properties

        public static CropFilterCpu Instance
        {
            get
            {
                return _instance ?? (_instance = new CropFilterCpu());
            }
        }

        #endregion

        #region Methods

        public IImage GetResult(IImage source, CropParameter parameter)
        {
            if (source is Image<Gray, byte> == false)
            {
                return null;
            }

            return ((Image<Gray, byte>)source).GetSubRect(parameter.Region.ToRectangle());
        }

        #endregion

        #region Instance

        private CropFilterCpu()
        {
        }

        #endregion
    }
}
