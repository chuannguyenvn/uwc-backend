﻿using System;
using System.Collections.Generic;
using Commons.Models;
using Commons.Types;

namespace Commons.Communications.Tasks
{
    public class AddTasksRequest
    {
        public int AssignerAccountId { get; set; }
        public int? AssigneeAccountId { get; set; }
        public List<int> McpDataIds { get; set; }
        public DateTime CompleteByTimestamp { get; set; }
        public RoutingOptimizationScope RoutingOptimizationScope { get; set; }
        public AutoAssignmentOptimizationStrategy AutoAssignmentOptimizationStrategy { get; set; }
    }

    public class AddTasksBroadcastData
    {
        public List<TaskData> NewTasks { get; set; }
    }
}