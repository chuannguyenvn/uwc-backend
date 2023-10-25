using Repositories.Managers;
using Commons.Communications.Vehicles;
using Commons.Models;
using Commons.RequestStatuses;

namespace Services.Vehicles;

public class VehicleDataService : IVehicleDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public RequestResult AddNewVehicle(AddNewVehicleRequest request)
    {
        var vehicleData = new VehicleData
        {
            LicensePlate = request.LicensePlate,
            Model = request.Model,
            VehicleType = request.VehicleType
        };
        _unitOfWork.VehicleData.Add(vehicleData);
        _unitOfWork.Complete();
        
        return new RequestResult(new Success());
    }

    public RequestResult UpdateVehicle(UpdateVehicleRequest request)
    {
        if (!_unitOfWork.VehicleData.DoesIdExist(request.VehicleId)) return new RequestResult(new DataEntryNotFound());
        
        var vehicleData = _unitOfWork.VehicleData.GetById(request.VehicleId);
        if (request.NewLicensePlate != null) vehicleData.LicensePlate = request.NewLicensePlate;
        if (request.NewModel != null) vehicleData.Model = request.NewModel;
        if (request.NewVehicleType != null) vehicleData.VehicleType = request.NewVehicleType.Value;
        _unitOfWork.Complete();
        
        return new RequestResult(new Success());
    }

    public RequestResult RemoveVehicle(RemoveVehicleRequest request)
    {
        if (!_unitOfWork.VehicleData.DoesIdExist(request.VehicleId)) return new RequestResult(new DataEntryNotFound());
        
        var vehicleData = _unitOfWork.VehicleData.GetById(request.VehicleId);
        _unitOfWork.VehicleData.Remove(vehicleData);
        _unitOfWork.Complete();
        
        return new RequestResult(new Success());
    }
}