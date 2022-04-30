using OpenTK;

namespace CricketVR
{
    public class VrElement
    {
        public Vector3 Position;
        public Vector3 Orientation;

        public VrElement(Vector3 position, Vector3 orientation)
        {
            Position = position;
            Orientation = orientation;
        }

        public override string ToString()
        {
            return "Position: " + Position + "  " + "Orientation: " + Orientation;
        }


        public static VrElement operator +(VrElement el1, VrElement el2)
        {
            Vector3 FinalPos = el1.Position + el2.Position;
            Vector3 FinalOrientation = el1.Orientation + el2.Orientation;
            return new VrElement(FinalPos, FinalOrientation);

        }

        public static VrElement operator -(VrElement el1, VrElement el2)
        {
            Vector3 FinalPos = el1.Position - el2.Position;
            Vector3 FinalOrientation = el1.Orientation - el2.Orientation;
            return new VrElement(FinalPos, FinalOrientation);

        }

        public static VrElement operator *(VrElement el1, float gain)
        {
            Vector3 FinalPos = el1.Position * gain;

            Vector3 FinalOrientation = el1.Orientation * gain;

            return new VrElement(FinalPos, FinalOrientation);

        }

        public static VrElement operator *(VrElement el1, VrElement el2)
        {
            Vector3 FinalPos = el1.Position * el2.Position;

            Vector3 FinalOrientation = el1.Orientation * el2.Orientation;

            return new VrElement(FinalPos, FinalOrientation);

        }
    }
}