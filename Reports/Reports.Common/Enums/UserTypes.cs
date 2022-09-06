namespace Reports.Common.Enums
{
    public enum UserTypes
    {
        /// <summary>
        /// Does not have a leader.
        /// </summary>
        TeamLeader = 1,

        /// <summary>
        /// Has leader and at least one subordinate.
        /// </summary>
        Leader = 2,

        /// <summary>
        /// Does not have subordinates.
        /// </summary>
        Employee = 3
    }
}