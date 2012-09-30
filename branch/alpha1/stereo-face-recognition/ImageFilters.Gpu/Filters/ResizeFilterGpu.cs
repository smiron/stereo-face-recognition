using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common.Interfaces;

namespace ImageFilters.Cpu.Filters
{
    public class ResizeFilterGpu : IFilter<IImage, IImage, ResizeParameter>
    {
        #region Fields

        private static ResizeFilterGpu _instance;

        #endregion

        #region Properties

        public static ResizeFilterGpu Instance
        {
            get { return _instance ?? (_instance = new ResizeFilterGpu()); }
        }

        #endregion

        #region Methods

        public IImage GetResult(IImage source, ResizeParameter parameter)
        {
            if (source is GpuImage<Gray, byte> == false)
            {
                return null;
            }

            return new Image<Gray, byte>(source.Bitmap).Resize(parameter.NewWidth, parameter.NewHeight, INTER.CV_INTER_LINEAR);
        }

        #endregion

        #region Instance

        private ResizeFilterGpu()
        {
        }

        #endregion
    }
}