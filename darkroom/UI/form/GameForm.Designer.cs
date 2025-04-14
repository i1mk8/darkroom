using System.Drawing.Drawing2D;

namespace darkroom.UI.form;

sealed partial class GameForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        
        PaintMap(e.Graphics);

        var fov = _game.MainPlayer.Fov.GetFov();
        if (fov.Vertices.Count >= 3)
        {
            PaintFov(e.Graphics, fov);
            PaintPlayers(e.Graphics, fov);
        }
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
        SuspendLayout();
        // 
        // GameForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 800);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
        MaximizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Darkroom";
        ResumeLayout(false);
    }

    #endregion
}