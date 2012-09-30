using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageFilters.Cpu;
using ImageFilters.Gpu;
using ImageFilters.Filters;
using Emgu.CV;
using ImageFilters.Cpu.Filters;
using ImageFilters.Gpu.Filters;
using ImageFilters.Common.Interfaces;
using ImageFilters.Common.FilterParameters;

namespace ImageFilters.Common
{
    public class FilterInstances
    {
        #region Fields

        private Mode _mode;

        #endregion

        #region Properties

        public Mode Mode
        {
            get
            {
                return _mode;
            }
            private set
            {
                _mode = value;

                switch (_mode)
                {
                    case Mode.Cpu:
                        {
                            EyeFilter = EyeFilterCpu.Instance;
                            MouthFilter = MouthFilterCpu.Instance;
                            FaceFilter = FaceFilterCpu.Instance;
                            PrepareFilter = PrepareImageFilterCpu.Instance;
                            CropFilter = CropFilterCpu.Instance;
                            ResizeFilter = ResizeFilterCpu.Instance;
                            break;
                        }
                    case Mode.Gpu:
                        {
                            EyeFilter = EyeFilterGpu.Instance;
                            MouthFilter = MouthFilterGpu.Instance;
                            FaceFilter = FaceFilterGpu.Instance;
                            PrepareFilter = PrepareImageFilterGpu.Instance;
                            CropFilter = CropFilterGpu.Instance;
                            ResizeFilter = ResizeFilterGpu.Instance;
                            break;
                        }
                }
            }
        }

        public IFacePartFilter EyeFilter
        {
            get;
            private set;
        }

        public IFacePartFilter MouthFilter
        {
            get;
            private set;
        }

        public IFacePartFilter FaceFilter
        {
            get;
            private set;
        }

        public IFilter<IImage, IImage, PrepareImageParameter> PrepareFilter
        {
            get;
            private set;
        }

        public IFilter<IImage, IImage, CropParameter> CropFilter
        {
            get;
            private set;
        }

        public IFilter<IImage, IImage, ResizeParameter> ResizeFilter
        {
            get;
            private set;
        }

        #endregion

        #region Instance

        public FilterInstances(Mode mode)
        {
            Mode = mode;
        }

        #endregion
    }

    public enum Mode
    {
        Cpu,
        Gpu
    }
}
