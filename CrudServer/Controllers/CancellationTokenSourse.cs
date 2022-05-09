using System;
using System.Threading;

namespace CrudServer.Controllers
{
    public class CancellationTokenSourse
    {
        public CancellationToken Token { get; internal set; }

        internal void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}