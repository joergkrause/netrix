using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Comzept.Library.Drawing
{
    public static class ColorUtils
    {
        public static ColorHLS BlendColors(ColorHLS backColor, ColorHLS foreColor, float alphaPercentage)
        {
            if (alphaPercentage > 100)
                throw new ArgumentOutOfRangeException("alphaPercentage must be under 100");

            if (alphaPercentage == 0)
                return foreColor;

            float redDiff = foreColor.Red - backColor.Red;
            float greenDiff = foreColor.Green - backColor.Green;
            float blueDiff = foreColor.Blue - backColor.Blue;

            alphaPercentage = alphaPercentage / 100f;

            redDiff = (redDiff * alphaPercentage) + backColor.Red;
            greenDiff = (greenDiff * alphaPercentage) + backColor.Green;
            blueDiff = (blueDiff * alphaPercentage) + backColor.Blue;

            return new ColorHLS(255, (byte)redDiff, (byte)greenDiff, (byte)blueDiff);
        }

        public static float[] GetGradientColorStep(ColorHLS startColor, ColorHLS endColor, int stepCount)
        {
            if (stepCount == 0)
            {
                return new float[0];
            }
            if (stepCount == 1)
            {
                return new float[] { 0 };
            }

            float stepR = (endColor.Red - startColor.Red) / (float)stepCount;
            float stepG = (endColor.Green - startColor.Green) / (float)stepCount;
            float stepB = (endColor.Blue - startColor.Blue) / (float)stepCount;

            return new float[] { stepR, stepG, stepB };
        }

        public static float[,] GetGradientColorSteps(ColorHLS startColor, ColorHLS endColor, int stepCount)
        {
            if (stepCount == 0)
            {
                return new float[0,0];
            }

            float r = startColor.Red;
            float g = startColor.Green;
            float b = startColor.Blue;

            float[,] steps = new float[stepCount, 3];

            if (stepCount == 1)
            {
                for (int i = 0; i < stepCount; i++)
                {
                    steps[i, 0] = r;
                    steps[i, 1] = g;
                    steps[i, 2] = b;
                }
                return steps;
            }

            float stepR = (endColor.Red - startColor.Red) / (float)stepCount;
            float stepG = (endColor.Green - startColor.Green) / (float)stepCount;
            float stepB = (endColor.Blue - startColor.Blue) / (float)stepCount;

            for (int i = 0; i < stepCount; i++)
            {
                steps[i, 0] = r;
                steps[i, 1] = g;
                steps[i, 2] = b;

                r += stepR;
                g += stepG;
                b += stepB;
            }

            return steps;
        }

        public static ColorHLS[] CreateGradientColorArray(ColorHLS startColor, ColorHLS endColor, int stepCount)
        {
            if (stepCount == 0)
            {
                return new ColorHLS[0];
            }
            if (stepCount == 1)
            {
                return new ColorHLS[] { new ColorHLS(255, (byte)((startColor.Red + endColor.Red) / 2), (byte)((startColor.Green + endColor.Green) / 2), (byte)((startColor.Blue + endColor.Blue) / 2)) };
            }

            float stepR = (endColor.Red - startColor.Red) / (float)stepCount;
            float stepG = (endColor.Green - startColor.Green) / (float)stepCount;
            float stepB = (endColor.Blue - startColor.Blue) / (float)stepCount;

            float r = startColor.Red;
            float g = startColor.Green;
            float b = startColor.Blue;

            ColorHLS[] colors = new ColorHLS[stepCount];
            for (int i = 0; i < stepCount - 1; i++)
            {
                colors[i] = new ColorHLS(
                    255,
                    (byte)Math.Round(r, MidpointRounding.ToEven),
                    (byte)Math.Round(g, MidpointRounding.ToEven),
                    (byte)Math.Round(b, MidpointRounding.ToEven)
                    );

                r += stepR;
                g += stepG;
                b += stepB;
            }
            colors[colors.Length - 1] = endColor;

            return colors;
        }

        public static ColorHLS[] CreateGradientColorArray(ColorHLS[] colors, int stepCount)
        {
            if (stepCount == 0||colors.Length==0)
            {
                return new ColorHLS[0];
            }
            if (colors.Length == 1)
            {
                return colors;
            }
            if (stepCount == 1)
            {
                return new ColorHLS[] { new ColorHLS(ColorUtils.BlendColors(colors[0],colors[colors.Length-1],50) )};
            }

            ColorHLS[] retColors = new ColorHLS[stepCount];
            

            float step =  stepCount / (float)(colors.Length-1);
            int currentStep = 0;

            for (int i = 0; i < colors.Length-1; i++)
            {
                float r = colors[i].Red;
                float g = colors[i].Green;
                float b = colors[i].Blue;

                ColorHLS c1 = colors[i];
                ColorHLS c2 = colors[i + 1];
                float stepR = (c2.Red - c1.Red) / step;
                float stepG = (c2.Green - c1.Green) / step;
                float stepB = (c2.Blue- c1.Blue) / step;

                int count = (int)(step * (i+1));
                int k = (int)(step * i);
                while (k < count)
                {
                    retColors[currentStep] = new ColorHLS(255, (byte)r, (byte)g, (byte)b);
                    r += stepR;
                    g += stepG;
                    b += stepB;
                    currentStep++;
                    k++;
                }
            }
            retColors[stepCount - 1] = colors[colors.Length - 1].Clone();

            return retColors;
        }

    }
}
