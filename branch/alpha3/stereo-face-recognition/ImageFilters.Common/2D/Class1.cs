using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System;
namespace WindowsFormsApplication1
{
    public partial class frmCamShift
    {

        private Capture cap;
        private HaarCascade haar;

        private Image faceS;

        private static RangeF range = new RangeF(0, 180);
        private DenseHistogram hist = new DenseHistogram(16, range);

        private Image hue = null;
        private Image mask = null;
        private Image backproject = null;
        private Image hsv = null;

        private IntPtr[] img = null;
        private Rectangle trackwin;
        private MCvConnectedComp trackcomp = new MCvConnectedComp();
        private MCvBox2D trackbox = new MCvBox2D();

        private bool isTrack = false;

        private void frmCamShift_Load(object sender, EventArgs e)
        {
            cap = new Capture(0);
            //haar = new HaarCascade("haarcascade_frontalface_alt.xml");
            haar = new HaarCascade("haarcascade.xml");
            cap.FlipHorizontal = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Image nextframe = cap.QueryFrame())
            {
                if (nextframe != null)
                {
                    if (isTrack == false)
                    {
                        Image grayframe = nextframe.Convert();
                        grayframe._EqualizeHist();

                        var faces = grayframe.DetectHaarCascade(haar, 1.4, 4, HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT | HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(40, 40))[0];

                        hsv = new Image(grayframe.Width, grayframe.Height);
                        hsv = nextframe.Convert();
                        hsv._EqualizeHist();

                        hue = new Image(grayframe.Width, grayframe.Height);
                        mask = new Image(grayframe.Width, grayframe.Height);
                        backproject = new Image(grayframe.Width, grayframe.Height);

                        Emgu.CV.CvInvoke.cvInRangeS(hsv, new MCvScalar(0, 30, Math.Min(10, 255), 0), new MCvScalar(180, 256, Math.Max(10, 255), 0), mask);
                        Emgu.CV.CvInvoke.cvSplit(hsv, hue, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                        picHue.Image = hue.ToBitmap();


                        foreach (var face in faces)
                        {

                            // Rectangle roi = new Rectangle(face.rect.X + face.rect.Width / 4, face.rect.Y + face.rect.Height / 4, face.rect.Width / 2, face.rect.Height / 2);
                            // Rectangle roi = new Rectangle(face.rect.X, face.rect.Y, face.rect.Width / 2, face.rect.Height / 2);

                            Emgu.CV.CvInvoke.cvSetImageROI(hue, face.rect);
                            Emgu.CV.CvInvoke.cvSetImageROI(mask, face.rect);

                            nextframe.Draw(face.rect, new Bgr(0, double.MaxValue, 1), 2);
                            picMask.Image = mask.ToBitmap();
                            trackwin = face.rect;

                        }
                        img = new IntPtr[1]
{
hue
};

                        Emgu.CV.CvInvoke.cvCalcHist(img, hist, false, mask);

                        Emgu.CV.CvInvoke.cvResetImageROI(hue);
                        Emgu.CV.CvInvoke.cvResetImageROI(mask);

                        CapImg.Image = nextframe.ToBitmap();
                        isTrack = true;
                        // isTrack = true;
                    }
                    else
                    {
                        if (trackwin != null)
                        {
                            hsv = nextframe.Convert();
                            Emgu.CV.CvInvoke.cvInRangeS(hsv, new MCvScalar(0, 30, 10, 0), new MCvScalar(180, 256, 256, 0), mask);
                            Emgu.CV.CvInvoke.cvSplit(hsv, hue, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                            picMask.Image = mask.ToBitmap();
                            picHue.Image = hue.ToBitmap();

                        }

                        img = new IntPtr[1]
{
hue
};

                        Emgu.CV.CvInvoke.cvCalcBackProject(img, backproject, hist);
                        Emgu.CV.CvInvoke.cvAnd(backproject, mask, backproject, IntPtr.Zero);

                        Image grayframe = nextframe.Convert();
                        grayframe._EqualizeHist();


                        var faces = grayframe.DetectHaarCascade(haar, 1.4, 4, HAAR_DETECTION_TYPE.FIND_BIGGEST_OBJECT | HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(40, 40))[0];
                        foreach (var face in faces)
                        {
                            nextframe.Draw(face.rect, new Bgr(Color.Black), 2);
                        }


                        if (trackwin.Width == 0) trackwin.Width = 40;
                        if (trackwin.Height == 0) trackwin.Height = 40;

                        Emgu.CV.CvInvoke.cvCamShift(backproject, trackwin, new MCvTermCriteria(10, 0.1), out trackcomp, out trackbox);
                        trackwin = trackcomp.rect;

                        // CvInvoke.cvEllipseBox(nextframe, trackbox, new MCvScalar(0, 255, 0), 2, LINE_TYPE.CV_AA, 0);


                        nextframe.Draw(trackwin, new Bgr(Color.Blue), 3);
                        CapImg.Image = nextframe.ToBitmap();
                        faceS = nextframe.Copy(trackwin);
                        picFace.Image = faceS.ToBitmap();

                    }
                }


            }
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //isTrack = false;
            //trackwin.Width = 0;
            //trackwin.Height = 0;
            //hue.Dispose();
            //mask.Dispose();
            //hsv.Dispose();
            //backproject.Dispose();
            //img = null;
            //timer1.Enabled = true; 
        }
    }
}