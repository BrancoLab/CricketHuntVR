using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace CricketVR
{
    [Combinator]
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    public class ClampPositionUpdate
    {

        private float numericCorrectionFactor = 0.99f;
        [Description("Correction factor to adjust the numeric bounds. [0:1]")]
        public float NumericCorrectionFactor {
            get
            {
                return numericCorrectionFactor;
            }
            set
            {
                numericCorrectionFactor = value;
         }
        }

        public IObservable<VrElement> Process(IObservable<Tuple<VrElement, VrElement, Tuple<float, float, float, float, float, float>>> source)
        {
            return source.Select(value => {

                var Accumulation = value.Item1;
                var Update = value.Item2;
                var ClampedUpdate = Update;
                var bounds = value.Item3;

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

        public static float ClampPosition(float In, float max, float min){
            if (In < min) return min;
            else if(In > max) return max;
            else return In;
            }
    }
}