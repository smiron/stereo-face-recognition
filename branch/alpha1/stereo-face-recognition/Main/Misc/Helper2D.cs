using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using ImageFilters.Common;
using ImageFilters.Common.FilterParameters;

namespace FaceDetection.Misc
{
    public static class Helper2D
    {
        #region Properties

        public static FilterInstances FilterInstances { get; set; }

        #endregion

        #region Methods

        public static FaceRegion2D[] GetFaceRegion2Ds(Image<Bgr, byte> image,
                                                      int faceWidth, int faceHeight, bool findEyes = true, bool findMouthRegions = true)
        {
            var ret = new List<FaceRegion2D>();

            var parameter = new PrepareImageParameter
                            {
                            };

            using (IImage gray = FilterInstances.PrepareFilter.GetResult(image, parameter))
            {
                foreach (Region2D faceRegion in FilterInstances.FaceFilter.GetResult(gray, new FacePartParameter
                                                                                           {
                                                                                               MinWidth = faceWidth,
                                                                                               MinHeight = faceHeight
                                                                                           }))
                {
                    Region2D leftEyeRegion = null;
                    Region2D rightEyeRegion = null;
                    Region2D mouthRegion = null;
                    double angle = 0;
                    IImage faceImage = null;

                    using (IImage cropFace = FilterInstances.CropFilter.GetResult(gray, new CropParameter {Region = faceRegion}))
                    {
                        #region Face parts detection

                        if (findEyes)
                        {
                            #region Detect Eyes

                            #region Left

                            var leftRegion = new Region2D
                                             {
                                                 Location = new Point
                                                            {
                                                                X = 0,
                                                                Y = 0
                                                            },
                                                 Width = faceRegion.Width/2,
                                                 Height = faceRegion.Height/2
                                             };

                            IEnumerable<Region2D> leftEyes = FilterInstances.EyeFilter.GetResult
                                (cropFace, new FacePartParameter {Region = leftRegion});

                            if (leftEyes.Any())
                            {
                                leftEyeRegion = new Region2D
                                                {
                                                    Location = new Point
                                                               {
                                                                   X = (int) leftEyes.Average(item => item.Location.X) + leftRegion.Location.X,
                                                                   Y = (int) leftEyes.Average(item => item.Location.Y) + leftRegion.Location.Y
                                                               },
                                                    Width = (int) leftEyes.Average(item => item.Width),
                                                    Height = (int) leftEyes.Average(item => item.Height)
                                                };
                            }

                            #endregion

                            #region Right

                            var rightRegion = new Region2D
                                              {
                                                  Location = new Point
                                                             {
                                                                 X = faceRegion.Width/2,
                                                                 Y = 0,
                                                             },
                                                  Width = faceRegion.Width/2,
                                                  Height = faceRegion.Height/2
                                              };

                            IEnumerable<Region2D> rightEyes = FilterInstances.EyeFilter.GetResult
                                (cropFace, new FacePartParameter {Region = rightRegion});

                            if (rightEyes.Any())
                            {
                                rightEyeRegion = new Region2D
                                                 {
                                                     Location = new Point
                                                                {
                                                                    X = (int) rightEyes.Average(item => item.Location.X) + rightRegion.Location.X,
                                                                    Y = (int) rightEyes.Average(item => item.Location.Y) + rightRegion.Location.Y
                                                                },
                                                     Width = (int) rightEyes.Average(item => item.Width),
                                                     Height = (int) rightEyes.Average(item => item.Height)
                                                 };
                            }

                            #endregion

                            #endregion

                            #region Calculate Face Angle

                            if (leftEyeRegion != null
                                && rightEyeRegion != null)
                            {
                                int yCoord = (rightEyeRegion.Location.Y + rightEyeRegion.Height/2)
                                             - (leftEyeRegion.Location.Y + leftEyeRegion.Height/2);

                                int xCoord = (rightEyeRegion.Location.X + rightEyeRegion.Width/2 + leftRegion.Width)
                                             - (leftEyeRegion.Location.X + leftEyeRegion.Width/2);

                                // calc rotation angle in radians
                                angle = -Math.Atan2(yCoord, xCoord)*(180.0/Math.PI);
                            }

                            #endregion
                        }

                        if (findMouthRegions)
                        {
                            #region Mouth

                            var mouthSearchRegion = new Region2D
                                                    {
                                                        Location = new Point
                                                                   {
                                                                       X = 0,
                                                                       Y = faceRegion.Height/2
                                                                   },
                                                        Width = faceRegion.Width,
                                                        Height = faceRegion.Height/2
                                                    };

                            IEnumerable<Region2D> mouths = FilterInstances.MouthFilter.GetResult(cropFace, new FacePartParameter
                                                                                                           {
                                                                                                               Region = mouthSearchRegion
                                                                                                           });

                            if (mouths.Any())
                            {
                                mouthRegion = new Region2D
                                              {
                                                  Location = new Point
                                                             {
                                                                 X = (int) mouths.Average(item => item.Location.X) + mouthSearchRegion.Location.X,
                                                                 Y = (int) mouths.Average(item => item.Location.Y) + mouthSearchRegion.Location.Y
                                                             },
                                                  Width = (int) mouths.Average(item => item.Width),
                                                  Height = (int) mouths.Average(item => item.Height)
                                              };
                            }

                            #endregion
                        }

                        #endregion

                        faceImage = FilterInstances.ResizeFilter.GetResult(cropFace, new ResizeParameter {NewHeight = faceHeight, NewWidth = faceWidth});
                    }

                    ret.Add(new FaceRegion2D
                            {
                                Face = faceRegion,
                                LeftEye = leftEyeRegion,
                                RightEye = rightEyeRegion,
                                Mouth = mouthRegion,
                                EyeAngle = angle,
                                FaceImage = (Image<Gray, byte>) faceImage,
                                SourceImage = (gray as Image<Gray, byte>) ?? ((GpuImage<Gray, byte>) gray).ToImage()
                            });
                }
            }

            return ret.ToArray();
        }

        #endregion
    }
}