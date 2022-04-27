using OpenTK;

namespace CricketVR
{
    public struct VrElement
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
    }
}