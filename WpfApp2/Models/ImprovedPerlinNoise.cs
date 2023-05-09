using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public class ImprovedPerlinNoise
    {
        private readonly int[] permutation = new int[512];
        private readonly int[] p;

        public ImprovedPerlinNoise()
        {
            p = new int[256];

            for (int i = 0; i < 256; i++)
                p[i] = i;

            for (int i = 0; i < 256; i++)
            {
                int j = new Random().Next(256 - i) + i;
                int temp = p[i];
                p[i] = p[j];
                p[j] = temp;
            }

            for (int i = 0; i < 512; i++)
                permutation[i] = p[i & 255];
        }

        public double Noise(double x, double y, double z)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            int Z = (int)Math.Floor(z) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);
            z -= Math.Floor(z);

            double u = Fade(x);
            double v = Fade(y);
            double w = Fade(z);

            int A = permutation[X] + Y, AA = permutation[A] + Z, AB = permutation[A + 1] + Z;
            int B = permutation[X + 1] + Y, BA = permutation[B] + Z, BB = permutation[B + 1] + Z;

            return Lerp(w, Lerp(v, Lerp(u, Grad(permutation[AA], x, y, z),
                                           Grad(permutation[BA], x - 1, y, z)),
                                   Lerp(u, Grad(permutation[AB], x, y - 1, z),
                                           Grad(permutation[BB], x - 1, y - 1, z))),
                           Lerp(v, Lerp(u, Grad(permutation[AA + 1], x, y, z - 1),
                                           Grad(permutation[BA + 1], x - 1, y, z - 1)),
                                   Lerp(u, Grad(permutation[AB + 1], x, y - 1, z - 1),
                                           Grad(permutation[BB + 1], x - 1, y - 1, z - 1))));
        }

        private double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        private double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y;
            double v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }

}
