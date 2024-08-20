using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    public class Util
    {
        public static string floatToTime(float x)
        {
            var t0 = (int)x;
            var m = t0 / 60;
            var s = t0 - m * 60;
            var ms = (int)((x - t0) * 100);
            return $"{m:00}:{s:00}:{ms:00}";
        }
    }
}
