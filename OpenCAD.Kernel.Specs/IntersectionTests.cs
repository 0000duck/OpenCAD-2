using Machine.Specifications;
using OpenCAD.Kernel.Geometry;
using OpenCAD.Kernel.Maths;
using OpenCAD.Kernel.Intersection;
using developwithpassion.specifications.fakeiteasy;

namespace OpenCAD.Kernel.Specs
{

    [Subject(typeof(Point))]
    public class with_Point : Observes<Point>
    {

        Establish c = () => depends.on(new Vect3(0, 0, 0));
    }

    public class intersecting_point : with_Point
    {
        It should_be_on_point = () => sut.On(new Point(new Vect3(0, 0, 0))).ShouldBeTrue();
        It should_be_not_on_point = () => sut.On(new Point(new Vect3(1, 1, 1))).ShouldBeFalse();
    }






}
