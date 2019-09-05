using System;
using System.Globalization;
using System.Net;
using UnityEngine;


namespace HappyUnity.Network
{
    public static class NetworkDateTime
    {
        public static DateTime NistDateTime
        {
            // throws SocketException
            get
            {
                var myHttpWebRequest = (HttpWebRequest) WebRequest.Create("http://www.microsoft.com");
                var response = myHttpWebRequest.GetResponse();
                string todaysDates = response.Headers["date"];
                return DateTime.ParseExact(todaysDates,
                    "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                    CultureInfo.InvariantCulture.DateTimeFormat,
                    DateTimeStyles.AssumeUniversal);
            }
        }

        public static bool NetworkReachable
        {
            get
            {
                return Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                       Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }
    }
}