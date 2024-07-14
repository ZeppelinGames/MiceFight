using Editor.Controls;
using System;
using System.Windows.Forms;

namespace Editor
{
    public partial class Form1 : Form
    {
        public static Form1 instance;

        public MainControl mainControl => _mainControl;
        private MainControl _mainControl;

        public Form1() {
            InitializeComponent();

            instance = this;

            _mainControl = new MainControl() {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
            };
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.Controls.Add(_mainControl);
            _mainControl?.UpdateWindow();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e) {
            _mainControl?.UpdateWindow();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            _mainControl?.UpdateWindow();
        }
    }
}
