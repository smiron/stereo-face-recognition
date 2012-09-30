using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageFilters.Common;

namespace FaceDetection.Misc
{
    public class FaceRegionsEventArgs : EventArgs
    {
        #region Properties

        public IEnumerable<FaceRegion2D> Left
        {
            get;
            private set;
        }

        public IEnumerable<FaceRegion2D> Right
        {
            get;
            private set;
        }

        public IEnumerable<Tuple<int, int>> Corespondences
        {
            get;
            private set;
        }

        #endregion

        #region Instance

        public FaceRegionsEventArgs(IEnumerable<FaceRegion2D> left, IEnumerable<FaceRegion2D> right,
            IEnumerable<Tuple<int, int>> corespondences)
        {
            Left = left;
            Right = right;
            Corespondences = corespondences;
        }

        #endregion
    }
}
