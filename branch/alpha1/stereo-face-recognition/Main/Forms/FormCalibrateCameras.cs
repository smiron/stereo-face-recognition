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

namespace FaceDetection.Forms
{
    public partial class FormCalibrateCameras : Form
    {
        #region Fields

        private readonly FormMain _main;
        private bool _isCalibrating;
        private bool _isRecording;
        private List<FaceRegion2D> _leftFaceRegions;
        private List<FaceRegion2D> _rightFaceRegions;

        #endregion

        #region Properties

        private bool IsRecording
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

        #endregion

        #region Methods

        private void OnButtonCalibrateClick(object sender, EventArgs e)
        {
            if (_leftFaceRegions.Count < 3)
            {
                return;
            }

            IsCalibrating = true;

            try
            {
                var objectPoints = new MCvPoint3D32f[_leftFaceRegions.Count][];

                FaceRegion2D firstLeft = _leftFaceRegions.First();

                for (int i = 0; i < _leftFaceRegions.Count; i++)
                {
                    objectPoints[i] = new MCvPoint3D32f[4];

                    objectPoints[i][0] = new MCvPoint3D32f(firstLeft.Mouth.Location.X, firstLeft.Mouth.Location.Y, 0);
                    objectPoints[i][1] = new MCvPoint3D32f(firstLeft.LeftEye.Location.X, firstLeft.LeftEye.Location.Y, 0);
                    objectPoints[i][2] = new MCvPoint3D32f(firstLeft.RightEye.Location.X, firstLeft.RightEye.Location.Y, 0);
                    objectPoints[i][3] = new MCvPoint3D32f(firstLeft.Face.Location.X, firstLeft.Face.Location.Y, 0);
                }

                var leftImagePoints = new PointF[_leftFaceRegions.Count][];

                for (int i = 0; i < _leftFaceRegions.Count; i++)
                {
                    leftImagePoints[i] = new PointF[4];

                    leftImagePoints[i][0] = new PointF(_leftFaceRegions[i].Mouth.Location.X, _leftFaceRegions[i].Mouth.Location.Y);
                    leftImagePoints[i][1] = new PointF(_leftFaceRegions[i].LeftEye.Location.X, _leftFaceRegions[i].LeftEye.Location.Y);
                    leftImagePoints[i][2] = new PointF(_leftFaceRegions[i].RightEye.Location.X, _leftFaceRegions[i].RightEye.Location.Y);
                    leftImagePoints[i][3] = new PointF(_leftFaceRegions[i].Face.Location.X, _leftFaceRegions[i].Face.Location.Y);
                }

                var rightImagePoints = new PointF[_leftFaceRegions.Count][];

                for (int i = 0; i < _leftFaceRegions.Count; i++)
                {
                    rightImagePoints[i] = new PointF[4];

                    rightImagePoints[i][0] = new PointF(_rightFaceRegions[i].Mouth.Location.X, _rightFaceRegions[i].Mouth.Location.Y);
                    rightImagePoints[i][1] = new PointF(_rightFaceRegions[i].LeftEye.Location.X, _rightFaceRegions[i].LeftEye.Location.Y);
                    rightImagePoints[i][2] = new PointF(_rightFaceRegions[i].RightEye.Location.X, _rightFaceRegions[i].RightEye.Location.Y);
                    rightImagePoints[i][3] = new PointF(_rightFaceRegions[i].Face.Location.X, _rightFaceRegions[i].Face.Location.Y);
                }

                var intrinsicCameraParameters1 = new IntrinsicCameraParameters();
                var intrinsicCameraParameters2 = new IntrinsicCameraParameters();

                ExtrinsicCameraParameters extrinsicCameraParameters;
                Matrix<double> foundamentalMatrix;
                Matrix<double> essentialMatrix;

                CameraCalibration.StereoCalibrate(objectPoints, leftImagePoints, rightImagePoints,
                                                  intrinsicCameraParameters1, intrinsicCameraParameters2,
                                                  new Size(firstLeft.SourceImage.Width, firstLeft.SourceImage.Height),
                                                  CALIB_TYPE.DEFAULT,
                                                  new MCvTermCriteria(), out extrinsicCameraParameters, out foundamentalMatrix, out essentialMatrix);

                Options.StereoCalibrationOptions = new StereoCalibrationOptions
                                                   {
                                                       EssentialMatrix = essentialMatrix,
                                                       ExtrinsicCameraParameters = extrinsicCameraParameters,
                                                       FoundamentalMatrix = foundamentalMatrix
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

        private void OnButtonStartCaptureClick(object sender, EventArgs e)
        {
            _leftFaceRegions.Clear();
            _rightFaceRegions.Clear();

            IsRecording = true;

            _main.FacesAvailable += OnMainFacesAvailable;
        }

        private void OnButtonEndCaptureClick(object sender, EventArgs e)
        {
            if (_leftFaceRegions.Count < 3)
            {
                return;
            }

            _main.FacesAvailable -= OnMainFacesAvailable;

            IsRecording = false;
        }

        private void OnMainFacesAvailable(object sender, FaceRegionsEventArgs e)
        {
            FaceRegion2D leftRegion;
            FaceRegion2D rightRegion;

            if (IsRecording == false
                || e.Left == null
                || e.Right == null
                || e.Left.Count() != 1
                || e.Right.Count() != 1
                || (leftRegion = e.Left.First()).LeftEye == null
                || (rightRegion = e.Right.First()).LeftEye == null
                || leftRegion.RightEye == null
                || rightRegion.RightEye == null
                || leftRegion.Mouth == null
                || rightRegion.Mouth == null)
            {
                return;
            }

            _leftFaceRegions.Add(leftRegion);
            _rightFaceRegions.Add(rightRegion);

            Invoke(new Action(delegate { labelAvailableImages.Text = _leftFaceRegions.Count.ToString(CultureInfo.InvariantCulture); }));
        }

        private void OnFormCalibrateCamerasLoad(object sender, EventArgs e)
        {
            _leftFaceRegions = new List<FaceRegion2D>();
            _rightFaceRegions = new List<FaceRegion2D>();
        }

        private void OnButtonCloseClick(object sender, EventArgs e)
        {
            Close();
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