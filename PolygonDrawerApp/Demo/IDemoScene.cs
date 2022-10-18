using Polygons;

namespace PolygonDrawerApp.Demo
{
    /// <summary>
    /// Interfejs do implementacji gotowych scen z wielokątami
    /// </summary>
    public interface IDemoScene
    {
        void Load(PolygonManager manager);
    }
}
