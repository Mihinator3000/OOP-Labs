namespace Reports.Common.Enums
{
    public enum TaskChangeTypes
    {
        /// <summary>
        /// State of the task changed
        /// </summary>
        State = 1,

        /// <summary>
        /// Changed assigned employee
        /// </summary>
        AssignedUser = 2,

        /// <summary>
        /// Comment added to the task
        /// </summary>
        Comment = 3,

        /// <summary>
        /// Description was changed
        /// </summary>
        Description = 4
    }
}