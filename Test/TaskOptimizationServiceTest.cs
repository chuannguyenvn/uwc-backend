using System.Runtime.InteropServices.JavaScript;
using Commons.Models;
using Commons.Types;
using Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Repositories.Managers;
using Services.Map;
using Services.Mcps;
using Services.OnlineStatus;
using Services.Tasks;

namespace Test;

public class TaskOptimizationServiceTest
{
    private readonly Mock<IHubContext<BaseHub>> _mockBaseHub = new();

    private MockUnitOfWork _mockUnitOfWork;
    private MockTaskService _mockTaskService;
    private MockMcpFillLevelService _mockMcpFillLevelService;
    private MockLocationService _mockLocationService;
    private MockOnlineStatusService _mockOnlineStatusService;
    private TaskOptimizationService _routeOptimizationService;

    [SetUp]
    public void Setup()
    {
        _mockBaseHub.Setup(mock => mock.Clients.Client(It.IsAny<string>()).SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default));

        _mockUnitOfWork = new MockUnitOfWork();
        _mockTaskService = new MockTaskService(_mockUnitOfWork);
        _mockMcpFillLevelService = new MockMcpFillLevelService(_mockUnitOfWork);
        _mockLocationService = new MockLocationService(_mockUnitOfWork);
        _mockOnlineStatusService = new MockOnlineStatusService();
        _routeOptimizationService = new TaskOptimizationService(_mockUnitOfWork, _mockTaskService, _mockLocationService, _mockMcpFillLevelService,
            _mockOnlineStatusService);
    }

    // ------------------------------------------ GEN 1 TESTCASE -------------------------------------------------------
    [Test]
    public void Test_TaskNormalAssignment()
    {
        // Worker 1 is assigned to Mcp1
        // Worker 2 is assigned to Mcp2
        // => Both worker are assigned with 1 more task

        #region Arrange

        // The supervisor's Id
        int supervisorId = 1;

        // The workers' Id
        int worker1Id = 11;
        int worker2Id = 12;

        // The Mcps' Id
        int mcp1Id = 1;
        int mcp2Id = 2;

        // Start with a clean state (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(10.77, 106.65));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = _routeOptimizationService.GetMcpCoordinateById(mcp1Id);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = _routeOptimizationService.GetMcpCoordinateById(mcp2Id);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.15f);

        // Tasks count before distribution 
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;
        int tasksCountBeforeWorker2 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id).Count;

        // Assign task to worker
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp2Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> unassignedTasks = _routeOptimizationService.GetUnassignedTaskIn24Hours();

        #endregion


        #region Assert

        Assert.That(unassignedTasks.Count, Is.EqualTo(2));
        _mockTaskService.AssignWorkerToTask(unassignedTasks[0].Id, worker1Id);
        _mockTaskService.AssignWorkerToTask(unassignedTasks[1].Id, worker2Id);

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1 + 1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id)); // The first worker must get the task

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(tasksCountBeforeWorker2 + 1)); // The second worker must not get the task
        Assert.That(worker2Tasks[0].McpDataId, Is.EqualTo(mcp2Id));

        #endregion
    }

    // ----------------------------------------- GEN 2 TESTCASE --------------------------------------------------------
    [Test]
    public void Test_AlreadyScheduleWithDijkstra()
    {
        // The original order is (1,2,3,4,5)
        // => The Dijkstra does not change the order because it has already been optimal

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcp's Id
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;
        int mcp4Id = 4;
        int mcp5Id = 5;

        // Start with a clean state (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(0, 0));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(0, 2);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(0, 3);
        _mockUnitOfWork.McpDataRepository.GetById(mcp4Id).Coordinate = new Coordinate(0, 4);
        _mockUnitOfWork.McpDataRepository.GetById(mcp5Id).Coordinate = new Coordinate(0, 5);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.65f);
        _mockMcpFillLevelService.SetFillLevel(mcp4Id, 0.24f);
        _mockMcpFillLevelService.SetFillLevel(mcp5Id, 0.47f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp3Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp4Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp5Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), true);

        #endregion


        #region Assert

        // Assert if all 5 tasks have been assigned
        Assert.That(optimizedTasks.Count, Is.EqualTo(5));

        // Assert the order of the tasks after optimized does not change the order
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp1Id)); // First task must be mcp3
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp2Id)); // Second task must be mcp4
        Assert.That(optimizedTasks[2].McpDataId, Is.EqualTo(mcp3Id)); // Third task must be mcp5
        Assert.That(optimizedTasks[3].McpDataId, Is.EqualTo(mcp4Id)); // Fourth task must be mcp1
        Assert.That(optimizedTasks[4].McpDataId, Is.EqualTo(mcp5Id)); // Fifth task must be mcp2

        #endregion
    }

    [Test]
    public void Test_AlreadyScheduleWithPermutation()
    {
        // The original order is (1,2,3,4,5)
        // => The permutation algorithm does not change the order because it has been optimal.

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcp's Id
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;
        int mcp4Id = 4;
        int mcp5Id = 5;

        // Start with a clean state (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(0, 0));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(0, 2);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(0, 3);
        _mockUnitOfWork.McpDataRepository.GetById(mcp4Id).Coordinate = new Coordinate(0, 4);
        _mockUnitOfWork.McpDataRepository.GetById(mcp5Id).Coordinate = new Coordinate(0, 5);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.65f);
        _mockMcpFillLevelService.SetFillLevel(mcp4Id, 0.24f);
        _mockMcpFillLevelService.SetFillLevel(mcp5Id, 0.47f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp3Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp4Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp5Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), false);

        #endregion


        #region Assert

        Assert.That(optimizedTasks.Count, Is.EqualTo(5));

        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp1Id)); // First task must be mcp3
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp2Id)); // Second task must be mcp4
        Assert.That(optimizedTasks[2].McpDataId, Is.EqualTo(mcp3Id)); // Third task must be mcp5
        Assert.That(optimizedTasks[3].McpDataId, Is.EqualTo(mcp4Id)); // Fourth task must be mcp1
        Assert.That(optimizedTasks[4].McpDataId, Is.EqualTo(mcp5Id)); // Fifth task must be mcp2

        #endregion
    }

    [Test]
    public void Test_ScheduleWithDijkstra()
    {
        // The original order is (1,2,3,4,5)
        // => Change the order to (3,2,1,4,5)

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcp's Id
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;
        int mcp4Id = 4;
        int mcp5Id = 5;

        // Start with a clean state (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(0, 0));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 4);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(2, 2);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(1, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp4Id).Coordinate = new Coordinate(5, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp5Id).Coordinate = new Coordinate(10, 0);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.65f);
        _mockMcpFillLevelService.SetFillLevel(mcp4Id, 0.24f);
        _mockMcpFillLevelService.SetFillLevel(mcp5Id, 0.47f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp3Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp4Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp5Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), true);

        #endregion


        #region Assert

        // Assert the total number of tasks unchanged
        Assert.That(optimizedTasks.Count, Is.EqualTo(5));

        // Assert the order of task changed from (1,2,3,4,5) to (3,2,1,4,5)
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp3Id)); // First task must be mcp3
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp2Id)); // Second task must be mcp2
        Assert.That(optimizedTasks[2].McpDataId, Is.EqualTo(mcp1Id)); // Third task must be mcp1
        Assert.That(optimizedTasks[3].McpDataId, Is.EqualTo(mcp4Id)); // Fourth task must be mcp4
        Assert.That(optimizedTasks[4].McpDataId, Is.EqualTo(mcp5Id)); // Fifth task must be mcp5

        #endregion
    }

    [Test]
    public void Test_ScheduleWithDijkstraEdgeCase()
    {
        //   A --------(1) -----------B
        //   |   \                /   |
        //   |      \        /        |
        //  (2)        \/            (10)
        //   |      /       \         |
        //   |  /                  \  |
        //   D ----------(1)----------C         => AC = 2, BD = 10

        // Run permutation, can only get ABDC, which is 6
        // Dijkstra helps achieve optimal solution: A->B->A->D->C, which is 5

        // Consider only in the case include traffic because it is trigonometrically incorrect, AB + AC = 3 < 10 = BC
    }

    [Test]
    public void Test_ScheduleWithPermutation()
    {
        // The original order is (1,2,3,4,5)
        // => The order after permutation algorithm is (3,2,1,4,5)

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcp's Id
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;
        int mcp4Id = 4;
        int mcp5Id = 5;

        // Start with a clean state (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(0, 0));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 4);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(2, 2);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(1, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp4Id).Coordinate = new Coordinate(5, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp5Id).Coordinate = new Coordinate(10, 0);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.65f);
        _mockMcpFillLevelService.SetFillLevel(mcp4Id, 0.24f);
        _mockMcpFillLevelService.SetFillLevel(mcp5Id, 0.47f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp3Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp4Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp5Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), true);

        #endregion


        #region Assert

        // Assert the total number of tasks unchanged
        Assert.That(optimizedTasks.Count, Is.EqualTo(5));

        // Assert the order of task changed from (1,2,3,4,5) to (3,2,1,4,5)
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp3Id)); // First task must be mcp3
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp2Id)); // Second task must be mcp2
        Assert.That(optimizedTasks[2].McpDataId, Is.EqualTo(mcp1Id)); // Third task must be mcp1
        Assert.That(optimizedTasks[3].McpDataId, Is.EqualTo(mcp4Id)); // Fourth task must be mcp4
        Assert.That(optimizedTasks[4].McpDataId, Is.EqualTo(mcp5Id)); // Fifth task must be mcp5

        #endregion
    }

    [Test]
    public void Test_ScheduleWithPermutationEdgeCase()
    {
        // The original order is (1,2,3)
        // The order of running Dijkstra is (2,3,1) --> Cost ~ 6.3
        // The order of running Permutation algorithm is more optimal, which is (3,2,1) -> Cost ~ 5.6

        #region Arrange

        int supervisorId = 1;
        int workerId = 11;
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;

        // Start with a clean state (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(0, 0));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 2);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(1, 1);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(0, -1.5);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.15f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.15f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp3Id, DateTime.Now.AddHours(1));

        #endregion


        #region Act

        List<TaskData> optimizedTasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), false);

        #endregion


        #region Assert

        Assert.That(optimizedTasks.Count, Is.EqualTo(3));
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp3Id)); // First task must be mcp3
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp2Id)); // Second task must be mcp2
        Assert.That(optimizedTasks[2].McpDataId, Is.EqualTo(mcp1Id)); // Third task must be mcp1

        #endregion
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

        // Start with a clean state (no pre-made tasks)
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

        List<bool> priority = new List<bool>();
        priority.Add(false);
        priority.Add(false);
        priority[1] = true;
        List<TaskData> optimizedTasks =
            _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), true, priority);

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

        List<bool> priority = new List<bool>();
        priority.Add(false);
        priority.Add(false);
        priority[0] = true;

        List<TaskData> optimizedTasks =
            _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), true, priority);

        #endregion


        #region Assert

        Assert.That(optimizedTasks.Count, Is.EqualTo(2));
        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp2Id)); // First task must be mcp2
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp1Id)); // Second task must be mcp1

        #endregion
    }

    [Test]
    public void Test_BothPriority()
    {
        // The fill level of mcp 1 is larger than the fill level of mcp 2
        // The deadlines of mcp 2 is earlier than the deadline of mcp 1
        // => The worker should go to mcp 2 first because the deadline end sooner

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int workerId = 11;

        // The mcps' IDs
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;

        // Start with a clean slate (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's location
        _mockLocationService.UpdateLocation(workerId, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(11, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(10.5, 99.5);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.5f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.4f);

        // Set deadline for tasks
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp1Id, DateTime.Now.AddHours(10));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp2Id, DateTime.Now.AddHours(2));
        _mockTaskService.AddTaskWithWorker(supervisorId, workerId, mcp3Id, DateTime.Now.AddHours(2));

        #endregion


        #region Act

        List<bool> priority = new List<bool>();
        priority.Add(true);
        priority.Add(true);

        List<TaskData> optimizedTasks =
            _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(workerId), true, priority);

        #endregion


        #region Assert

        Assert.That(optimizedTasks.Count, Is.EqualTo(3));

        Assert.That(optimizedTasks[0].McpDataId, Is.EqualTo(mcp2Id)); // First task must be mcp2
        Assert.That(optimizedTasks[1].McpDataId, Is.EqualTo(mcp3Id)); // Second task must be mcp3
        Assert.That(optimizedTasks[2].McpDataId, Is.EqualTo(mcp1Id)); // Third task must be mcp1

        #endregion
    }

    // -------------------------------------- GEN 3 TESTCASE -----------------------------------------------------------
    [Test]
    public void Test_TaskDistributionWithAllFreeWorkers()
    {
        // Assign to the worker who is nearest to the chosen mcp

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The mcps' IDs
        int mcp1Id = 1;

        // Start with a clean slate (no pre-made tasks)
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(10.4, 100));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10.44, 100);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);

        // Set deadline for tasks
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp1Id, DateTime.Now.AddHours(10));

        #endregion


        #region Act

        bool costOrFast = true;
        bool option = true;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id)); // The free worker must get the task

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(0)); // The busy worker must not get the task

        #endregion
    }

    [Test]
    public void Test_TasksDistributionWithSomeFreeWorkers1()
    {
        // Worker 1 is busy (having a task at mcp1).
        // Worker 2 is free.
        // There is a task of mcp2 in the common pool
        // The additional time for worker1 to take less costly compared to that of worker2
        // => Worker 1 should take the additional task (to minimize the traveling cost for the whole system) and optimized route again

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The busy worker's ID
        int worker1Id = 11;

        // Another free worker's ID
        int worker2Id = 12;

        // The mcpId assigned to the busy worker
        int mcp1Id = 1;

        // The newly created task in the common pool
        int mcp2Id = 2;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(10.4, 100));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10.44, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(10.455, 100);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.7f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp2Id, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;
        int tasksCountBeforeWorker2 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id).Count;

        #endregion


        #region Act

        bool costOrFast = true;
        bool option = true;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1 + 1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id)); // The busy worker must get the task
        Assert.That(worker1Tasks[1].McpDataId, Is.EqualTo(mcp2Id));

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(tasksCountBeforeWorker2)); // The free worker must not get the task

        #endregion
    }

    [Test]
    public void Test_TasksDistributionWithSomeFreeWorkers2()
    {
        // Worker 1 is busy (having a task at mcp1).
        // Worker 2 is free.
        // There is a task of mcp2 in the common pool
        // The additional time for worker1 to take less costly compared to that of worker2
        // However, worker1 working time increase from 9 to 10, where the collecting time of worker2 - mcp2 is 5 hours
        // => Worker 2 should take the additional task (to minimize the maximal working time of all workers)

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The busy worker's ID
        int worker1Id = 11;

        // Another free worker's ID
        int worker2Id = 12;

        // The mcpId assigned to the busy worker
        int mcp1Id = 1;

        // The newly created task in the common pool
        int mcp2Id = 2;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(10.4, 100));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10.44, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(10.455, 100);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.7f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp2Id, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;
        int tasksCountBeforeWorker2 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id).Count;

        #endregion


        #region Act

        bool costOrFast = false;
        bool option = true;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id)); // The busy worker must get the task

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(tasksCountBeforeWorker2 + 1)); // The free worker must not get the task
        Assert.That(worker2Tasks[0].McpDataId, Is.EqualTo(mcp2Id));

        #endregion
    }

    [Test]
    public void Test_TasksDistributionWithAllBusyWorkers1()
    {
        // Worker 1 is busy (having a task at mcp1).
        // Worker 2 is busy (having a task at mcp2).
        // There is a task of mcp3 in the common pool
        // The additional time for worker1 to take less costly compared to that of worker2
        // => Worker 1 should take the additional task (to minimize the time cost of the system)

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The busy worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The mcpId assigned to the busy worker
        int mcp1Id = 1;
        int mcp2Id = 2;

        // The newly created task in the common pool
        int mcp3Id = 3;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(10.4, 100));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10.43, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(10.48, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(10.45, 100);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.7f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.5f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, worker2Id, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp3Id, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;
        int tasksCountBeforeWorker2 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id).Count;

        #endregion


        #region Act

        bool costOrFast = true;
        bool option = true;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1 + 1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id)); // The busy worker must get the task
        Assert.That(worker1Tasks[1].McpDataId, Is.EqualTo(mcp3Id));

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(tasksCountBeforeWorker2)); // The free worker must not get the task
        Assert.That(worker2Tasks[0].McpDataId, Is.EqualTo(mcp2Id));

        #endregion
    }

    [Test]
    public void Test_TasksDistributionWithAllBusyWorkers2()
    {
        // Worker 1 is busy (having a task at mcp1).
        // Worker 2 is busy (having a task at mcp2).
        // There is a task of mcp3 in the common pool
        // The additional time for worker1 to take less costly compared to that of worker2
        // However, worker1 working time increase from 9 to 10, where the collecting time of worker2 - mcp2 is 5 hours
        // => Worker 2 should take the additional task (to minimize the maximal working time of all workers)

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The busy worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The mcpId assigned to the busy worker
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp4Id = 4;

        // The newly created task in the common pool
        int mcp3Id = 3;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(10.4, 100));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(10.5, 100));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(10.43, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(10.48, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(10.45, 100);
        _mockUnitOfWork.McpDataRepository.GetById(mcp4Id).Coordinate = new Coordinate(10.44, 99.9);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.7f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.5f);
        _mockMcpFillLevelService.SetFillLevel(mcp4Id, 0.5f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp4Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, worker2Id, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp3Id, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;
        int tasksCountBeforeWorker2 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id).Count;

        #endregion


        #region Act

        bool costOrFast = false;
        bool option = true;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id)); // The busy worker must get the task
        Assert.That(worker1Tasks[1].McpDataId, Is.EqualTo(mcp4Id));

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(tasksCountBeforeWorker2 + 1)); // The free worker must not get the task
        Assert.That(worker2Tasks[0].McpDataId, Is.EqualTo(mcp2Id));
        Assert.That(worker2Tasks[1].McpDataId, Is.EqualTo(mcp3Id));

        #endregion
    }

    [Test]
    public void Test_TaskDistributionScheduleTasksAgain()
    {
        // Worker 1 is busy (having a task at mcp1, mcp2).
        // There is a task in the common pool mcp3
        // Worker 1 is assigned with the additional task
        // => Adding the new task re-planned the route that he/she has to travel.

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The busy worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The mcpId assigned to the busy worker
        int mcp1Id = 1;
        int mcp2Id = 2;

        // The newly created task in the common pool
        int mcp3Id = 3;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(0, 0));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(22, 22));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 3);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(0, 10);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(1, 1);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.7f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.5f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp3Id, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;

        #endregion


        #region Act

        bool costOrFast = true;
        bool option = true;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        List<List<TaskData>> result = _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = result[0];
        List<TaskData> worker2Tasks = result[1];

        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1 + 1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp3Id));
        Assert.That(worker1Tasks[1].McpDataId, Is.EqualTo(mcp1Id));
        Assert.That(worker1Tasks[2].McpDataId, Is.EqualTo(mcp2Id));

        #endregion
    }

    [Test]
    public void Test_TaskDistributionScheduleTasksAgain2()
    {
        // Worker 1 is busy (having a task at mcp1, mcp2).
        // There is a task in the common pool mcp3
        // Worker 1 is assigned with the additional task
        // => Adding the new task re-planned the route that he/she has to travel.

        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The busy worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The mcpId assigned to the busy worker
        int mcp1Id = 1;
        int mcp2Id = 2;

        // The newly created task in the common pool
        int mcp3Id = 3;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(0, 0));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(22, 22));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 3);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(0, 10);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(1, 1);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.7f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.5f);

        // Assign task to worker
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithWorker(supervisorId, worker1Id, mcp2Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp3Id, DateTime.Now.AddHours(1));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;

        #endregion


        #region Act

        bool costOrFast = true;
        bool option = false;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(worker1Id),
            option);
        List<TaskData> worker2Tasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(worker2Id),
            option);

        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1 + 1));
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp3Id));
        Assert.That(worker1Tasks[1].McpDataId, Is.EqualTo(mcp1Id));
        Assert.That(worker1Tasks[2].McpDataId, Is.EqualTo(mcp2Id));

        #endregion
    }

    [Test]
    public void Test_TaskDistributionWithDeadlinePriority()
    {
        #region Arrange

        // The supervisor's ID
        int supervisorId = 1;

        // The worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The newly created task in the common pool
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set thw worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(0, 0));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(22, 22));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 3);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(0, 10);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(1, 1);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.7f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.5f);

        // Assign task to worker
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp1Id, DateTime.Now.AddHours(1));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp2Id, DateTime.Now.AddHours(5));
        _mockTaskService.AddTaskWithoutWorker(supervisorId, mcp3Id, DateTime.Now.AddHours(2));

        // Tasks count before distribution of worker 2 (worker 1 has no tasks)
        int tasksCountBeforeWorker1 = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id).Count;

        #endregion


        #region Act

        bool costOrFast = true;
        bool option = false;
        Dictionary<int, UserProfile> workerProfile = new Dictionary<int, UserProfile>();
        workerProfile.Add(worker1Id, _mockUnitOfWork.UserProfileRepository.GetById(worker1Id));
        workerProfile.Add(worker2Id, _mockUnitOfWork.UserProfileRepository.GetById(worker2Id));
        List<bool> priority = new List<bool>();

        priority.Add(false);
        priority.Add(false);
        priority[0] = true;
        _routeOptimizationService.DistributeTasksFromPoolGen3(workerProfile, costOrFast, option, priority);

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(worker1Id),
            option, priority);
        List<TaskData> worker2Tasks = _routeOptimizationService.OptimizeRouteGen2(_mockUnitOfWork.UserProfileRepository.GetById(worker2Id),
            option, priority);

        // Check the total number of tasks assigned
        Assert.That(worker1Tasks.Count, Is.EqualTo(tasksCountBeforeWorker1 + 3));

        // Check the order of task assignment adhere to the deadline priority or not
        Assert.That(worker1Tasks[0].McpDataId, Is.EqualTo(mcp1Id));
        Assert.That(worker1Tasks[1].McpDataId, Is.EqualTo(mcp3Id));
        Assert.That(worker1Tasks[2].McpDataId, Is.EqualTo(mcp2Id));

        #endregion
    }

    // --------------------------------------- GEN 4 TESTCASE ----------------------------------------------------------
    [Test]
    public void Test_Automation()
    {
        #region Arrange

        // The worker's ID
        int worker1Id = 11;
        int worker2Id = 12;

        // The mcpId
        int mcp1Id = 1;
        int mcp2Id = 2;
        int mcp3Id = 3;

        // Remove all tasks of the free worker
        _mockUnitOfWork.TaskDataDataRepository.RemoveAll();

        // Set the worker's online status
        _mockOnlineStatusService.SetAsOnline(worker1Id);
        _mockOnlineStatusService.SetAsOnline(worker2Id);

        // Set the worker's location
        _mockLocationService.UpdateLocation(worker1Id, new Coordinate(0, 0));
        _mockLocationService.UpdateLocation(worker2Id, new Coordinate(2, 1));

        // Set the mcp locations
        _mockUnitOfWork.McpDataRepository.GetById(mcp1Id).Coordinate = new Coordinate(0, 3);
        _mockUnitOfWork.McpDataRepository.GetById(mcp2Id).Coordinate = new Coordinate(0, 10);
        _mockUnitOfWork.McpDataRepository.GetById(mcp3Id).Coordinate = new Coordinate(1, 1);

        // Set the mcp fill levels
        _mockMcpFillLevelService.SetFillLevel(mcp1Id, 0.9f);
        _mockMcpFillLevelService.SetFillLevel(mcp2Id, 0.7f);
        _mockMcpFillLevelService.SetFillLevel(mcp3Id, 0.86f);

        #endregion


        #region Act

        _routeOptimizationService.DistributeTasksFromPool();

        #endregion


        #region Assert

        List<TaskData> worker1Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker1Id);
        Assert.That(worker1Tasks.Count, Is.EqualTo(0));

        List<TaskData> worker2Tasks = _mockUnitOfWork.TaskDataDataRepository.GetTasksByWorkerId(worker2Id);
        Assert.That(worker2Tasks.Count, Is.EqualTo(2));
        Assert.That(worker2Tasks[0].McpDataId, Is.EqualTo(mcp1Id));
        Assert.That(worker2Tasks[1].McpDataId, Is.EqualTo(mcp3Id));

        #endregion
    }
}