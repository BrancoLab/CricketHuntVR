using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenCV.Net;


namespace CricketVR
{
    [Combinator]
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]


    public class DrawArmPosition
    {
        private int ImSize_px = 1000;
        public int CanvasSize
        {
            get { return ImSize_px; }
            set { ImSize_px = value; }
        }
        private Scalar CricketColor = Scalar.Rgb(255, 0, 0);
        private int CricketSize = 10;
        private float FullSize = 0.5f;
        private float ArenaSize = 0.3f;
        private float DeadZoneSize = 0.15f;


        public IObservable<IplImage> Process(IObservable<Point2f> source)
        {
            return source.Select(value =>
            {
                Size ImSize = new Size(ImSize_px, ImSize_px);
                var im_center = new Point(ImSize_px / 2, ImSize_px / 2);
                //Calculate meter to pixel conversion
                float ScalingFactor = (float)ImSize_px / (FullSize * 2);
                //Geometry size in pixels
                var scaledFullSize = (int)(ScalingFactor * FullSize);
                var scaledArenaSize = (int)(ScalingFactor * ArenaSize);
                var scaledDeadZoneSize = (int)(ScalingFactor * DeadZoneSize);

                //Create Image
                var inputImage = new IplImage(ImSize, IplDepth.U8, 3);
                //Set the background
                inputImage.Set(Scalar.Rgb(0, 0, 0));
                CV.Circle(inputImage, im_center, scaledFullSize, Scalar.Rgb(50, 50, 50), -1);
                CV.Circle(inputImage, im_center, scaledArenaSize, Scalar.Rgb(200, 200, 200), -1);
                CV.Circle(inputImage, im_center, scaledDeadZoneSize, Scalar.Rgb(50, 50, 50), -1);
                // Calculate and draw cricket position
                Point cricketPosition = new Point((int)(value.X * -ScalingFactor), (int)(value.Y * -ScalingFactor));
                CV.Circle(inputImage, im_center + cricketPosition, CricketSize, CricketColor, -1);

                return inputImage;
            });
        }

    }
}