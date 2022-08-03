using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;
using OpenTK;
namespace CricketVR
{
    [Combinator]
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]

    public class DrawFakeVRWorld
    {
        /// <summary>
        /// Sets the size (in pixels) of the to-be-drawn canvas.
        /// </summary>
        private int canvasSize = 1000;

        [Description("Sets the size (in pixels) of the to-be-drawn canvas.")]
        public int CanvasSize
        {
            get { return canvasSize; }
            set { canvasSize = value; }
        }

        /// <summary>
        /// Sets the size (in VR units) of the VR box square.
        /// </summary>
        private float vRWorldSize = 0.5f;

        [Description("Sets the size (in VR units) of the VR box square.")]
        public float VRWorldSize
        {
            get { return vRWorldSize; }
            set { vRWorldSize = value; }
        }

        /// <summary>
        /// Sets the size (in VR units) of the monitor bounding box square.
        /// </summary>
        private float monitorBoundingBoxSize = 0.3f;

        [Description("Sets the size (in VR units) of the monitor bounding box square.")]
        public float MonitorBoundingBoxSize
        {
            get { return monitorBoundingBoxSize; }
            set { monitorBoundingBoxSize = value; }
        }

        /// <summary>
        /// Sets the radius (in VR units) of arm's dead zone.
        /// </summary>
        private float armDeadZoneRadius = 0.15f;

        [Description("Sets the radius (in VR units) of arm's dead zone.")]
        public float ArmDeadZoneRadius
        {
            get { return armDeadZoneRadius; }
            set { armDeadZoneRadius = value; }
        }

        /// <summary>
        /// Sets the position (in VR units) of the virtual subject.
        /// </summary>
        private Vector3 vRRodentPosition = new Vector3(0);
        [TypeConverter(typeof(NumericRecordConverter))]
        [Description(" Sets the position (in VR units) of the virtual subject.")]
        public Vector3 VRRodentPosition
        {
            get { return vRRodentPosition; }
            set { vRRodentPosition = value; }
        }

        /// <summary>
        /// Sets the angle of the virtual subject around the Y axis of rotation.
        /// </summary>
        private float vRRodentAngle = 0.0f;
        [Description("Sets the angle of the virtual subject around the Y axis of rotation.")]
        public float VRRodentAngle
        {
            get { return vRRodentAngle; }
            set { vRRodentAngle = value; }
        }

        /// <summary>
        /// Sets the position (in VR units) of the virtual cricket.
        /// </summary>
        private Vector3 vRCricketPosition = new Vector3(0);

        [TypeConverter(typeof(NumericRecordConverter))]
        [Description(" Sets the position (in VR units) of the virtual cricket.")]
        public Vector3 VRCricketPosition
        {
            get { return vRCricketPosition; }
            set { vRCricketPosition = value; }
        }

        /// <summary>
        /// Sets the position (in VR units) of the real cricket.
        /// </summary>
        private Point2f realCricketPosition = new Point2f(0, 0);

        [Description(" Sets the position (in VR units) of the real cricket.")]
        public Point2f RealCricketPosition
        {
            get { return realCricketPosition; }
            set { realCricketPosition = value; }
        }

        /// Virtual Cricket visualization properties
        private Scalar virtualCricketColor = Scalar.Rgb(255, 0, 0);
        private int virtualCricketSize = 10;

        /// Real Cricket visualization properties
        private Scalar realCricketColor = Scalar.Rgb(0, 255, 0);
        private int realCricketSize = 10;

        /// Mouse visualization properties
        private Scalar mouseColor = Scalar.Rgb(120, 120, 255);
        private int mouseSize = 25;

        public IObservable<IplImage> Process<TSource>(IObservable<TSource> source)
        {


            return source.Select(value =>
            {
                var ImSize = new Size(canvasSize, canvasSize);
                var inputImage = new IplImage(ImSize, IplDepth.U8, 3);
                // Create the canvas
                var im_center = new Point(canvasSize / 2, canvasSize / 2);

                //Calculate meter to pixel conversion
                float ScalingFactor = (float)canvasSize / (vRWorldSize * 2);

                //Convert everything to pixel units

                var scaledVRRodentPosition = new Point(
                                            (int) (vRRodentPosition.X * ScalingFactor),
                                            (int) (vRRodentPosition.Z * ScalingFactor)
                                            );
                var scaledVRCricketPosition = new Point(
                                            (int)(vRCricketPosition.X * ScalingFactor),
                                            (int)(vRCricketPosition.Z * ScalingFactor)
                                            );

                var scaledRealCricketPosition = new Point(realCricketPosition*ScalingFactor);

                var scaledArenaSize = (int)(ScalingFactor * monitorBoundingBoxSize);
                var scaledDeadZoneSize = (int)(ScalingFactor * armDeadZoneRadius);

                var mouse_ref = scaledVRRodentPosition + im_center; //Add this to make the frame of ref shift
                //Create Image
                //Set the background
                inputImage.Set(Scalar.Rgb(0, 0, 0));
                //(Monitor Bounding box)
                CV.Rectangle(inputImage,
                    mouse_ref + new Point(scaledArenaSize, scaledArenaSize),
                    mouse_ref - new Point(scaledArenaSize, scaledArenaSize),
                    Scalar.Rgb(200, 200, 200),
                    -1); //Should be relative to mouse VR position

                //Dead Zone
                CV.Circle(inputImage,
                    mouse_ref,
                    scaledDeadZoneSize,
                    Scalar.Rgb(50, 50, 50), -1); //Should be relative to mouse VR position

                //Draw the virtual cricket
                CV.Circle(inputImage,
                    scaledVRCricketPosition + im_center,
                    virtualCricketSize,
                    virtualCricketColor, -1); //Should be absolute

                //Draw the real cricket
                CV.Circle(inputImage,
                    scaledRealCricketPosition + mouse_ref,
                    realCricketSize,
                    realCricketColor, -1); //Should be relative to mouse VR position

               //Draw the virtual mouse with orientation
                var Pt1 = new Point(
                    (int) (mouseSize * Math.Cos(vRRodentAngle)),
                    (int) (mouseSize * Math.Sin(vRRodentAngle))) + mouse_ref;
                var Pt2 = new Point(
                    (int) (0.5 * mouseSize * Math.Cos(vRRodentAngle + (2.0/3.0)*Math.PI)),
                    (int) (0.5 * mouseSize * Math.Sin(vRRodentAngle + (2.0/3.0)*Math.PI))) + mouse_ref;
                var Pt3 = new Point(
                    (int) (0.5 * mouseSize * Math.Cos(vRRodentAngle - (2.0/3.0)*Math.PI)),
                    (int) (0.5 * mouseSize * Math.Sin(vRRodentAngle - (2.0/3.0)*Math.PI))) + mouse_ref;
                Point[] Triangle = new Point[]{Pt1, Pt2, Pt3};

                CV.FillPoly(inputImage, new Point[][] {Triangle}, mouseColor);

                CV.Flip(inputImage,inputImage, FlipMode.Vertical);
                return inputImage;
            });
        }
    }
}
