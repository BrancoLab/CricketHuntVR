using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CricketVR;
using OpenTK;

namespace CricketVR
{

    [Combinator]
    [Description("")]
    [WorkflowElementCategory(ElementCategory.Transform)]

    public class RotateVec3
    {
        private Vector3 angleVector;

        [TypeConverter(typeof(NumericRecordConverter))]
        public Vector3 AngleVector
        {
            get { return angleVector; }
            set { angleVector = value; }
        }

        private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public IObservable<Vector3> Process(IObservable<Vector3> source)
        {
            return source.Select(value => {
                var outVec = value * Matrix3.CreateFromAxisAngle(AngleVector, Angle);
                return outVec;
            });
        }
    }
}