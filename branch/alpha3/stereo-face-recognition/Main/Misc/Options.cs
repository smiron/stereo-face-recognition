using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceDetection.Misc
{
    public static class Options
    {
        #region Properties

        public static bool ProcessFaces { get; set; }

        public static bool UseGpu { get; set; }

        public static CaptureResolution Resolution { get; set; }

        public static int CaptureWidth
        {
            get
            {
                switch (Resolution)
                {
                    case CaptureResolution.R1280x720:
                        return 1280;
                    case CaptureResolution.R1920x1080:
                        return 1920;
                }

                return 0;
            }
        }

        public static int CaptureHeight
        {
            get
            {
                switch (Resolution)
                {
                    case CaptureResolution.R1280x720:
                        return 720;
                    case CaptureResolution.R1920x1080:
                        return 1080;
                }

                return 0;
            }
        }

        public static StereoCalibrationOptions StereoCalibrationOptions
        {
            get; 
            set;
        }

        #endregion

        #region Instance

        static Options()
        {
            ProcessFaces = true;
            UseGpu = false;
            Resolution = CaptureResolution.R1280x720;
        }

        #endregion
    }

    public enum CaptureResolution
    {
        R1280x720,
        R1920x1080,
    }
}
