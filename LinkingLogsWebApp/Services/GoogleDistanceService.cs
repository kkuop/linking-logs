using LinkingLogsWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinkingLogsWebApp.Services
{
    public class GoogleDistanceService
    {
        public async Task<DistanceJson> GetDistance(Job job, Trucker trucker, string endPoints)
        {
            string url = "";
            if(endPoints == "HomeToSite")
            {
                url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={trucker.HomeAddress}&destinations={job.Site.Latitude},{job.Site.Longitude}&key={ApiKeys.GoogleKey}";
            } else if(endPoints == "SiteToMill")
            {
                url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={job.Site.Latitude},{job.Site.Longitude}&destinations={job.Mill.Address}&key={ApiKeys.GoogleKey}";
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<DistanceJson>(json);
            }
            return null;
        }


        public class DistanceJson
        {
            public string[] destination_addresses { get; set; }
            public string[] origin_addresses { get; set; }
            public Row[] rows { get; set; }
            public string status { get; set; }
        }

        public class Row
        {
            public Element[] elements { get; set; }
        }

        public class Element
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public string status { get; set; }
        }

        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }

    }
}
