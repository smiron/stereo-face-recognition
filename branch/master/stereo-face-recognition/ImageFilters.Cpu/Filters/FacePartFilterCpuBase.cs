using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using ImageFilters.Common.Interfaces;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common;

namespace ImageFilters.Cpu.Filters
{
    public abstract class FacePartFilterCpuBase : IFacePartFilter
    {
        #region Properties

        protected abstract HaarCascade CascadeClassifier
        {
            get;
        }

        #endregion

        #region Methods

        private MCvAvgComp[] Detect(IImage source, Size size)
        {
            return CascadeClassifier.Detect((Image<Gray, byte>)source, 1.1, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, size, Size.Empty);
        }

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
                ret = Detect((Image<Gray, byte>)source, size).Select(item => item.rect.ToRegion()).ToList();
            }
            else
            {
                using (var sourceCrop = ((Image<Gray, byte>)source).GetSubRect(parameter.Region.ToRectangle()))
                {
                    ret = Detect(sourceCrop, size).Select(item => item.rect.ToRegion()).ToList();
                }
            }

            return ret;
        }

        #endregion
    }
}
