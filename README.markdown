# Slim-API c# #

###Basic definition

- Rest Api principle
- enables manipulating with ATAXO database
- it is possible to filter and granulate request result
- supports predefined Ataxo methods and virtual basic methods for customization
- supports constants and basic enumerations
- all information are available at http://slimapi.stage.ataxo.com

###Code overview
- ***SlimAPIRequester*** is the main object have to be initialized
- requires ***AccessToken***, ***Taxonomy***, ***DomainUrl*** and ***APiVersion*** parameters in constructor to be defined
- ***SlimAPIRequester*** inherits from ***IRequesterBase*** interface which consists of basic methods for requesting:
  ***SendRequestSync()***, ***SendRequestAsync()*** and ***RespCallback()*** as a callback method of asynchronous sending result (they are implemented as virtual)
- ***SendRequestAsync()*** expects ***Action< Webresponse, ResponseData >*** delegate to be passed compared to ***SendRequestSync()***
- parameters to pass into sending methods are:
  ***RequestMethod*** requestMethod - represents one of the possibilities : ***POST, PUT, GET, DEL***  
  string model - represents name of model to be manipulating with (***Model.Campaigns, Model.Clients, ...***) model constatnts are accesible from ***Ataxo.SlimAPIClient.Entities.Constants*** namespace   
 string additional_concretization - defines additional path of url to be added after the domain url 
  ***IDictionary< string, object >*** requestParams - parameters included to the request (generally in case of POST or PUT are used)  
  ***IEnumerable< QueryItem >*** queryItems - represent filtration of result (limitation of request records generally used in GET requests)   
  ***Granularity*** granularity - limit, offset and ordering is defined here
- return object is represented by ResponseData class containing raw json string as a request result and it's structured Dictionary<string, object> representation
- there is possibility to generate empty Dictionary by ***GenerateModelDictionary*** method depending on concrete model


###Predefined ataxo methods
####Campaigns:
      ResponseData GetCampaignsSync(int? campmapignId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetCampaignsAsync(int? campaignId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData GetCampaignsByContractIdSync(int contractId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetCampaignsByContractIdAsync(int contractId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData UpdateCampaignSync(int campaignId, IDictionary<string, object> parameters = null);
      void UpdateCampaignAsync(int campaignId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);

####Clients:
      ResponseData GetClientsSync(int? clientId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetClientsAsync(int? clientId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData CreateClientSync(IDictionary<string, object> parameters = null);
      void CreateClientAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData UpdateClientSync(int clientId, IDictionary<string, object> parameters = null);
      void UpdateClientAsync(int clientId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData DeleteClientSync(int clientId);
      void DeleteClientAsync(int clientId, Action<WebResponse, ResponseData> listener = null);

####Contracts:
      ResponseData GetContractsSync(int? contractId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetContractsAsync(int? contractId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData GetContractsByClinetIdSync(int clientId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetContractsByClinetIdASync(int clientId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData CreateContractSync(IDictionary<string, object> parameters = null);
      void CreateContractAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData UpdateContractSync(int contractId, IDictionary<string, object> parameters = null);
      void UpdateContractAsync(int contractId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData DeleteContractSync(int contractId);
      void DeleteContracttAsync(int contractId, Action<WebResponse, ResponseData> listener = null);

####Categories:
      ResponseData GetCategoriesSync(int? categoryId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetCategoriesAsync(int? categoryId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData CreateCategorySync(IDictionary<string, object> parameters = null);
      void CreateCategoryAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData UpdateCategorySync(int categoryId, IDictionary<string, object> parameters = null);
      void UpdateCategoryAsync(int categoryId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData DeleteCategorySync(int categoryId);
      void DeleteCategoryAsync(int categoryId, Action<WebResponse, ResponseData> listener = null);

####Statistics:
      ResponseData GetStatisticsSync(int? campaignId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetStatisticsAsync(int? campaignId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);


###Examples
####initialization of query params:

      queryItems = new List<QueryItem>();
      QueryItem item = new QueryItem(RequestParamKeys.Campaign.LimitImpressions, QueryOperators.Equals, "0");
      queryItems.Add(item);

####initialization of request params:

      client = new Dictionary<string, string>();	
      client[RequestParamKeys.Client.Name] = "TEST NAME";
      client[RequestParamKeys.Client.Email] = "test@test.cz";
      result = slimApiRequester.CreateClientSync(client);
      slimApiRequester.CreateClientAsync(client, action);
 
####initialization of granularity:
      granularity = new Granularity();
      granularity.Limit = 3;
      granularity.PropertyOrders = new List<PropertyOrder>();

      PropertyOrder propertyOrder = new PropertyOrder();
      propertyOrder.PropertyName = RequestParamKeys.Campaign.Id;
      propertyOrder.Order = Ordering.Ascending;

      PropertyOrder propertyOrder1 = new PropertyOrder();
      propertyOrder1.PropertyName = RequestParamKeys.Campaign.LimitClicks;
      propertyOrder1.Order = Ordering.Ascending;

      granularity.PropertyOrders.Add(propertyOrder);
      granularity.PropertyOrders.Add(propertyOrder1);

####generating of campaign model empty params dictionary:
      SlimAPIRequester slimApiRequester = new SlimAPIRequester(accessToken, taxonomy, domainUrl, apiVersion);
      Dictionary<string, object> gen = slimApiRequester.GenerateModelDictionary(Model.Campaigns);

####getting first contract:
      var result = slimApiRequester.GetContractsSync(12345678);
      Dictionary<string, object> contract = ((Dictionary<string, object>)((object[])result.ResponseParams[Model.Contracts])[0]).ToDictionary(d => d.Key, d => d.Value);