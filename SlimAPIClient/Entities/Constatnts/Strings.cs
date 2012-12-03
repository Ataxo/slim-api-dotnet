using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ataxo.SlimAPIClient.Entities.Constants
{
  /// <summary>
  /// model strings
  /// </summary>
  public class Model
  {
    public const string Campaigns = "campaigns";
    public const string Clients = "clients";
    public const string Contracts = "contracts";
    public const string Categories = "categories";
    public const string Statistics = "statistics";
  }

  /// <summary>
  /// currency variants
  /// </summary>
  public class Currency
  {
    public const string Czk = "czk";
    public const string Eur = "eur";
    public const string Pln = "pln";
  }

  /// <summary>
  /// basic campign statuses
  /// </summary>
  public class Status
  {
    public const string Storned = "storned";
    public const string StornoRequest = "storno_request";
    public const string ReturnRequest = "return_request";
    public const string Updated = "updated";
    public const string ChangeRequest = "change_request";
    public const string Created = "created";
    public const string Started = "started";
    public const string Paused = "paused";
    public const string Finished = "finished";
    public const string Failed = "failed";
    public const string Pending = "pending";
  }

  /// <summary>
  /// country variants
  /// </summary>
  public class Country
  {
    public const string Cz = "cz";
    public const string Sk = "sk";
    public const string Pl = "pl";
  }

  /// <summary>
  /// granularity query strings mapping
  /// </summary>
  public class GranularityQuery
  {
    public const string Order = "order";
    public const string Limit = "limit";
    public const string Offset = "offset";
  }

  /// <summary>
  /// mapping of parameters for specific models
  /// </summary>
  public class RequestParamKeys
  {
    [KeysAttributte(factoriable = true, model = Model.Campaigns)]
    public class Campaign
    {
      public const string Id = "id";
      public const string ClientId = "client_id";	
      public const string ContractId = "contract_id";	
      public const string AdminId = "admin_id";
      public const string Admin = "admin";
      public const string RealUrl = "real_url";
      public const string DatWillStart = "date_will_start";
      public const string DateCreated = "date_created";
      public const string DateStarted = "date_started";
      public const string DateFinished = "date_finished";
      public const string DatePaused = "date_paused";
      public const string LimitClicks = "limit_clicks";
      public const string LimitImpressions = "limit_impressions";
      public const string LimitPrice = "limit_price";
      public const string Period = "period";
      public const string Currency = "currency";
      public const string Status = "status";

    }

    [KeysAttributte(factoriable = true, model = Model.Clients)]
    public class Client
    {
      public const string Id = "id";
      public const string Name = "name";
      public const string Ico = "ico";
      public const string Email = "email";
      public const string Country = "country";	
      public const string OriginalId = "original_id";	
      public const string DatCreated = "date_created";	
      public const string Adress = "address";
      public const string Phone = "phone";
      public const string PostalCode = "postal_code";
    }

    [KeysAttributte(factoriable = true, model = Model.Contracts)]
    public class Contract
    {
      public const string Id = "id";
      public const string ClientId = "client_id";	
      public const string AdminId = "admin_id";
      public const string Admin = "admin";
      public const string CategoryId = "category_id";	
      public const string CategoryName = "category";
      public const string RenewalId = "renewal_id";
      public const string Email = "email";
      public const string Product = "product";
      public const string LimitClicks = "limit_clicks";
      public const string LimitImpressions = "limit_impressions";
      public const string LimitPrice = "limit_price";
      public const string Period = "period";
      public const string Currency = "currency";
      public const string DateCreated = "date_created";
      public const string DateWillStart = "date_will_start";
      public const string DateStorned = "date_storned";
      public const string LandingPage = "landing_page";
      public const string TargetUrl = "target_url";
      public const string Geotargeting = "geotargeting";
      public const string Keywords = "keywords";
      public const string Note = "note"; 
    }

    [KeysAttributte(factoriable = true, model = Model.Categories)]
    public class Category
    {
      public const string Id = "id";
      public const string Name = "category";
    }

    [KeysAttributte(factoriable = true, model = Model.Statistics)]
    public class Statistics
    {
      public const string CampaignId = "campaign_id";
    }
  }
}
