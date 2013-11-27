using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCAD.Kernel.Maths
{
    public static class MathsExtensions
    {
        public static bool NearlyEquals(this double x, double y, double epsilon = 0.00000001)
        {
            return MathsHelper.NearlyEquals(x, y, epsilon);
        }
    }
}
