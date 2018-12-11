using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IpcServiceFrameworkTest
{
    class ComputingService : IComputingService
    {
        public float AddFloat(float x, float y)
        {
            return x + y;
        }

        public Stream GetMemoryStream()
        {
            return new MemoryStream(Enumerable.Range(0, 1000).Select(x => (byte)x).ToArray());
        }
    }
}
