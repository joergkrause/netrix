using System;
using System.Drawing;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Drawing
{
    public class ColorHLS
    {
        public enum ColorComponentsSwap
        {
            RedGreen=0,RedBlue,GreenBlue
        }

        public ColorHLS(Color color)
            :this(color.A,color.R,color.G,color.B)
        {
        }
        public ColorHLS(byte alpha, byte red, byte green, byte blue)
        {
            _alpha = alpha;
            _red = red;
            _green = green;
            _blue = blue;

            CalculateHLS();
        }
        public ColorHLS(float hue, float luminance, float saturation)
        {
            _hue = hue;
            _luminance = luminance;
            _saturation = saturation;

            CalculateColor();
        }
        public ColorHLS(ColorHLS hls)
        {
            _alpha = hls.Alpha;
            _red = hls.Red;
            _blue = hls.Blue;
            _green = hls.Green;
            _luminance = hls.Luminance;
            _hue = hls.Hue;
            _saturation = hls.Saturation;
        }

        private byte _alpha, _red, _green, _blue;
        private float _hue, _luminance, _saturation;

        public byte Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }
        }
        public byte Red
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
                CalculateHLS();
            }
        }
        public byte Green
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
                CalculateHLS();
            }
        }
        public byte Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
                CalculateHLS();
            }
        }

        public float Hue
        {
            get
            {
                return _hue;
            }
            set
            {
                _hue = value % 360;
                CalculateColor();
            }
        }
        public float Luminance
        {
            get
            {
                return _luminance;
            }
            set
            {
                _luminance = value;
                if (_luminance < 0f) _luminance = 0f;
                if (_luminance > 1.0f) _luminance = 1f;

                CalculateColor();
            }
        }
        public float Saturation
        {
            get
            {
                return _saturation;
            }
            set
            {
                _saturation = value;
                if (_saturation < 0f) _saturation = 0f;
                if (_saturation > 1.0f) _saturation = 1f;

                CalculateColor();
            }
        }

        public Color Color
        {
            get
            {
                Color c = Color.FromArgb(_alpha, _red, _green, _blue);
                return c;
            }
            set
            {
                _alpha = value.A;
                _red = value.R;
                _green = value.G;
                _blue = value.B;

                CalculateHLS();
            }
        }
        public Color Complementary
        {
            get
            {
                return Color.FromArgb(_alpha, 255 - _red, 255 - _green, 255 - _blue);
            }
        }

        public void Lighten(float value)
        {
            this.Luminance += value;
        }
        public void Darken(float value)
        {
            this.Luminance -= value;
        }

        public ColorHLS Clone()
        {
            ColorHLS c = new ColorHLS(this);
            return c;
        }
        public void SwapComponents(ColorComponentsSwap comps)
        {
            switch (comps)
            {
                case ColorComponentsSwap.GreenBlue:
                    SwapComps(ref _green, ref  _blue);
                    break;
                case ColorComponentsSwap.RedBlue:
                    SwapComps(ref _red, ref  _blue);
                    break;
                case ColorComponentsSwap.RedGreen:
                    SwapComps(ref _red, ref  _green);
                    break;
            }
        }
        public void TraslateRGB(byte deltaR, byte deltaG, byte deltaB)
        {
            _red += deltaR;
            _green += deltaG;
            _blue += deltaB;
            CalculateHLS();
        }
        public void SetRGB(byte r, byte g, byte b)
        {
            _red = r;
            _green = g;
            _blue = b;
            CalculateHLS();
        }

        private void SwapComps(ref byte a,ref byte b)
        {
            byte x = a;
            a = b;
            b = x;
        }
        private void CalculateColor()
        {
            if (_saturation == 0.0) 
            {
                _red = (byte)(_luminance * 255.0F);
                _green = _red;
                _blue = _red;
            }
            else
            {
                float rm1;
                float rm2;

                if (_luminance <= 0.5f)
                {
                    rm2 = _luminance + (_luminance * _saturation);
                }
                else
                {
                    rm2 = _luminance + _saturation - (_luminance * _saturation);
                }
                rm1 = 2.0f * _luminance - rm2;
                _red = ToRGB(rm1, rm2, _hue + 120.0f);
                _green = ToRGB(rm1, rm2, _hue);
                _blue = ToRGB(rm1, rm2, _hue - 120.0f);
            }
        }
        private void CalculateHLS()
        {
            byte minval = Math.Min(_red, Math.Min(_green, _blue));
            byte maxval = Math.Max(_red, Math.Max(_green, _blue));

            float mdiff = (float)(maxval - minval);
            float msum = (float)(maxval + minval);

            _luminance = msum / 510.0f;

            if (maxval == minval)
            {
                _saturation = 0.0f;
                _hue = 0.0f;
            }
            else
            {
                float rnorm = (maxval - _red) / mdiff;
                float gnorm = (maxval - _green) / mdiff;
                float bnorm = (maxval - _blue) / mdiff;

                _saturation = (_luminance <= 0.5f) ? (mdiff / msum) : (mdiff / (510.0f - msum));

                if (_red == maxval)
                {
                    _hue = (60.0f * (6.0f + bnorm - gnorm))%360;
                }
                if (_green == maxval)
                {
                    _hue = 60.0f * (2.0f + rnorm - bnorm);
                }
                if (_blue == maxval)
                {
                    _hue = 60.0f * (4.0f + gnorm - rnorm);
                }

                if (_hue > 360.0f)
                {
                    _hue = _hue - 360.0f;
                }
            }
        }
        private byte ToRGB(float rm1, float rm2, float rh)
        {
            if (rh > 360.0f)
            {
                rh -= 360.0f;
            }
            else if (rh < 0.0f)
            {
                rh += 360.0f;
            }

            if (rh < 60.0f)
            {
                rm1 = rm1 + (rm2 - rm1) * rh / 60.0f;
            }
            else if (rh < 180.0f)
            {
                rm1 = rm2;
            }
            else if (rh < 240.0f)
            {
                rm1 = rm1 + (rm2 - rm1) * (240.0f - rh) / 60.0f;
            }

            return (byte)(rm1 * 255);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ColorHLS: Alpha=" + _alpha.ToString());
            sb.Append(" , Red=" + _red.ToString());
            sb.Append(" , Green=" + _green.ToString());
            sb.Append(" , Blue=" + _blue.ToString());
            sb.Append(" , Hue=" + _hue.ToString());
            sb.Append(" , Luminance=" + _luminance.ToString());
            sb.Append(" , Saturation=" + _saturation.ToString());
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
