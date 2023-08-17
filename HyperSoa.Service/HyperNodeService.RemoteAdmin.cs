using HyperSoa.Contracts.RemoteAdmin;
using HyperSoa.Contracts.RemoteAdmin.Models;

namespace HyperSoa.Service
{
    public partial class HyperNodeService
    {
        #region System Commands

        internal HyperNodeTaskProgressInfo? GetCachedTaskProgressInfo(string taskId)
        {
            return _taskProgressCacheMonitor.GetTaskProgressInfo(taskId);
        }

        internal IEnumerable<CommandStatus> GetCommandStatuses()
        {
            return _commandModuleConfigurations.Keys.Select(
                commandName => new CommandStatus
                {
                    CommandName = commandName,
                    CommandType = (RemoteAdminCommandName.IsRemoteAdminCommand(commandName) ? HyperNodeCommandType.RemoteAdmin : HyperNodeCommandType.Custom),
                    Enabled = _commandModuleConfigurations[commandName].Enabled
                }
            );
        }

        internal IEnumerable<ActivityMonitorStatus> GetActivityMonitorStatuses()
        {
            lock (Lock)
            {
                return _customActivityMonitors.Select(
                    m => new ActivityMonitorStatus
                    {
                        Name = m.Name,
                        Enabled = m.Enabled
                    }
                );
            }
        }

        internal IEnumerable<LiveTaskStatus> GetLiveTaskStatuses()
        {
            return _liveTasks.Keys.Select(
                taskId => new LiveTaskStatus
                {
                    TaskId = taskId,
                    CommandName = _liveTasks[taskId].CommandName,
                    IsCancellationRequested = _liveTasks[taskId].Token.IsCancellationRequested,
                    Elapsed = _liveTasks[taskId].Elapsed
                }
            );
        }

        internal bool IsKnownCommand(string? commandName)
        {
            return _commandModuleConfigurations.ContainsKey(commandName ?? "");
        }

        internal bool IsKnownActivityMonitor(string activityMonitorName)
        {
            return _customActivityMonitors.Any(a => a.Name == activityMonitorName);
        }

        internal bool EnableCommandModule(string commandName, bool enable)
        {
            var result = false;

            if (_commandModuleConfigurations.TryGetValue(commandName, out var commandConfig))
            {
                commandConfig.Enabled = enable;
                result = true;
            }

            return result;
        }

        internal bool EnableActivityMonitor(string activityMonitorName, bool enable)
        {
            var result = false;

            var activityMonitor = _customActivityMonitors.FirstOrDefault(a => a.Name == activityMonitorName);
            if (activityMonitor != null)
            {
                activityMonitor.Enabled = enable;
                result = true;
            }

            return result;
        }

        internal bool RenameActivityMonitor(string oldName, string newName)
        {
            var result = false;

            var activityMonitor = _customActivityMonitors.FirstOrDefault(a => a.Name == oldName);
            if (activityMonitor != null)
            {
                activityMonitor.Name = newName;
                result = true;
            }

            return result;
        }

        internal bool CancelTask(string taskId)
        {
            var result = false;

            if (_liveTasks.TryGetValue(taskId, out var taskInfo))
            {
                taskInfo.Cancel();
                result = true;
            }

            return result;
        }

        #endregion System Commands
    }
}