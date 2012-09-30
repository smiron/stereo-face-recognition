using System.Collections.Generic;
using Emgu.CV;
using ImageFilters.Common.FilterParameters;

namespace ImageFilters.Common.Interfaces
{
    public interface IFacePartFilter : IFilter<IImage, IEnumerable<Region2D>, FacePartParameter>
    {
    }
}