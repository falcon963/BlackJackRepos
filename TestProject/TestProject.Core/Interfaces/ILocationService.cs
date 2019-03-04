using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Interfaces
{
    public interface ILocationService
    {
        Boolean TryGetLatestLocation(out Double lat, out Double lng);
        void Start();
        void Stop();
    }
}
