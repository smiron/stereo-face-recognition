using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using ImageFilters.Common.FilterParameters;

namespace ImageFilters.Common.Interfaces
{
    public interface IFacePartFilter : IFilter<IImage, IEnumerable<Region2D>, FacePartParameter>
    {
    }
}
