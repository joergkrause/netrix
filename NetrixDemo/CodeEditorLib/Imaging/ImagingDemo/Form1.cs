using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Comzept.Library.Drawing;

namespace ImagingDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CurrentImage = null;
        }


        private Comzept.Library.Drawing.FreeImage currentImage;

        public Comzept.Library.Drawing.FreeImage CurrentImage
        {
            get 
            { 
                return currentImage; 
            }
            set 
            { 
                currentImage = value;
                if (value == null)
                {
                    toolStripMenuItem4.Enabled = false;
                    toolStripMenuItem5.Enabled = false;
                    toolStripMenuItem6.Enabled = false;
                }
                else
                {
                    toolStripMenuItem4.Enabled = true;
                    toolStripMenuItem5.Enabled = true;
                    toolStripMenuItem6.Enabled = true;
                }
            }
        }

        private void PaintImage()
        {
            Bitmap b = new Bitmap(currentImage.Width, currentImage.Height); 
            currentImage.PaintToBitmap(b, 0, 0, b.Width, b.Height, 0, 0);
            pictureBoxImage.Image = b;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            { 
                CurrentImage = new Comzept.Library.Drawing.FreeImage(openFileDialog1.FileName);                
                PaintImage();
            }           
        }



        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CurrentImage = CurrentImage.Rotate(90.0);
            PaintImage();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CurrentImage = CurrentImage.Rotate(180.0);
            PaintImage();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            CurrentImage = CurrentImage.Rotate(270.0);
            PaintImage();
        }        

    }
}