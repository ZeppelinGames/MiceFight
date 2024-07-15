using Editor.Controls;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Editor
{
    public partial class MainForm : Form
    {
        public static MainForm instance;

        public MainControl mainControl => _mainControl;
        private MainControl _mainControl;

        public MainForm() {
            InitializeComponent();

            Cursor.Hide();

            instance = this;

            _mainControl = new MainControl() {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
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
            _mainControl?.UpdateWindow();

            Debug.WriteLine("Main Control initialised");
        }

        private void Form1_ResizeEnd(object sender, EventArgs e) {
            _mainControl?.UpdateWindow();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            _mainControl?.UpdateWindow();
        }
    }
}
