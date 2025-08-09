using System;
using System.Collections.Generic;
using System.Linq;


namespace FoundryRulesAndUnits.Models;


public partial class Location : UDTO_Base
{
    public double lat;
    public double lng;
    public double alt;


    public Location() : base()
    {
    }

    public Location(Location obj) : base()
    {
        lat = obj.lat;
        lng = obj.lng;
        alt = obj.alt;
    }

    public Location AsLocation()
    {
        return new Location(this);
    }


    public Location SetLocationTo(Location loc)
    {
        this.lat = loc.lat;
        this.lng = loc.lng;
        this.alt = loc.alt;
        return this;
    }
}

