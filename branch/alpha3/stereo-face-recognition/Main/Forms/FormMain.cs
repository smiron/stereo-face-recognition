using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using FaceDetection.Misc;
using ImageFilters.Common;
using ImageFilters.Common.Misc;
using Main.Common.Misc;

namespace FaceDetection.Forms
{
    public partial class FormMain : Form
    {
        #region Fields

        private const int SleepInterval = 10;

        private const int FaceWidth = 80;
        private const int FaceHeight = 80;

        private List<Camera> _cameras;
        private Capture _capture1;
        private Capture _capture2;
        private bool _captureContinue = true;
        private Thread _captureThread;
        private bool _debugCorespondence;
        private bool _debugRotation;
        private IEnumerable<Image<Gray, byte>> _faces;
        private MCvFont _font = new MCvFont(FONT.CV_FONT_HERSHEY_DUPLEX, 1, 1);
        private int _processQueueLength;
        private Thread _processThread;

        private Random _random;

        private bool _resetCorrelation;

        private Bgr _rotateBackground;

        private Image<Gray, byte> _transformed;
        private double _leftRoll;
        private double _rightRoll;

        private Emgu.CV.GPU.GpuStereoBM _stereoSolver;
        

        private Image<Gray, byte> _imagePointsLeft;
        private Image<Gray, byte> _imagePointsRight;
        private Image<Gray, byte> _imagePointsDisparity;

        public event EventHandler<FaceRegionsEventArgs> FacesAvailable;
        public event EventHandler<RawImagesEventArgs> RawImagesAvailable;

        private Face3D _foundFace3d;

        #endregion

        #region Methods

        private void OnFormMainLoad(object sender, EventArgs e)
        {
            try
            {
                _capture1 = new Capture(0);
                _capture2 = new Capture(1);

                OnResolutionMenuItemClick(resolutionToolStripMenuItem.DropDownItems[0], EventArgs.Empty);
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            var SADWindowSize = 3;

            _stereoSolver = new GpuStereoBM(GpuStereoBM.PresetType.BasicPreset, 64, SADWindowSize);


            _random = new Random();
            _cameras = new List<Camera>(2)
                       {
                           new Camera(),
                           new Camera()
                       };

            _rotateBackground = new Bgr(Color.Black);

            Options.StereoCalibrationOptions = new StereoCalibrationOptions();
            Options.StereoCalibrationOptions.Load("Calibration720p.sc");

            _captureThread = new Thread(CaptureFrames) {IsBackground = false};
            _captureThread.Start();

            _processThread = new Thread(ProcessFrame) {IsBackground = false};
            _processThread.Start();
        }

        private void OnButtonSaveClick(object sender, EventArgs e)
        {
            string packagePath;

            using (var sfd = new SaveFileDialog())
            {
                sfd.AddExtension = true;
                sfd.AutoUpgradeEnabled = true;
                sfd.CheckPathExists = true;
                sfd.DefaultExt = ".fr";
                sfd.Filter = "Face Recognition (*.fr)|*fr";

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                packagePath = sfd.FileName;
            }

            HelperFaces.Save(packagePath);
        }

        private void OnButtonLoadClick(object sender, EventArgs e)
        {
            string packagePath;

            using (var ofd = new OpenFileDialog())
            {
                ofd.AddExtension = true;
                ofd.AutoUpgradeEnabled = true;
                ofd.CheckPathExists = true;
                ofd.DefaultExt = ".fr";
                ofd.Filter = "Face Recognition (*.fr)|*fr";

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                packagePath = ofd.FileName;
            }

            HelperFaces.Load(packagePath);
        }

        private void CaptureFrames()
        {
            while (_captureContinue)
            {
                while (_processQueueLength == 2)
                {
                    Thread.Sleep(SleepInterval);
                }

                Invoke(new Action(delegate
                                  {
                                      _cameras[0].Image = _capture1.QueryGrayFrame();
                                      _cameras[1].Image = _capture2.QueryGrayFrame();

                                      _processQueueLength = 2;
                                  }));

                while (_processQueueLength != 0)
                {
                    Thread.Sleep(SleepInterval);
                }

                if (IsDisposed == false)
                {
                    Invoke(new Action(delegate
                                      {
                                          Image<Bgr, byte> image = _cameras[0].Image.ConcateHorizontal(_cameras[1].Image).Convert<Bgr, byte>();

                                          if (_debugCorespondence)
                                          {
                                              (from left in _cameras[0].FaceRegions
                                               join right in _cameras[1].FaceRegions on left.Id equals right.Id
                                               select new {Left = left, Right = right}).ToList().
                                                  ForEach(pair => image.Draw(new LineSegment2D(new Point(pair.Left.Face.Location.X + pair.Left.Face.Width/2, pair.Left.Face.Location.Y + pair.Left.Face.Height/2),
                                                                                               new Point(pair.Right.Face.Location.X + _cameras[0].Image.Width + pair.Right.Face.Width/2, pair.Right.Face.Location.Y + pair.Right.Face.Height/2)),
                                                                             pair.Left.BoundingBoxColor, 2));
                                          }

                                          imageBoxCamera.Image = image;

                                          if (_foundFace3d != null)
                                          {
                                              decimal roll = (decimal)Helper2D.CalculateFaceRoll(_foundFace3d);
                                              decimal yaw = (decimal)Helper2D.CalculateFaceYaw(_foundFace3d);
                                              decimal pitch = (decimal)Helper2D.CalculateFacePitch(_foundFace3d);
                                              nudFaceRoll.Value = roll == 0 ? nudFaceRoll.Value : roll;
                                              nudFaceYaw.Value = yaw == 0 ? nudFaceYaw.Value : yaw;
                                              nudFacePitch.Value = pitch == 0 ? nudFacePitch.Value : pitch;
                                          }

                                          nudLeftRoll.Value = (decimal)_leftRoll;
                                          nudRightRoll.Value = (decimal)_rightRoll;

                                          if (_imagePointsDisparity != null)
                                          {
                                              imageBoxPoints.Image = _imagePointsDisparity;
                                          }

                                          //imageBoxTransformed.Image = _transformed;

                                          //if (_faces != null)
                                          //{
                                          //    Image<Gray, byte> firstFace = _faces.First();

                                          //    _faces.Skip(1).ToList().ForEach(face => firstFace = firstFace.ConcateHorizontal(face));

                                          //    imageBoxFaces.Image = firstFace;
                                          //}
                                          //else
                                          //{
                                          //    imageBoxFaces.Image = null;
                                          //}

                                          Application.DoEvents();
                                      }));
                }
            }
        }

        private void ProcessFrame()
        {
            while (_captureContinue)
            {
                while (_processQueueLength != 2)
                {
                    Thread.Sleep(SleepInterval);
                }

                if ((Helper2D.FilterInstances.Mode == Mode.Cpu && useGpuMenuItem.Checked)
                    || (Helper2D.FilterInstances.Mode == Mode.Gpu && useGpuMenuItem.Checked == false))
                {
                    Helper2D.FilterInstances = new FilterInstances(useGpuMenuItem.Checked ? Mode.Gpu : Mode.Cpu);
                }

                var rawImagesAvailableHandler = RawImagesAvailable;
                if (rawImagesAvailableHandler != null)
                {
                    rawImagesAvailableHandler(this, new RawImagesEventArgs(_cameras[0].Image, _cameras[1].Image));
                    Application.DoEvents();
                }

                if (Options.ProcessFaces)
                {
                    ProcessFrameFindFaces();
                }

                PostProcess();

                lock (this)
                {
                    _processQueueLength = 0;
                }
            }
        }

        

        private void ProcessFrameFindFaces()
        {
            var stereoCalibration = Options.StereoCalibrationOptions;

            if (stereoCalibration == null)
            {
                return;
            }

            var leftImageR = new Image<Gray, byte>(_cameras[0].Image.Width, _cameras[0].Image.Height);
            var rightImageR = new Image<Gray, byte>(_cameras[1].Image.Width, _cameras[1].Image.Height);

            CvInvoke.cvRemap(_cameras[0].Image.Ptr, leftImageR.Ptr,
                stereoCalibration.MapXLeft, stereoCalibration.MapYLeft, 0, new MCvScalar(0));

            CvInvoke.cvRemap(_cameras[1].Image.Ptr, rightImageR.Ptr,
                stereoCalibration.MapXRight, stereoCalibration.MapYRight, 0, new MCvScalar(0));

            // find first face points
            var leftFaceRegions = Helper2D.GetFaceRegion2Ds(leftImageR, FaceWidth, FaceHeight, true, true);
            var rightFaceRegions = Helper2D.GetFaceRegion2Ds(rightImageR, FaceWidth, FaceHeight, true, true);

            FaceRegion2D leftFace;
            FaceRegion2D rightFace;

            if (leftFaceRegions != null
                && rightFaceRegions != null
                && (leftFace = leftFaceRegions.FirstOrDefault()) != null
                && (rightFace = rightFaceRegions.FirstOrDefault()) != null)
            {

                if (leftFace.EyeAngle != 0)
                {
                    _leftRoll = leftFace.EyeAngle;
                }
                if (rightFace.EyeAngle != 0)
                {
                    _rightRoll = rightFace.EyeAngle;
                }

                var leftPoints = new Point[4]; // face location, left eye, right eye, mouth
                var rightPoints = new Point[4];

                #region Points

                // face
                leftPoints[0] = new Point(leftFace.Face.Location.X + leftFace.Face.Width / 2, leftFace.Face.Location.Y + leftFace.Face.Height / 2);
                rightPoints[0] = new Point(rightFace.Face.Location.X + rightFace.Face.Width / 2, rightFace.Face.Location.Y + rightFace.Face.Height / 2);

                // left eye
                if (leftFace.LeftEye != null && rightFace.LeftEye != null)
                {
                    leftPoints[1] = new Point(leftFace.Face.Location.X + leftFace.LeftEye.Location.X + leftFace.LeftEye.Width / 2,
                        leftFace.Face.Location.Y + leftFace.LeftEye.Location.Y + leftFace.LeftEye.Height / 2);

                    rightPoints[1] = new Point(rightFace.Face.Location.X + rightFace.LeftEye.Location.X + rightFace.LeftEye.Width / 2,
                        rightFace.Face.Location.Y + rightFace.LeftEye.Location.Y + rightFace.LeftEye.Height / 2);
                }

                // right eye
                if (leftFace.RightEye != null && rightFace.RightEye != null)
                {
                    leftPoints[2] = new Point(leftFace.Face.Location.X + leftFace.RightEye.Location.X + leftFace.RightEye.Width / 2,
                        leftFace.Face.Location.Y + leftFace.RightEye.Location.Y + leftFace.RightEye.Height / 2);

                    rightPoints[2] = new Point(rightFace.Face.Location.X + rightFace.RightEye.Location.X + rightFace.RightEye.Width / 2,
                        rightFace.Face.Location.Y + rightFace.RightEye.Location.Y + rightFace.RightEye.Height / 2);
                }

                // mouth
                if (leftFace.Mouth != null && rightFace.Mouth != null)
                {
                    leftPoints[3] = new Point(leftFace.Face.Location.X + leftFace.Mouth.Location.X + leftFace.Mouth.Width / 2,
                        leftFace.Face.Location.Y + leftFace.Mouth.Location.Y + leftFace.Mouth.Height / 2);

                    rightPoints[3] = new Point(rightFace.Face.Location.X + rightFace.Mouth.Location.X + rightFace.Mouth.Width / 2,
                        rightFace.Face.Location.Y + rightFace.Mouth.Location.Y + rightFace.Mouth.Height / 2);
                }

                #endregion

                #region Manual Point Cloud Calculation

                {
                    var pointCloud = new MCvPoint3D64f[leftPoints.Length];

                    #region Calculate Point Cloud

                    for (int i = 0; i < leftPoints.Length; i++)
                    {
                        if (leftPoints[i].X == 0 && leftPoints[i].Y == 0)
                        {
                            continue;
                        }

                        var d = rightPoints[i].X - leftPoints[i].X;

                        var X = leftPoints[i].X * stereoCalibration.Q[0, 0] + stereoCalibration.Q[0, 3];
                        var Y = leftPoints[i].Y * stereoCalibration.Q[1, 1] + stereoCalibration.Q[1, 3];
                        var Z = stereoCalibration.Q[2, 3];
                        var W = d * stereoCalibration.Q[3, 2] + stereoCalibration.Q[3, 3];

                        X = X / W;
                        Y = Y / W;
                        Z = Z / W;

                        leftImageR.Draw(string.Format("{0:0.0} {1:0.0} {2:0.0}", X, Y, Z), ref _font, leftPoints[i], new Gray(255));
                        rightImageR.Draw(string.Format("{0:0.0} {1:0.0} {2:0.0}", X, Y, Z), ref _font, rightPoints[i], new Gray(255));

                        pointCloud[i] = new MCvPoint3D64f(X, Y, Z);
                    }

                    #endregion


                    _foundFace3d = new Face3D()
                    {
                        Location = pointCloud[0].x == 0 && pointCloud[0].y == 0 && pointCloud[0].z == 0 ? (MCvPoint3D64f?)null : pointCloud[0],
                        LeftEye = pointCloud[1].x == 0 && pointCloud[1].y == 0 && pointCloud[1].z == 0 ? (MCvPoint3D64f?)null : pointCloud[1],
                        RightEye = pointCloud[2].x == 0 && pointCloud[2].y == 0 && pointCloud[2].z == 0 ? (MCvPoint3D64f?)null : pointCloud[2],
                        Mouth = pointCloud[3].x == 0 && pointCloud[3].y == 0 && pointCloud[3].z == 0 ? (MCvPoint3D64f?)null : pointCloud[3],
                    };

                    if (_foundFace3d.LeftEye != null
                        && _foundFace3d.RightEye != null
                        && _foundFace3d.Mouth != null)
                    {
                        var srcMatrix = new Matrix<float>(3,4);

                        srcMatrix[0, 0] = (float)_foundFace3d.LeftEye.Value.x;
                        srcMatrix[1, 0] = (float)_foundFace3d.LeftEye.Value.y;
                        srcMatrix[2, 0] = (float)_foundFace3d.LeftEye.Value.z;

                        srcMatrix[0, 1] = (float)_foundFace3d.RightEye.Value.x;
                        srcMatrix[1, 1] = (float)_foundFace3d.RightEye.Value.y;
                        srcMatrix[2, 1] = (float)_foundFace3d.RightEye.Value.z;

                        srcMatrix[0, 2] = (float)_foundFace3d.Mouth.Value.x;
                        srcMatrix[1, 2] = (float)_foundFace3d.Mouth.Value.y;
                        srcMatrix[2, 2] = (float)_foundFace3d.Mouth.Value.z;

                        srcMatrix[0, 3] = (float)_foundFace3d.Location.Value.x;
                        srcMatrix[1, 3] = (float)_foundFace3d.Location.Value.y;
                        srcMatrix[2, 3] = (float)_foundFace3d.Location.Value.z;


                        var dstMatrix = new Matrix<float>(3, 4);

                        dstMatrix[0, 0] = (float)_foundFace3d.LeftEye.Value.x;
                        dstMatrix[1, 0] = (float)_foundFace3d.LeftEye.Value.y;
                        dstMatrix[2, 0] = (float)30;
                        
                        dstMatrix[0, 1] = (float)_foundFace3d.RightEye.Value.x;
                        dstMatrix[1, 1] = (float)_foundFace3d.RightEye.Value.y;
                        dstMatrix[2, 1] = (float)30;
                        
                        dstMatrix[0, 2] = (float)_foundFace3d.Mouth.Value.x;
                        dstMatrix[1, 2] = (float)_foundFace3d.Mouth.Value.y;
                        dstMatrix[2, 2] = (float)30;
                        
                        dstMatrix[0, 3] = (float)_foundFace3d.Location.Value.x;
                        dstMatrix[1, 3] = (float)_foundFace3d.Location.Value.y;
                        dstMatrix[2, 3] = (float)30;

                        HomographyMatrix homographyMatrix = CameraCalibration.FindHomography(srcMatrix, dstMatrix, HOMOGRAPHY_METHOD.DEFAULT, 1);

                        if (homographyMatrix != null)
                        {
                            try
                            {
                                leftImageR = leftImageR.WarpPerspective(homographyMatrix, INTER.CV_INTER_LINEAR, WARP.CV_WARP_DEFAULT, new Gray(0));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        
                    }
                }

                #endregion

                #region Automatic Point Cloud

                {
                    _imagePointsDisparity = new Image<Gray, byte>(_cameras[0].Image.Width, _cameras[0].Image.Height);
                    _imagePointsLeft = new Image<Gray, byte>(_cameras[0].Image.Width, _cameras[0].Image.Height, new Gray(255));
                    _imagePointsRight = new Image<Gray, byte>(_cameras[0].Image.Width, _cameras[0].Image.Height, new Gray(255));

                    for (int i = 0; i < leftPoints.Length; i++)
                    {
                        if (leftPoints[i].X == 0 && leftPoints[i].Y == 0)
                        {
                            continue;
                        }

                        _imagePointsLeft.Draw(new Rectangle(new Point(leftPoints[i].X, leftPoints[i].Y), new Size(10, 10)), new Gray(0), 10);
                        _imagePointsRight.Draw(new Rectangle(new Point(rightPoints[i].X, rightPoints[i].Y), new Size(10, 10)), new Gray(0), 10);
                    }

                    var imagePointsDisparityGpu = new GpuImage<Gray, byte>(_imagePointsDisparity);

                    _stereoSolver.FindStereoCorrespondence(new GpuImage<Gray, byte>(_imagePointsLeft), new GpuImage<Gray, byte>(_imagePointsRight),
                        imagePointsDisparityGpu, null);

                    _imagePointsDisparity = imagePointsDisparityGpu.ToImage();









                    //MCvPoint3D32f[] pointCloud = PointCollection.ReprojectImageTo3D(_imagePointsDisparity, stereoCalibration.Q);

                    //var filteredPointCloud = pointCloud.
                    //    Where(item => item.z != 10000).
                    //    GroupBy(item => item.z).
                    //    Select(item => new
                    //    {
                    //        z = item.Key,
                    //        x = item.Average(point => point.x),
                    //        y = item.Average(point => point.y)
                    //    }).ToArray();

                    //for (int i = 0; i < filteredPointCloud.Length; i++)
                    //{
                    //    _imagePointsDisparity.Draw(string.Format("{0:0.0} {1:0.0} {2:0.0}", filteredPointCloud[i].x, filteredPointCloud[i].y, filteredPointCloud[i].z),
                    //        ref _font, new Point((int)filteredPointCloud[i].x, (int)filteredPointCloud[i].y), new Gray(255));
                    //}
                }

                #endregion
            }

            var oldLeft = _cameras[0].Image;
            var oldRight = _cameras[1].Image;

            _cameras[0].Image = leftImageR;
            _cameras[1].Image = rightImageR;

            oldLeft.Dispose();
            oldRight.Dispose();
        }

        private void PostProcess()
        {
            if (IsDisposed == false)
            {
                try
                {
                    Invoke(new Action(Application.DoEvents));
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                }
            }
        }

        private void OnFormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            _captureContinue = false;
        }

        private void OnResolutionMenuItemClick(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem) sender;

            resolutionToolStripMenuItem.DropDownItems.Cast<ToolStripMenuItem>().ToList().ForEach(item => item.Checked = item == menuItem);

            SetResolution((CaptureResolution) menuItem.Tag);
        }

        private void SetResolution(CaptureResolution resolution)
        {
            Options.Resolution = resolution;

            _capture1.Pause();
            _capture2.Pause();

            Options.StereoCalibrationOptions = null;

            _capture1.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, Options.CaptureWidth);
            _capture1.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, Options.CaptureHeight);

            _capture2.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, Options.CaptureWidth);
            _capture2.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, Options.CaptureHeight);

            _capture1.Start();
            _capture2.Start();
        }

        private void OnResetCorrelationToolStripMenuItemClick(object sender, EventArgs e)
        {
            _resetCorrelation = true;
        }

        private void OnUseGpuMenuItemClick(object sender, EventArgs e)
        {
            useGpuMenuItem.Checked = !useGpuMenuItem.Checked;
        }

        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnFacesToolStripMenuItemClick(object sender, EventArgs e)
        {
            new FormFaces(this).ShowDialog();
        }

        private void OnStereoCalibrateToolStripMenuItemClick(object sender, EventArgs e)
        {
            new FormCalibrateCameras(this).ShowDialog();
        }

        private void OnDebugRotationToolStripMenuItemClick(object sender, EventArgs e)
        {
            _debugRotation = !_debugRotation;
            rotationToolStripMenuItem.Checked = _debugRotation;
        }

        private void OnDebugCorespondenceToolStripMenuItemClick(object sender, EventArgs e)
        {
            _debugCorespondence = !_debugCorespondence;
            corespondenceToolStripMenuItem.Checked = _debugCorespondence;
        }

        private void OnFindFacesToolStripMenuItemClick(object sender, EventArgs e)
        {
            Options.ProcessFaces = !Options.ProcessFaces;
            findFacesToolStripMenuItem.Checked = Options.ProcessFaces;
        }

        #endregion

        #region Instance

        public FormMain()
        {
            InitializeComponent();

            Enum.GetValues(typeof (CaptureResolution)).Cast<CaptureResolution>().ToList().
                ForEach(item =>
                        {
                            var menuItem = new ToolStripMenuItem
                                           {
                                               Tag = item,
                                               Text = item.ToString()
                                           };

                            menuItem.Click += OnResolutionMenuItemClick;

                            resolutionToolStripMenuItem.DropDownItems.Add(menuItem);
                        });

            useGpuMenuItem.Enabled = GpuInvoke.HasCuda;
            useGpuMenuItem.Checked = useGpuMenuItem.Enabled;
            findFacesToolStripMenuItem.Checked = Options.ProcessFaces;

            Helper2D.FilterInstances = new FilterInstances(useGpuMenuItem.Checked ? Mode.Gpu : Mode.Cpu);
        }

        #endregion
    }
}