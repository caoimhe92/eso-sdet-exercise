using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace UnitTestProject
{
    [TestClass]
    public class TVMazeGetRequests
    {
        List<Shows> showsList;
        List<Episodes> episodeList;

        [TestInitialize]
        public async Task SetUp()
        {
            //Setup
            var client = new RestClient("https://api.tvmaze.com/");
            var request = new RestRequest("shows");

            //Action
            RestResponse response = await client.ExecuteAsync(request);
            showsList = JsonConvert.DeserializeObject<List<Shows>>(response.Content);
            showsList.RemoveAll(s => s.network?.name != "HBO" || !s.genres.Contains("Drama") || DateTime.Parse(s.premiered).Year < 2013 || DateTime.Parse(s.premiered).Year > 2015);
        }

        [TestMethod]
        public async Task ExerciseOne()
        {
            //Assertions
            Assert.AreEqual(showsList.Count, 3);
            Assert.AreEqual(showsList[0].name, "True Detective");
            Assert.AreEqual(showsList[1].name, "The Leftovers");
            Assert.AreEqual(showsList[2].name, "Looking");

        }

        [TestMethod]
        public async Task ExerciseTwo()
        {
            //Setup
            var client = new RestClient("https://api.tvmaze.com/");
            foreach (var show in showsList)
            {
                var request = new RestRequest("shows/" + show.id + "/episodes");

                //Action
                RestResponse response = await client.ExecuteAsync(request);
                episodeList = JsonConvert.DeserializeObject<List<Episodes>>(response.Content);

                if (show.name == "The Leftovers")
                {
                    List<Episodes> seasonOne = episodeList.FindAll(e => e.season == 1);
                    List<Episodes> seasonTwo = episodeList.FindAll(e => e.season == 2);
                    List<Episodes> seasonThree = episodeList.FindAll(e => e.season == 3);

                    int seasonOneEpisodes = (seasonOne.Count);
                    int seasonOneTotalRuntime = (seasonOne.Sum(x => x.runtime));
                    int seasonTwoEpisodes = (seasonTwo.Count);
                    int seasonTwoTotalRuntime = (seasonTwo.Sum(x => x.runtime));
                    int seasonThreeEpisodes = (seasonThree.Count);
                    int seasonThreeTotalRuntime = (seasonThree.Sum(x => x.runtime));

                    //Assertions
                    Assert.AreEqual(seasonOneEpisodes, 10);
                    Assert.AreEqual(seasonOneTotalRuntime, 600);
                    Assert.AreEqual(seasonTwoEpisodes, 10);
                    Assert.AreEqual(seasonTwoTotalRuntime, 600);
                    Assert.AreEqual(seasonThreeEpisodes, 8);
                    Assert.AreEqual(seasonThreeTotalRuntime, 495);
                }
                if (show.name == "True Detective")
                {
                    List<Episodes> seasonOne = episodeList.FindAll(e => e.season == 1);
                    List<Episodes> seasonTwo = episodeList.FindAll(e => e.season == 2);
                    List<Episodes> seasonThree = episodeList.FindAll(e => e.season == 3);

                    int seasonOneEpisodes = (seasonOne.Count);
                    int seasonOneTotalRuntime = (seasonOne.Sum(x => x.runtime));
                    int seasonTwoEpisodes = (seasonTwo.Count);
                    int seasonTwoTotalRuntime = (seasonTwo.Sum(x => x.runtime));
                    int seasonThreeEpisodes = (seasonThree.Count);
                    int seasonThreeTotalRuntime = (seasonThree.Sum(x => x.runtime));

                    //Assertions
                    Assert.AreEqual(seasonOneEpisodes, 8);
                    Assert.AreEqual(seasonOneTotalRuntime, 480);
                    Assert.AreEqual(seasonTwoEpisodes, 8);
                    Assert.AreEqual(seasonTwoTotalRuntime, 510);
                    Assert.AreEqual(seasonThreeEpisodes, 8);
                    Assert.AreEqual(seasonThreeTotalRuntime, 503);
                }
                if (show.name == "Looking")
                {
                    List<Episodes> seasonOne = episodeList.FindAll(e => e.season == 1);
                    List<Episodes> seasonTwo = episodeList.FindAll(e => e.season == 2);

                    int seasonOneEpisodes = (seasonOne.Count);
                    int seasonOneTotalRuntime = (seasonOne.Sum(x => x.runtime));
                    int seasonTwoEpisodes = (seasonTwo.Count);
                    int seasonTwoTotalRuntime = (seasonTwo.Sum(x => x.runtime));

                    //Assertions
                    Assert.AreEqual(seasonOneEpisodes, 8);
                    Assert.AreEqual(seasonOneTotalRuntime, 240);
                    Assert.AreEqual(seasonTwoEpisodes, 10);
                    Assert.AreEqual(seasonTwoTotalRuntime, 300);
                }
            }
        }

        [TestMethod]
        public async Task ExerciseThree()
        {
            //Setup
            int highestRuntime = 0;
            string seriesName = ("");

            var client = new RestClient("https://api.tvmaze.com/");
            List<Episodes> fullepisodeList = new List<Episodes>();
            foreach (var show in showsList)
            {
                var request = new RestRequest("shows/" + show.id + "/episodes");

                //Action
                RestResponse response = await client.ExecuteAsync(request);
                episodeList = JsonConvert.DeserializeObject<List<Episodes>>(response.Content);
                int showAverage = episodeList.Sum(x => x.runtime) / episodeList.Count();
                if (showAverage > highestRuntime)
                {
                    highestRuntime = showAverage;
                    seriesName = show.name;
                }
            }

            //Assertions
            Assert.AreEqual(seriesName, "True Detective");
            Assert.AreEqual(highestRuntime, 62);
        }
    }
}
