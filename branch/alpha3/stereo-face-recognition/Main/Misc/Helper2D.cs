using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using ImageFilters.Common;
using ImageFilters.Common.FilterParameters;
using Main.Common.Misc;

namespace FaceDetection.Misc
{
    public static class Helper2D
    {
        #region Properties

        public static FilterInstances FilterInstances { get; set; }

        #endregion

        #region Methods

        public static FaceRegion2D[] GetFaceRegion2Ds(IImage gray,
                                                      int faceWidth, int faceHeight, bool findEyes = true, bool findMouthRegions = true)
        {
            var ret = new List<FaceRegion2D>();

            var parameter = new PrepareImageParameter
            {
            };

            using (var grayGpu = new GpuImage<Gray, byte>((Image<Gray, byte>)gray))
            {
                IEnumerable<Region2D> faceRegions = FilterInstances.FaceFilter.GetResult(grayGpu, new FacePartParameter
                                                                                                           {
                                                                                                               MinWidth = faceWidth,
                                                                                                               MinHeight = faceHeight
                                                                                                           });

                if (faceRegions == null)
                {
                    return ret.ToArray();
                }


                foreach (Region2D faceRegion in faceRegions)
                {
                    Region2D leftEyeRegion = null;
                    Region2D rightEyeRegion = null;
                    Region2D mouthRegion = null;
                    double angle = 0;
                    IImage faceImage = null;

                    using (IImage cropFace = FilterInstances.CropFilter.GetResult(grayGpu, new CropParameter { Region = faceRegion }))
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
                                                 Width = faceRegion.Width / 2,
                                                 Height = faceRegion.Height / 2
                                             };

                            IEnumerable<Region2D> leftEyes = FilterInstances.EyeFilter.GetResult
                                (cropFace, new FacePartParameter { Region = leftRegion });

                            if (leftEyes.Any())
                            {
                                leftEyeRegion = new Region2D
                                                {
                                                    Location = new Point
                                                               {
                                                                   X = (int)leftEyes.Average(item => item.Location.X) + leftRegion.Location.X,
                                                                   Y = (int)leftEyes.Average(item => item.Location.Y) + leftRegion.Location.Y
                                                               },
                                                    Width = (int)leftEyes.Average(item => item.Width),
                                                    Height = (int)leftEyes.Average(item => item.Height)
                                                };
                            }

                            #endregion

                            #region Right

                            var rightRegion = new Region2D
                                              {
                                                  Location = new Point
                                                             {
                                                                 X = faceRegion.Width / 2,
                                                                 Y = 0,
                                                             },
                                                  Width = faceRegion.Width / 2,
                                                  Height = faceRegion.Height / 2
                                              };

                            IEnumerable<Region2D> rightEyes = FilterInstances.EyeFilter.GetResult
                                (cropFace, new FacePartParameter { Region = rightRegion });

                            if (rightEyes.Any())
                            {
                                rightEyeRegion = new Region2D
                                                 {
                                                     Location = new Point
                                                                {
                                                                    X = (int)rightEyes.Average(item => item.Location.X) + rightRegion.Location.X,
                                                                    Y = (int)rightEyes.Average(item => item.Location.Y) + rightRegion.Location.Y
                                                                },
                                                     Width = (int)rightEyes.Average(item => item.Width),
                                                     Height = (int)rightEyes.Average(item => item.Height)
                                                 };
                            }

                            #endregion

                            #endregion

                            #region Calculate Face Angle

                            if (leftEyeRegion != null
                                && rightEyeRegion != null)
                            {
                                int yCoord = (rightEyeRegion.Location.Y + rightEyeRegion.Height / 2)
                                             - (leftEyeRegion.Location.Y + leftEyeRegion.Height / 2);

                                int xCoord = (rightEyeRegion.Location.X + rightEyeRegion.Width / 2 + leftRegion.Width)
                                             - (leftEyeRegion.Location.X + leftEyeRegion.Width / 2);

                                // calc rotation angle in radians
                                angle = -Math.Atan2(yCoord, xCoord) * (180.0 / Math.PI);                            }

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
                                                                       Y = faceRegion.Height / 2
                                                                   },
                                                        Width = faceRegion.Width,
                                                        Height = faceRegion.Height / 2
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
                                                                 X = (int)mouths.Average(item => item.Location.X) + mouthSearchRegion.Location.X,
                                                                 Y = (int)mouths.Average(item => item.Location.Y) + mouthSearchRegion.Location.Y
                                                             },
                                                  Width = (int)mouths.Average(item => item.Width),
                                                  Height = (int)mouths.Average(item => item.Height)
                                              };
                            }

                            #endregion
                        }

                        #endregion

                        faceImage = FilterInstances.ResizeFilter.GetResult(cropFace, new ResizeParameter { NewHeight = faceHeight, NewWidth = faceWidth });
                    }

                    ret.Add(new FaceRegion2D
                            {
                                Face = faceRegion,
                                LeftEye = leftEyeRegion,
                                RightEye = rightEyeRegion,
                                Mouth = mouthRegion,
                                EyeAngle = angle,
                                FaceImage = (Image<Gray, byte>)faceImage,
                                SourceImage = (gray as Image<Gray, byte>) ?? ((GpuImage<Gray, byte>)grayGpu).ToImage()
                            });
                }
            }

            return ret.ToArray();

        }


        public static FaceRegion2D[] GetFaceRegion2Ds(Image<Bgr, byte> image,
                                                      int faceWidth, int faceHeight, bool findEyes = true, bool findMouthRegions = true)
        {
            var parameter = new PrepareImageParameter
                            {
                            };

            using (IImage gray = FilterInstances.PrepareFilter.GetResult(image, parameter))
            {
                return GetFaceRegion2Ds((Image<Gray, byte>)gray, faceWidth, faceHeight, findEyes, findMouthRegions);
            }
        }

        public static double CalculateAngle2D(PointF p1, PointF p2)
        {
            double xCoord = p2.X - p1.X;
            double yCoord = p2.Y - p1.Y;

            return -Math.Atan2(yCoord, xCoord) * (180.0 / Math.PI);
        }

        public static double GetDistance(MCvPoint3D64f p1, MCvPoint3D64f p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2) + Math.Pow(p1.z - p2.z, 2));
        }

        public static double CalculateAngle3D(MCvPoint3D64f p1, MCvPoint3D64f p2, MCvPoint3D64f norm)
        {
            var ip = GetDistance(p1, p2);
            var cat1 = GetDistance(p1, norm);
            var cat2 = GetDistance(p2, norm);

            return CalculateAngle2D(new PointF(0, 0), new PointF((float)cat1, (float)cat2));
        }

        public static double CalculateFaceRoll(Face3D face)
        {
            if (face.LeftEye == null || face.RightEye == null)
            {
                return 0;
            }

            return CalculateAngle3D(face.LeftEye.Value, face.RightEye.Value,
                new MCvPoint3D64f(face.RightEye.Value.x, face.LeftEye.Value.y, face.RightEye.Value.z) /*the point directly under the right eye*/);
        }

        public static double CalculateFaceYaw(Face3D face)
        {
            if (face.LeftEye == null || face.RightEye == null)
            {
                return 0;
            }

            return CalculateAngle3D(face.LeftEye.Value, face.RightEye.Value, 
                new MCvPoint3D64f(face.RightEye.Value.x, face.RightEye.Value.y, face.LeftEye.Value.z));
        }

        public static double CalculateFacePitch(Face3D face)
        {
            if (face.Location == null || face.Mouth == null)
            {
                return 0;
            }

            return CalculateAngle3D(face.Location.Value, face.Mouth.Value, 
                new MCvPoint3D64f(face.Location.Value.x, face.Mouth.Value.y, face.Location.Value.z));
        }

        #endregion
    }
}