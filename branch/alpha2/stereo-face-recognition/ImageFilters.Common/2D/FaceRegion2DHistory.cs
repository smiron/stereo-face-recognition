namespace ImageFilters.Common._2D
{
    public class FaceRegion2DHistory
    {
        public FaceRegion2DHistory(FaceRegion2D faceRegion)
        {
            Face = faceRegion.Face;
            LeftEye = faceRegion.LeftEye;
            RightEye = faceRegion.RightEye;
            Mouth = faceRegion.Mouth;
            EyeAngle = faceRegion.EyeAngle;
        }

        /// <summary>
        /// Face region
        /// </summary>
        public Region2D Face { get; private set; }

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
    }
}