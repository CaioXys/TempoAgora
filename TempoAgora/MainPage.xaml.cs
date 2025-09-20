using System.Threading.Tasks;
using TempoAgora.Models;

namespace TempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if(t != null)
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
                    } else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }

                } else
                {
                    lbl_res.Text = "Preencha a cidade";
                }
            } catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }

}
