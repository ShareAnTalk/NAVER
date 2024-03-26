using Newtonsoft.Json;

using System.Runtime.Serialization;

namespace ShareInvest;

class FinancialStatements
{
    [DataMember, JsonProperty("YYMM")]
    internal string? Date
    {
        get; set;
    }
    [DataMember, JsonProperty("SALES")]
    internal string? Sales
    {
        get; set;
    }
    [DataMember, JsonProperty("YOY")]
    internal string? YearOnYear
    {
        get; set;
    }
    [DataMember, JsonProperty("OP")]
    internal string? OperatingProfit
    {
        get; set;
    }
    [DataMember, JsonProperty("NP")]
    internal string? NetProfit
    {
        get; set;
    }
    [DataMember, JsonProperty(nameof(EPS))]
    internal string? EPS
    {
        get; set;
    }
    [DataMember, JsonProperty(nameof(BPS))]
    internal string? BPS
    {
        get; set;
    }
    [DataMember, JsonProperty(nameof(PER))]
    internal string? PER
    {
        get; set;
    }
    [DataMember, JsonProperty(nameof(PBR))]
    internal string? PBR
    {
        get; set;
    }
    [DataMember, JsonProperty(nameof(ROE))]
    internal string? ROE
    {
        get; set;
    }
    [DataMember, JsonProperty(nameof(EV))]
    internal string? EV
    {
        get; set;
    }
    [DataMember, JsonProperty("MAIN")]
    internal string? Main
    {
        get; set;
    }
    [DataMember, JsonProperty("TOT_ROW")]
    internal string? Row
    {
        get; set;
    }
}