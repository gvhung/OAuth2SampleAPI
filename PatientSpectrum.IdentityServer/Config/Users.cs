using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;


namespace PatientSpectrum.IdentityServer.Config
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>();
        }
    }
}