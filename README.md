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
- **NetGeo.Newtonsoft.Json**: [![NuGet](https://img.shields.io/nuget/v/NetGeo.Newtonsoft.Json?style=flat)](https://www.nuget.org/packages/NetGeo.Newtonsoft.Json/)

[![Package Publisher](https://github.com/schivei/netgeo/actions/workflows/main.yml/badge.svg)](https://github.com/schivei/netgeo/actions/workflows/main.yml)

## Install

Install using nuget:

```sh
dotnet add package NetGeo.Json
dotnet add package NetGeo.Newtonsoft.Json
```

> The packages NetGeo.Newtonsoft.Json already references NetGeo.Json.

## Supported Versions

| Packages version | dotnet Version |
| ---------------- | -------------- |
|      2023.X      | .net 7         |

## Notes

Removed the netstandard support, because the System.Text.Json is not supported in netstandard.

Removed the net6.0 support, because the System.Text.Json Default Options is not supported in net6.0.

If your using Newtonsoft.Json, you need to add the following code to your startup:

```csharp
GeoExtensions.SetDefaults();
```

The extensions methods runs with Newtonsoft.Json and System.Text.Json.

For System.Text.Json, use the following namespace:

```csharp
using NetGeo.Json.SystemText;
```

## In Development

* CRS Convertion
* Other types where is not GeoJSON
* .NET 8
  * Comming in Dec 2023 
