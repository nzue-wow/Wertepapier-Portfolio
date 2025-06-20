using System.Text.Json;

namespace Programmierprojekt1
{
    internal class Program
    {
        static readonly string filePath = "portfolio.json";

        static async Task Main(string[] args)
        {
            string token = "636d8ad7fbe0d66b929e147730283024d5135102";
            List<PortfolioEntry> portfolio = LoadPortfolioFromFile();

            // Funktionen des Programms
            int option = -1;
            do
            {
                Console.WriteLine("-------------------");
                Console.WriteLine("Wertpapierportfolio");
                Console.WriteLine("-------------------");
                Console.WriteLine("1. Portfolio anzeigen");
                Console.WriteLine("2. Wertpapier hinzufügen");
                Console.WriteLine("3. Wertpapier entfernen");
                Console.WriteLine("4. Preise laden");
                Console.WriteLine("5. Rendite berechnen");
                Console.WriteLine("6. Beenden");
                option = ReadNumber("Was möchten Sie tun? ");

                switch (option)
                {
                    case 1:
                        Console.Clear();
                        ShowPortfolio(portfolio);
                        break;
                    case 2:
                        AddToPortfolio(portfolio, token);
                        break;
                    case 3:
                        RemoveFromPortfolio(portfolio);
                        break;
                    case 4:
                        await UpdatePriceData(portfolio, token);
                        break;
                    case 5:
                        CalculatePortfolioReturns(portfolio);
                        break;
                    case 6:
                        Console.WriteLine("Speichere Portfolio...");
                        SavePortfolioToFile(portfolio);
                        Console.WriteLine("Programm beendet.");
                        break;


                    default:
                        Console.WriteLine("Fehlerhafte Eingabe!");
                        break;
                }
                Console.WriteLine();
            } while (option != 6);
        }

        static void SavePortfolioToFile(List<PortfolioEntry> portfolio)
        {
            try
            {
                string json = JsonSerializer.Serialize(portfolio, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine("Portfolio wurde erfolgreich gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern des Portfolios: {ex.Message}");
            }
        }


        static List<PortfolioEntry> LoadPortfolioFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Keine gespeicherten Daten gefunden. Leeres Portfolio wird erstellt.");
                    return new List<PortfolioEntry>();
                }

                string json = File.ReadAllText(filePath);

                // Wenn die Datei leer ist, zurücksetzen
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("Datei ist leer. Leeres Portfolio wird erstellt.");
                    return new List<PortfolioEntry>();
                }

                return JsonSerializer.Deserialize<List<PortfolioEntry>>(json) ?? new List<PortfolioEntry>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden des Portfolios: {ex.Message}");
                return new List<PortfolioEntry>();
            }
        }




        // Tabelle fürs gesamte Portfolio
        static void ShowPortfolio(List<PortfolioEntry> portfolio)
        {
            double totalPrice = 0;
            Console.WriteLine("Sie haben aktuell folgende Wertpapiere im Portfolio:");
            Console.WriteLine("Anzahl | Tickersymbol |    Preis |      Total |           Kaufpreis ");
            Console.WriteLine("-------+--------------+----------+------------+---------------------------");
            foreach (PortfolioEntry portfolioEntry in portfolio)
            {
                double price = portfolioEntry.quantity * portfolioEntry.valuePerPiece;
                totalPrice += price;
                Console.WriteLine($"{portfolioEntry.quantity,6} | {portfolioEntry.ticker,12} | {portfolioEntry.valuePerPiece,8:N2} | {price,10:N2} | {portfolioEntry.anfangsPreis,9:N2}");
            }
            Console.WriteLine($"Gesamtwert des Portfolios: {totalPrice:N2}€");

        }

        // Wertepapiere hinzufügen
        static async void AddToPortfolio(List<PortfolioEntry> portfolio, string token)
        {
            Console.Write("Geben Sie das Tickersymbol ein: ");
            string ticker = Console.ReadLine();
            int quantity = ReadNumber("Anzahl der Aktien: ");
            Console.Write("Wann haben Sie die Aktien gekauft? (yyyy-mm-dd): ");
            string date = Console.ReadLine();
            
            try
            {
                string url = GetPriceUrl(ticker, token, date);
                ApiResponse<PriceData> response = await GetPriceData(url);
                double anfangsPreis = response.data[0].close;

                string aktuellUrl = GetPriceUrl(ticker, token, null);
                ApiResponse<PriceData> aktuellResponse = await GetPriceData(aktuellUrl);
                double aktuellerPreis = aktuellResponse.data[0].close;
                string aktuellesDatum = aktuellResponse.data[0].date;
                portfolio.Add(new PortfolioEntry(quantity, ticker, aktuellerPreis, anfangsPreis, aktuellesDatum));
                Console.WriteLine($"{quantity} Aktien von {ticker} mit Kaufpreis {anfangsPreis}€ wurden hinzugefügt.");
            }
            catch
            {
                Console.WriteLine($"Fehler beim Hinzufügen der Aktie {ticker}");
            }
        }

        // Wertepapiere vom Portfolio entfernen
        static void RemoveFromPortfolio(List<PortfolioEntry> portfolio)
        {
            Console.Write("Geben Sie das Tickersymbol ein, das entfernt werden soll: ");
            string ticker = Console.ReadLine();
            portfolio.RemoveAll(p => p.ticker == ticker);
            Console.WriteLine($"{ticker} wurde aus dem Portfolio entfernt.");
        }

        // Preise auf neusten Stand bringen
        static async Task UpdatePriceData(List<PortfolioEntry> portfolio, string token)
        {
            foreach (var entry in portfolio)
            {
                try
                {
                    string url = GetPriceUrl(entry.ticker, token, null);
                    ApiResponse<PriceData> response = await GetPriceData(url);

                    if (response?.data == null || response.data.Length == 0)
                    {
                        Console.WriteLine($"Keine Preisdaten für {entry.ticker} gefunden.");
                        continue;
                    }

                    PriceData latestData = response.data[0];
                    entry.valuePerPiece = latestData.close; // Wert aktualisieren
                    entry.priceDate = latestData.date;     // Datum aktualisieren

                    Console.WriteLine($"Preis für {entry.ticker} aktualisiert: {latestData.close}€ ({latestData.date})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Aktualisieren von {entry.ticker}: {ex.Message}");
                }
            }
        }

        // 
        static void CalculatePortfolioReturns(List<PortfolioEntry> portfolio)
        {
            double totalInitialInvestment = 0;
            double totalCurrentValue = 0;

            foreach (var entry in portfolio)
            {
               
                totalInitialInvestment += entry.quantity * entry.anfangsPreis;
                totalCurrentValue += entry.quantity * entry.valuePerPiece;
            }

            double totalReturn = CalculateRendit(totalInitialInvestment, totalCurrentValue);
            Console.WriteLine($"Gesamtinvestition: {totalInitialInvestment:N2}€");
            Console.WriteLine($"Aktueller Wert: {totalCurrentValue:N2}€");
            Console.WriteLine($"Gesamtrendite: {totalReturn:F2}%");
        }

        static int ReadNumber(string prompt)
        {
            Console.Write(prompt);
            return int.TryParse(Console.ReadLine(), out int number) ? number : 0;
        }

        // Daten aus dem Internet holen
        static string GetPriceUrl(string ticker, string token, string? date)
        {
            if (date != null)
            {
                return $"https://api.tiingo.com/tiingo/daily/{ticker}/prices?startDate={date}&endDate={date}&token={token}";

            }
            return $"https://api.tiingo.com/tiingo/daily/{ticker}/prices?token={token}";

        }

        // Preise aus dem nternet holen
        static async Task<ApiResponse<PriceData>> GetPriceData(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                    throw new Exception($"Fehler bei der API-Antwort: {resp.StatusCode}");

                string content = await resp.Content.ReadAsStringAsync();
                content = $"{{\"data\":{content}}}";
                return JsonSerializer.Deserialize<ApiResponse<PriceData>>(content);
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Abrufen der Daten", ex);
            }
        }

        // Rendite berechnen
        static double CalculateRendit(double purchasePrice, double currentPrice)
        {
            return ((currentPrice - purchasePrice) / purchasePrice) * 100;
        }

        // Speicherung der Daten
        class PortfolioEntry
        {
            public int quantity { get; set; }
            public double anfangsPreis { get; set; }
            public string ticker { get; set; }
            public double valuePerPiece { get; set; }
            public string? priceDate { get; set; }

            public PortfolioEntry(int quantity, string ticker, double valuePerPiece, double anfangsPreis, string? priceDate)
            {
                this.quantity = quantity;
                this.anfangsPreis = anfangsPreis;
                this.ticker = ticker;
                this.valuePerPiece = valuePerPiece;
                this.priceDate = priceDate;
            }
        }

        record ApiResponse<T>(T[] data);

        record PriceData(double close, string date);
    }
}

