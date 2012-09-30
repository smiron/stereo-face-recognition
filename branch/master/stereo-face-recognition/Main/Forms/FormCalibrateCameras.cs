using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaceDetection.Misc;
using ImageFilters.Common;
using Emgu.CV.Structure;
using Emgu.CV;

namespace FaceDetection.Forms
{
    public partial class FormCalibrateCameras : Form
    {
        #region Fields

        private FormMain _main;
        private List<FaceRegion2D> _leftFaceRegions;
        private List<FaceRegion2D> _rightFaceRegions;

        private bool _isRecording;
        private bool _isCalibrating;

        #endregion

        #region Properties

        private bool IsRecording
        {
            get
            {
                return _isRecording;
            }
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

        private bool IsCalibrating
        {
            get
            {
                return _isCalibrating;
            }
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

                var firstLeft = _leftFaceRegions.First();

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

                var intrinsicCameraParameters1 = new Emgu.CV.IntrinsicCameraParameters();
                var intrinsicCameraParameters2 = new Emgu.CV.IntrinsicCameraParameters();

                ExtrinsicCameraParameters extrinsicCameraParameters;
                Matrix<double> foundamentalMatrix;
                Matrix<double> essentialMatrix;

                
                //Emgu.CV.CvInvoke.cvStereoCalibrate()

                //Emgu.CV.Matrix<
                
                Emgu.CV.CameraCalibration.

                Emgu.CV.CameraCalibration.StereoCalibrate(objectPoints, leftImagePoints, rightImagePoints,
                    intrinsicCameraParameters1, intrinsicCameraParameters2,
                    new Size(firstLeft.SourceImage.Width, firstLeft.SourceImage.Height),
                    Emgu.CV.CvEnum.CALIB_TYPE.DEFAULT,
                    new MCvTermCriteria(), out extrinsicCameraParameters, out foundamentalMatrix, out essentialMatrix);

                

                Options.StereoCalibrationOptions = new StereoCalibrationOptions()
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

        private void Calibrate(IEnumerable<Image<Gray, byte>> images, int nx, int ny, int useUncalibrated, float _squareSize /* Chessboard square size in cm */)
        {

            int displayCorners = 1;
    int showUndistorted = 1;
    bool isVerticalStereo = false;//OpenCV can handle left-right
                                      //or up-down camera arrangements
    const int maxScale = 1;
    
    //FILE* f = fopen(imageList, "rt");
    int i, j, lr, nframes, n = nx*ny, N = 0;

   List<MCvPoint3D32f> objectPoints;
            
   var points = new List<MCvPoint2D64f>[2];

   List<int> npoints;
   List<char>[] active = new List<char>[2];

   var temp = new PointF[n];
            
    var imageSize = new System.Drawing.Size(0,0);

    // ARRAY AND VECTOR STORAGE:
    Matrix<double> _M1 = new Matrix<double>(3, 3);
    Matrix<double> _M2 = new Matrix<double>(3, 3);
    Matrix<double> _D1 = new Matrix<double>(1, 5);
    Matrix<double> _D2 = new Matrix<double>(1, 5);
    Matrix<double> _R =  new Matrix<double>(3, 3);
    Matrix<double> _T =  new Matrix<double>(3, 1);
    Matrix<double> _E =  new Matrix<double>(3, 3);
    Matrix<double> _F =  new Matrix<double>(3, 3);
    Matrix<double> _Q =  new Matrix<double>(4, 4);

    double [,] M1 = _M1.Data;
    double [,] M2 = _M2.Data;
    double [,] D1 = _D1.Data;
    double [,] D2 = _D2.Data;

    double [,] R = _R.Data;
    double [,] T = _T.Data;
    double [,] E = _E.Data;
    double [,] F = _F.Data;

    double [,] Q = _Q.Data;

    for(i=0;i<images.Count();i++)
    {
        int count = 0, result=0;

        lr = i % 2;
        
        List<MCvPoint2D64f> pts = points[lr];

        var img = images.ElementAt(i);
        
        imageSize = img.Size;
        
        //FIND CHESSBOARDS AND CORNERS THEREIN:
        for( int s = 1; s <= maxScale; s++ )
        {
            var timg = img;

            if( s > 1 )
            {
                timg=new Image<Gray,byte>(img.Width * s, img.Height * s);

                img.Resize(timg.Width, timg.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }

            

            temp = Emgu.CV.CameraCalibration.FindChessboardCorners(timg, new Size(nx, ny),
                Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.NORMALIZE_IMAGE);

            Emgu.CV.CameraCalibration.

            result = temp.Count();

            
            if( timg != img )
                timg.Dispose();

            if( result == 0 || s == maxScale)
                for( j = 0; j < count; j++ )
                {
                    temp[j].X /= s;
                    temp[j].Y /= s;
                }
            if( result )
                break;
        }
        if( displayCorners )
        {
            printf("%s\n", buf);
            IplImage* cimg = cvCreateImage( imageSize, 8, 3 );
            cvCvtColor( img, cimg, CV_GRAY2BGR );
            cvDrawChessboardCorners( cimg, cvSize(nx, ny), &temp[0],
                count, result );
            cvShowImage( "corners", cimg );
            cvReleaseImage( &cimg );
            if( cvWaitKey(0) == 27 ) //Allow ESC to quit
                exit(-1);
        }
        else
            putchar('.');
        N = pts.size();
        pts.resize(N + n, cvPoint2D32f(0,0));
        active[lr].push_back((uchar)result);
    //assert( result != 0 );
        if( result )
        {
         //Calibration will suffer without subpixel interpolation
            cvFindCornerSubPix( img, &temp[0], count,
                cvSize(11, 11), cvSize(-1,-1),
                cvTermCriteria(CV_TERMCRIT_ITER+CV_TERMCRIT_EPS,
                30, 0.01) );
            copy( temp.begin(), temp.end(), pts.begin() + N );
        }
        cvReleaseImage( &img );
    }
    fclose(f);
    printf("\n");
// HARVEST CHESSBOARD 3D OBJECT POINT LIST:
    nframes = active[0].size();//Number of good chessboads found
    objectPoints.resize(nframes*n);
    for( i = 0; i < ny; i++ )
        for( j = 0; j < nx; j++ )
        objectPoints[i*nx + j] = cvPoint3D32f(i*squareSize, j*squareSize, 0);
    for( i = 1; i < nframes; i++ )
        copy( objectPoints.begin(), objectPoints.begin() + n,
        objectPoints.begin() + i*n );
    npoints.resize(nframes,n);
    N = nframes*n;
    CvMat _objectPoints = cvMat(1, N, CV_32FC3, &objectPoints[0] );
    CvMat _imagePoints1 = cvMat(1, N, CV_32FC2, &points[0][0] );
    CvMat _imagePoints2 = cvMat(1, N, CV_32FC2, &points[1][0] );
    CvMat _npoints = cvMat(1, npoints.size(), CV_32S, &npoints[0] );
    cvSetIdentity(&_M1);
    cvSetIdentity(&_M2);
    cvZero(&_D1);
    cvZero(&_D2);

// CALIBRATE THE STEREO CAMERAS
    printf("Running stereo calibration ...");
    fflush(stdout);
    cvStereoCalibrate( &_objectPoints, &_imagePoints1,
        &_imagePoints2, &_npoints,
        &_M1, &_D1, &_M2, &_D2,
        imageSize, &_R, &_T, &_E, &_F,
        cvTermCriteria(CV_TERMCRIT_ITER+
        CV_TERMCRIT_EPS, 100, 1e-5),
        CV_CALIB_FIX_ASPECT_RATIO +
        CV_CALIB_ZERO_TANGENT_DIST +
        CV_CALIB_SAME_FOCAL_LENGTH );
    printf(" done\n");
// CALIBRATION QUALITY CHECK
// because the output fundamental matrix implicitly
// includes all the output information,
// we can check the quality of calibration using the
// epipolar geometry constraint: m2^t*F*m1=0
    vector<CvPoint3D32f> lines[2];
    points[0].resize(N);
    points[1].resize(N);
    _imagePoints1 = cvMat(1, N, CV_32FC2, &points[0][0] );
    _imagePoints2 = cvMat(1, N, CV_32FC2, &points[1][0] );
    lines[0].resize(N);
    lines[1].resize(N);
    CvMat _L1 = cvMat(1, N, CV_32FC3, &lines[0][0]);
    CvMat _L2 = cvMat(1, N, CV_32FC3, &lines[1][0]);
//Always work in undistorted space
    cvUndistortPoints( &_imagePoints1, &_imagePoints1,
        &_M1, &_D1, 0, &_M1 );
    cvUndistortPoints( &_imagePoints2, &_imagePoints2,
        &_M2, &_D2, 0, &_M2 );
    cvComputeCorrespondEpilines( &_imagePoints1, 1, &_F, &_L1 );
    cvComputeCorrespondEpilines( &_imagePoints2, 2, &_F, &_L2 );
    double avgErr = 0;
    for( i = 0; i < N; i++ )
    {
        double err = fabs(points[0][i].x*lines[1][i].x +
            points[0][i].y*lines[1][i].y + lines[1][i].z)
            + fabs(points[1][i].x*lines[0][i].x +
            points[1][i].y*lines[0][i].y + lines[0][i].z);
        avgErr += err;
    }
    printf( "avg err = %g\n", avgErr/(nframes*n) );
//COMPUTE AND DISPLAY RECTIFICATION
    if( showUndistorted )
    {
        CvMat* mx1 = cvCreateMat( imageSize.height,
            imageSize.width, CV_32F );
        CvMat* my1 = cvCreateMat( imageSize.height,
            imageSize.width, CV_32F );
        CvMat* mx2 = cvCreateMat( imageSize.height,

            imageSize.width, CV_32F );
        CvMat* my2 = cvCreateMat( imageSize.height,
            imageSize.width, CV_32F );
        CvMat* img1r = cvCreateMat( imageSize.height,
            imageSize.width, CV_8U );
        CvMat* img2r = cvCreateMat( imageSize.height,
            imageSize.width, CV_8U );
        CvMat* disp = cvCreateMat( imageSize.height,
            imageSize.width, CV_16S );
        CvMat* vdisp = cvCreateMat( imageSize.height,
            imageSize.width, CV_8U );
        CvMat* pair;
        double R1[3][3], R2[3][3], P1[3][4], P2[3][4];
        CvMat _R1 = cvMat(3, 3, CV_64F, R1);
        CvMat _R2 = cvMat(3, 3, CV_64F, R2);
// IF BY CALIBRATED (BOUGUET'S METHOD)
        if( useUncalibrated == 0 )
        {
            CvMat _P1 = cvMat(3, 4, CV_64F, P1);
            CvMat _P2 = cvMat(3, 4, CV_64F, P2);
            cvStereoRectify( &_M1, &_M2, &_D1, &_D2, imageSize,
                &_R, &_T,
                &_R1, &_R2, &_P1, &_P2, &_Q,
                0/*CV_CALIB_ZERO_DISPARITY*/ );
            isVerticalStereo = fabs(P2[1][3]) > fabs(P2[0][3]);
    //Precompute maps for cvRemap()
            cvInitUndistortRectifyMap(&_M1,&_D1,&_R1,&_P1,mx1,my1);
            cvInitUndistortRectifyMap(&_M2,&_D2,&_R2,&_P2,mx2,my2);
            
    //Save parameters
            cvSave("M1.xml",&_M1);
            cvSave("D1.xml",&_D1);
            cvSave("R1.xml",&_R1);
            cvSave("P1.xml",&_P1);
            cvSave("M2.xml",&_M2);
            cvSave("D2.xml",&_D2);
            cvSave("R2.xml",&_R2);
            cvSave("P2.xml",&_P2);
            cvSave("Q.xml",&_Q);
            cvSave("mx1.xml",mx1);
            cvSave("my1.xml",my1);
            cvSave("mx2.xml",mx2);
            cvSave("my2.xml",my2);

        }
//OR ELSE HARTLEY'S METHOD
        else if( useUncalibrated == 1 || useUncalibrated == 2 )
     // use intrinsic parameters of each camera, but
     // compute the rectification transformation directly
     // from the fundamental matrix
        {
            double H1[3][3], H2[3][3], iM[3][3];
            CvMat _H1 = cvMat(3, 3, CV_64F, H1);
            CvMat _H2 = cvMat(3, 3, CV_64F, H2);
            CvMat _iM = cvMat(3, 3, CV_64F, iM);
    //Just to show you could have independently used F
            if( useUncalibrated == 2 )
                cvFindFundamentalMat( &_imagePoints1,
                &_imagePoints2, &_F);
            cvStereoRectifyUncalibrated( &_imagePoints1,
                &_imagePoints2, &_F,
                imageSize,
                &_H1, &_H2, 3);
            cvInvert(&_M1, &_iM);
            cvMatMul(&_H1, &_M1, &_R1);
            cvMatMul(&_iM, &_R1, &_R1);
            cvInvert(&_M2, &_iM);
            cvMatMul(&_H2, &_M2, &_R2);
            cvMatMul(&_iM, &_R2, &_R2);
    //Precompute map for cvRemap()
            cvInitUndistortRectifyMap(&_M1,&_D1,&_R1,&_M1,mx1,my1);

            cvInitUndistortRectifyMap(&_M2,&_D1,&_R2,&_M2,mx2,my2);
        }
        else
            assert(0);
        cvNamedWindow( "rectified", 1 );
// RECTIFY THE IMAGES AND FIND DISPARITY MAPS
        if( !isVerticalStereo )
            pair = cvCreateMat( imageSize.height, imageSize.width*2,
            CV_8UC3 );
        else
            pair = cvCreateMat( imageSize.height*2, imageSize.width,
            CV_8UC3 );
//Setup for finding stereo corrrespondences
        CvStereoBMState *BMState = cvCreateStereoBMState();
        assert(BMState != 0);
        BMState->preFilterSize=41;
        BMState->preFilterCap=31;
        BMState->SADWindowSize=41;
        BMState->minDisparity=-64;
        BMState->numberOfDisparities=128;
        BMState->textureThreshold=10;
        BMState->uniquenessRatio=15;
        for( i = 0; i < nframes; i++ )
        {
            IplImage* img1=cvLoadImage(imageNames[0][i].c_str(),0);
            IplImage* img2=cvLoadImage(imageNames[1][i].c_str(),0);
            if( img1 && img2 )
            {
                CvMat part;
                cvRemap( img1, img1r, mx1, my1 );
                cvRemap( img2, img2r, mx2, my2 );
                if( !isVerticalStereo || useUncalibrated != 0 )
                {
              // When the stereo camera is oriented vertically,
              // useUncalibrated==0 does not transpose the
              // image, so the epipolar lines in the rectified
              // images are vertical. Stereo correspondence
              // function does not support such a case.
                    cvFindStereoCorrespondenceBM( img1r, img2r, disp,
                        BMState);
                    cvNormalize( disp, vdisp, 0, 256, CV_MINMAX );
                    cvNamedWindow( "disparity" );
                    cvShowImage( "disparity", vdisp );
                }
                if( !isVerticalStereo )
                {
                    cvGetCols( pair, &part, 0, imageSize.width );
                    cvCvtColor( img1r, &part, CV_GRAY2BGR );
                    cvGetCols( pair, &part, imageSize.width,
                        imageSize.width*2 );
                    cvCvtColor( img2r, &part, CV_GRAY2BGR );
                    for( j = 0; j < imageSize.height; j += 16 )
                        cvLine( pair, cvPoint(0,j),
                        cvPoint(imageSize.width*2,j),
                        CV_RGB(0,255,0));
                }
                else
                {
                    cvGetRows( pair, &part, 0, imageSize.height );
                    cvCvtColor( img1r, &part, CV_GRAY2BGR );
                    cvGetRows( pair, &part, imageSize.height,
                        imageSize.height*2 );
                    cvCvtColor( img2r, &part, CV_GRAY2BGR );
                    for( j = 0; j < imageSize.width; j += 16 )
                        cvLine( pair, cvPoint(j,0),
                        cvPoint(j,imageSize.height*2),
                        CV_RGB(0,255,0));
                }
                cvShowImage( "rectified", pair );
                if( cvWaitKey() == 27 )
                    break;
            }
            cvReleaseImage( &img1 );
            cvReleaseImage( &img2 );
        }
        cvReleaseStereoBMState(&BMState);
        cvReleaseMat( &mx1 );
        cvReleaseMat( &my1 );
        cvReleaseMat( &mx2 );
        cvReleaseMat( &my2 );
        cvReleaseMat( &img1r );
        cvReleaseMat( &img2r );
        cvReleaseMat( &disp );
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
            FaceRegion2D leftRegion = null;
            FaceRegion2D rightRegion = null;

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

            this.Invoke(new Action(delegate()
                {
                    labelAvailableImages.Text = _leftFaceRegions.Count.ToString();
                }));
        }

        private void OnFormCalibrateCamerasLoad(object sender, EventArgs e)
        {
            _leftFaceRegions = new List<FaceRegion2D>();
            _rightFaceRegions = new List<FaceRegion2D>();
        }

        private void OnButtonCloseClick(object sender, EventArgs e)
        {
            this.Close();
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
