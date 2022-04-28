using Bonsai;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using OpenTK;

namespace CricketVR
{
    [Source]
    [Description("Creates a VR Element from a Tuple of two Vector3")]
    [WorkflowElementCategory(ElementCategory.Source)]
    public class CreateVrElement : Source<VrElement>
    {

        /// <summary>
        /// Sets a 3D vector with the VR position of the element
        /// </summary>
        [TypeConverter(typeof(NumericRecordConverter))]
        [Description("Specifies a 3D vector with the VR position of the element.")]
        public Vector3 Position {get;set;}

        /// <summary>
        /// Sets a 3D vector with the VR Orientation of the element
        /// </summary>
        [TypeConverter(typeof(NumericRecordConverter))]
        [Description("Specifies a 3D vector with the VR orientation of the element.")]
        public Vector3 Orientation{get;set;}

        /// <summary>
        ///Initializes a VR_Element with the UI set values
        /// </summary>
        public override IObservable<VrElement> Generate()
        {
            return Observable.Defer(() => Observable.Return(
                new VrElement(Position, Orientation)
            ));
        }

        /// <summary>
        /// Initializes a VR_Element with the UI set values once an event is received
        /// </summary>
        public IObservable<VrElement> Generate<TSource>(IObservable<TSource> source)
        {
            return source.Select(input =>{
                return new VrElement(Position, Orientation);
            });
        }


        /// <summary>
        /// Initializes a VR_Element from a Tuple of a pair of Vector3 [Position, Orientation]
        /// </summary>
        public IObservable<VrElement> Generate(IObservable<Tuple<Vector3, Vector3>> source)
        {
            return source.Select(input =>{
                return new VrElement(input.Item1, input.Item2);
            });
        }

    }
}
