using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FaceDetection.Misc;
using ImageFilters.Common;
using System.IO.Packaging;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;

namespace FaceDetection.Forms
{
    public partial class FormCalibrateCameras : Form
    {
        #region Fields

        private readonly FormMain _main;

        private bool _isCalibrating;
        private bool _isRecording;

        private List<PointF[]> _leftImages;
        private List<PointF[]> _rightImages;

        private int _imageWidth;
        private int _imageHeight;

        private Size _patternSize;

        private MCvTermCriteria _findCornerSubPixelCriteria = new MCvTermCriteria(30, 0.01)
        {
            type = TERMCRIT.CV_TERMCRIT_ITER | TERMCRIT.CV_TERMCRIT_EPS
        };

        private Size _findCornerSubPixelWin = new Size(11, 11);

        #endregion

        #region Properties

        public bool IsRecording
        {
            get { return _isRecording; }
            set
            {
                _isRecording = value;

                switch (_isRecording)
                {
                    case true:
                    {
                        buttonStartCapture.Enabled = false;
                        buttonEndCapture.Enabled = true;
                        buttonCalibrate.Enabled = false;
                        buttonClose.Enabled = false;

                        break;
                    }
                    case false:
                    {
                        buttonStartCapture.Enabled = true;
                        buttonEndCapture.Enabled = false;
                        buttonCalibrate.Enabled = true;
                        buttonClose.Enabled = true;

                        break;
                    }
                }
            }
        }

        public bool IsCalibrating
        {
            get { return _isCalibrating; }
            set
            {
                _isCalibrating = value;

                switch (_isCalibrating)
                {
                    case true:
                    {
                        buttonStartCapture.Enabled = false;
                        buttonEndCapture.Enabled = false;
                        buttonCalibrate.Enabled = false;
                        buttonClose.Enabled = false;

                        break;
                    }
                    case false:
                    {
                        buttonStartCapture.Enabled = true;
                        buttonEndCapture.Enabled = false;
                        buttonCalibrate.Enabled = true;
                        buttonClose.Enabled = true;

                        break;
                    }
                }
            }
        }

        public int Nx
        {
            get
            {
                return (int)numericUpDownSquaresX.Value;
            }
        }

        public int Ny
        {
            get
            {
                return (int)numericUpDownSquaresY.Value;
            }
        }

        public float SquareSize
        {
            get
            {
                return (float)numericUpDownSquareSize.Value;
            }
        }

        #endregion

        #region Methods

        private void OnMainRawImagesAvailable(object sender, RawImagesEventArgs e)
        {
            if (e.Left == null
                || e.Right == null)
            {
                return;
            }

            _imageWidth = e.Left.Width;
            _imageHeight = e.Left.Height;

            PointF[] leftPoints = Emgu.CV.CameraCalibration.FindChessboardCorners
                (e.Left, _patternSize, CALIB_CB_TYPE.ADAPTIVE_THRESH | CALIB_CB_TYPE.NORMALIZE_IMAGE);

            PointF[] rightPoints = Emgu.CV.CameraCalibration.FindChessboardCorners
                (e.Right, _patternSize, CALIB_CB_TYPE.ADAPTIVE_THRESH | CALIB_CB_TYPE.NORMALIZE_IMAGE);

            if (leftPoints != null
                && rightPoints != null)
            {
                e.Left.FindCornerSubPix(new PointF[][] { leftPoints }, _findCornerSubPixelWin, new Size(-1, -1), _findCornerSubPixelCriteria);
                e.Right.FindCornerSubPix(new PointF[][] { rightPoints }, _findCornerSubPixelWin, new Size(-1, -1), _findCornerSubPixelCriteria);

                Emgu.CV.CameraCalibration.DrawChessboardCorners(e.Left, _patternSize, leftPoints);
                Emgu.CV.CameraCalibration.DrawChessboardCorners(e.Right, _patternSize, rightPoints);

                _leftImages.Add(leftPoints);
                _rightImages.Add(rightPoints);

                Invoke(new Action(delegate { labelAvailableImages.Text = _leftImages.Count.ToString(CultureInfo.InvariantCulture); }));
            }
        }

        private void OnButtonCalibrateClick(object sender, EventArgs e)
        {
            if (_leftImages.Count < 3)
            {
                return;
            }

            IsCalibrating = true;

            // Stereo Rectify images           
            Matrix<double> R1;
            Matrix<double> R2;
            Matrix<double> P1;
            Matrix<double> P2;
            Matrix<double> Q;

            try
            {
                MCvPoint3D32f[][] objectPoints = null;
                PointF[][] imagePointsLeft = new PointF[_leftImages.Count][];
                PointF[][] imagePointsRight = new PointF[_leftImages.Count][]; // count should be equal to _rightImages.Count

                var firstLeft = _leftImages.First();

                #region ObjectPoints

                objectPoints = new MCvPoint3D32f[_leftImages.Count][];
                objectPoints[0] = new MCvPoint3D32f[Nx * Ny];

                for (int i = 0; i < Ny; i++)
                {
                    for (int j = 0; j < Nx; j++)
                    {
                        objectPoints[0][i * Nx + j] = new MCvPoint3D32f(i * SquareSize, j * SquareSize, 0);
                    }
                }

                for (int i = 1; i < _leftImages.Count; i++)
                {
                    objectPoints[i] = new MCvPoint3D32f[Nx * Ny];
                    objectPoints[0].CopyTo(objectPoints[i], 0);
                }

                #endregion

                #region ImagePoints

                for (int i = 0; i < _leftImages.Count; i++)
                {
                    imagePointsLeft[i] = _leftImages[i];
                    imagePointsRight[i] = _rightImages[i];
                }

                #endregion

                var intrinsicCameraParametersLeft = new IntrinsicCameraParameters();
                var intrinsicCameraParametersRight = new IntrinsicCameraParameters();

                ExtrinsicCameraParameters extrinsicCameraParameters;
                Matrix<double> foundamentalMatrix;
                Matrix<double> essentialMatrix;

                CameraCalibration.StereoCalibrate(objectPoints, imagePointsLeft, imagePointsRight,
                                                  intrinsicCameraParametersLeft, intrinsicCameraParametersRight,
                                                  new Size(_imageWidth, _imageHeight),
                                                  CALIB_TYPE.DEFAULT,
                                                  new MCvTermCriteria(100, 1e-5) { type = TERMCRIT.CV_TERMCRIT_EPS | TERMCRIT.CV_TERMCRIT_ITER },
                                                  out extrinsicCameraParameters, out foundamentalMatrix, out essentialMatrix);

                Matrix<float> mapXLeft;
                Matrix<float> mapYLeft;
                Matrix<float> mapXRight;
                Matrix<float> mapYRight;

                intrinsicCameraParametersLeft.InitUndistortMap(_imageWidth, _imageHeight, out mapXLeft, out mapYLeft);
                intrinsicCameraParametersLeft.InitUndistortMap(_imageWidth, _imageHeight, out mapXRight, out mapYRight);

                var validPixROI1 = new Rectangle();
                var validPixROI2 = new Rectangle();

                StereoRectify(intrinsicCameraParametersLeft, intrinsicCameraParametersRight, 
                    new Size(_imageWidth, _imageHeight), extrinsicCameraParameters, 
                    out R1, out R2, out P1, out P2, out Q, 0, 0, 
                    new Size(_imageWidth, _imageHeight), ref validPixROI1, ref validPixROI2);

                Options.StereoCalibrationOptions = new StereoCalibrationOptions
                {
                    IntrinsicCameraParametersLeft = intrinsicCameraParametersLeft,
                    IntrinsicCameraParametersRight = intrinsicCameraParametersRight,
                    EssentialMatrix = essentialMatrix,
                    ExtrinsicCameraParameters = extrinsicCameraParameters,
                    FoundamentalMatrix = foundamentalMatrix,
                    MapXLeft = mapXLeft,
                    MapYLeft = mapYLeft,
                    MapXRight = mapXRight,
                    MapYRight = mapYRight,
                    P1 = P1,
                    P2 = P2,
                    R1 = R1,
                    R2 = R2,
                    Q = Q
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                IsCalibrating = false;
            }
        }

        public void StereoRectify
            (IntrinsicCameraParameters intrinsicParam1,
            IntrinsicCameraParameters intrinsicParam2,
            Size imageSize,
            ExtrinsicCameraParameters extrinsicParams,
            out Matrix<double> R1,
            out Matrix<double> R2,
            out Matrix<double> P1,
            out Matrix<double> P2,
            out Matrix<double> Q,
            STEREO_RECTIFY_TYPE flags,
            double alpha,
            Size newImageSize,
            ref Rectangle validPixROI1,
            ref Rectangle validPixROI2
            )
        {
            R1 = new Matrix<double>(3, 3);
            R2 = new Matrix<double>(3, 3);
            P1 = new Matrix<double>(3, 4);
            P2 = new Matrix<double>(3, 4);
            Q = new Matrix<double>(4, 4);

            CvInvoke.cvStereoRectify(
                intrinsicParam1.IntrinsicMatrix.Ptr,
                intrinsicParam2.IntrinsicMatrix.Ptr,
                intrinsicParam1.DistortionCoeffs.Ptr,
                intrinsicParam2.DistortionCoeffs.Ptr,
                imageSize,
                extrinsicParams.RotationVector.Ptr,
                extrinsicParams.TranslationVector.Ptr,
                R1.Ptr,
                R2.Ptr,
                P1.Ptr,
                P2.Ptr,
                Q.Ptr,
                STEREO_RECTIFY_TYPE.DEFAULT,
                alpha,
                newImageSize,
                ref validPixROI1,
                ref validPixROI1);
        }

        private void OnButtonStartCaptureClick(object sender, EventArgs e)
        {
            _leftImages.Clear();
            _rightImages.Clear();

            _patternSize = new Size(Nx, Ny);

            IsRecording = true;

            _main.RawImagesAvailable += OnMainRawImagesAvailable;
        }

        private void OnButtonEndCaptureClick(object sender, EventArgs e)
        {
            if (_leftImages.Count < 3)
            {
                return;
            }

            _main.RawImagesAvailable -= OnMainRawImagesAvailable;

            IsRecording = false;
        }

        private void OnFormCalibrateCamerasLoad(object sender, EventArgs e)
        {
            _leftImages = new List<PointF[]>();
            _rightImages = new List<PointF[]>();
        }

        private void OnButtonCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnButtonSaveClick(object sender, EventArgs e)
        {
            if (Options.StereoCalibrationOptions == null)
            {
                return;
            }

            string path = string.Empty;

            using (var sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.AutoUpgradeEnabled = true;
                sfd.CheckPathExists = true;
                sfd.DefaultExt = ".sc";
                sfd.Filter = "Stereo Calibration (*.sc)|*sc";

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                path = sfd.FileName;
            }

            Options.StereoCalibrationOptions.Save(path);
        }

        private void OnButtonLoadClick(object sender, EventArgs e)
        {
            string path = string.Empty;

            using (var ofd = new OpenFileDialog())
            {
                ofd.AddExtension = true;
                ofd.AutoUpgradeEnabled = true;
                ofd.CheckPathExists = true;
                ofd.DefaultExt = ".sc";
                ofd.Filter = "Stereo Calibration (*.sc)|*sc";

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                path = ofd.FileName;
            }

            Options.StereoCalibrationOptions = new StereoCalibrationOptions();

            Options.StereoCalibrationOptions.Load(path);
        }

        #endregion

        #region Instance

        public FormCalibrateCameras(FormMain main)
        {
            InitializeComponent();

            _main = main;
        }

        #endregion
    }
}