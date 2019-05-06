using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Services.Interfaces
{
    public interface ILocationService
    {
        bool TryGetLatestLocation(out double lat, out double lng);

        void Start();

        void Stop();
    }
}
