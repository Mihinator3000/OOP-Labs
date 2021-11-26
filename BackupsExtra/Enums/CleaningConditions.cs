namespace BackupsExtra.Enums
{
    public enum CleaningConditions
    {
        /// <summary>
        /// Delete a point if it does not fit at least one set limit
        /// </summary>
        DoesNotFitOneLimit = 1,

        /// <summary>
        /// Delete a point if it does not fit all of the limits
        /// </summary>
        DoesNotFitAllLimits = 2
    }
}