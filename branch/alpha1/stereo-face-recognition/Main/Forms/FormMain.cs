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

        public event EventHandler<FaceRegionsEventArgs> FacesAvailable;

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

            _random = new Random();
            _cameras = new List<Camera>(2)
                       {
                           new Camera(),
                           new Camera()
                       };

            _rotateBackground = new Bgr(Color.Black);

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
                                      _cameras[0].Image = _capture1.QueryFrame();
                                      _cameras[1].Image = _capture2.QueryFrame();

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
                                          Image<Bgr, byte> image = _cameras[0].Image.ConcateHorizontal(_cameras[1].Image);

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

                                          if (_faces != null)
                                          {
                                              Image<Gray, byte> firstFace = _faces.First();

                                              _faces.Skip(1).ToList().ForEach(face => firstFace = firstFace.ConcateHorizontal(face));

                                              imageBoxFaces.Image = firstFace;
                                          }
                                          else
                                          {
                                              imageBoxFaces.Image = null;
                                          }

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

                double distance;

                if (_cameras[0].FaceRegions == null
                    || _cameras[1].FaceRegions == null)
                {
                    _resetCorrelation = true;
                }

                if (_resetCorrelation == false)
                {
                    #region Find correlation with previous face regions

                    // iterate through all cameras and track faces
                    foreach (Camera camera in _cameras)
                    {
                        var newRawFaceRegions = new List<FaceRegion2D>();

                        // iterate through every face found previously, rotate image and find faces
                        foreach (FaceRegion2D faceRegion in camera.FaceRegions)
                        {
                            Image<Bgr, byte> image = camera.Image.Rotate(faceRegion.EyeAngle,
                                                                         new PointF(faceRegion.Face.Location.X + faceRegion.Face.Width/2, faceRegion.Face.Location.Y + faceRegion.Face.Height/2),
                                                                         INTER.CV_INTER_CUBIC, _rotateBackground, true);

                            if (_debugRotation)
                            {
                                camera.Image = image;
                            }

                            // find faces in rotated image
                            newRawFaceRegions.AddRange(Helper2D.GetFaceRegion2Ds(image, FaceWidth, FaceHeight, true, false));
                        }

                        // find best corespondence between old faces and new faces
                        IEnumerable<Tuple<int, int>> corespondences = Helper.FindCorespondence
                            (camera.FaceRegions.Select(item => item.Face.Location).ToArray(),
                             newRawFaceRegions.Select(item => item.Face.Location).ToArray(),
                             out distance);

                        if (corespondences == null
                            || corespondences.Any() == false)
                        {
                            // face regions lost .. RESET both cameras
                            _resetCorrelation = true;
                            break;
                        }

                        var newFaceRegions = new FaceRegion2D[corespondences.Count()];

                        for (int i = 0; i < corespondences.Count(); i++)
                        {
                            FaceRegion2D faceRegion = newRawFaceRegions.ElementAt(corespondences.ElementAt(i).Item2);

                            faceRegion.SetHistory(camera.FaceRegions.ElementAt(corespondences.ElementAt(i).Item1));

                            newFaceRegions[i] = faceRegion;
                        }

                        camera.FaceRegions = newFaceRegions;
                    }

                    #endregion
                }

                if (_resetCorrelation)
                {
                    #region Reset Found Faces

                    foreach (Camera camera in _cameras)
                    {
                        camera.FaceRegions = Helper2D.GetFaceRegion2Ds(camera.Image, FaceWidth, FaceHeight, true, false);
                    }

                    #endregion
                }

                if (_cameras[0].FaceRegions.Length > 0
                    && _cameras[1].FaceRegions.Length > 0)
                {
                    #region Find correlation in stereo images and add history

                    IEnumerable<Point>[] points = _cameras.Select(camera => camera.FaceRegions.
                                                                                Select(region => region.Face.Location)).ToArray();

                    List<Tuple<int, int>> correlations = Helper.FindCorespondence(points.ElementAt(0), points.ElementAt(1), out distance).ToList();


                    // images have incorect correlations and history
                    if (_resetCorrelation == false
                        && correlations.Any(item => _cameras[0].FaceRegions.ElementAt(item.Item1).Id != _cameras[1].FaceRegions.ElementAt(item.Item2).Id))
                    {
                        _resetCorrelation = true;
                    }

                    if (_resetCorrelation)
                    {
                        // assign faces color and Id
                        foreach (var correlation in correlations)
                        {
                            var color = new Bgr(_random.NextDouble()*255, _random.NextDouble()*255, _random.NextDouble()*255);

                            FaceRegion2D leftFaceRegion = _cameras[0].FaceRegions.ElementAt(correlation.Item1);
                            FaceRegion2D rightFaceRegion = _cameras[1].FaceRegions.ElementAt(correlation.Item2);

                            rightFaceRegion.Id = leftFaceRegion.Id;

                            leftFaceRegion.BoundingBoxColor = color;
                            rightFaceRegion.BoundingBoxColor = color;
                        }
                    }

                    #endregion

                    #region Recognize Faces

                    _cameras.ForEach(camera =>
                                     {
                                         if (camera.FaceRegions != null)
                                         {
                                             camera.FaceRegions.ToList().ForEach(faceRegion =>
                                                                                 {
                                                                                     Helper.DrawFaceRegionCircle(camera.Image, faceRegion, faceRegion.BoundingBoxColor);

                                                                                     string label = HelperFaces.Recognize(faceRegion.FaceImage);

                                                                                     camera.Image.Draw(string.Format("{0}", label),
                                                                                                       ref _font, faceRegion.Face.Location, new Bgr(0, 0, 255));
                                                                                 });
                                         }
                                     });

                    #endregion

                    EventHandler<FaceRegionsEventArgs> facesAvailableHandler = FacesAvailable;
                    if (facesAvailableHandler != null)
                    {
                        facesAvailableHandler(this, new FaceRegionsEventArgs(_cameras[0].FaceRegions, _cameras[1].FaceRegions, null));
                    }

                    _faces = _cameras.SelectMany(camera => camera.FaceRegions).Select(item => item.FaceImage).ToArray();
                }

                _resetCorrelation = false;

                PostProcess();

                lock (this)
                {
                    _processQueueLength = 0;
                }
            }
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

            Helper2D.FilterInstances = new FilterInstances(useGpuMenuItem.Checked ? Mode.Gpu : Mode.Cpu);
        }

        #endregion
    }
}