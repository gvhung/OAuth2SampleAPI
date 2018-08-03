using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientSpectrum.UnitTests.Helper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientJourney.GlobalConstants;
using PatientJourney.BusinessModel.BuilderModels;
using System.Net.Http;
using PatientJourney.BusinessModel;
using System.Text;

namespace PatientSpectrum.UnitTests
{
    [TestClass]
    public class PatientJourneyUnitTest
    {
        public PatientJourneyUnitTest()
        {
            setup();
        }
        private PatientHttpClient _client;
        private void setup()
        {
            //log in as Admin(ROrxW encrypted is TST-spectrum1)
            _client = new PatientHttpClient(
                uri: OAuthPJConstants.PatientResourceAPIURI,
                scope: OAuthPJConstants.ResourceScopes,
                user_5_1_1_id: "ROrxW / 50bpVuEoOBcADs1R8KyfB1noQ6PgmMbfxNtOBGTmYHiJqXq + rz0qrVhFVKoFPrDfbb / J6OAIXvyTyY4hAasNB5sZsnGI6ind7LoW8cABifVhyr + otyirOi6MoDsVr93k7ihfnt00HiAlYhgH504hrsBDX3B3c / VsCQtsNYe4b4Ou3NQUgOxg + Wt4W9Lw / uDGsbcNRnTZTqhC5hEGGcnExH1jqb6DFXV6b84v2T0v0Vtcsaeudyc8RHFLLEnhlBAK / Fl5pbMsPisBhvNFvkOySy8CkU / NuLeQ7UElnQR5e2wLXWKbliwYzENDFjYEQ5MfxuvjnCnIsZAIfT0Q ==",
                another_user_param: "NARAYSX2");
        }
        [TestMethod]
        public async Task TestGetJourney_PositiveFlow()
        {
            var rspTrips = await _client.GetClient().GetAsync("api/PatientJourney/GetJourney").ConfigureAwait(false);

            if (rspTrips.IsSuccessStatusCode)
            {
                var lstTripsAsString = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);

                var vm = JsonConvert.DeserializeObject<List<PatientJourneyModel>>(lstTripsAsString).ToList();
                Assert.AreEqual(1, vm.Count);
            }
            else
            {
                if (rspTrips != null && rspTrips.IsSuccessStatusCode == false)
                {
                    var result = rspTrips.Content.ReadAsStringAsync().Result;
                    Console.Out.WriteLine("Http operation unsuccessfull");
                    Console.Out.WriteLine(string.Format("Status: '{0}'", rspTrips.StatusCode));
                    Console.Out.WriteLine(string.Format("Reason: '{0}'", rspTrips.ReasonPhrase));
                    Console.Out.WriteLine(result);
                }
                Assert.Fail("API call Failed " + rspTrips.ReasonPhrase);
            }
        }

        [TestMethod]
        public async Task TestGetJourneyAdmin_NegativeFlow()
        {
            var rspTrips = await _client.GetClient().GetAsync("api/PatientJourney/GetJourneyAdmin").ConfigureAwait(false);

            if (rspTrips.IsSuccessStatusCode == false)
            {
                Assert.IsTrue((rspTrips.StatusCode == System.Net.HttpStatusCode.Forbidden));
            }
            else
            {
                Assert.Fail("API accessible to users with roles other than Admin " + rspTrips.ReasonPhrase);
            }
        }


        [TestMethod]
        public async Task TestGetTherapeuticandArchetypeData_PositiveFlow()
        {
            var rspTrips = await _client.GetClient().GetAsync("api/PatientJourney/GetTherapeuticandArchetypeData?countryIds=9").ConfigureAwait(false);

            if (rspTrips.IsSuccessStatusCode)
            {
                var lstTripsAsString = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);

                var vm = JsonConvert.DeserializeObject<PatientAdminMasterData>(lstTripsAsString);
                Assert.IsNotNull(vm);
                Assert.AreEqual(1, vm.AreaList.Count);
                Assert.AreEqual(4, vm.TherapeuticList.Count);
                Assert.AreEqual(2, vm.YearList.Count);
            }
            else
            {
                if (rspTrips != null && rspTrips.IsSuccessStatusCode == false)
                {
                    var result = rspTrips.Content.ReadAsStringAsync().Result;
                    Console.Out.WriteLine("Http operation unsuccessfull");
                    Console.Out.WriteLine(string.Format("Status: '{0}'", rspTrips.StatusCode));
                    Console.Out.WriteLine(string.Format("Reason: '{0}'", rspTrips.ReasonPhrase));
                    Console.Out.WriteLine(result);
                }
                Assert.Fail("API call Failed " + rspTrips.ReasonPhrase);
            }
        }

        [TestMethod]
        public async Task TestGetTherapeuticDataPJ_PositiveFlow()
        {
            var rspTrips = await _client.GetClient().GetAsync("api/PatientJourney/GetTherapeuticDataPJ?TherapeuticId=1").ConfigureAwait(false);

            if (rspTrips.IsSuccessStatusCode)
            {
                var lstTripsAsString = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);

                var vm = JsonConvert.DeserializeObject<PatientAdminMasterData>(lstTripsAsString);
                Assert.IsNotNull(vm);

            }
            else
            {
                if (rspTrips != null && rspTrips.IsSuccessStatusCode == false)
                {
                    var result = rspTrips.Content.ReadAsStringAsync().Result;
                    Console.Out.WriteLine("Http operation unsuccessfull");
                    Console.Out.WriteLine(string.Format("Status: '{0}'", rspTrips.StatusCode));
                    Console.Out.WriteLine(string.Format("Reason: '{0}'", rspTrips.ReasonPhrase));
                    Console.Out.WriteLine(result);
                }
                Assert.Fail("API call Failed " + rspTrips.ReasonPhrase);
            }
        }


  

        [TestMethod]
        public async Task CreatePatientJounrey_PositiveFlow()
        {
       
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(new { JourneyTitle = "PARKINSON'S-DUODOPA-GLOBAL", JourneyDescription = "Test", BrandID = "22", CountryID = "9" , StatusID = "1" , Year = "2017" , TherapeuticID = "17", SubTherapeuticID = "12" , IndicationID = "17" , ArchetypeID = "1" , AreaID = "5", TherapeuticArea = "SPECIALTY", SubTherapeuticArea = "NEUROLOGY", Indication = "PARKINSON'S", Brand = "DUODOPA", Area = "GLOBAL", Country = "GLOBAL", User511id = "SRINISX5" });
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response =await _client.GetClient().PostAsync(string.Format("api/PatientJourney/AddJourney"), contentPost).ConfigureAwait(false);
           
            if (response.IsSuccessStatusCode)
            {
                var lstTripsAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jounreyID = JsonConvert.DeserializeObject<Int32?>(lstTripsAsString);

                var Stageparam = Newtonsoft.Json.JsonConvert.SerializeObject(new { PatientJourneyId = jounreyID, StageTitle = "ADVANCED TREATMENT", PopulationStatistics = "100", TimeStatistics = "100", PopulationScale = "100", TimeScale = "1", StageMasterId = "50", User511id = "SRINISX5" });
                HttpContent StagecontentPost = new StringContent(Stageparam, Encoding.UTF8, "application/json");

                var Stageresponse = await _client.GetClient().PostAsync(string.Format("api/PatientJourney/AddJourneyStage"), StagecontentPost).ConfigureAwait(false);

                if (Stageresponse.IsSuccessStatusCode)
                {
                    var lstTripsAsString1 = await Stageresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var StageID = JsonConvert.DeserializeObject<Int32?>(lstTripsAsString1);

                    var Transactionparam = Newtonsoft.Json.JsonConvert.SerializeObject(new { TransactionMasterId = "225", TransactionTitle = "Advance disease state Treatment care team", LocationId = "11", LocationName = "ALLIED SERVICES", PatientJourneyId = jounreyID, PatientStageId = StageID, TransactionDescription = "Test", HCPDescription = "Test", PayerDescription = "Test", PatientDescription = "Test", HCPRating = "4", PayerRating = "3", PatientRating = "3", FeasibilityRating = "", ViabilityRating = "", StatusID = "1", User511id = "SRINISX5" });
                    HttpContent TransactioncontentPost = new StringContent(Transactionparam, Encoding.UTF8, "application/json");

                    var Transactionresponse = await _client.GetClient().PostAsync(string.Format("api/PatientJourney/AddTransaction"), TransactioncontentPost).ConfigureAwait(false);

                    if (Transactionresponse.IsSuccessStatusCode)
                    {
                        string SubmitJounrey = "api/PatientJourney/SubmitJourney?JourneyId=" + jounreyID+ "&Comment=UnitTest&User511=SRINISX5&Email=TEST@TEST";
                        var rspTrips = await _client.GetClient().GetAsync(SubmitJounrey).ConfigureAwait(false);

                        if (rspTrips.IsSuccessStatusCode)
                        {
                            var submit = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);

                            var JourneyCreateCompletion = JsonConvert.DeserializeObject<Int32?>(submit);
                            Assert.AreEqual(1,JourneyCreateCompletion);

                        }
                        else
                        {
                            Assert.Fail("Submit Jounrey Failed");
                        }
                    }
                    else
                    {
                        Assert.Fail("Add Transaction Failed");
                    }
                }
                else
                {
                    Assert.Fail("Add Stage Failed ");
                }

            }
            else
            {
                Assert.Fail("Add journey Failed ");
               // var lstTripsAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                //Console.WriteLine(lstTripsAsString);
                
            }
        }

        /// <summary>
        /// Negative flow for create patient journey where TherapeuticArea and 511id is given as empty input
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreatePatientJounrey_NegativeFlow()
        {

            var param = Newtonsoft.Json.JsonConvert.SerializeObject(new { JourneyTitle = "PARKINSON'S-DUODOPA-GLOBAL", JourneyDescription = "Test", BrandID = "22", CountryID = "9", StatusID = "1", Year = "2017", TherapeuticID = "17", SubTherapeuticID = "12", IndicationID = "17", ArchetypeID = "1", AreaID = "5", TherapeuticArea = "", SubTherapeuticArea = "NEUROLOGY", Indication = "PARKINSON'S", Brand = "DUODOPA", Area = "GLOBAL", Country = "GLOBAL", User511id = "" });
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await _client.GetClient().PostAsync(string.Format("api/PatientJourney/AddJourney"), contentPost).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var lstTripsAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jounreyID = JsonConvert.DeserializeObject<Int32?>(lstTripsAsString);

                var Stageparam = Newtonsoft.Json.JsonConvert.SerializeObject(new { PatientJourneyId = jounreyID, StageTitle = "ADVANCED TREATMENT", PopulationStatistics = "100", TimeStatistics = "100", PopulationScale = "100", TimeScale = "1", StageMasterId = "50", User511id = "SRINISX5" });
                HttpContent StagecontentPost = new StringContent(Stageparam, Encoding.UTF8, "application/json");

                var Stageresponse = await _client.GetClient().PostAsync(string.Format("api/PatientJourney/AddJourneyStage"), StagecontentPost).ConfigureAwait(false);

                if (Stageresponse.IsSuccessStatusCode)
                {
                    var lstTripsAsString1 = await Stageresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var StageID = JsonConvert.DeserializeObject<Int32?>(lstTripsAsString1);

                    var Transactionparam = Newtonsoft.Json.JsonConvert.SerializeObject(new { TransactionMasterId = "225", TransactionTitle = "Advance disease state Treatment care team", LocationId = "11", LocationName = "ALLIED SERVICES", PatientJourneyId = jounreyID, PatientStageId = StageID, TransactionDescription = "Test", HCPDescription = "Test", PayerDescription = "Test", PatientDescription = "Test", HCPRating = "4", PayerRating = "3", PatientRating = "3", FeasibilityRating = "", ViabilityRating = "", StatusID = "1", User511id = "SRINISX5" });
                    HttpContent TransactioncontentPost = new StringContent(Transactionparam, Encoding.UTF8, "application/json");

                    var Transactionresponse = await _client.GetClient().PostAsync(string.Format("api/PatientJourney/AddTransaction"), TransactioncontentPost).ConfigureAwait(false);

                    if (Transactionresponse.IsSuccessStatusCode)
                    {
                        string SubmitJounrey = "api/PatientJourney/SubmitJourney?JourneyId=" + jounreyID + "&Comment=UnitTest&User511=SRINISX5&Email=TEST@TEST";
                        var rspTrips = await _client.GetClient().GetAsync(SubmitJounrey).ConfigureAwait(false);

                        if (rspTrips.IsSuccessStatusCode)
                        {
                            var submit = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);

                            var JourneyCreateCompletion = JsonConvert.DeserializeObject<Int32?>(submit);
                            Assert.AreEqual(1, JourneyCreateCompletion);

                        }
                        else
                        {
                            Assert.AreEqual(rspTrips.StatusCode, System.Net.HttpStatusCode.BadRequest);
                            string lstTripsAsString3 = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);
                            System.Diagnostics.Trace.WriteLine(lstTripsAsString3);
                        }
                    }
                    else
                    {
                        Assert.AreEqual(Transactionresponse.StatusCode, System.Net.HttpStatusCode.BadRequest);
                        string lstTripsAsString2 = await Transactionresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        System.Diagnostics.Trace.WriteLine(lstTripsAsString2);
                    }
                }
                else
                {
                    Assert.AreEqual(Stageresponse.StatusCode, System.Net.HttpStatusCode.BadRequest);
                    string lstTripsAsString1 = await Stageresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    System.Diagnostics.Trace.WriteLine(lstTripsAsString1);
                }

            }
            else
            {
                     Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
                     string lstTripsAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                     System.Diagnostics.Trace.WriteLine(lstTripsAsString);


            }
        }
    }
}
