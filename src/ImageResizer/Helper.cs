using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageResizer
{
    public class Helper
    {
        public static void Run(string oldFilePath, string newFilePath, int width, int height)
        {

            if (!oldFilePath.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
            {
                using (var image = new MagickImage(oldFilePath))
                {
                    Process(width, height, image);
                    image.Write(newFilePath);
                }
            }
            else
            {
                using (var collection = new MagickImageCollection(oldFilePath))
                {

                    foreach (var image in collection)
                    {
                        Process(width, height, image);
                    }
                    collection.Write(newFilePath);
                }
            }
        }

        private static void Process(int width, int height, MagickImage image)
        {
            //模版的宽高比例
            var templateRate = (double)width / height;

            //原图片的宽高比例
            var nowRate = (double)image.Width / image.Height;

            if (templateRate < nowRate)
            {
                //以高为准缩放
                // Resize each image in the collection to a width of 200. When zero is specified for the height
                // the height will be calculated with the aspect ratio.
                image.Resize(0, height);
                image.ChopHorizontal(width, int.MaxValue);
            }
            else
            {
                //以宽为准缩放
                image.Resize(width, 0);
                image.ChopVertical(height, int.MaxValue);
            }
        }
    }
}

