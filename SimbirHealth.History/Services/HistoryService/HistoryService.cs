using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Common.Services.Account;
using SimbirHealth.Common.Services.Db.Repositories;
using SimbirHealth.Common.Services.Web.AuthValidationService;
using SimbirHealth.Common.Services.Web.ExternalApiService;
using SimbirHealth.Data.Models.Account;
using SimbirHealth.Data.Models.History;
using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;
using SimbirHealth.History.Models.Requests;
using SimbirHealth.History.Models.Responses;

namespace SimbirHealth.History.Services.HistoryService;

public class HistoryService : IHistoryService
{
    private readonly IExternalApiService _externalApiService;
    private readonly IAuthValidationService _authValidationService;
    private readonly IRepositoryBase<HistoryModel> _historyRepository;

    public HistoryService(IExternalApiService externalApiService,
        IAuthValidationService authValidationService,
        IRepositoryBase<HistoryModel> historyRepository)
    {
        _externalApiService = externalApiService;
        _authValidationService = authValidationService;
        _historyRepository = historyRepository;
    }

    public async Task<IResult> PostHistory(AddOrUpdateHistoryRequest request, 
        string accessToken){
        var doctor = await _externalApiService.GetDoctorByGuid(request.DoctorId, accessToken);
        var pacient = await _externalApiService.GetAccountByGuid(request.PacientId, accessToken);
        var hospital = await _externalApiService.GetHospitalByGuid(request.HospitalId, accessToken);
        var rooms = await _externalApiService.GetHospitalRoomsByGuid(request.HospitalId, accessToken);

        if (doctor != null && pacient != null &&
            pacient.Roles != null && hospital != null && rooms != null && 
            pacient.Roles!.Any(r => r.RoleName == PossibleRoles.User) &&
            rooms.Any(r => r.RoomName == request.RoomName)){
            
            _historyRepository.Add(
                new(){
                    Date = request.Date,
                    Data = request.Data,
                    PacientGuid = pacient.Guid,
                    DoctorGuid = doctor.Guid,
                    HospitalGuid = hospital.Guid,
                    RoomGuid = rooms.First(r => r.RoomName == request.RoomName).RoomGuid
                }
            );

            await _historyRepository.SaveChangesAsync();
            return Results.Ok();
        }

        return Results.BadRequest();
    }


    public async Task<IResult> PutHistory(Guid historyGuid, AddOrUpdateHistoryRequest request, 
        string accessToken){
        var historyModel = await _historyRepository.Query().FirstOrDefaultAsync(h => h.Guid == historyGuid);
        var doctor = await _externalApiService.GetDoctorByGuid(request.DoctorId, accessToken);
        var pacient = await _externalApiService.GetAccountByGuid(request.PacientId, accessToken);
        var hospital = await _externalApiService.GetHospitalByGuid(request.HospitalId, accessToken);
        var rooms = await _externalApiService.GetHospitalRoomsByGuid(request.HospitalId, accessToken);

        if (historyModel != null && doctor != null && pacient != null &&
            pacient.Roles != null && hospital != null && rooms != null && 
            pacient.Roles!.Any(r => r.RoleName == PossibleRoles.User) &&
            rooms.Any(r => r.RoomName == request.RoomName)){
            
            historyModel.Date = request.Date;
            historyModel.Data = request.Data;
            historyModel.PacientGuid = pacient.Guid;
            historyModel.DoctorGuid = doctor.Guid;
            historyModel.HospitalGuid = hospital.Guid;
            historyModel.RoomGuid = rooms.First(r => r.RoomName == request.RoomName).RoomGuid;

            _historyRepository.Update(historyModel);
            await _historyRepository.SaveChangesAsync();
            return Results.Ok();
        }

        return Results.BadRequest();
    }


    public async Task<IResult> GetAccountHistories(Guid pacientGuid, string accessToken){
        var pacient = await _externalApiService.GetAccountByGuid(pacientGuid, accessToken);
        var claims = await _externalApiService.ValidateToken(accessToken);
        
        if (claims == null || pacient == null) return Results.BadRequest();

        _authValidationService.LoadClaims(claims);
        
        if (!_authValidationService.ContainsSpecificRoles([PossibleRoles.Doctor]) &&
            !_authValidationService.GuidsEqual(pacient.Guid))
            return Results.Forbid();
        
        var hists = await _historyRepository.Query()
            .Where(h => h.PacientGuid == pacientGuid)
            .ToListAsync();
        
        Dictionary<Guid, DoctorResponse> doctors = new();
        Dictionary<Guid, HospitalResponse> hospitals = new();
        Dictionary<Guid, List<RoomResponse>> roomsDict = new();

        var response = new List<GetHistoryResponse>();
        foreach (var hist in hists)
        {
            DoctorResponse? doctor;
            if(!doctors.TryGetValue(hist.DoctorGuid, out doctor))
                doctor = await _externalApiService.GetDoctorByGuid(hist.DoctorGuid, accessToken);
            HospitalResponse? hospital;
            if(!hospitals.TryGetValue(hist.HospitalGuid, out hospital))
                hospital = await _externalApiService.GetHospitalByGuid(hist.HospitalGuid, accessToken);
            List<RoomResponse>? rooms;
            if(!roomsDict.TryGetValue(hist.HospitalGuid, out rooms))
                rooms = await _externalApiService.GetHospitalRoomsByGuid(hist.HospitalGuid, accessToken);

            if (doctor == null || hospital == null ||
                rooms.IsNullOrEmpty() ||
                !rooms!.Any(r => r.RoomGuid == hist.RoomGuid)) return Results.NotFound();
            
            response.Add(new(
                hist.Date, hist.Data, 
                hospital.Name, string.Join(" ", pacient.LastName, pacient.FirstName),
                string.Join(" ", doctor.LastName, doctor.FirstName),
                rooms!.First(r => r.RoomGuid == hist.RoomGuid).RoomName
            ));

        }

        if (response.IsNullOrEmpty()) return Results.NotFound();
        return Results.Ok(response);
    }

    public async Task<IResult> GetHistory(Guid historyGuid, string accessToken){
        var claims = await _externalApiService.ValidateToken(accessToken);
        var history = await _historyRepository.Query().FirstOrDefaultAsync(h => h.Guid == historyGuid);
        
        if (claims == null || history == null) return Results.BadRequest();
        var pacient = await _externalApiService.GetAccountByGuid(history.PacientGuid, accessToken);

        if (pacient == null) return Results.NotFound();

        _authValidationService.LoadClaims(claims);
        
        if (!_authValidationService.ContainsSpecificRoles([PossibleRoles.Doctor]) &&
            !_authValidationService.GuidsEqual(pacient.Guid))
            return Results.Forbid();
        
        var doctor = await _externalApiService.GetDoctorByGuid(history.DoctorGuid, accessToken);
        var hospital = await _externalApiService.GetHospitalByGuid(history.HospitalGuid, accessToken);
        var rooms = await _externalApiService.GetHospitalRoomsByGuid(history.HospitalGuid, accessToken);

        if (doctor == null || hospital == null ||
            rooms.IsNullOrEmpty() ||
            !rooms!.Any(r => r.RoomGuid == history.RoomGuid)) return Results.NotFound();
        
        return Results.Ok(
            new GetHistoryResponse(
                history.Date, history.Data, 
                hospital.Name, string.Join(" ", pacient.LastName, pacient.FirstName),
                string.Join(" ", doctor.LastName, doctor.FirstName),
                rooms!.First(r => r.RoomGuid == history.RoomGuid).RoomName
        ));
    }

}
