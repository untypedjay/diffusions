using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace Diffusions
{
    public abstract class ImageGenerator : IImageGenerator
    {
        public bool Finished { get; protected set; } = false;

        private Thread thread;
        private bool isCancelled = false;

        public void Start(Area area)
        {
            Finished = false;
            isCancelled = false;
            thread = new Thread(new ParameterizedThreadStart(Run));
            thread.Start(area);
        }

        public void Stop()
        {
            if (!Finished)
            {
                isCancelled = true;
            }
        }

        protected abstract void UpdateMatrix(Area area);

        public event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;
        protected void OnImageGenerated(Area area, Bitmap bitmap, TimeSpan timespan)
        {
            var handler = ImageGenerated;
            if (handler != null) handler(this, new EventArgs<Tuple<Area, Bitmap, TimeSpan>>(new Tuple<Area, Bitmap, TimeSpan>(area, bitmap, timespan)));
        }

        private void Run(object a)
        {
            Area area = (Area)a;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < Settings.Default.MaxIterations; i++)
            {
                if (!isCancelled)
                {
                    UpdateMatrix(area);

                    if (i % Settings.Default.DisplayInterval == 0)
                    {
                        OnImageGenerated(area, ColorSchema.GenerateBitmap(area), sw.Elapsed);
                    }
                }
            }
            sw.Stop();
            Finished = true;
            OnImageGenerated(area, ColorSchema.GenerateBitmap(area), sw.Elapsed);
        }
    }
}
