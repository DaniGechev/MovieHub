namespace MovieHub.Web.Infrastructure
{
    /// <summary>
    /// Application-wide constant values. Keeping the role name here means you
    /// reference the exact same string in the seeder and in your
    /// [Authorize(Roles = ...)] attributes, so they can never get out of sync.
    /// </summary>
    public static class GlobalConstants
    {
        public const string AdministratorRoleName = "Administrator";

        // NOTE: For a real application these would live in user-secrets or
        // environment variables, never in source control. For a school project
        // a constant is acceptable, but be ready to explain that distinction.
        public const string AdminEmail = "admin@moviehub.com";

        public const string AdminPassword = "Admin123!";
    }
}
