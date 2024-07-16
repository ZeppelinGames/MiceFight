using Editor.Controls;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Editor {
    public partial class MainForm : Form {
        public static MainForm instance;

        public MainControl mainControl => _mainControl;
        private MainControl _mainControl;

        public MainForm() {
            InitializeComponent();

            Cursor.Hide();

            instance = this;

            _mainControl = new MainControl() {
                Margin = new Padding(0),
                Padding = new Padding(0),
                MaximumSize = new Size(2000, 2000),
            };

            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.FormClosing += Form1_Closing;
            this.DoubleBuffered = true;

        }

        public void CloseGame() {
            _mainControl.DeregisterHook();
            this.Close();
        }

        private void Form1_Closing(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {
            this.Controls.Add(_mainControl);
           
            this.Controls.Add(new Panel() {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(61, 54, 69)
            });
            _mainControl?.UpdateWindow();

            Debug.WriteLine("Main Control initialised");
        }

        private void Form1_ResizeEnd(object sender, EventArgs e) {
            _mainControl?.UpdateWindow();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            int mainSize = Math.Min(this.Width, this.Height);
            int hMainSize = mainSize / 2;
            _mainControl.Location = new Point(this.Width / 2 - hMainSize, this.Height / 2 - hMainSize);
            _mainControl.Size = new Size(mainSize, mainSize);
            _mainControl?.UpdateWindow();
        }
    }
}
