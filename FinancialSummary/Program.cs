using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using ShareInvest.Utilities;

using System.Net;

Queue<string> stock = new();

using (var client = new RestClient("https://localhost:44301"))
{
    var response = await client.ExecuteAsync(new RestRequest(""));

    if (HttpStatusCode.OK == response.StatusCode && !string.IsNullOrEmpty(response.Content))
    {
        var items = JToken.Parse(response.Content)["items"];

        if (items == null)
        {
            return;
        }
        foreach (var item in items.Children().OrderBy(ks => Guid.NewGuid()))
        {
            var code = Convert.ToString(item["code"]);

            if (string.IsNullOrEmpty(code))
            {
                continue;
            }
            stock.Enqueue(code);
        }
    }
    while (stock.TryDequeue(out string? code))
    {
        if (string.IsNullOrEmpty(code))
        {
            continue;
        }
        foreach (var fs in await FinancialSummary.ExecuteAsync(code))
        {
            fs.Code = code;
            fs.Date = fs.ReceiveDate?[..7].Replace('.', '-');
            fs.Estimated = fs.ReceiveDate?[^2] == 'E';

            response = await client.ExecuteAsync(new RestRequest
            {
                Resource = ShareInvest.Parameter.TransformOutbound(fs.GetType().Name),
                Method = Method.Post
            }.AddJsonBody(fs));

            if (response.StatusCode == HttpStatusCode.OK && int.TryParse(response.Content, out int saveChanges) && saveChanges > 0)
            {
                Console.WriteLine(JsonConvert.SerializeObject(fs, Formatting.Indented));
            }
        }
    }
}