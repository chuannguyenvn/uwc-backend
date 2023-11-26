using Commons.Models;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Repositories.Managers;
using Services.Map;
using Services.Mcps;
using Services.Tasks;

namespace Test;

public class TaskOptimizationServiceTest
{
    private readonly Mock<IHubContext<BaseHub>> _mockBaseHub = new();

    private MockUnitOfWork _mockUnitOfWork;
    private MockTaskService _mockTaskService;
    private MockMcpFillLevelService _mockMcpFillLevelService;
    private MockLocationService _mockLocationService;
    private TaskOptimizationService _routeOptimizationService;

    [SetUp]
    public void Setup()
    {
        _mockBaseHub.Setup(mock => mock.Clients.Client(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default));

        _mockUnitOfWork = new MockUnitOfWork();
        _mockTaskService = new MockTaskService(_mockUnitOfWork);
        _mockMcpFillLevelService = new MockMcpFillLevelService(_mockUnitOfWork);
        _mockLocationService = new MockLocationService(_mockUnitOfWork);
        _routeOptimizationService = new TaskOptimizationService(_mockUnitOfWork, _mockTaskService, _mockLocationService, _mockMcpFillLevelService);
    }

    [Test]
    public void Test_FillLevelPriority()
    {
        // The distance between the worker and mcp 1 is equal to the distance between the worker and mcp 2
        // The deadlines of the tasks are equal
        // The fill level of mcp 2 is higher than the fill level of mcp 1
        // => The worker should go to mcp 2 first

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcps' IDs
        int mcp1Id = 1;
        int mcp2Id = 2;

        // Start with a clean slate (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(11, 100);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.1f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.9f);

        // Task with a nearly full mcp
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(1));

        // Task with an empty mcp
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteForWorker(_mockUnitOfWork.UserProfileRepository.GetById(workerId));

        #endregion


        #region Assert

        Assert.That(optimizedTasks.Count, Is.EqualTo(2));
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp2Id)); // First task must be mcp2
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp1Id)); // Second task must be mcp1

        #endregion
    }

    [Test]
    public void Test_DeadlinePriority()
    {
        // The distance between the worker and mcp 1 is equal to the distance between the worker and mcp 2
        // The fill level of mcp 1 is equal to the fill level of mcp 2
        // The deadlines of mcp 2 is earlier than the deadline of mcp 1
        // => The worker should go to mcp 2 first

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcps' IDs
        int mcp1Id = 1;
        int mcp2Id = 2;

        // Start with a clean slate (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(11, 100);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.5f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.5f);

        // Task with later deadline
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(10)); // Later deadline

        // Task with earlier deadline
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1)); // Earlier deadline

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteForWorker(_mockUnitOfWork.UserProfileRepository.GetById(workerId));

        #endregion


        #region Assert

        Assert.That(optimizedTasks.Count, Is.EqualTo(2));
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp2Id)); // First task must be mcp2
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp1Id)); // Second task must be mcp1

        #endregion
    }

    [Test]
    public void Test_TasksDistributionWithOneFreeWorker()
    {
        // All workers are busy except one
        // => The free worker should receive a new task

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The free worker's ID
        int worker1Id = 15;

        // Another busy worker's ID
        int worker2Id = 12;

        // The mcps' ID
        int mcpId = 1;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAllTasksOfWorker(worker1Id);

        // A single unassigned task. The free worker must get this task
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcpId, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker2 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id).Count;

        #endregion


        #region Act

        _routeOptimizationService.DistributeTasksFromPool();

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcpId)); // The free worker must get the task

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(tasksCountBeforeWorker2)); // The busy worker must not get the task

        #endregion
    }

    [Test]
    public void Test_TestName() // Template
    {
        #region Arrange

        // ...    

        #endregion


        #region Act

        // ...

        #endregion


        #region Assert

        // ...

        #endregion
    }
}