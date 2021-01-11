using System.Windows.Forms;

namespace Diffusions {
  public partial class SettingsDialog : Form {
    public SettingsDialog() {
      InitializeComponent();

      propertyGrid.SelectedObject = Settings.Default;
    }
  }
}