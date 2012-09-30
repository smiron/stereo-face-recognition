using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.GPU;
using ImageFilters.Common.Interfaces;
using System.Drawing;
using ImageFilters.Common.FilterParameters;

namespace ImageFilters.Gpu.Filters
{
    public class PrepareImageFilterGpu : IFilter<IImage, IImage, PrepareImageParameter>
    {
        #region Fields

        private static PrepareImageFilterGpu _instance;

        #endregion

        #region Properties

        public static PrepareImageFilterGpu Instance
        {
            get
            {
                return _instance ?? (_instance = new PrepareImageFilterGpu());
            }
        }

        #endregion

        #region Methods

        public IImage GetResult(IImage source, PrepareImageParameter parameter)
        {
            if (source is Image<Bgr, byte> == false)
            {
                return null;
            }

            using (var image = new GpuImage<Bgr, byte>((Image<Bgr, byte>)source))
            {
                return image.Convert<Gray, byte>();
            }
        }

        #endregion

        #region Instance

        private PrepareImageFilterGpu()
        {
        }

        #endregion
    }
}
