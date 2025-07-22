using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Globalization;
using System.Net.Http;



namespace Weather_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void searchBtn_Click(object sender, EventArgs e)
        {
            getWeather();
        }

        async void getWeather()
        {
            string city = citytxt.Text.Trim();
            string APIKey = "effcb62812d63ea0abbfed8c1c210f51";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={APIKey}";

            using (HttpClient client = new HttpClient())
            {
               try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        MessageBox.Show($"{city} not found. Please check the name and try again.", "404 Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();

                    WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                    picIcon.ImageLocation = "https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png";
                    labCondition.Text = Info.weather[0].main;
                    labDetails.Text = Info.weather[0].description.ToUpper();
                    labSunset.Text = ConvertDateTime(Info.sys.sunset).ToShortTimeString();
                    labSunrise.Text = ConvertDateTime(Info.sys.sunrise).ToShortTimeString();
                    labWind.Text = Info.wind.speed.ToString();
                    labPressure.Text = Info.main.pressure.ToString() + " hpa";
                  
                    humidlbl.Text = Info.main.humidity.ToString() +"%";

                    double kelvinTemp = Info.main.temp;
                    double celsiusTemp = kelvinTemp - 273.15;
                    labtemp.Text = $"{celsiusTemp:F2}°C";
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Network error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public DateTime ConvertDateTime(long secs)
        {
            DateTime day= new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc).ToLocalTime();
            day=day.AddSeconds(secs).ToLocalTime();
            return day;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    } }
