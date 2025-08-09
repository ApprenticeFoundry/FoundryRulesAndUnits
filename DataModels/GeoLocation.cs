using System;
using System.Collections.Generic;
using System.Linq;


namespace FoundryRulesAndUnits.Models;


public partial class GeoLocation : UDTO_Base
{
    public double lat { get; set; }
    public double lng { get; set; }
    public double alt { get; set; }


    public GeoLocation() : base()
    {
    }

    public GeoLocation(GeoLocation obj) : base()
    {
        lat = obj.lat;
        lng = obj.lng;
        alt = obj.alt;
    }

    public GeoLocation AsLocation()
    {
        return new GeoLocation(this);
    }


    public GeoLocation SetLocationTo(GeoLocation loc)
    {
        this.lat = loc.lat;
        this.lng = loc.lng;
        this.alt = loc.alt;
        return this;
    }
}

