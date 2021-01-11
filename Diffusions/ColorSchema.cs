using System.Collections.Generic;
using System.Drawing;

namespace Diffusions {
  public static class ColorSchema {
    private const int shades = 100;

    public static IList<Color> Colors { get; private set; }

    static ColorSchema() {
      var colors = new List<Color>();
      int stepWidth = (256 * 4) / shades;
      int currentValue;
      int currentClass;

      for (int i = 0; i < shades; i++) {
        currentValue = (i * stepWidth) % 256;
        currentClass = (i * stepWidth) / 256;
        switch (currentClass) {
          case 0: { colors.Add(Color.FromArgb(0, currentValue, 255)); break; }        // blue -> cyan
          case 1: { colors.Add(Color.FromArgb(0, 255, 255 - currentValue)); break; }  // cyan -> green
          case 2: { colors.Add(Color.FromArgb(currentValue, 255, 0)); break; }        // green -> yellow
          case 3: { colors.Add(Color.FromArgb(255, 255 - currentValue, 0)); break; }  // yellow -> red
        }
      }

      Colors = colors.AsReadOnly();
    }

    public static Bitmap GenerateBitmap(Area area) {
      int maxColorIndex = Colors.Count;
      Bitmap bitmap = new Bitmap(area.Width, area.Height);

      for (int i = 0; i < area.Width; i++) {
        for (int j = 0; j < area.Height; j++) {
          int idx = (int)area.Matrix[i, j] % maxColorIndex;
          bitmap.SetPixel(i, j, Colors[idx]);
        }
      }
      return bitmap;
    }
  }
}
