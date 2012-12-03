using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities;
using Ataxo.SlimAPIClient.Entities.Response;
using Ataxo.SlimAPIClient.Entities.Request.Query;
using System.Net;

namespace Ataxo.SlimAPIClient
{
  /// <summary>
  /// default ataxo basic methods to connect slim api functionality
  /// </summary>
  public interface IAtaxoPredefinedMethods
  {
    #region Campaigns
      ResponseData GetCampaignsSync(int? campmapignId = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetCampaignsAsync(int? campaignId = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData GetCampaignsByContractIdSync(int contractId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetCampaignsByContractIdAsync(int contractId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData UpdateCampaignSync(int campaignId, IDictionary<string, object> parameters = null);
      void UpdateCampaignAsync(int campaignId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
    #endregion

    #region Clients
      ResponseData GetClientsSync(int? clientId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetClientsAsync(int? clientId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData CreateClientSync(IDictionary<string, object> parameters = null);
      void CreateClientAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData UpdateClientSync(int clientId, IDictionary<string, object> parameters = null);
      void UpdateClientAsync(int clientId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData DeleteClientSync(int clientId);
      void DeleteClientAsync(int clientId, Action<WebResponse, ResponseData> listener = null);
    #endregion

    #region Contracts
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
    #endregion

    #region Categories
      ResponseData GetCategoriesSync(int? categoryId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetCategoriesAsync(int? categoryId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      ResponseData CreateCategorySync(IDictionary<string, object> parameters = null);
      void CreateCategoryAsync(IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData UpdateCategorySync(int categoryId, IDictionary<string, object> parameters = null);
      void UpdateCategoryAsync(int categoryId, IDictionary<string, object> parameters = null, Action<WebResponse, ResponseData> listener = null);
      ResponseData DeleteCategorySync(int categoryId);
      void DeleteCategoryAsync(int categoryId, Action<WebResponse, ResponseData> listener = null);
    #endregion

    #region Statistics
      ResponseData GetStatisticsSync(int? campaignId, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
      void GetStatisticsAsync(int? campaignId, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
    #endregion
  }
}
