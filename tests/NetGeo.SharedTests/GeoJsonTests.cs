#if SYSTEMTEXTJSON
using NetGeo.Json.SystemText;
#else
using Newtonsoft.Json;
#endif
using NetGeo.Json;

namespace NetGeo.SharedTests;

[Collection("GeoObjects")]
public class GeoJsonTests : Assert
{
    public GeoJsonTests()
    {
        GeoExtensions.SetDefaults();
    }

    public static string Serialize<T>(T obj)
    {
        GeoExtensions.SetDefaults();

        return obj.ToGeoJson();
    }

    public static T? Deserialize<T>(string json)
    {
        GeoExtensions.SetDefaults();

        return json.ToGeoObject<T>();
    }

    [Theory(DisplayName = "Point serialize / deserialize")]
    [InlineData(@"{""type"":""Point"",""coordinates"":[100.1,0.1]}", "[100.1,0.1]", null)]
    [InlineData(@"{""type"":""Point"",""coordinates"":[100.1,0.1,10.1]}", "[100.1,0.1,10.1]", null)]
    // with bbox
    [InlineData(@"{""type"":""Point"",""bbox"":[99.1,-1.1,101.1,1.1],""coordinates"":[100.1,0.1]}", "[100.1,0.1]", "[99.1,-1.1,101.1,1.1]")]
    public void TestPoint(string geoJson, string strCoordinates, string? strBbox)
    {
        var coordinates = Deserialize<double[]>(strCoordinates)!;
        var bbox = strBbox is null ? null : Deserialize<double[]>(strBbox);

        var dataToSerialize = new Point
        {
            Coordinates = coordinates,
            Bbox = bbox
        };

        var json = Serialize(dataToSerialize);

        Equal(geoJson, json);

        var data = Deserialize<Point>(geoJson);
        Equivalent(dataToSerialize, data);

        True(data!.Coordinates.SequenceEqual(coordinates));
    }

    [Theory(DisplayName = "Polygon serialize / deserialize")]
    [InlineData(@"{""type"":""Polygon"",""coordinates"":[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]]]}", "[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]]]", null)]
    [InlineData(@"{""type"":""Polygon"",""coordinates"":[[[100.1,0.1,10.1],[101.1,0.1,10.1],[101.1,1.1,10.1],[100.1,1.1,10.1],[100.1,0.1,10.1]]]}", "[[[100.1,0.1,10.1],[101.1,0.1,10.1],[101.1,1.1,10.1],[100.1,1.1,10.1],[100.1,0.1,10.1]]]", null)]
    // with bbox
    [InlineData(@"{""type"":""Polygon"",""bbox"":[99.1,-1.1,101.1,1.1],""coordinates"":[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]]]}", "[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]]]", "[99.1,-1.1,101.1,1.1]")]
    public void TestPolygon(string geoJson, string strCoordinates, string? strBbox)
    {
        var coordinates = Deserialize<double[][][]>(strCoordinates)!;

        var bbox = strBbox is null ? null : Deserialize<double[]>(strBbox);

        var dataToSerialize = new Polygon
        {
            Coordinates = coordinates,
            Bbox = bbox
        };

        var json = Serialize(dataToSerialize);

        Equal(geoJson, json);

        var data = Deserialize<Polygon>(geoJson);
        Equivalent(dataToSerialize, data);
    }

    [Theory(DisplayName = "MultiPolygon serialize / deserialize")]
    [InlineData(@"{""type"":""MultiPolygon"",""coordinates"":[[[[102.1,2.1],[103.1,2.1],[103.1,3.1],[102.1,3.1],[102.1,2.1]]],[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]]}", "[[[[102.1,2.1],[103.1,2.1],[103.1,3.1],[102.1,3.1],[102.1,2.1]]],[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]]", null)]
    [InlineData(@"{""type"":""MultiPolygon"",""coordinates"":[[[[102.1,2.1,10.1],[103.1,2.1,10.1],[103.1,3.1,10.1],[102.1,3.1,10.1],[102.1,2.1,10.1]]],[[[100.1,0.1,10.1],[101.1,0.1,10.1],[101.1,1.1,10.1],[100.1,1.1,10.1],[100.1,0.1,10.1]],[[100.2,0.2,10.1],[100.8,0.2,10.1],[100.8,0.8,10.1],[100.2,0.8,10.1],[100.2,0.2,10.1]]]]}", "[[[[102.1,2.1,10.1],[103.1,2.1,10.1],[103.1,3.1,10.1],[102.1,3.1,10.1],[102.1,2.1,10.1]]],[[[100.1,0.1,10.1],[101.1,0.1,10.1],[101.1,1.1,10.1],[100.1,1.1,10.1],[100.1,0.1,10.1]],[[100.2,0.2,10.1],[100.8,0.2,10.1],[100.8,0.8,10.1],[100.2,0.8,10.1],[100.2,0.2,10.1]]]]", null)]
    // with bbox
    [InlineData(@"{""type"":""MultiPolygon"",""bbox"":[100.1,0.1,103.1,3.1],""coordinates"":[[[[102.1,2.1],[103.1,2.1],[103.1,3.1],[102.1,3.1],[102.1,2.1]]],[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]]}", "[[[[102.1,2.1],[103.1,2.1],[103.1,3.1],[102.1,3.1],[102.1,2.1]]],[[[100.1,0.1],[101.1,0.1],[101.1,1.1],[100.1,1.1],[100.1,0.1]],[[100.2,0.2],[100.8,0.2],[100.8,0.8],[100.2,0.8],[100.2,0.2]]]]", "[100.1,0.1,103.1,3.1]")]
    public void TestMultiPolygon(string geoJson, string strCoordinates, string? strBbox)
    {
        var coordinates = Deserialize<double[][][][]>(strCoordinates)!;
        var bbox = strBbox != null ? Deserialize<double[]>(strBbox) : null;

        var dataToSerialize = new MultiPolygon
        {
            Coordinates = coordinates,
            Bbox = bbox
        };

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<MultiPolygon>();
        Equivalent(dataToSerialize, data);
    }

    [Theory(DisplayName = "LineString serialize / deserialize")]
    [InlineData(@"{""type"":""LineString"",""coordinates"":[[100.1,0.1],[101.1,1.1]]}", "[[100.1,0.1],[101.1,1.1]]", null)]
    [InlineData(@"{""type"":""LineString"",""coordinates"":[[100.1,0.1,10.1],[101.1,1.1,10.1]]}", "[[100.1,0.1,10.1],[101.1,1.1,10.1]]", null)]
    // with bbox
    [InlineData(@"{""type"":""LineString"",""bbox"":[100.1,0.1,101.1,1.1],""coordinates"":[[100.1,0.1],[101.1,1.1]]}", "[[100.1,0.1],[101.1,1.1]]", "[100.1,0.1,101.1,1.1]")]
    public void TestLineString(string geoJson, string strCoordinates, string? strBbox)
    {
        var coordinates = Deserialize<double[][]>(strCoordinates)!;
        var bbox = strBbox != null ? Deserialize<double[]>(strBbox) : null;

        var dataToSerialize = new LineString
        {
            Coordinates = coordinates,
            Bbox = bbox
        };

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<LineString>();
        Equivalent(dataToSerialize, data);
    }

    [Theory(DisplayName = "MultiLineString serialize / deserialize")]
    [InlineData(@"{""type"":""MultiLineString"",""coordinates"":[[[100.1,0.1],[101.1,1.1]],[[102.1,2.1],[103.1,3.1]]]}", "[[[100.1,0.1],[101.1,1.1]],[[102.1,2.1],[103.1,3.1]]]", null)]
    [InlineData(@"{""type"":""MultiLineString"",""coordinates"":[[[100.1,0.1,10.1],[101.1,1.1,10.1]],[[102.1,2.1,10.1],[103.1,3.1,10.1]]]}", "[[[100.1,0.1,10.1],[101.1,1.1,10.1]],[[102.1,2.1,10.1],[103.1,3.1,10.1]]]", null)]
    // with bbox
    [InlineData(@"{""type"":""MultiLineString"",""bbox"":[100.1,0.1,103.1,3.1],""coordinates"":[[[100.1,0.1],[101.1,1.1]],[[102.1,2.1],[103.1,3.1]]]}", "[[[100.1,0.1],[101.1,1.1]],[[102.1,2.1],[103.1,3.1]]]", "[100.1,0.1,103.1,3.1]")]
    public void TestMultiLineString(string geoJson, string strCoordinates, string? strBbox)
    {
        var coordinates = Deserialize<double[][][]>(strCoordinates)!;
        var bbox = strBbox != null ? Deserialize<double[]>(strBbox) : null;

        var dataToSerialize = new MultiLineString
        {
            Coordinates = coordinates,
            Bbox = bbox
        };

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<MultiLineString>();
        Equivalent(dataToSerialize, data);
    }

    [Theory(DisplayName = "MultiPoint serialize / deserialize")]
    [InlineData(@"{""type"":""MultiPoint"",""coordinates"":[[100.1,0.1],[101.1,1.1]]}", "[[100.1,0.1],[101.1,1.1]]", null)]
    [InlineData(@"{""type"":""MultiPoint"",""coordinates"":[[100.1,0.1,10.1],[101.1,1.1,10.1]]}", "[[100.1,0.1,10.1],[101.1,1.1,10.1]]", null)]
    // with bbox
    [InlineData(@"{""type"":""MultiPoint"",""bbox"":[100.1,0.1,101.1,1.1],""coordinates"":[[100.1,0.1],[101.1,1.1]]}", "[[100.1,0.1],[101.1,1.1]]", "[100.1,0.1,101.1,1.1]")]
    public void TestMultiPoint(string geoJson, string strCoordinates, string? strBbox)
    {
        var coordinates = Deserialize<double[][]>(strCoordinates)!;
        var bbox = strBbox != null ? Deserialize<double[]>(strBbox) : null;

        var dataToSerialize = new MultiPoint
        {
            Coordinates = coordinates,
            Bbox = bbox
        };

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<MultiPoint>();
        Equivalent(dataToSerialize, data);
    }

    [Theory(DisplayName = "GeometryCollection serialize / deserialize")]
    // 2d
    [InlineData(@"{""type"":""GeometryCollection"",""geometries"":[{""type"":""Point"",""coordinates"":[100.1,0.1]},{""type"":""LineString"",""coordinates"":[[101.1,0.1],[102.1,1.1]]}]}", 1)]
    // 3d
    [InlineData(@"{""type"":""GeometryCollection"",""geometries"":[{""type"":""Point"",""coordinates"":[100.1,0.1,10.1]},{""type"":""LineString"",""coordinates"":[[101.1,0.1,10.1],[102.1,1.1,10.1]]}]}", 2)]
    // with bbox
    [InlineData(@"{""type"":""GeometryCollection"",""geometries"":[{""type"":""Point"",""coordinates"":[100.1,0.1]},{""type"":""LineString"",""bbox"":[100.1,0.1,102.1,1.1],""coordinates"":[[101.1,0.1],[102.1,1.1]]}]}", 3)]
    public void TestGeometryCollection(string geoJson, int item)
    {
        var dataToSerialize = new GeometryCollection
        {
            Geometries =
            [
                item == 1 ?
                new Point { Coordinates = [100.1, 0.1d] } :
                item == 2 ?
                new Point { Coordinates = [100.1,0.1,10.1d] } :
                new Point{ Coordinates = [100.1, 0.1d] },
                item == 1 ?
                new LineString { Coordinates = [[101.1, 0.1d], [102.1, 1.1d]] } :
                item == 2 ?
                new LineString { Coordinates = [[101.1, 0.1d, 10.1d], [102.1, 1.1d, 10.1d]] } :
                new LineString {
                    Coordinates = [[101.1, 0.1d], [102.1, 1.1d]],
                    Bbox = [100.1, 0.1d, 102.1, 1.1d]
                }
            ]
        };

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<GeometryCollection>();
        Equivalent(dataToSerialize, data);
    }

    [Theory(DisplayName = "Feature serialize / deserialize")]
    // 2 with diferent types (one 2D other 3D)
    // 1 with properties
    // 1 with bbox
    // 1 with different crs
    // pass coordinate, type and bbox to test method as string
    // do not repeat the same type on different inlinedata
    // Point
    [InlineData(@"{""type"":""Feature"",""geometry"":{""type"":""Point"",""coordinates"":[102.1,0.5]}}", "[102.1,0.5]", "Point", null, null)]
    // MultiPoint 3D
    [InlineData(@"{""type"":""Feature"",""geometry"":{""type"":""MultiPoint"",""coordinates"":[[102.1,0.1,10.1],[103.1,1.1,10.1]]}}", "[[102.1,0.1,10.1],[103.1,1.1,10.1]]", "MultiPoint", null, null)]
    // LineString bbox wo properties
    [InlineData(@"{""type"":""Feature"",""geometry"":{""type"":""LineString"",""bbox"":[102.1,0.1,103.1,1.1],""coordinates"":[[102.1,0.1],[103.1,1.1]]}}", "[[102.1,0.1],[103.1,1.1]]", "LineString", "[102.1,0.1,103.1,1.1]", null)]
    // LineString 3D wo bbox and properties with crs
    [InlineData(@"{""type"":""Feature"",""geometry"":{""type"":""LineString"",""coordinates"":[[102.1,0.1,10.1],[103.1,1.1,10.1]]},""crs"":{""type"":""name"",""properties"":{""name"":""urn:ogc:def:crs:EPSG::4674""}}}", "[[102.1,0.1,10.1],[103.1,1.1,10.1]]", "LineString", null, @"{""type"":""name"",""properties"":{""name"":""urn:ogc:def:crs:EPSG::4674""}}")]
    // test method
    public void TestFeature(string geoJson, string strCoordinates, string strType, string? strBbox, string? strCrs)
    {
        var type = (GeoType)Enum.Parse(typeof(GeoType), strType);
        var bbox = strBbox != null ? Deserialize<double[]>(strBbox) : null;

        var dataToSerialize = new Feature
        {
            Geometry = new Geometry
            {
                Type = type,
                Bbox = bbox
            }
        };

        if (strCrs != null)
        {
            dataToSerialize.Crs = Deserialize<Crs>(strCrs);
        }

        if (type == GeoType.Point)
        {
            dataToSerialize.Geometry.BaseCoordinates = Deserialize<double[]>(strCoordinates)!;
            dataToSerialize.Geometry = new Point(dataToSerialize.Geometry);
        }
        else if (type == GeoType.MultiPoint)
        {
            dataToSerialize.Geometry.BaseCoordinates = Deserialize<double[][]>(strCoordinates)!;
            dataToSerialize.Geometry = new MultiPoint(dataToSerialize.Geometry);
        }
        else if (type == GeoType.LineString)
        {
            dataToSerialize.Geometry.BaseCoordinates = Deserialize<double[][]>(strCoordinates)!;
            dataToSerialize.Properties = null;
            dataToSerialize.Geometry = new LineString(dataToSerialize.Geometry);
        }

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<Feature>();
        Equivalent(dataToSerialize, data);

        Equal(data.Geometry.Type, type);

        // check crs
        if (strCrs != null)
        {
            Equivalent(data.Crs, Deserialize<Crs>(strCrs));
        }
        // else default crs
        else
        {
            Equal("urn:ogc:def:crs:OGC:1.3:CRS84", data.Crs.Properties.Name);
        }
    }

    [Theory(DisplayName = "FeatureCollection serialize / deserialize")]
    // 2 with diferent types (one 2D other 3D)
    // 1 with properties
    // 1 with bbox
    // 1 with different crs
    // pass coordinate, type and bbox to test method as string
    // do not repeat the same type on different inlinedata
    // Point
    [InlineData(@"{""type"":""FeatureCollection"",""features"":[{""type"":""Feature"",""geometry"":{""type"":""Point"",""coordinates"":[102.1,0.5]}}]}", "[102.1,0.5]", "Point", null, null)]
    // MultiPoint 3D
    [InlineData(@"{""type"":""FeatureCollection"",""features"":[{""type"":""Feature"",""geometry"":{""type"":""MultiPoint"",""coordinates"":[[102.1,0.1,10.1],[103.1,1.1,10.1]]}}]}", "[[102.1,0.1,10.1],[103.1,1.1,10.1]]", "MultiPoint", null, null)]
    // LineString bbox wo properties
    [InlineData(@"{""type"":""FeatureCollection"",""features"":[{""type"":""Feature"",""geometry"":{""type"":""LineString"",""bbox"":[102.1,0.1,103.1,1.1],""coordinates"":[[102.1,0.1],[103.1,1.1]]}}]}", "[[102.1,0.1],[103.1,1.1]]", "LineString", "[102.1,0.1,103.1,1.1]", null)]
    // LineString 3D wo bbox and properties with crs urn:ogc:def:crs:EPSG::4674
    [InlineData(@"{""type"":""FeatureCollection"",""features"":[{""type"":""Feature"",""geometry"":{""type"":""LineString"",""coordinates"":[[102.1,0.1],[103.1,1.1]]}}],""crs"":{""type"":""name"",""properties"":{""name"":""urn:ogc:def:crs:EPSG::4674""}}}", "[[102.1,0.1],[103.1,1.1]]", "LineString", null, @"{""type"":""name"",""properties"":{""name"":""urn:ogc:def:crs:EPSG::4674""}}")]
    // test method
    public void TestFeatureCollection(string geoJson, string strCoordinates, string strType, string? strBbox, string? strCrs)
    {
        var type = (GeoType)Enum.Parse(typeof(GeoType), strType);
        var bbox = strBbox != null ? Deserialize<double[]>(strBbox) : null;

        var dataToSerialize = new FeatureCollection
        {
            Features =
            [
                new Feature
                {
                    Geometry = new Geometry
                    {
                        Type = type,
                        Bbox = bbox
                    }
                }
            ]
        };

        if (type == GeoType.Point)
        {
            dataToSerialize.Features[0].Geometry.BaseCoordinates = Deserialize<double[]>(strCoordinates)!;
            dataToSerialize.Features[0].Geometry = new Point(dataToSerialize.Features[0].Geometry);
        }
        else if (type == GeoType.MultiPoint)
        {
            dataToSerialize.Features[0].Geometry.BaseCoordinates = Deserialize<double[][]>(strCoordinates)!;
            dataToSerialize.Features[0].Geometry = new MultiPoint(dataToSerialize.Features[0].Geometry);
        }
        else if (type == GeoType.LineString)
        {
            dataToSerialize.Features[0].Geometry.BaseCoordinates = Deserialize<double[][]>(strCoordinates)!;
            dataToSerialize.Features[0].Properties = null;
            dataToSerialize.Features[0].Geometry = new LineString(dataToSerialize.Features[0].Geometry);

            if (bbox == null)
            {
                dataToSerialize.Crs = new()
                {
                    Properties = new()
                    {
                        Name = "urn:ogc:def:crs:EPSG::4674"
                    },
                    Type = "name"
                };
            }
        }

        var json = dataToSerialize.ToGeoJson();

        Equal(geoJson, json);

        var data = geoJson.ToGeoObject<FeatureCollection>();
        Equivalent(dataToSerialize, data);

        Equal(data.Features[0].Geometry.Type, type);

        // check crs
        if (strCrs != null)
        {
            Equivalent(data.Crs, Deserialize<Crs>(strCrs));
        }
        // else default crs
        else
        {
            Equal("urn:ogc:def:crs:OGC:1.3:CRS84", data.Crs.Properties.Name);
        }
    }
}
