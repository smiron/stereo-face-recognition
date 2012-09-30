using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ImageFilters.Common.Models
{
    public class FaceModel : ModelBase
    {
        #region Properties

        public string Label
        {
            get;
            private set;
        }

        public List<Image<Gray, byte>> Images
        {
            get;
            private set;
        }

        public int ImagesCount
        {
            get
            {
                return Images.Count;
            }
        }

        #endregion

        #region Instance

        public FaceModel(string label)
            : this(label, new List<Image<Gray, byte>>())
        {
        }

        public FaceModel(string label, IEnumerable<Image<Gray, byte>> images)
        {
            Label = label;
            Images = new List<Image<Gray,byte>>(images);
        }

        #endregion
    }
}
