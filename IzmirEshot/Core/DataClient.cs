using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IzmirEshot.IzmirEshotService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using IzmirEshot.Helpers;
using IzmirEshot.Models;
using System.Net.Http;
using HtmlAgilityPack;
using System.Net;
using System.Linq;

namespace IzmirEshot.Core
{
    public sealed class DataClient
    {
        private Service1Client serviceClient;
        private HtmlDocument htmlDocument;
        private HttpClient client;


        public DataClient()
        {
            this.serviceClient = new Service1Client();
            this.htmlDocument = new HtmlDocument();
            this.client = new HttpClient();
            this.ServiceInitialize();
        }


        public void ServiceInitialize()
        {
            using (new OperationContextScope(serviceClient.InnerChannel))
            {
                var username = MessageHeader.CreateHeader("UserName", "CH1", ConnectionHelper.Username);
                var password = MessageHeader.CreateHeader("Password", "CH2", ConnectionHelper.Password);
                
                OperationContext.Current.OutgoingMessageHeaders.Add(username);
                OperationContext.Current.OutgoingMessageHeaders.Add(password);
            }
        }

        public async void deneme()
        {
            string content = null;
            HttpResponseMessage req = await client.GetAsync("http://www.eshot.gov.tr/tr/Anasayfa");

            if (req != null && req.IsSuccessStatusCode)
            {
                var data = await req.Content.ReadAsStringAsync();
                htmlDocument.LoadHtml(WebUtility.HtmlDecode(data));
                HtmlNode parent = htmlDocument.DocumentNode.Descendants("select").First(o => o.GetAttributeValue("class", "") == "chosen-select form-control");

                content = parent.InnerHtml;
            }
        }

        public async Task<string> GetAbout()
        {
            string content = null;
            HttpResponseMessage req = await client.GetAsync("http://www.eshot.gov.tr/tr/Tarihce/26/19?AspxAutoDetectCookieSupport=1");

            if (req != null && req.IsSuccessStatusCode)
            {
                var data = await req.Content.ReadAsStringAsync();
                htmlDocument.LoadHtml(WebUtility.HtmlDecode(data));
                HtmlNode parent = htmlDocument.DocumentNode.Descendants("td").First(o => o.GetAttributeValue("style", "") == "text-align: left; vertical-align: top;");

                content = parent.InnerHtml;
            }

            return content;
        }

        public async Task<List<BusModel>> GetBuses()
        {
            List<BusModel> list = new List<BusModel>();

            var response = await serviceClient.HatlariGetirAsync();
            foreach (var item in response)
            {
                list.Add(new BusModel()
                {
                    Id = item.Id,
                    Name = item.Adi,
                    StartStation = item.Gidis,
                    FinishStation = item.Donus
                });
            }

            return list;
        }

        public async Task<List<ScheduleModel>> GetSchedules()
        {
            List<ScheduleModel> list = new List<ScheduleModel>();

            var response = await serviceClient.TarifeleriGetirAsync(1);
            foreach (var item in response)
            {
                list.Add(new ScheduleModel()
                {
                    Id = item.TarifeId,
                    Name = item.Adi
                });
            }

            return list;
        }
        
        public async Task<List<TransportationTimeModel>> GetTransportationTime(int busId, int scheduleId)
        {
            List<TransportationTimeModel> list = new List<TransportationTimeModel>();

            int i = 0;
            var response = await serviceClient.HareketSaatleriniHattaTarifeyeGoreGetirAsync(busId, scheduleId);
            foreach (var item in response)
            {
                list.Add(new TransportationTimeModel()
                {
                    Gidis = item.Gidis,
                    Donus = item.Donus,
                    BgColor = (i++ % 2 == 0) ? "#FFFFFF" : "#F9F9F9"
                });
            }

            return list;
        }

        public async Task<List<BusModel>> SearchBuses(string keyword)
        {
            List<BusModel> list = new List<BusModel>();

            var response = await serviceClient.HatAraAsync(keyword);
            foreach (var item in response)
            {
                list.Add(new BusModel()
                {
                    Id = item.Id,
                    Name = item.Adi,
                    StartStation = item.Gidis,
                    FinishStation = item.Donus
                });
            }

            return list;
        }

        public async Task<List<NearbyBusModel>> GetOncomingBusesToStation(int stationId)
        {
            List<NearbyBusModel> list = new List<NearbyBusModel>();

            var response = await serviceClient.YaklasanOtobusleriDuragaGoreGetirAsync(stationId);
            foreach (var item in response)
            {
                list.Add(new NearbyBusModel()
                {
                    Id = item.HatNumarasi,
                    Name = item.HatAdi,
                    RemainingTime = item.KalanDakika.ToString()
                });
            }

            return list;
        }

        public async Task<List<StationModel>> GetBusStops(int busId, int direction)
        {
            List<StationModel> list = new List<StationModel>();

            var response = await serviceClient.HatDuraklariniGetirAsync(busId, direction);
            foreach (var item in response)
            {
                list.Add(new StationModel()
                {
                    Id = item.Id,
                    Name = item.Adi,
                    Bus = item.GecenHatNumaralari,
                    Latitude = Convert.ToDouble(item.KoorY),
                    Longitude = Convert.ToDouble(item.KoorX),
                    Distance = Convert.ToDouble(item.Mesafe)
                });
            }

            return list;
        }

        public async Task<List<StationModel>> SearchBusStops(string keyword)
        {
            List<StationModel> list = new List<StationModel>();

            var response = await serviceClient.DurakAraAsync(keyword);
            foreach (var item in response)
            {
                list.Add(new StationModel()
                {
                    Id = item.Id,
                    Name = item.Adi,
                    Bus = item.GecenHatNumaralari,
                    Latitude = Convert.ToDouble(item.KoorY),
                    Longitude = Convert.ToDouble(item.KoorX),
                    Distance = Convert.ToDouble(item.Mesafe)
                });
            }

            return list;
        }

        public async Task<List<StationModel>> GetNearStations(double latitude, double longitude)
        {
            List<StationModel> stationList = new List<StationModel>();

            var resposne = await serviceClient.NoktayaEnYakinDuraklariGetirAsync(longitude, latitude);
            foreach (var item in resposne)
            {
                stationList.Add(new StationModel()
                {
                    Id = item.Id,
                    Name = item.Adi,
                    Bus = item.GecenHatNumaralari,
                    Latitude = Convert.ToDouble(item.KoorY),
                    Longitude = Convert.ToDouble(item.KoorX),
                    Distance = Convert.ToDouble(item.Mesafe)
                });
            }

            return stationList;
        }

        public async Task<string> GetCardBalance(string cardId)
        {
            return await serviceClient.UlasimKartiBakiyesiGetirAsync(cardId);
        }

        public async Task<List<BaseModel>> GetMetroStations()
        {
            List<BaseModel> list = new List<BaseModel>();

            var response = await serviceClient.MetroIstasyonlariniGetirAsync();
            foreach (var item in response)
            {
                list.Add(new BaseModel()
                {
                    Id = item.Id,
                    Name = Helpers.StationHelper.StationChanger(item.Adi.ToUpper()),
                });
            }

            return list;
        }

        public async Task<List<BaseModel>> GetIzbanStations()
        {
            List<BaseModel> list = new List<BaseModel>();

            var response = await serviceClient.IzbanIstasyonlariniGetirAsync();
            foreach (var item in response)
            {
                list.Add(new BaseModel()
                {
                    Id = item.Id,
                    Name = item.Adi.ToUpper()
                });
            }

            return list;
        }

        public async Task<List<BaseModel>> GetVapurStations()
        {
            List<BaseModel> list = new List<BaseModel>();

            var response = await serviceClient.VapurIskeleleriniGetirAsync();
            foreach (var item in response)
            {
                list.Add(new BaseModel()
                {
                    Id = item.Id,
                    Name = item.Adi.ToUpper()
                });
            }

            return list;
        }
    }
}