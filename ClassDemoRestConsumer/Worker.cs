using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModelLib.model;
using Newtonsoft.Json;

namespace ClassDemoRestConsumer
{
    internal class Worker
    {

        private const string URI = "http://localhost:50935/api/Facilitet";

        public Worker()
        {
        }

        public void Start()
        {
            List<Facilitet> hotels = GetAll();

            foreach (var hotel in hotels)
            {
                Console.WriteLine("Facilitet:: " + hotel.Name);
            }

            Console.WriteLine("Henter nummer 2");
            Console.WriteLine("Facilitet :: " + GetOne(2));


            Console.WriteLine("Sletter nummer 2");
            Console.WriteLine("Resultat = " + Delete(2));

            Console.WriteLine("Opretter nyt Facilitet object id findes ");
            Console.WriteLine("Resultat = " + Post(new Facilitet(){Facilitetnr = 1, Name = "Lagkage"}));

            Console.WriteLine("Opretter nyt hotel object id findes ikke");
            Console.WriteLine("Resultat = " + Post(new Facilitet(){Facilitetnr = 2, Name = "Bold Tennis"}));

            Console.WriteLine("Opdaterer nr 50");
            Console.WriteLine("Resultat = " + Put(2, new Facilitet(){ Facilitetnr = 2, Name = "Bord Tennis"}));
        }


        private List<Facilitet> GetAll()
        {
            List<Facilitet> hoteller = new List<Facilitet>();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI);
                String jsonStr = resTask.Result;

                hoteller = JsonConvert.DeserializeObject<List<Facilitet>>(jsonStr);
            }


            return hoteller;
        }
        


        private Facilitet GetOne(int id)
        {
            Facilitet hotel = new Facilitet();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                String jsonStr = resTask.Result;

                hotel = JsonConvert.DeserializeObject<Facilitet>(jsonStr);
            }


            return hotel;
        }

        private bool Delete(int id)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(URI + "/" + id);

                HttpResponseMessage resp = deleteAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    String jsonStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }


            return ok;
        }

        private bool Post(Facilitet hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.ASCII, "application/json");

                Task<HttpResponseMessage> postAsync = client.PostAsync(URI,content);

                HttpResponseMessage resp = postAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    String jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonResStr);
                }
                else
                {
                    ok = false;
                }
            }


            return ok;
        }

        private bool Put(int id, Facilitet hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                String jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> putAsync = client.PutAsync(URI+ "/" + id, content);

                HttpResponseMessage resp = putAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    String jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonResStr);
                }
                else
                {
                    ok = false;
                }
            }


            return ok;
        }



    }
}