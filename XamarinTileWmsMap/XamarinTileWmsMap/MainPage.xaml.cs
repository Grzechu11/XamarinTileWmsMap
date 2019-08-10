using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace XamarinTileWmsMap
{
    public partial class MainPage : ContentPage
    {
        private TileLayer linesTile = null;
        private TileLayer pointsTile = null;

        public MainPage()
        {
            InitializeComponent();
            SetupMap();
        }

        public void SetupMap()
        {
            double minLat = 50.0142652674738;
            double minLon = 19.6629558230123;
            double maxLat = 50.3237297996534;
            double maxLon = 21.127929227143;

            MyMap.MoveToRegion(MapSpan.FromBounds(
                new Bounds(new Position(minLat, minLon),
                new Position(maxLat, maxLon))),
                true);

            if (linesTile != null)
            {
                MyMap.TileLayers.Remove(linesTile);
            }

            linesTile = TileLayer.FromTileUri((int x, int y, int zoom) =>
                new Uri(WMSTileProvider.GetTileUrl(x, y, zoom, "gaz%3Agaz-linie")));

            linesTile.Tag = "Gazociag"; // Can set any object
            MyMap.TileLayers.Add(linesTile);


            if (pointsTile != null)
            {
                MyMap.TileLayers.Remove(pointsTile);
            }

            pointsTile = TileLayer.FromTileUri((int x, int y, int zoom) =>
                new Uri(WMSTileProvider.GetTileUrl(x, y, zoom, "gaz%3Agaz-punkty")));

            pointsTile.Tag = "Punkty"; // Can set any object
            MyMap.TileLayers.Add(pointsTile);
        }
    }
}
