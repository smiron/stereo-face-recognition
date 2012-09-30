using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ImageFilters.Common;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common.Interfaces;

namespace ImageFilters.Cpu.Filters
{
    public abstract class FacePartFilterCpuBase : IFacePartFilter
    {
        #region Properties

        protected abstract HaarCascade CascadeClassifier { get; }

        #endregion

        #region Methods

        public IEnumerable<Region2D> GetResult(IImage source, FacePartParameter parameter)
        {
            if (source is Image<Gray, byte> == false)
            {
                return null;
            }

            Size size = parameter == null
                            ? Size.Empty
                            : new Size(parameter.MinWidth, parameter.MinHeight);

            IEnumerable<Region2D> ret;

            if (parameter.Region == null)
            {
                ret = Detect(source, size).Select(item => item.rect.ToRegion()).ToList();
            }
            else
            {
                using (Image<Gray, byte> sourceCrop = ((Image<Gray, byte>) source).GetSubRect(parameter.Region.ToRectangle()))
                {
                    ret = Detect(sourceCrop, size).Select(item => item.rect.ToRegion()).ToList();
                }
            }

            return ret;
        }

        private MCvAvgComp[] Detect(IImage source, Size size)
        {
            return CascadeClassifier.Detect((Image<Gray, byte>) source, 1.1, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, size, Size.Empty);
        }

        #endregion
    }
}