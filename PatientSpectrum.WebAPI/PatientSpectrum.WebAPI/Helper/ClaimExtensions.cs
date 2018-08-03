using System.Security.Claims;

namespace PatientSpectrum.WebAPI.Helper
{
    internal static class ClaimExtensions
    {
        public static bool HasValue(this Claim claim)
        {
            return (claim != null && claim.Value.IsPresent());
        }
    }
}