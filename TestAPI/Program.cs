using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient;
using Ataxo.SlimAPIClient.Entities;
using Ataxo.SlimAPIClient.Entities.Response;
using Ataxo.SlimAPIClient.Entities.Request.Query;
using System.Net;
using System.Threading;
using Ataxo.SlimAPIClient.ModelFactory;
using Ataxo.SlimAPIClient.Entities.Constants;

namespace Ataxo.TestAPI
{
  class Program
  {
    private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
    private static object _locker = new object();

    static void Main(string[] args)
    {
      string accessToken = "f2bbe88119e71cc31f87f0f35eb0951f23ac2873";
      string taxonomy = "sandbox";
      string domainUrl = "http://slimapi.stage.ataxo.com";
      string apiVersion = "v1";

      SlimAPIRequester slimApiRequester = new SlimAPIRequester(accessToken, taxonomy, domainUrl, apiVersion);
      Dictionary<string, object> gen = slimApiRequester.GenerateModelDictionary(Model.Campaigns);
      //List<QueryItem> queryItems;

      Granularity granularity;
      ResponseData result = null;


      //ResponseData responseData = slimApiRequester.GetCampaignsSync(dict);
      try
      {
        Action<WebResponse, ResponseData> action = AsyncHandler;

        /*
        TmpInitQueryItems(out queryItems);
        TmpInitGranularity(out granularity);
        result = slimApiRequester.GetCampaignsSync(null, queryItems, granularity);
        slimApiRequester.GetCampaignsAsync(1548808, action);
         
        result = slimApiRequester.GetCampaignsByContractIdSync(12345685);
        slimApiRequester.GetCampaignsByContractIdAsync(12345685, action);

        Dictionary<string, string> parameters;
        TmpInitCampaignUpdateParams(out parameters);
        result = slimApiRequester.UpdateCampaignSync(1219, parameters);
        slimApiRequester.UpdateCampaignAsync(1219, parameters, action);
        
        result = slimApiRequester.GetClientsSync(3923);
        slimApiRequester.GetClientsAsync(3923, action);

        result = slimApiRequester.GetClientsSync(123456799);

        Dictionary<string, string> client = ((Dictionary<string, object>)((object[])result.ResponseParams[Model.Clients])[0]).ToDictionary(d => d.Key, d => d.Value);
        client[RequestParamKeys.Client.Id] = null;
        client[RequestParamKeys.Client.Name] = "TEST NAME";
        client[RequestParamKeys.Client.Email] = "test@test.cz";
        result = slimApiRequester.CreateClientSync(client);
        slimApiRequester.CreateClientAsync(client, action);

        client[RequestParamKeys.Client.Phone] = "123456789";
        result = slimApiRequester.UpdateClientSync(123456799, client);
        slimApiRequester.UpdateClientAsync();

        result = slimApiRequester.DeleteClientSync(123456799);
        slimApiRequester.DeleteClientAsync();

        result = slimApiRequester.GetContractsSync();
        slimApiRequester.GetContractsAsync();

        result = slimApiRequester.GetContractsSync(12345678);
        Dictionary<string, object> contract = ((Dictionary<string, object>)((object[])result.ResponseParams[Model.Contracts])[0]).ToDictionary(d => d.Key, d => d.Value);
        slimApiRequester.GetContractsByClinetIdASync()

        contract[RequestParamKeys.Contract.Id] = null;
        contract[RequestParamKeys.Contract.CategoryName] = "CCCCCC";
        result = slimApiRequester.CreateContractSync(contract);
        slimApiRequester.CreateContractAsync();

        result = slimApiRequester.UpdateContractSync(12345678, contract);
        slimApiRequester.UpdateContractAsync();

        result = slimApiRequester.DeleteContractSync(12345679);

        List<QueryItem> queryItems = new List<QueryItem>();
        queryItems.Add(new QueryItem()
        {
          Parameter = RequestParamKeys.Category.Id,
          QueryOperator = QueryOperators.Containing,
          Values = new List<object> { 100224, 100616 }
        });
        result = slimApiRequester.GetCategoriesSync(null, null);

        Dictionary<string, object> category = (Dictionary<string, object>) ((object[])result.ResponseParams[Model.Categories])[0];

        category[RequestParamKeys.Category.Id] = null;
        category[RequestParamKeys.Category.Name] = "AHOJ";
        slimApiRequester.GetCategoriesAsync();
        result = slimApiRequester.CreateCategorySync(category);
        slimApiRequester.CreateCategoryAsync();

        result = slimApiRequester.GetStatisticsSync();
        slimApiRequester.GetStatisticsAsync();

        slimApiRequester.GetCampaignsAsync(dict, action);

        ResponseData responseData = slimApiRequester.GetCampaignsByContractIdSync(1219, dict, granularity);
        slimApiRequester.GetCampaignsByContractIdAsync(1219, dict, action, granularity);

        dict.Add("period", "3");
        result = slimApiRequester.UpdateCampaignSync(1219, dict, granularity);

        */
      }
      catch (Exception ex)
      {
      }


      _manualResetEvent.WaitOne();
    }

    private static void TmpInitGranularity(out Granularity granularity)
    {
      granularity = new Granularity();
      granularity.Limit = 3;
      //granularity.Offset = 1;
      granularity.PropertyOrders = new List<PropertyOrder>();

      PropertyOrder propertyOrder = new PropertyOrder();
      propertyOrder.PropertyName = RequestParamKeys.Campaign.Id;
      propertyOrder.Order = Ordering.Ascending;

      PropertyOrder propertyOrder1 = new PropertyOrder();
      propertyOrder1.PropertyName = RequestParamKeys.Campaign.LimitClicks;
      propertyOrder1.Order = Ordering.Ascending;

      granularity.PropertyOrders.Add(propertyOrder);
      granularity.PropertyOrders.Add(propertyOrder1);
    }

    private static void TmpInitCampaignUpdateParams(out Dictionary<string, string> parameters)
    {
      parameters = new Dictionary<string, string>();
      parameters.Add(RequestParamKeys.Campaign.Period, "1111");
    }

    private static void TmpInitQueryItems(out List<QueryItem> queryItems)
    {
      queryItems = new List<QueryItem>();

      QueryItem item = new QueryItem(RequestParamKeys.Campaign.LimitImpressions, QueryOperators.Equals, "0");

      queryItems.Add(item);
    }

    public static void AsyncHandler(WebResponse webResponse, ResponseData repData)
    {
      lock(_locker)
      {
        _manualResetEvent.Set();
      }
    }
  }
}
