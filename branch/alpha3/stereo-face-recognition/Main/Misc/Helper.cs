using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ImageFilters.Common;
using ImageFilters.Common._2D;

namespace FaceDetection.Misc
{
    public static class Helper
    {
        /// <summary>
        /// Performs brute-force to find the coresponding points with the smallest distance.
        /// Performs N steps, each with N-1 sub-steps, where each step performs a permutation 
        /// in the second list indices
        /// </summary>
        /// <param name="points1"></param>
        /// <param name="points2"></param>
        /// <returns>The best corespondence between the two lists</returns>
        public static IEnumerable<Tuple<int, int>> FindCorespondenceEqual(IEnumerable<Point> points1, IEnumerable<Point> points2, out double distance)
        {
            List<Point> points1List;
            List<Point> points2List;
            distance = double.MaxValue;

            if (points1 == null || points2 == null
                || (points1List = points1.ToList()).Count == 0
                || (points2List = points2.ToList()).Count == 0
                || points1List.Count != points2List.Count)
            {
                return null;
            }

            int N = points1List.Count;

            if (N == 1)
            {
                return new[] { new Tuple<int, int>(0, 0) };
            }

            // compute all permutations
            var p1Indices = GetSeries(N);

            var p2IndicesArray = new int[N * (N - 1)][];

            for (int i = 0; i < p2IndicesArray.Count(); i++)
            {
                p2IndicesArray[i] = new int[N];
            }

            int tmp;
            int p2Index = 0;

            for (int permScope = 0; permScope < N; permScope++)
            {
                for (int perm = 0; perm < N - 1; perm++)
                {
                    var indices = new int[N];
                    if (permScope == 0 && perm == 0)
                    {
                        // first iteration .. just copy form p1Indices
                        Array.Copy(p1Indices, indices, N);
                    }
                    else
                    {
                        // copy from previous and do perm
                        Array.Copy(p2IndicesArray[p2Index - 1], indices, N);

                        tmp = indices[perm];
                        indices[perm] = indices[perm + 1];
                        indices[perm + 1] = tmp;
                    }

                    p2IndicesArray[p2Index++] = indices;
                }
            }

            // find all distances and minimum permutation
            int minDistanceIndex = -1;

            for (int index = 0; index < p2IndicesArray.Length; index++)
            {
                var p2Indices = p2IndicesArray[index];
                double currentDistance = 0;

                for (int i = 0; i < N; i++)
                {
                    currentDistance += GetDistance(points1List[p1Indices[i]], points2List[p2Indices[i]]);
                }

                if (currentDistance < distance)
                {
                    minDistanceIndex = index;
                    distance = currentDistance;
                }
            }

            var ret = new Tuple<int, int>[N];

            for (int index = 0; index < N; index++)
            {
                ret[index] = new Tuple<int, int>(p1Indices[index], p2IndicesArray[minDistanceIndex][index]);
            }

            return ret;
        }

        public static IEnumerable<Tuple<int, int>> FindCorespondence(IEnumerable<Point> points1, IEnumerable<Point> points2, out double totalDistance)
        {
            totalDistance = double.MaxValue;

            List<Point> points1List;
            List<Point> points2List;
            totalDistance = double.MaxValue;

            if (points1 == null || points2 == null
                || (points1List = points1.ToList()).Count == 0
                || (points2List = points2.ToList()).Count == 0)
            {
                return null;
            }

            if (points1List.Count == points2List.Count)
            {
                return FindCorespondenceEqual(points1, points2, out totalDistance);
            }

            var pointsSmall = points1List.Count < points2List.Count ? points1List : points2List;
            var pointsLarge = points1List.Count > points2List.Count ? points1List : points2List;

            var NSmall = pointsSmall.Count;
            var NLarge = pointsLarge.Count;


            int[] seriesLarge = GetSeries(NLarge);

            var items = GetCombinations(seriesLarge, NSmall).Union(GetCombinations(seriesLarge.Reverse(), NSmall)).
                Select(comb =>
                {
                    double currentDistance;

                    return new
                    {
                        Corespondence = FindCorespondenceEqual(pointsSmall, comb.Select(pointIndex => pointsLarge[pointIndex]), out currentDistance),
                        Distance = currentDistance
                    };
                }).ToList();

            if (items.Count == 0)
            {
                return null;
            }

            var minItem = items.First();

            foreach (var item in items.Skip(1))
            {
                if (item.Distance < minItem.Distance)
                {
                    minItem = item;
                }
            }

            totalDistance = minItem.Distance;

            return minItem.Corespondence;
        }

        public static ValidateCorespondenceStatus[] ValidateCorespondence(FaceRegion2D[] points1, FaceRegion2D[] points2)
        {
            if (points1 == null
                || points1 == null
                || points1.Length == 0
                || points2.Length == 0
                || points1.Length != points2.Length)
            {
                return null;
            }

            var ret = new ValidateCorespondenceStatus[points1.Length];

            for (int i = 0; i < points1.Length; i++)
            {
                ret[i] = ValidateCorespondenceStatus.NoError;

                var firstFaceRegion = points1[i];
                var secondFaceRegion = points2[i];

                #region Validate Large Historic Shift

                {
                    FaceRegion2DHistory firstFaceRegionHistory;
                    FaceRegion2DHistory secondFaceRegionHistory;

                    // evaluate if we can apply validator
                    // for this validator we need the face region history
                    if (firstFaceRegion.History != null
                        && (firstFaceRegionHistory = firstFaceRegion.History.FirstOrDefault()) != null
                        && secondFaceRegion.History != null
                        && (secondFaceRegionHistory = secondFaceRegion.History.FirstOrDefault()) != null)
                    {
                        // compute distance from history
                        var firstDistance = GetDistance(firstFaceRegion.Face.Location, firstFaceRegionHistory.Face.Location);
                        var secondDistance = GetDistance(secondFaceRegion.Face.Location, secondFaceRegionHistory.Face.Location);

                        // difference between shifts
                        var difference = Math.Abs(firstDistance - secondDistance);

                        // compute average face size
                        var averageFaceSize = 
                            (firstFaceRegion.Face.Width + firstFaceRegion.Face.Height
                            + secondFaceRegion.Face.Width + secondFaceRegion.Face.Height
                            + firstFaceRegionHistory.Face.Width + firstFaceRegionHistory.Face.Height
                            + secondFaceRegionHistory.Face.Width + secondFaceRegionHistory.Face.Height) / 8d;

                        // allow a difference in shift between first and second of maximum 10% of average of face dimensions
                        if (difference > averageFaceSize / 10d)
                        {
                            ret[i] = firstDistance > secondDistance
                                ? ValidateCorespondenceStatus.LargeShiftInFirstRegion
                                : ValidateCorespondenceStatus.LargeShiftInSecondRegion;

                            continue;
                        }

                    }
                }

                #endregion
            }

            return ret;
        }

        public static int[] GetSeries(int max)
        {
            var ret = new int [max];

            for (int i = 0; i < max; i++)
            {
                ret[i] = i;
            }

            return ret;
        }

        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0
                ? new[] { new T[0] } 
                : elements.SelectMany((e, i) => elements.Skip(i + 1).GetCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static void DrawFaceRegion(Image<Bgr, Byte> image, FaceRegion2D faceRegion, Bgr color)
        {
            image.Draw(faceRegion.Face.ToRectangle(), color, 2);

            if (faceRegion.LeftEye != null)
            {
                image.Draw(new Rectangle
                    (faceRegion.Face.Location.X + faceRegion.LeftEye.Location.X,
                    faceRegion.Face.Location.Y + faceRegion.LeftEye.Location.Y,
                    faceRegion.LeftEye.Width,
                    faceRegion.LeftEye.Height),
                    color, 2);
            }

            if (faceRegion.RightEye != null)
            {
                image.Draw(new Rectangle
                    (faceRegion.Face.Location.X + faceRegion.RightEye.Location.X,
                    faceRegion.Face.Location.Y + faceRegion.RightEye.Location.Y,
                    faceRegion.RightEye.Width,
                    faceRegion.RightEye.Height),
                    color, 2);
            }

            if (faceRegion.Mouth != null)
            {
                image.Draw(new Rectangle
                    (faceRegion.Face.Location.X + faceRegion.Mouth.Location.X,
                    faceRegion.Face.Location.Y + faceRegion.Mouth.Location.Y,
                    faceRegion.Mouth.Width,
                    faceRegion.Mouth.Height),
                    color, 2);
            }
        }

        public static void DrawFaceRegionCircle(Image<Bgr, Byte> image, FaceRegion2D faceRegion, Bgr color)
        {
            image.Draw(new CircleF(new PointF(faceRegion.Face.Location.X + faceRegion.Face.Width / 2, 
                faceRegion.Face.Location.Y + faceRegion.Face.Height / 2), 
                (faceRegion.Face.Width + faceRegion.Face.Height) / 4), color, 2);

            if (faceRegion.LeftEye != null)
            {
                image.Draw(new CircleF(new PointF
                    (faceRegion.Face.Location.X + faceRegion.LeftEye.Location.X + faceRegion.LeftEye.Width / 2, 
                    faceRegion.Face.Location.Y + faceRegion.LeftEye.Location.Y + faceRegion.LeftEye.Height / 2),
                    (faceRegion.LeftEye.Width + faceRegion.LeftEye.Height) / 4),
                    color, 2);
            }

            if (faceRegion.RightEye != null)
            {
                image.Draw(new CircleF(new PointF
                    (faceRegion.Face.Location.X + faceRegion.RightEye.Location.X + faceRegion.RightEye.Width / 2,
                    faceRegion.Face.Location.Y + faceRegion.RightEye.Location.Y + faceRegion.RightEye.Height / 2),
                    (faceRegion.RightEye.Width + faceRegion.RightEye.Height) / 4),
                    color, 2);
            }
        }

        public enum ValidateCorespondenceStatus
        {
            NoError,
            LargeShiftInFirstRegion,
            LargeShiftInSecondRegion,
        }
    }
}
