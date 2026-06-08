namespace MovieHub.Data.Common
{
    /// <summary>
    /// Central place for all validation limits. The same constants are reused in the
    /// entity models (database constraints) and later in the input/view models
    /// (user-facing validation), so the rules can never drift apart.
    /// </summary>
    public static class DataConstants
    {
        // ----- Genre -----
        public const int GenreNameMaxLength = 50;
        public const int GenreNameMinLength = 2;

        // ----- Movie -----
        public const int MovieTitleMaxLength = 100;
        public const int MovieTitleMinLength = 2;

        public const int MovieDescriptionMaxLength = 2000;
        public const int MovieDescriptionMinLength = 10;

        public const int MovieDirectorMaxLength = 100;
        public const int MovieDirectorMinLength = 3;

        public const int MovieImageUrlMaxLength = 2048;

        public const int MovieMinReleaseYear = 1888; // the first film ever made
        public const int MovieMaxReleaseYear = 2100;

        // ----- Review -----
        public const int ReviewContentMaxLength = 1000;
        public const int ReviewContentMinLength = 5;

        public const int RatingMin = 1;
        public const int RatingMax = 10;

        // ----- ApplicationUser -----
        public const int NickNameMaxLength = 50;
        public const int NickNameMinLength = 2;
    }
}
