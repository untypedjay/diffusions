using System;
using System.Diagnostics;
using System.Drawing;

namespace Diffusions {
  public abstract class ImageGenerator : IImageGenerator {
    public bool Finished { get; protected set; } = false;

    public void Start(Area area) {
      Finished = false;

      Stopwatch sw = new Stopwatch();
      sw.Start();
      for (int i = 0; i < Settings.Default.MaxIterations; i++) {
        UpdateMatrix(area);

        if (i % Settings.Default.DisplayInterval == 0) {
          OnImageGenerated(area, ColorSchema.GenerateBitmap(area), sw.Elapsed);
        }
      }
      sw.Stop();
      Finished = true;
      OnImageGenerated(area, ColorSchema.GenerateBitmap(area), sw.Elapsed);
    }

    public void Stop() {
      // TODO
    }

    protected abstract void UpdateMatrix(Area area);

    public event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;
    protected void OnImageGenerated(Area area, Bitmap bitmap, TimeSpan timespan) {
      var handler = ImageGenerated;
      if (handler != null) handler(this, new EventArgs<Tuple<Area, Bitmap, TimeSpan>>(new Tuple<Area, Bitmap, TimeSpan>(area, bitmap, timespan)));
    }

  }
}
