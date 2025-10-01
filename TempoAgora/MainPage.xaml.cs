using System.Threading.Tasks;
using TempoAgora.Models;

namespace TempoAgora
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao =
                            $"Cidade: {txt_cidade.Text}\n" +
                            $"Clima: {t.main} - {t.description}\n" +
                            $"Visibilidade: {t.visibility} m\n" +
                            $"Vento: {t.speed} m/s\n" +
                            $"Latitude: {t.lat}\n" +
                            $"Longitude: {t.lon}\n" +
                            $"Nascer do sol: {t.sunrise}\n" +
                            $"Pôr do sol: {t.sunset}\n" +
                            $"Temp Máx: {t.temp_max}\n" +
                            $"Temp Min: {t.temp_min}\n";

                        lbl_res.Text = dados_previsao;

                        string mapa = $"https://embed.windy.com/embed.html?" +
                                      $"type=map&location=coordinates&metricRain=mm&metricTemp=°C" +
                                      $"&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                                      $"&lat={t.lat.ToString().Replace(",", ".")}&lon={t.lon.ToString().Replace(",", ".")}";

                        wv_mapa.Source = mapa;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }

                }
                else
                {
                    lbl_res.Text = "Preencha a cidade";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request =
                    new GeolocationRequest(
                        GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10));

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude}\n" +
                        $"Longitude: {local.Longitude}";

                    lbl_coords.Text = local_disp;

                    // Pega o nome da cidade que está nas coordenadas
                    GetCidade(local.Latitude, local.Longitude);

                }
                else
                {
                    lbl_coords.Text = "Nenhuma localização";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não suporta ", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: localização desabilitada ", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão da localização", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                if (place != null)
                {
                    txt_cidade.Text = place.Locality;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: obtenção do nome da cidade", ex.Message, "OK");
            }
        }
    }
}
