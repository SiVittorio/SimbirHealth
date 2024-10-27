using System;
using SimbirHealth.History.Models.Requests;

namespace SimbirHealth.History.Services.HistoryService;

public interface IHistoryService
{
    Task<IResult> PostHistory(AddOrUpdateHistoryRequest request, 
        string accessToken);
    Task<IResult> PutHistory(Guid historyGuid, AddOrUpdateHistoryRequest request, 
        string accessToken);
    Task<IResult> GetAccountHistories(Guid pacientGuid, string accessToken);
    Task<IResult> GetHistory(Guid historyGuid, string accessToken);
}
