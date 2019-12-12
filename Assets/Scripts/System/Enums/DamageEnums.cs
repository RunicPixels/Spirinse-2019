using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spirinse.System.Enums
{
    public class DamageEnums
    {
        [Flags]
        public enum DamageType {
            None = 0,
            BasicAttack = 1 << 0,
            Dash = 1 << 1,
            Grab = 1 << 2,
            Everything = -1
        }
    }
}
