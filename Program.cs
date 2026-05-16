
using Programmierprojekt1.Models;
using Programmierprojekt1.Services;
using Programmierprojekt1.Data;

namespace Programmierprojekt1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            List<PortfolioEntry> portfolio =
PortfolioStorage.LoadPortfolio();

            int option = -1;

            do
            {
                Console.WriteLine();
                Console.WriteLine("===== WERTPAPIERPORTFOLIO =====");
                Console.WriteLine("1. Portfolio anzeigen");
                Console.WriteLine("2. Aktie hinzufügen");
                Console.WriteLine("3. Aktie entfernen");
                Console.WriteLine("4. Preise aktualisieren");
                Console.WriteLine("5. Rendite berechnen");
                Console.WriteLine("6. Beenden");

                option = ReadNumber("Auswahl: ");
                
                switch (option)
                {
                    case 1:
                        PortfolioService.ShowPortfolio(portfolio);
                        break;

                    case 2:
                        await PortfolioService.AddToPortfolio(portfolio);
                        break;

                    case 3:
                        PortfolioService.RemoveFromPortfolio(portfolio);
                        break;

                    case 4:
                        await PortfolioService.UpdatePrices(portfolio);
                        break;

                    case 5:
                        PortfolioService.CalculateReturns(portfolio);
                        break;

                    case 6:
                        PortfolioStorage.SavePortfolio(portfolio);
                        Console.WriteLine("Portfolio gespeichert.");
                        break;

                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        break;
                }

            } while (option != 6);
        }

        static int ReadNumber(string text)
        {
            Console.Write(text);

            if (int.TryParse(Console.ReadLine(), out int number))
                return number;

            return 0;
        }
    }
}
