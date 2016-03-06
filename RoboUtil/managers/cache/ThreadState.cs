using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboUtil.managers.cache
{
    public class ThreadState
    {
        public int IncrementValue;
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;

        public ThreadState(int incrementValue, bool isTimerCanceled)
        {
            IncrementValue = incrementValue;
            TimerCanceled = isTimerCanceled;
        }
    }
}
