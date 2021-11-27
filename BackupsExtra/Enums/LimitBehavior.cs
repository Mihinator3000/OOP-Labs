namespace BackupsExtra.Enums
{
    public enum LimitBehavior
    {
        /// <summary>
        /// Delete restore point when the limit is exceeded
        /// </summary>
        Delete = 1,

        /// <summary>
        /// Merge restore point when the limit is exceeded
        /// </summary>
        Merge = 2
    }
}