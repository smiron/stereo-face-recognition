using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageFilters.Common
{
    public static class Helpers
    {
        public static Region2D ToRegion(this Rectangle rectangle)
        {
            return new Region2D
            {
                Width = rectangle.Size.Width,
                Height = rectangle.Size.Height,
                Location = new Point
                {
                    X = rectangle.Left,
                    Y = rectangle.Top
                }
            };
        }

        public static Rectangle ToRectangle(this Region2D region)
        {
            return new Rectangle(new Point(region.Location.X, region.Location.Y), new Size(region.Width, region.Height));
        }
    }
}
