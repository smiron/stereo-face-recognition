using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageFilters.Common.Models;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO.Packaging;
using System.IO;

namespace FaceDetection.Misc
{
    public static class HelperFaces
    {
        #region Fields

        private static MCvTermCriteria _criteria;

        #endregion

        #region Properties

        public static List<FaceModel> Faces
        {
            get;
            private set;
        }

        public static EigenObjectRecognizer Recognizer
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        private static List<FaceTrainingCase> GetTrainingCases()
        {
            return Faces.SelectMany(face => face.Images.Select(faceImage => new FaceTrainingCase(face.Label, faceImage))).ToList();
        }

        public static string Recognize(Image<Gray, byte> image)
        {
            var recognizer = Recognizer;

            if (recognizer == null)
            {
                return string.Empty;
            }

            image._EqualizeHist();

            var distances = recognizer.GetEigenDistances(image);

            int minIndex = 0;
            var distance = distances[0];

            for (int i = 1; i < distances.Length; i++)
            {
                if (distances[i] < distance)
                {
                    distance = distances[i];
                    minIndex = i;
                }
            }

            return recognizer.Labels[minIndex];
        }

        public static void TrainRecognizer()
        {
            var trainingCases = GetTrainingCases();

            if (trainingCases != null
                && trainingCases.Count > 0)
            {
                Image<Gray, byte>[] images = trainingCases.Select(item => item.Image).ToArray();
                string[] labels = trainingCases.Select(item => item.Label).ToArray();

                Recognizer = new EigenObjectRecognizer(images, labels, ref _criteria);
            }
        }

        public static void Save(string path)
        {
            using (Package package = Package.Open(path, FileMode.Create))
            {
                foreach (var trainingCase in GetTrainingCases())
                {
                    // create part
                    PackagePart part = package.CreatePart
                        (new Uri(string.Format("/Resources/{0}/{1}.pgm", trainingCase.Label, Guid.NewGuid().ToString()), UriKind.Relative), "image/pgm");

                    // write data
                    using (var dstream = part.GetStream())
                    using (var sWriter = new StreamWriter(dstream))
                    {
                        sWriter.WriteLine("P2");

                        sWriter.WriteLine("{0} {1}", trainingCase.Image.Width, trainingCase.Image.Height);

                        sWriter.WriteLine("255");

                        for (int y = 0; y < trainingCase.Image.Height; y++)
                        {
                            for (int x = 0; x < trainingCase.Image.Width; x++)
                            {
                                sWriter.Write("{0} ", trainingCase.Image.Data[y, x, 0]);
                            }

                            sWriter.Write(sWriter.NewLine);
                        }

                        sWriter.Flush();
                        dstream.Flush();
                    }

                    // create relationship
                    package.CreateRelationship(part.Uri, TargetMode.Internal,
                        "http://schemas.openxmlformats.org/package/2006/relationships/meta data/core-properties");
                }
            }
        }

        public static void Load(string path)
        {
            var trainingCases = new List<FaceTrainingCase>();

            using (Package package = Package.Open(path, FileMode.Open))
            {
                foreach (var part in package.GetParts())
                {
                    string[] uriParts = part.Uri.OriginalString.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                    if (uriParts[1] == ".rels")
                    {
                        continue;
                    }

                    using (var rStream = part.GetStream())
                    using (var sReader = new StreamReader(rStream))
                    {
                        sReader.ReadLine(); // P2
                        string[] widthHeight = sReader.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        int width = int.Parse(widthHeight[0]);
                        int height = int.Parse(widthHeight[1]);

                        sReader.ReadLine(); // 255

                        var data = new byte[width, height, 1];

                        for (int y = 0; y < height; y++)
                        {
                            string[] lineData = sReader.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < width; x++)
                            {
                                data[y, x, 0] = byte.Parse(lineData[x]);
                            }
                        }

                        var image = new Image<Gray, byte>(data);
                        trainingCases.Add(new FaceTrainingCase(uriParts[1], image));
                    }
                }
            }

            Faces = (from trainingCase in trainingCases
                     let label = trainingCase.Label
                     group trainingCase by trainingCase.Label into groupings
                     select new FaceModel(groupings.Key, groupings.Select(item => item.Image))).ToList();

            TrainRecognizer();
        }

        #endregion

        #region Instance

        static HelperFaces()
        {
            _criteria = new MCvTermCriteria();

            Faces = new List<FaceModel>();
        }

        #endregion
    }
}
