using System;

namespace BanklinksDotNet
{
    public class TimeProvider
    {
        public virtual DateTime Now { get { return DateTime.Now; } }
    }
}
