using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageFilters.Common.FilterParameters;

namespace ImageFilters.Common.Interfaces
{
    public interface IFilter<TIn, TOut>
    {
        TOut GetResult(TIn source);
    }

    public interface IFilter<TIn, TOut, TParameter> 
        where TParameter : ParameterBase
    {
        TOut GetResult(TIn source, TParameter parameter);
    }
}
