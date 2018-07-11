using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp
{

    public class Process
    {
        public void LoadJson(string url)
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            string key = null;

            string json = ReadUrl(url);

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            List<string> toppings = new List<string>();
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    //Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                    string str = reader.Value.ToString();
                    toppings.Add(str);
                }
                else
                {
                    //Console.WriteLine("Token: {0}", reader.TokenType);
                    if (reader.TokenType.ToString().Equals("StartArray"))
                    {
                        toppings = new List<string>();
                    }
                    if (reader.TokenType.ToString().Equals("EndArray"))
                    {
                        key = null;
                        int i = 0;

                        var sortedToppings = toppings.OrderBy(s => s).ToList();
                        foreach(string item in sortedToppings)
                        {
                            i++;
                            if (i == toppings.Count)
                                key = key + item;
                            else
                                key = key + item + "-";
                        }
                        //Console.WriteLine(key);
                        if (dict.ContainsKey(key))
                        {
                            dict[key] = dict[key] + 1;
                        }
                        else
                        {
                            dict.Add(new KeyValuePair<string, int>(key, 1));
                        }

                    }
                }
            }

            var sortedDict = dict.OrderByDescending(m => m.Value);

            int j = 0;
            foreach (KeyValuePair<string, int> kvp in sortedDict)
            {
                if (j < 20)
                    Console.WriteLine(kvp.Key + ", " + kvp.Value);
                j++;
            }
        }

        public string ReadUrl(string url)
        {
            Uri address = new Uri(url);

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (WebClient webClient = new WebClient())
            {
                var stream = webClient.OpenRead(address);
                using (StreamReader sr = new StreamReader(stream))
                {
                    var json = sr.ReadToEnd();

                    return json;
                }
            }
        }
    }
}
