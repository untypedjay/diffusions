using System;
using System.Drawing;

namespace Diffusions {
  public interface IImageGenerator {
    void Start(Area area);

    event EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>> ImageGenerated;

    void Stop();

    bool Finished { get; }
  }
}
