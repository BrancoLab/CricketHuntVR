using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenTK;
namespace CricketVR
{
    [Combinator]
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ClampPositionUpdate
    {

        private float numericCorrectionFactor = 1f;
        [Description("Correction factor to adjust the numeric bounds. [0:1]")]
        public float NumericCorrectionFactor
        {
            get
            {
                return numericCorrectionFactor;
            }
            set
            {
                numericCorrectionFactor = value;
            }
        }


        private Vector3 offsetBounds = new Vector3(0);
        [TypeConverter(typeof(NumericRecordConverter))]
        [Description("Offsets the rectangular arena")]
        public Vector3 OffsetBounds
        {
            get
            {
                return offsetBounds;
            }
            set
            {
                offsetBounds = value;
            }
        }


        // Full VrElement input
        public IObservable<VrElement> Process(IObservable<Tuple<VrElement, VrElement, Tuple<float, float, float, float, float, float>>> source)
        {
            return source.Select(value =>
            {

                var Accumulation = value.Item1;
                var Update = value.Item2;
                var ClampedUpdate = Update;
                var bounds = ApplyOffset(value.Item3, offsetBounds);

                // Update the position
                Accumulation.Position.X = ClampPosition(
                    Accumulation.Position.X + Update.Position.X,
                    bounds.Item1 * NumericCorrectionFactor,
                    bounds.Item2 * NumericCorrectionFactor);

                Accumulation.Position.Y = ClampPosition(
                    Accumulation.Position.Y + Update.Position.Y,
                    bounds.Item3 * NumericCorrectionFactor,
                    bounds.Item4 * NumericCorrectionFactor);

                Accumulation.Position.Z = ClampPosition(
                    Accumulation.Position.Z + Update.Position.Z,
                    bounds.Item5 * NumericCorrectionFactor,
                    bounds.Item6 * NumericCorrectionFactor);

                return (Accumulation);

            });
        }

        // Full VrElement input
        public IObservable<Vector3> Process(IObservable<Tuple<Vector3, Vector3, Tuple<float, float, float, float, float, float>>> source)
        {
            return source.Select(value =>
            {
                var Accumulation = value.Item1;
                var Update = value.Item2;
                var ClampedUpdate = Update;
                var bounds = ApplyOffset(value.Item3, offsetBounds);

                // Update the position
                Accumulation.X = ClampPosition(
                    Accumulation.X + Update.X,
                    bounds.Item1 * NumericCorrectionFactor,
                    bounds.Item2 * NumericCorrectionFactor);

                Accumulation.Y = ClampPosition(
                    Accumulation.Y + Update.Y,
                    bounds.Item3 * NumericCorrectionFactor,
                    bounds.Item4 * NumericCorrectionFactor);

                Accumulation.Z = ClampPosition(
                    Accumulation.Z + Update.Z,
                    bounds.Item5 * NumericCorrectionFactor,
                    bounds.Item6 * NumericCorrectionFactor);

                return (Accumulation);

            });
        }

        public IObservable<Vector3> Process(IObservable<Tuple<Vector3, Tuple<float, float, float, float, float, float>>> source)
        {
            return source.Select(value =>
            {
                var Accumulation = value.Item1;
                var bounds = ApplyOffset(value.Item2, offsetBounds);

                // Update the position
                Accumulation.X = ClampPosition(
                    Accumulation.X,
                    bounds.Item1 * NumericCorrectionFactor,
                    bounds.Item2 * NumericCorrectionFactor);

                Accumulation.Y = ClampPosition(
                    Accumulation.Y,
                    bounds.Item3 * NumericCorrectionFactor,
                    bounds.Item4 * NumericCorrectionFactor);

                Accumulation.Z = ClampPosition(
                    Accumulation.Z,
                    bounds.Item5 * NumericCorrectionFactor,
                    bounds.Item6 * NumericCorrectionFactor);

                return (Accumulation);

            });
        }

        public static float ClampPosition(float In, float max, float min)
        {
            if (In < min) return min;
            else if (In > max) return max;
            else return In;
        }

        public static Tuple<float, float, float, float, float, float> ApplyOffset(Tuple<float, float, float, float, float, float> bounds, Vector3 offset)
        {

            return Tuple.Create(
                bounds.Item1 + offset.X,
                bounds.Item2 - offset.X ,
                bounds.Item3 + offset.Y,
                bounds.Item4 - offset.Y,
                bounds.Item5 + offset.Z,
                bounds.Item6 - offset.Z
                );
        }
    }
}