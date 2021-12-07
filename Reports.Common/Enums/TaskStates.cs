namespace Reports.Common.Enums
{
    public enum TaskStates
    {
        /// <summary>
        /// The task has been created, but the work on it has not been started yet.
        /// </summary>
        Open = 1,

        /// <summary>
        /// The work on the task is in progress.
        /// </summary>
        Active = 2,

        /// <summary>
        /// Task is completed.
        /// </summary>
        Resolved = 3
    }
}