# NetGeo

This library handle GeoJson supports for C#.

Also you can convert GeoJson to OGC WKT, OGC WKT 2, ESRI WKT, PROJ.4, Proj4Js, JSON and vice versa.

Also you can parse across CRS (Coordinate Reference Systems) and projections.

## Table of contents

  - [Packages](#packages)
  - [Install](#install)
  - [Supported Versions](#supported-versions)
  - [Notes](#notes)
  - [In Development](#in-development)
  
## Packages
- **NetGeo.Json**: [![NuGet](https://img.shields.io/nuget/v/NetGeo.Json?style=flat)](https://www.nuget.org/packages/NetGeo.Json/)
- **NetGeo.System.Text.Json**: [![NuGet](https://img.shields.io/nuget/v/NetGeo.System.Text.Json?style=flat)](https://www.nuget.org/packages/NetGeo.System.Text.Json/)
- **NetGeo.Newtonsoft.Json**: [![NuGet](https://img.shields.io/nuget/v/NetGeo.Newtonsoft.Json?style=flat)](https://www.nuget.org/packages/NetGeo.Newtonsoft.Json/)

[![Package Publisher](https://github.com/schivei/netgeo/actions/workflows/main.yml/badge.svg)](https://github.com/schivei/netgeo/actions/workflows/main.yml)

## Install

Install using nuget:

```sh
dotnet add package NetGeo.Json
dotnet add package NetGeo.System.Text.Json
dotnet add package NetGeo.Newtonsoft.Json
```

> The packages NetGeo.{{third-party}}.Json already references NetGeo.Json.

## Supported Versions

| Packages version | dotnet Version |
| ---------------- | -------------- |
|      2023.X      | netstandard2.0 |
|      2023.X      | netstandard2.1 |
|      2023.X      | .net 6         |
|      2023.X      | .net 7         |

## Notes

If your using Newtonsoft.Json, you need to add the following code to your startup:

```csharp
GeoExtensions.SetDefaults();
```

If your using System.Text.Json, you need to use extensions methods:

```csharp
var geoJson = "{\"type\":\"Point\",\"coordinates\":[1,2]}";
var geoObject = geoJson.ToGeoObject();
var geoPoint = geoJson.ToGeoObject<Point>();

var json = geoObject.ToGeoJson();
var jsonPoint = geoPoint.ToGeoJson();
```

The extensions methods runs with Newtonsoft.Json and System.Text.Json.

## In Development

* CRS Convertion
* Other types where is not GeoJSON
* .NET 8
  * Comming in Dec 2023 
