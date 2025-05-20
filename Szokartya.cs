using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzotanitoJatek
{
    class Szokartya : Label
    {
        public int ParID { get; set; }
        private Point offset;
        private bool dragging = false;

        public Szokartya(string szoveg, int parID)
        {
            this.Text = szoveg;
            this.ParID = parID;
            this.AutoSize = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.LightYellow;

            this.MouseDown += Szokartya_MouseDown;
            this.MouseMove += Szokartya_MouseMove;
            this.MouseUp += Szokartya_MouseUp;
        }

        private void Szokartya_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                offset = e.Location;
                dragging = true;
            }
        }

        private void Szokartya_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                this.Left += e.X - offset.X;
                this.Top += e.Y - offset.Y;
            }
        }

        private void Szokartya_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }

}
