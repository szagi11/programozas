using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SzotanitoJatek
{
    public partial class Form1 : Form
    {
        private List<(string magyar, string idegen)> szoparok = new();

        public Form1()
        {
            InitializeComponent();
            this.Text = "Szótanító játék";
            this.Width = 800;
            this.Height = 600;
            this.AllowDrop = true;

            Button betoltoGomb = new Button() { Text = "Szótár betöltése", Location = new Point(250, 250) };
            betoltoGomb.Height = 100;
            betoltoGomb.Width = 200;
            betoltoGomb.Click += (s, e) =>
            {
                BetoltSzotar();
                KirakSzavakat();
                betoltoGomb.Visible = false;
            };
            this.Controls.Add(betoltoGomb);
        }

        private void BetoltSzotar()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Szótárfájl kiválasztása",
                Filter = "Szótár fájl (*.txt)|*.txt|Minden fájl (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    szoparok.Clear();
                    foreach (var sor in File.ReadAllLines(ofd.FileName))
                    {
                        var parts = sor.Split(';');
                        if (parts.Length == 2)
                        {
                            szoparok.Add((parts[0].Trim(), parts[1].Trim()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a fájl betöltésekor: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void KirakSzavakat()
        {
            this.Controls.OfType<Szokartya>().ToList().ForEach(k => this.Controls.Remove(k));

            var rnd = new Random();
            var kivalasztott = szoparok.OrderBy(x => rnd.Next()).Take(10).ToList();

            List<Szokartya> kartyak = new();
            for (int i = 0; i < kivalasztott.Count; i++)
            {
                kartyak.Add(new Szokartya(kivalasztott[i].magyar, i));
                kartyak.Add(new Szokartya(kivalasztott[i].idegen, i));
            }

            kartyak = kartyak.OrderBy(x => rnd.Next()).ToList();

            int x = 20, y = 60;
            foreach (var k in kartyak)
            {
                k.Location = new Point(x, y);
                k.MouseUp += Kartya_MouseUp;
                this.Controls.Add(k);
                y += 40;
                if (y > 500)
                {
                    y = 60;
                    x += 200;
                }
            }
        }

        private void Kartya_MouseUp(object sender, MouseEventArgs e)
        {
            Szokartya aktiv = sender as Szokartya;
            var cel = this.Controls.OfType<Szokartya>()
                .FirstOrDefault(k => k != aktiv && k.Bounds.IntersectsWith(aktiv.Bounds));

            if (cel != null && cel.ParID == aktiv.ParID)
            {
                this.Controls.Remove(aktiv);
                this.Controls.Remove(cel);

                if (!this.Controls.OfType<Szokartya>().Any())
                {
                    MessageBox.Show("Minden pár megtalálva! 🎉");
                    KirakSzavakat();
                }
            }
        }
    }
}
