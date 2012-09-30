using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using ImageFilters.Common._2D;

namespace ImageFilters.Common
{
    public class FaceRegion2D
    {
        #region Properties

        /// <summary>
        /// Face Region Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Face region
        /// </summary>
        public Region2D Face { get; set; }

        /// <summary>
        /// Left eye region. Null if not detected
        /// </summary>
        public Region2D LeftEye { get; set; }

        /// <summary>
        /// Right eye region. Null if not detected
        /// </summary>
        public Region2D RightEye { get; set; }

        /// <summary>
        /// Mouth region. Null if not detected
        /// </summary>
        public Region2D Mouth { get; set; }

        /// <summary>
        /// Face angle calculated using the eyes of the face.
        /// 0 (zero) if not both eyes are detected
        /// </summary>
        public double EyeAngle { get; set; }

        /// <summary>
        /// Face image
        /// </summary>
        public Image<Gray, byte> FaceImage { get; set; }

        /// <summary>
        /// Source Image
        /// </summary>
        public Image<Gray, byte> SourceImage { get; set; }

        /// <summary>
        /// Bounding box region color
        /// </summary>
        public Bgr BoundingBoxColor { get; set; }

        public IEnumerable<FaceRegion2DHistory> History { get; set; }

        #endregion

        #region Methods

        public void SetHistory(FaceRegion2D parent)
        {
            Id = parent.Id;
            BoundingBoxColor = parent.BoundingBoxColor;
            EyeAngle += parent.EyeAngle;

            History = parent.History == null
                ? new[] { new FaceRegion2DHistory(parent) }
                : new[] { new FaceRegion2DHistory(parent) }.Union(parent.History).Take(10);
        }

        #endregion

        #region Instance

        public FaceRegion2D()
        {
            Id = Guid.NewGuid();
        }

        #endregion
    }
}
