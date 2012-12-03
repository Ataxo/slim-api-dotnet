using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities;
using Ataxo.SlimAPIClient.Entities.Constants;
using Ataxo.SlimAPIClient.Entities.Response;
using Ataxo.SlimAPIClient.Entities.Request;
using Ataxo.SlimAPIClient.Entities.Request.Query;
using System.Net;
using System.IO;
using Ataxo.SlimAPIClient.Helpers;
using Ataxo.SlimAPIClient.ModelFactory;

namespace Ataxo.SlimAPIClient
{
  /// <summary>
  /// slim api requester implementation of sync and async request calling with needed parameters and predefined ataxo methods of basic data access
  /// </summary>
  public class SlimAPIRequester : IRequesterBase, IAtaxoPredefinedMethods
  {
    private readonly IDataBase _propertiesData;
    public IDataBase PropertiesData
    {
      get
      {
        return _propertiesData;
      }
    }

    /// <summary>
    /// initialises objekt with basic connection parameters
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="taxonomy"></param>
    /// <param name="domainUrl"></param>
    /// <param name="apiVersion"></param>
    public SlimAPIRequester(string accessToken, string taxonomy, string domainUrl, string apiVersion)
    {
      _propertiesData = new RequestParams(accessToken, domainUrl, apiVersion, taxonomy);
    }

    #region virtual methods
    /// <summary>
    /// sends synchronous request to server
    /// </summary>
    /// <param name="requestMethod">request method</param>
    /// <param name="model">model to connect</param>
    /// <param name="additional_concretization">additional sub_url added behind the backslash</param>
    /// <param name="requestParams">params of request usually used to update database on server</param>
    /// <param name="queryItems">specifies server data filtration usually by get request</param>
    /// <param name="granularity">specifies pagination, limit and order of returning data</param>
    /// <returns></returns>
    public virtual ResponseData SendRequestSync(RequestMethod requestMethod, string model, string additional_concretization = null, IDictionary<string, object> requestParams = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
    {
      HttpWebRequest request = CreateRequest(requestMethod, model, additional_concretization, requestParams, queryItems, granularity);
      request.SetStreamData(requestParams);

      string result;

      try
      {
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
          result = GetResponseString(response);
        }
      }
      catch (WebException ex)
      {
        result = GetResponseString(ex.Response);
      }
      catch (Exception ex)
      {
        throw ex;
      }

      return new ResponseData(result);
    }

    /// <summary>
    /// catches asynchronous request result
    /// </summary>
    /// <param name="asynchronousResult"></param>
    public virtual void RespCallback(IAsyncResult asynchronousResult)
    {
      Tuple<HttpWebRequest, Action<WebResponse, ResponseData>> asyncResultParams = (Tuple<HttpWebRequest, Action<WebResponse, ResponseData>>)asynchronousResult.AsyncState;
      Action<WebResponse, ResponseData> action = asyncResultParams.Item2;
      HttpWebResponse response = null;

      try
      {
        response = (HttpWebResponse)asyncResultParams.Item1.EndGetResponse(asynchronousResult);
      }
      catch (WebException ex)
      {
        string result = GetResponseString(ex.Response);
        if(action != null)
        {
          action(ex.Response, new ResponseData(result));
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

      if (action != null)
      {
        action(response, new ResponseData(GetResponseString(response)));
      }
    }

    /// <summary>
    /// sends asynchronous request to server
    /// </summary>
    /// <param name="requestMethod">request method</param>
    /// <param name="model">model to connect</param>
    /// <param name="additional_concretization">additional sub_url added behind the backslash</param>
    /// <param name="requestParams">params of request usually used to update database on server</param>
    /// <param name="queryItems">specifies server data filtration usually by get request</param>
    /// <param name="granularity">specifies pagination, limit and order of returning data</param>
    /// <returns></returns>
    public virtual void SendRequestAsync(RequestMethod requestMethod, string model, string additional_concretization = null, IDictionary<string, object> requestParams = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
    {
      HttpWebRequest request = CreateRequest(requestMethod, model, additional_concretization, requestParams, queryItems, granularity);
      request.SetStreamData(requestParams);

      try
      {
        IAsyncResult result = request.BeginGetResponse(new AsyncCallback(RespCallback), new Tuple<HttpWebRequest, Action<WebResponse, ResponseData>>(request, listener));
      }
      catch (WebException ex)
      {
        string result = GetResponseString(ex.Response);
        listener(ex.Response, new ResponseData(result));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// generates null values model params dictionary
    /// </summary>
    /// <param name="model"></param>
    /// <returns>model of slim api (campaign, contract, ..)</returns>
    public virtual Dictionary<string, object> GenerateModelDictionary(string model)
    {
      return ModelGenerator.GenerateModelDictionary(model);
    }
    #endregion

    #region private methods
    /// <summary>
    /// converts WebResponse to string
    /// </summary>
    /// <param name="response">WebResponse</param>
    /// <returns></returns>
    private string GetResponseString(WebResponse response)
    {
      string result = null;

      Encoding encoding = Encoding.GetEncoding(((HttpWebResponse)response).CharacterSet);
      using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
      {
        result = sr.ReadToEnd();
      }

      return result;
    }

    /// <summary>
    /// creates HttpWebRequest including IDataBase data, request params, query filter and granularity
    /// </summary>
    /// <param name="requestMethod">RequestMethod</param>
    /// <param name="model">Ataxo.SlimAPIClient.Entities.Constants.Model</param>
    /// <param name="requestParams">params od model usually used for updating</param>
    /// <param name="queryItems">filter params</param>
    /// <param name="granularity">Granularity</param>
    /// <returns></returns>
    protected HttpWebRequest CreateRequest(RequestMethod requestMethod, string model, string additional_concretization = null, IDictionary<string, object> requestParams = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
    {
      string url = ConstrucUrl(PropertiesData, model, additional_concretization, queryItems, granularity);
      Uri uri = new Uri(url);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.ContentType = "application/json";
      request.Headers["Api-Token"] = PropertiesData.AccessToken;
      request.Method = requestMethod.ToString().ToUpper();

      return request;
    }

    protected string ConstrucUrl(IDataBase data, string model, string additional_concretization = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
    {
      return string.Format("{0}/{1}/{2}/{3}{4}?{5}", data.DomainUrl, data.ApiVersion, data.Taxonomy, model, additional_concretization == null ? "" : string.Format("/{0}", additional_concretization), QueryManager.GenerateQuery(queryItems, granularity));
    }
    #endregion

    #region predefined ataxo methods
      #region Campaigns
        /// <summary>
        /// gets campaigns synchronously
        /// </summary>
        /// <param name="campaignId">if campaign is null all campaigns are returned considering granularity and queriItems</param>
        /// <param name="granularity">defines limit, offset and ordering of result</param>
        /// <param name="queryItems">result specification by filter</param>
        /// <returns></returns>
        public ResponseData GetCampaignsSync(int? campaignId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (campaignId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Campaign.Id, QueryOperators.Equals, new List<object>() { campaignId }));
          }

          return SendRequestSync(RequestMethod.Get, Model.Campaigns, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets campaigns asynchronously
        /// </summary>
        /// <param name="campaignId">if campaign is null all campaigns are returned considering granularity and queriItems</param>
        /// <param name="granularity">defines limit, offset and ordering of result</param>
        /// <param name="queryItems">result specification by filter</param>
        /// <returns></returns>
        public void GetCampaignsAsync(int? campaignId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (campaignId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Campaign.Id, QueryOperators.Equals, new List<object>() { campaignId }));
          }

          SendRequestAsync(RequestMethod.Get, Model.Campaigns, null, null, listener, queryItems, granularity);
        }

        /// <summary>
        /// gets campaign by contract id synchronously
        /// </summary>
        /// <param name="contractId">contract id</param>
        /// <param name="granularity">defines limit, offset and ordering of result</param>
        /// <param name="queryItems">result specification by filter</param>
        /// <returns></returns>
        public ResponseData GetCampaignsByContractIdSync(int contractId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (queryItems == null)
          {
            queryItems = new List<QueryItem>();
          }

          ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Campaign.ContractId, QueryOperators.Equals, new List<object>() { contractId }));

          return SendRequestSync(RequestMethod.Get, Model.Campaigns, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets campaign by contract id asynchronously
        /// </summary>
        /// <param name="contractId">contract id</param>
        /// <param name="granularity">defines limit, offset and ordering of result</param>
        /// <param name="queryItems">result specification by filter</param>
        /// <returns></returns>
        public void GetCampaignsByContractIdAsync(int contractId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (queryItems == null)
          {
            queryItems = new List<QueryItem>();
          }

          ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Campaign.ContractId, QueryOperators.Equals, new List<object>() { contractId }));

          SendRequestAsync(RequestMethod.Get, Model.Campaigns, null, null, listener, queryItems, granularity);
        }

        /// <summary>
        /// updates campaign by id synchronously
        /// </summary>
        /// <param name="campaignId">campaign id</param>
        /// <param name="granularity">defines limit, offset and ordering of result</param>
        /// <param name="queryItems">result specification by filter</param>
        /// <returns></returns>
        public ResponseData UpdateCampaignSync(int campaignId, IDictionary<string, object> parameters = null)
        {
          parameters[RequestParamKeys.Campaign.Id] = campaignId.ToString();
          
          return SendRequestSync(RequestMethod.Put, Model.Campaigns, campaignId.ToString(), parameters, null, null);
        }

        /// <summary>
        /// updates campaign by id asynchronously
        /// </summary>
        /// <param name="campaignId">campaign id</param>
        /// <param name="granularity">defines limit, offset and ordering of result</param>
        /// <param name="queryItems">result specification by filter</param>
        /// <returns></returns>
        public void UpdateCampaignAsync(int campaignId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          parameters[RequestParamKeys.Campaign.Id] = campaignId.ToString();

          SendRequestAsync(RequestMethod.Put, Model.Campaigns, campaignId.ToString(), parameters, listener, null, null);
        }
      #endregion

      #region Clients
        /// <summary>
        /// get clientss
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData GetClientsSync(int? clientId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (clientId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Client.Id, QueryOperators.Equals, new List<object>() { clientId }));
          }

          return SendRequestSync(RequestMethod.Get, Model.Clients, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets clients
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void GetClientsAsync(int? clientId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (clientId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Client.Id, QueryOperators.Equals, new List<object>() { clientId }));
          }

          SendRequestAsync(RequestMethod.Get, Model.Clients, null, null, listener, queryItems, granularity);
        }

        /// <summary>
        /// creates client
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData CreateClientSync(IDictionary<string, object> parameters = null)
        {
          return SendRequestSync(RequestMethod.Post, Model.Clients, null, parameters, null, null);
        }

        /// <summary>
        /// creates client
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void CreateClientAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          SendRequestAsync(RequestMethod.Post, Model.Clients, null, parameters, listener, null, null);
        }

        /// <summary>
        /// updates client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="parameters"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData UpdateClientSync(int clientId, IDictionary<string, object> parameters = null)
        {
          parameters[RequestParamKeys.Client.Id] = clientId.ToString();

          return SendRequestSync(RequestMethod.Put, Model.Clients, clientId.ToString(), parameters, null, null);
        }

        /// <summary>
        /// updates client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="parameters"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void UpdateClientAsync(int clientId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          parameters[RequestParamKeys.Client.Id] = clientId.ToString();

          SendRequestAsync(RequestMethod.Put, Model.Clients, clientId.ToString(), parameters, listener, null, null);
        }

        /// <summary>
        /// deletes client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData DeleteClientSync(int clientId)
        {
          return SendRequestSync(RequestMethod.Delete, Model.Clients, clientId.ToString(), null, null, null);
        }

        /// <summary>
        /// deletes client by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void DeleteClientAsync(int clientId, Action<WebResponse, ResponseData> listener = null)
        {
          SendRequestAsync(RequestMethod.Delete, Model.Clients, clientId.ToString(), null, listener, null, null);
        }
      #endregion

      #region Contracts
        /// <summary>
        /// gets contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData GetContractsSync(int? contractId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (contractId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Contract.Id, QueryOperators.Equals, new List<object>() { contractId }));
          }

          return SendRequestSync(RequestMethod.Get, Model.Contracts, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets contracts
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void GetContractsAsync(int? contractId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (contractId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Contract.Id, QueryOperators.Equals, new List<object>() { contractId }));
          }

          SendRequestAsync(RequestMethod.Get, Model.Contracts, null, null, listener, queryItems, granularity);
        }

        /// <summary>
        /// gets contracts by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData GetContractsByClinetIdSync(int clientId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (queryItems == null)
          {
            queryItems = new List<QueryItem>();
          }

          ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Contract.ClientId, QueryOperators.Equals, new List<object>() { clientId }));

          return SendRequestSync(RequestMethod.Get, Model.Contracts, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets contracts by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void GetContractsByClinetIdASync(int clientId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (queryItems == null)
          {
            queryItems = new List<QueryItem>();
          }

          ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Contract.ClientId, QueryOperators.Equals, new List<object>() { clientId }));

          SendRequestAsync(RequestMethod.Get, Model.Clients, null, null, listener, queryItems, granularity);
        }

        /// <summary>
        /// creates contract
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData CreateContractSync(IDictionary<string, object> parameters = null)
        {
          return SendRequestSync(RequestMethod.Post, null, Model.Contracts, parameters, null, null);
        }

        /// <summary>
        /// creates contract
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void CreateContractAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          SendRequestAsync(RequestMethod.Post, Model.Contracts, null, parameters, listener, null, null);
        }

        /// <summary>
        /// updates contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="parameters"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData UpdateContractSync(int contractId, IDictionary<string, object> parameters = null)
        {
          parameters[RequestParamKeys.Contract.Id] = contractId.ToString();

          return SendRequestSync(RequestMethod.Put, Model.Contracts, contractId.ToString(), parameters, null, null);
        }

        /// <summary>
        /// updates contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="parameters"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void UpdateContractAsync(int contractId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          parameters[RequestParamKeys.Contract.Id] = contractId.ToString();

          SendRequestAsync(RequestMethod.Put, Model.Contracts, null, parameters, listener, null, null);
        }

        /// <summary>
        /// deletes contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData DeleteContractSync(int contractId)
        {
          return SendRequestSync(RequestMethod.Delete, Model.Contracts, contractId.ToString(), null, null, null);
        }

        /// <summary>
        /// deletes contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void DeleteContracttAsync(int contractId, Action<WebResponse, ResponseData> listener = null)
        {
          List<QueryItem> queryItems = new List<QueryItem>();
          ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Contract.Id, QueryOperators.Equals, new List<object>() { contractId }));

          SendRequestAsync(RequestMethod.Delete, Model.Contracts, contractId.ToString(), null, listener, null, null);
        }
      #endregion

      #region Categories
        /// <summary>
        /// gets categories
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData GetCategoriesSync(int? categoryId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (categoryId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Category.Id, QueryOperators.Equals, new List<object>() { categoryId }));
          }

          return SendRequestSync(RequestMethod.Get, Model.Categories, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets categories
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void GetCategoriesAsync(int? categoryId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (categoryId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Category.Id, QueryOperators.Equals, new List<object>() { categoryId }));
          }

          SendRequestAsync(RequestMethod.Get, Model.Categories, null, null, listener, queryItems, granularity);
        }

        /// <summary>
        /// creates categoriy
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData CreateCategorySync(IDictionary<string, object> parameters = null)
        {
          return SendRequestSync(RequestMethod.Post, Model.Categories, null, parameters, null, null);
        }

        /// <summary>
        /// creates category
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void CreateCategoryAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          SendRequestAsync(RequestMethod.Post, Model.Categories, null, parameters, listener, null, null);
        }

        /// <summary>
        /// updates category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="parameters"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData UpdateCategorySync(int categoryId, IDictionary<string, object> parameters = null)
        {
          parameters[RequestParamKeys.Category.Id] = categoryId.ToString();

          return SendRequestSync(RequestMethod.Put, Model.Categories, categoryId.ToString(), parameters, null, null);
        }

        /// <summary>
        /// updates category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="parameters"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void UpdateCategoryAsync(int categoryId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null)
        {
          parameters[RequestParamKeys.Category.Id] = categoryId.ToString();

          SendRequestAsync(RequestMethod.Put, Model.Categories, categoryId.ToString(), parameters, listener, null, null);
        }

        /// <summary>
        /// deletes category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData DeleteCategorySync(int categoryId)
        {
          return SendRequestSync(RequestMethod.Delete, Model.Categories, categoryId.ToString(), null, null, null);
        }

        /// <summary>
        /// deletes category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void DeleteCategoryAsync(int categoryId, Action<WebResponse, ResponseData> listener = null)
        {
          SendRequestAsync(RequestMethod.Delete, Model.Categories, categoryId.ToString(), null, listener, null, null);
        }
      #endregion

      #region Statistics
        /// <summary>
        /// gets statistics
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        /// <returns></returns>
        public ResponseData GetStatisticsSync(int? campaignId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (campaignId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Statistics.CampaignId, QueryOperators.Equals, new List<object>() { campaignId }));
          }

          return SendRequestSync(RequestMethod.Get, Model.Statistics, null, null, queryItems, granularity);
        }

        /// <summary>
        /// gets statistics
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="listener"></param>
        /// <param name="granularity"></param>
        /// <param name="queryItems"></param>
        public void GetStatisticsAsync(int? campaignId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null)
        {
          if (campaignId != null)
          {
            if (queryItems == null)
            {
              queryItems = new List<QueryItem>();
            }

            ((List<QueryItem>)queryItems).Add(new QueryItem(RequestParamKeys.Statistics.CampaignId, QueryOperators.Equals, new List<object>() { campaignId }));
          }

          SendRequestAsync(RequestMethod.Get, Model.Statistics, null, null, listener, queryItems, granularity);
        }
      #endregion
    #endregion

  }
}
