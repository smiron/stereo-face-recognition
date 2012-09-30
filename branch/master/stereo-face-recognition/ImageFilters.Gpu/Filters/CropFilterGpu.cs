using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.GPU;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common.Interfaces;
using ImageFilters.Common;

namespace ImageFilters.Gpu.Filters
{
    public class CropFilterGpu : IFilter<IImage, IImage, CropParameter>
    {
        #region Fields

        private static CropFilterGpu _instance;

        #endregion

        #region Properties

        public static CropFilterGpu Instance
        {
            get
            {
                return _instance ?? (_instance = new CropFilterGpu());
            }
        }

        #endregion

        #region Methods

        public IImage GetResult(IImage source, CropParameter parameter)
        {
            if (source is GpuImage<Gray, byte> == false)
            {
                return null;
            }

            return ((GpuImage<Gray, byte>)source).GetSubRect(parameter.Region.ToRectangle());
        }

        #endregion

        #region Instance

        private CropFilterGpu()
        {
        }

        #endregion
    }
}
