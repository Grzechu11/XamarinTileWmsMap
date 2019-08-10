using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace XamarinTileWmsMap
{
    public static class WMSTileProvider
    {
        //private static readonly int WIDTH = (int)App.ScreenWidth;
        //private static readonly int HEIGHT = (int)App.ScreenHeight;

        private static int WIDTH = 1024;
        private static int HEIGHT = 1024;

        private static readonly string WMS_SERVICE_PARAMETERS =
            "{0}/wms?" +
            "service=WMS" +
            "&version=1.1.0" +
            "&request=GetMap" +
            "&layers={5}" +
            "&bbox={1}%2C{2}%2C{3}%2C{4}" +
            "&width=" + WIDTH +
            "&height=" + HEIGHT +
            "&srs=EPSG%3A900913" +
            "&format=image%2Fpng" +
            "&transparent=true";

        private static readonly double[] TILE_ORIGIN = {
            -20037508.34789244, 20037508.34789244
        };

        // array indexes for that data
        private static readonly int ORIG_X = 0;
        private static readonly int ORIG_Y = 1; // "

        // Size of square world map in meters, using WebMerc projection.
        private static readonly double MAP_SIZE = 20037508.34789244 * 2;

        // array indexes for array to hold bounding boxes.
        private static readonly int MINX = 0;
        private static readonly int MAXX = 1;
        private static readonly int MINY = 2;
        private static readonly int MAXY = 3;

        // Return a web Mercator bounding box given tile x/y indexes and a zoom
        // level.
        private static double[] getBoundingBox(int x, int y, int zoom)
        {
            double tileSize = MAP_SIZE / Math.Pow(2, zoom);
            double minx = TILE_ORIGIN[ORIG_X] + x * tileSize;
            double maxx = TILE_ORIGIN[ORIG_X] + (x + 1) * tileSize;
            double miny = TILE_ORIGIN[ORIG_Y] - (y + 1) * tileSize;
            double maxy = TILE_ORIGIN[ORIG_Y] - y * tileSize;

            double[] bbox = new double[4];
            bbox[MINX] = minx;
            bbox[MINY] = miny;
            bbox[MAXX] = maxx;
            bbox[MAXY] = maxy;

            return bbox;
        }

        public static string GetTileUrl(int x, int y, int zoom, string layer)
        {
            double[] bbox = getBoundingBox(x, y, zoom);
            string geoserverAddress = "http://GEOSERVER:PORT/geoserver/gaz";
            string urlStr = string.Format(
                        WMS_SERVICE_PARAMETERS,
                        geoserverAddress,
                        bbox[MINX].ToString(System.Globalization.CultureInfo.InvariantCulture),
                        bbox[MINY].ToString(System.Globalization.CultureInfo.InvariantCulture),
                        bbox[MAXX].ToString(System.Globalization.CultureInfo.InvariantCulture),
                        bbox[MAXY].ToString(System.Globalization.CultureInfo.InvariantCulture),
                        layer);

            Console.WriteLine(urlStr);
            Debug.WriteLine(urlStr);
            return urlStr;
        }
    }
}
