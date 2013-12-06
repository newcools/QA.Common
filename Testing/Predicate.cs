namespace Automation.Common.Testing
{
    /// <summary>
    /// Represents the method that defines a set of criteria and determines whether those criteria are met.
    /// </summary>
    /// <returns>
    /// Return true if all the criteria defined within the method represented by this delegate are met; otherwise, false.
    /// </returns>
    public delegate bool Predicate();
}