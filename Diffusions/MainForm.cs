using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Diffusions {
  public partial class MainForm : Form {
    private Area currentArea;
    private IImageGenerator generator;
    private bool running;

    public MainForm() {
      InitializeComponent();

      //generator = new SequentialImageGenerator();
      generator = new ParallelImageGenerator();
      generator.ImageGenerated += generator_ImageGenerated;
    }

    private void InitArea() {
      currentArea = new Area(pictureBox.Width, pictureBox.Height);

      for (int i = 0; i < currentArea.Width; i++) {
        for (int j = 0; j < currentArea.Height; j++) {
          currentArea.Matrix[i, j] = 0;
        }
      }
      Reheat(currentArea, 5, 5, 100, 150);
      Reheat(currentArea, 100, 100, 80, 400);
    }

    private void Reheat(Area area, int x, int y, int size, double val) {
      for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
          area.Matrix[(x + i) % area.Width, (y + j) % area.Height] = val;
        }
      }
    }

    private void UpdateImage(Area area) {
      toolStripStatusLabel.Text = "Calculating ...";
      generator.Start(area);
    }

    private void generator_ImageGenerated(object sender, EventArgs<Tuple<Area, Bitmap, TimeSpan>> e) {
      if (InvokeRequired)
        Invoke(new EventHandler<EventArgs<Tuple<Area, Bitmap, TimeSpan>>>(generator_ImageGenerated), sender, e);
      else {
        currentArea = e.Value.Item1;
        pictureBox.Image = e.Value.Item2;
        if (generator.Finished) {
          running = false;
          startButton.Text = "Start";
          toolStripStatusLabel.Text = "Done (Runtime: " + e.Value.Item3 + ")";
        } else {
          running = true;
          startButton.Text = "Stop";
          toolStripStatusLabel.Text = "Calculating ... (Runtime: " + e.Value.Item3 + ")";
        }
      }
    }

    #region Menu events
    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
      if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
        string filename = saveFileDialog.FileName;

        ImageFormat format = null;
        if (filename.EndsWith("jpg")) format = ImageFormat.Jpeg;
        else if (filename.EndsWith("gif")) format = ImageFormat.Gif;
        else if (filename.EndsWith("png")) format = ImageFormat.Png;
        else format = ImageFormat.Bmp;

        pictureBox.Image.Save(filename, format);
      }
    }
    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      Application.Exit();
    }
    private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
      using (SettingsDialog dialog = new SettingsDialog()) {
        dialog.ShowDialog();
      }
    }
    #endregion

    #region Mouse events
    private void pictureBox_MouseUp(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        Reheat(currentArea, e.X, e.Y, Settings.Default.TipSize, Settings.Default.DefaultHeat);
      }
    }
    #endregion

    private void startButton_Click(object sender, EventArgs e) {
      if (running) {
        generator.Stop();
      } else {
        InitArea();
        UpdateImage(currentArea);
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
      generator.ImageGenerated -= generator_ImageGenerated;
      if (running)
        Invoke(new Action(generator.Stop));
    }
  }
}
