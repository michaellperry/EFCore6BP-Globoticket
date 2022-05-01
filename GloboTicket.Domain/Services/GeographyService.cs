using NetTopologySuite.Geometries;

namespace GloboTicket.Domain.Services;

public static class GeographyService
{
    public static Point GeographicLocation(double lattitude, double longitude)
    {
        return new Point(longitude, lattitude) { SRID = 4326 };
    }
}
