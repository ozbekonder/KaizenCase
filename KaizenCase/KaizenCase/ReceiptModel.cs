using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaizenCase
{
    public class ReceiptModel
    {
        public string locale { get; set; }
        public string description { get; set; }
        public BoundingPoly boundingPoly { get; set; }
    }
    public class BoundingPoly
    {
        public List<Vertex> vertices { get; set; }
        public Vertex center { get { return FindOrigin(); } }
        Vertex FindOrigin()
        {
            Vertex center = new Vertex();
            int minx = int.MaxValue;
            int maxx = int.MinValue;
            int miny = int.MaxValue;
            int maxy = int.MinValue;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (minx > vertices[i].x)
                    minx = vertices[i].x;
                if (maxx < vertices[i].x)
                    maxx = vertices[i].x;

                if (miny > vertices[i].y)
                    miny = vertices[i].y;
                if (maxy < vertices[i].y)
                    maxy = vertices[i].y;
            }
            center.x = minx + (maxx - minx) / 2;
            center.y = miny + (maxy - miny) / 2;
            return center;
        }
    }
    public class Vertex
    {
        public int x { get; set; }
        public int y { get; set; }
    }


}
