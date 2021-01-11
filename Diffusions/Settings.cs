using System.ComponentModel;

namespace Diffusions {
  [DefaultPropertyAttribute("MaxIterations")]
  public class Settings {
    public static Settings Default { get; } = new Settings();

    #region Generator Settings
    private int maxIterations;
    [CategoryAttribute("Generator Settings"),
     DescriptionAttribute("Maximum number of iterations")]
    public int MaxIterations {
      get { return maxIterations; }
      set { if (value > 0) maxIterations = value; }
    }

    private int displayInterval;
    [CategoryAttribute("Generator Settings"),
     DescriptionAttribute("Display image every nth iteration")]
    public int DisplayInterval {
      get { return displayInterval; }
      set { if (value > 0) displayInterval = value; }
    }

    private int tipSize;
    [CategoryAttribute("Generator Settings"),
     DescriptionAttribute("Tip size for reheating")]
    public int TipSize {
      get { return tipSize; }
      set { if (value > 0) tipSize = value; }
    }

    private double defaultHeat;
    [CategoryAttribute("Generator Settings"),
     DescriptionAttribute("Default heat for reheating")]
    public double DefaultHeat {
      get { return defaultHeat; }
      set { if (value > 0) defaultHeat = value; }
    }
    #endregion

    public Settings() {
      maxIterations = 1000;
      displayInterval = 10;
      tipSize = 50;
      defaultHeat = 400.0;
    }
  }
}
