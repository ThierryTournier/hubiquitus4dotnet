using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace HubiquitusDotNetPortable.hapi.hStructures
{
    public class HPos : JObject
    {
        public HPos()
            : base()
        {
        }

        public HPos(JObject jsonObj)
            : base(jsonObj)
        { 
        }

        public double? GetLat()
        {
            double? lat;
            try
            {
                lat = this["lat"].ToObject<double>();
            }
            catch ( Exception e)
            {
                Debug.WriteLine("{0} :  Can not fetch the lat attribute.", e.ToString());
                lat = null;
            }
            return lat;
        }

        public void SetLat(double lat)
        {
            try
            {
                this["lat"] = lat;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the lat attribute.", e.ToString());
            }
        }

        public double? GetLng()
        {
            double? lng;
            try
            {
                lng = this["lng"].ToObject<double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} :  Can not fetch the lng attribute.", e.ToString());
                lng = null;
            }
            return lng;
        }

        public void SetLng(double lng)
        {
            try
            {
                this["lng"] = lng;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the lng attribute.", e.ToString());
            }
        }

        public double? GetRadius()
        {
            double? radius;
            try
            {
                radius = this["radius"].ToObject<double>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} :  Can not fetch the radius attribute.", e.ToString());
                radius = null;
            }
            return radius;
        }

        public void SetRadius(double radius)
        {
            try
            {
                this["radius"] = radius;
            }
            catch (Exception e)
            {
                Debug.WriteLine("{0} : Can not update the radius attribute.", e.ToString());
            }
        }
        
    }
}
