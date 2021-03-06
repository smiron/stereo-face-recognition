﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using ImageFilters.Common;
using ImageFilters.Common.FilterParameters;
using ImageFilters.Common.Interfaces;

namespace ImageFilters.Gpu.Filters
{
    public abstract class FacePartFilterGpuBase : IFacePartFilter
    {
        #region Properties

        protected abstract GpuCascadeClassifier CascadeClassifier { get; }

        #endregion

        #region Methods

        public IEnumerable<Region2D> GetResult(IImage source, FacePartParameter parameter)
        {
            if (source is GpuImage<Gray, byte> == false)
            {
                return null;
            }

            Size size = parameter == null
                            ? Size.Empty
                            : new Size(parameter.MinWidth, parameter.MinHeight);

            IEnumerable<Region2D> ret;

            if (parameter.Region == null)
            {
                ret = Detect(source, size).Select(item => item.ToRegion()).ToList();
            }
            else
            {
                using (GpuImage<Gray, byte> sourceCrop = ((GpuImage<Gray, byte>) source).GetSubRect(parameter.Region.ToRectangle()))
                {
                    ret = Detect(sourceCrop, size).Select(item => item.ToRegion()).ToList();
                }
            }

            return ret;
        }

        private Rectangle[] Detect(IImage source, Size size)
        {
            return CascadeClassifier.DetectMultiScale((GpuImage<Gray, byte>) source, 1.1, 10, size);
        }

        #endregion
    }
}