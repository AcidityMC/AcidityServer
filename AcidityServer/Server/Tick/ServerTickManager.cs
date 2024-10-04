using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcidityServer.Server.Tick
{
    public class ServerTickManager
    {
        private void tick()
        {

        }

        public void startTickLoop()
        {
            new Thread(tick).Start();
        }
    }
}
