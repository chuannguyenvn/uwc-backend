using Commons.Communications.Mcps;
using Commons.RequestStatuses;
using Repositories.Managers;

namespace Services.Mcps;

public class MockMcpFillLevelService : IMcpFillLevelService
{
    public static Dictionary<int, float> FillLevelsById => _fillLevelsById;
    private static readonly Dictionary<int, float> _fillLevelsById = new();

    private readonly IUnitOfWork _unitOfWork;

    public MockMcpFillLevelService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        InitializeFillLevelDictionary();
    }

    public ParamRequestResult<GetFillLevelResponse> GetFillLevel(GetFillLevelRequest request)
    {
        var fillLevelsById = request.McpIds.ToDictionary(mcpId => mcpId, mcpId => _fillLevelsById[mcpId]);

        return new ParamRequestResult<GetFillLevelResponse>(new Success(), new GetFillLevelResponse
        {
            FillLevelsById = fillLevelsById,
        });
    }

    public ParamRequestResult<GetFillLevelResponse> GetAllFillLevel()
    {
        return new ParamRequestResult<GetFillLevelResponse>(new Success(), new GetFillLevelResponse
        {
            FillLevelsById = new Dictionary<int, float>(_fillLevelsById),
        });
    }

    public void SetFillLevel(int mcpId, float fillLevel)
    {
        _fillLevelsById[mcpId] = fillLevel;
    }

    public RequestResult SetFillLevel(SetFillLevelRequest request)
    {
        FillLevelsById[request.McpId] = request.FillLevel;

        Console.WriteLine($"Set fill level of mcp {request.McpId} to {request.FillLevel}");

        return new RequestResult(new Success());
    }

    public RequestResult EmptyMcp(EmptyMcpRequest request)
    {
        _fillLevelsById[request.McpId] = 0f;

        Console.WriteLine($"Emptied mcp {request.McpId}");

        return new RequestResult(new Success());
    }

    private void InitializeFillLevelDictionary()
    {
        foreach (var mcpData in _unitOfWork.McpDataRepository.GetAll())
        {
            _fillLevelsById.Add(mcpData.Id, 0f);
        }
    }
}