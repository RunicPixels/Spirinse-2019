using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spirinse.Interfaces
{
    public interface IAbility
    {
        void Play();
        bool Run();
        void Stop();
    }
}