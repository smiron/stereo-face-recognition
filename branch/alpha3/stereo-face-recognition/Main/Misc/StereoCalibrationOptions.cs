using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using System.IO.Packaging;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace FaceDetection.Misc
{
    public class StereoCalibrationOptions
    {
        public IntrinsicCameraParameters IntrinsicCameraParametersLeft
        {
            get;
            set;
        }

        public IntrinsicCameraParameters IntrinsicCameraParametersRight
        {
            get;
            set;
        }

        public ExtrinsicCameraParameters ExtrinsicCameraParameters
        {
            get;
            set;
        }

        public Matrix<double> FoundamentalMatrix
        {
            get;
            set;
        }

        public Matrix<double> EssentialMatrix
        {
            get;
            set;
        }

        public Matrix<float> MapXLeft
        {
            get;
            set;
        }

        public Matrix<float> MapYLeft
        {
            get;
            set;
        }

        public Matrix<float> MapXRight
        {
            get;
            set;
        }

        public Matrix<float> MapYRight
        {
            get;
            set;
        }

        public Matrix<double> R1
        {
            get;
            set;
        }

        public Matrix<double> R2
        {
            get;
            set;
        }

        public Matrix<double> P1
        {
            get;
            set;
        }

        public Matrix<double> P2
        {
            get;
            set;
        }
        
        public Matrix<double> Q
        {
            get;
            set;
        }

        public void Save(string path)
        {
            using (Package package = Package.Open(path, FileMode.Create))
            {
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.EssentialMatrix, "EssentialMatrix");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.ExtrinsicCameraParameters.RotationVector, "RotationVector");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.ExtrinsicCameraParameters.TranslationVector, "TranslationVector");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.FoundamentalMatrix, "FoundamentalMatrix");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.IntrinsicCameraParametersLeft.DistortionCoeffs, "LeftDistortionCoeffs");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.IntrinsicCameraParametersLeft.IntrinsicMatrix, "LeftIntrinsicMatrix");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.IntrinsicCameraParametersRight.DistortionCoeffs, "RightDistortionCoeffs");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.IntrinsicCameraParametersRight.IntrinsicMatrix, "RightIntrinsicMatrix");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.MapXLeft, "MapXLeft");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.MapXRight, "MapXRight");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.MapYLeft, "MapYLeft");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.MapYRight, "MapYRight");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.P1, "P1");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.P2, "P2");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.Q, "Q");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.R1, "R1");
                WriteMatrixToPackage(package, Options.StereoCalibrationOptions.R2, "R2");
            }
        }

        public void Load(string path)
        {
            using (Package package = Package.Open(path, FileMode.Open))
            {
                Options.StereoCalibrationOptions.EssentialMatrix = ReadMatrixFromPackage<double>(package, "EssentialMatrix");

                Options.StereoCalibrationOptions.ExtrinsicCameraParameters = new ExtrinsicCameraParameters();
                Options.StereoCalibrationOptions.ExtrinsicCameraParameters.RotationVector =
                    new RotationVector3D(ReadMatrixFromPackage<double>(package, "RotationVector").ManagedArray.Cast<double>().ToArray());
                Options.StereoCalibrationOptions.ExtrinsicCameraParameters.TranslationVector = ReadMatrixFromPackage<double>(package, "TranslationVector");

                Options.StereoCalibrationOptions.FoundamentalMatrix = ReadMatrixFromPackage<double>(package, "FoundamentalMatrix");

                Options.StereoCalibrationOptions.IntrinsicCameraParametersLeft = new IntrinsicCameraParameters();
                Options.StereoCalibrationOptions.IntrinsicCameraParametersLeft.DistortionCoeffs = ReadMatrixFromPackage<double>(package, "LeftDistortionCoeffs");
                Options.StereoCalibrationOptions.IntrinsicCameraParametersLeft.IntrinsicMatrix = ReadMatrixFromPackage<double>(package, "LeftIntrinsicMatrix");

                Options.StereoCalibrationOptions.IntrinsicCameraParametersRight = new IntrinsicCameraParameters();
                Options.StereoCalibrationOptions.IntrinsicCameraParametersRight.DistortionCoeffs = ReadMatrixFromPackage<double>(package, "RightDistortionCoeffs");
                Options.StereoCalibrationOptions.IntrinsicCameraParametersRight.IntrinsicMatrix = ReadMatrixFromPackage<double>(package, "RightIntrinsicMatrix");

                Options.StereoCalibrationOptions.MapXLeft = ReadMatrixFromPackage<float>(package, "MapXLeft");
                Options.StereoCalibrationOptions.MapXRight = ReadMatrixFromPackage<float>(package, "MapXRight");
                Options.StereoCalibrationOptions.MapYLeft = ReadMatrixFromPackage<float>(package, "MapYLeft");
                Options.StereoCalibrationOptions.MapYRight = ReadMatrixFromPackage<float>(package, "MapYRight");
                Options.StereoCalibrationOptions.P1 = ReadMatrixFromPackage<double>(package, "P1");
                Options.StereoCalibrationOptions.P2 = ReadMatrixFromPackage<double>(package, "P2");
                Options.StereoCalibrationOptions.Q = ReadMatrixFromPackage<double>(package, "Q");
                Options.StereoCalibrationOptions.R1 = ReadMatrixFromPackage<double>(package, "R1");
                Options.StereoCalibrationOptions.R2 = ReadMatrixFromPackage<double>(package, "R2");
            }
        }

        private void WriteMatrixToPackage<TDepth>(Package package, Matrix<TDepth> matrix, string matrixName)
            where TDepth : new()
        {
            // create part
            PackagePart part = package.CreatePart
                (new Uri(string.Format("/Resources/{0}.xml", matrixName), UriKind.Relative), "text/xml");

            // write data
            using (var dstream = part.GetStream())
            {
                var sb = new StringBuilder();
                (new XmlSerializer(typeof(Matrix<TDepth>))).Serialize(new StringWriter(sb), matrix);
                var xDoc = new XmlDocument();
                xDoc.LoadXml(sb.ToString());

                xDoc.Save(dstream);
            }
        }

        private Matrix<TDepth> ReadMatrixFromPackage<TDepth>(Package package, string matrixName)
            where TDepth : new()
        {
            // create part
            PackagePart part = package.GetPart(new Uri(string.Format("/Resources/{0}.xml", matrixName), UriKind.Relative));

            // write data
            using (var stream = part.GetStream())
            {
                var xDoc = new XmlDocument();
                xDoc.Load(stream);

                return (Matrix<TDepth>)(new XmlSerializer(typeof(Matrix<TDepth>))).Deserialize(new XmlNodeReader(xDoc));
            }
        }
    }
}
