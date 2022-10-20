using System;

namespace Project1
{
    public class Time
    {
        public static float deltaTime;

        private DateTime time1 = DateTime.Now;
        private DateTime time2 = DateTime.Now;

        public void Update()
        {
            time2 = DateTime.Now;
            deltaTime = (time2.Ticks - time1.Ticks) / 10000000f;
            time1 = time2;
        }

    }
}
