using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RibbonDemo
{
    public partial class BlackForm : MainForm
    {
        public BlackForm()
        {
            InitializeComponent();
        }

        private void BlackForm_Load(object sender, EventArgs e)
        {
            (ribbon1.Renderer as RibbonProfessionalRenderer).ColorTable = new RibbonProfesionalRendererColorTableBlack();
        }
    }
}