using Emgu.CV;
using Emgu.CV.Structure;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common.Interfaces;

namespace ImageFilters.Cpu.Filters
{
    /// <summary>
    /// Turns image to Image<Gray, byte>
    /// </summary>
    public class PrepareImageFilterCpu : IFilter<IImage, IImage, PrepareImageParameter>
    {
        #region Fields

        private static PrepareImageFilterCpu _instance;

        #endregion

        #region Properties

        public static PrepareImageFilterCpu Instance
        {
            get { return _instance ?? (_instance = new PrepareImageFilterCpu()); }
        }

        #endregion

        #region Methods

        public IImage GetResult(IImage source, PrepareImageParameter parameter)
        {
            if (source is Image<Bgr, byte> == false)
            {
                return null;
            }

            return ((Image<Bgr, byte>) source).Convert<Gray, byte>();
        }

        #endregion

        #region Instance

        private PrepareImageFilterCpu()
        {
        }

        #endregion
    }
}