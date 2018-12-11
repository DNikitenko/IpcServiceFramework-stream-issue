using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IpcServiceFrameworkTest
{
    public interface IComputingService
    {
        float AddFloat(float x, float y);

        Stream GetMemoryStream();
    }
}
