using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateLetterImages
{
    class Program
    {
        static void Main(string[] args)
        {
            for (char letter = 'a'; letter <= 'z' ; letter++)
            {WriteLetterAsImage(letter);}
        }

        private static void WriteLetterAsImage(char letter)
        {
            var bitmap = new Bitmap(50, 50);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(new SolidBrush(Color.White),0, 0, 50, 50 );
                graphics.DrawString(letter.ToString().ToUpperInvariant(), new Font(FontFamily.GenericSerif, 30), 
                    new SolidBrush(Color.Black),
                    5, 5);
                bitmap.Save($"{letter}.jpg", ImageFormat.Jpeg);
                var rotatedLetter = RotateImage(bitmap, 180);
                rotatedLetter.Save($"flip-{letter}.jpg", ImageFormat.Jpeg);
            }
        }
        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }
    }
}
