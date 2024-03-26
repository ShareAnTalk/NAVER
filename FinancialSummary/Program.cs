using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using ShareInvest;

using System.Net;

const string URL = "https://navercomp.wisereport.co.kr";
const string RESOURCE = "company/ajax/c1050001_data.aspx?flag=2&cmp_cd={code}&finGubun=MAIN&frq=0";

Queue<string> stock = new();

using (var client = new RestClient(URL))
{
    var response = await client.ExecuteAsync(new RestRequest(RESOURCE));

    if (HttpStatusCode.OK == response.StatusCode && !string.IsNullOrEmpty(response.Content))
    {
        var items = JToken.Parse(response.Content)["items"];

        if (items == null)
        {
            return;
        }
        foreach (var item in items.Children())
        {
            var code = Convert.ToString(item["code"]);

            if (string.IsNullOrEmpty(code))
            {
                continue;
            }
            stock.Enqueue(code);
        }
    }
}
while (stock.TryDequeue(out string? code))
{
    if (string.IsNullOrEmpty(code))
    {
        continue;
    }
    using (var client = new RestClient(URL))
    {
        var response = await client.ExecuteAsync(new RestRequest(RESOURCE));

        if (HttpStatusCode.OK != response.StatusCode || string.IsNullOrEmpty(response.Content))
        {
            continue;
        }
        var data = JToken.Parse(response.Content)["JsonData"];

        if (data == null || !data.HasValues)
        {
            continue;
        }
        var json = Convert.ToString(data);

        if (string.IsNullOrEmpty(json))
        {
            continue;
        }
        var e = JsonConvert.DeserializeObject<FinancialStatements[]>(json);

        Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
    }
}