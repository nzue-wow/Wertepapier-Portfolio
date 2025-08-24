# Wertpapierportfolio – Projekt IMS

Dieses Programm ermöglicht es, ein **Wertpapierportfolio** zu verwalten und die **Rendite einzelner Wertpapiere** sowie die **Gesamtentwicklung des Portfolios** zu berechnen. Es handelt sich um ein Konsolenprogramm in C#, das aktuelle Kursdaten aus dem Internet bezieht und die Wertentwicklung übersichtlich anzeigt.

## Funktionen

- **Portfolio verwalten:**  
  - Wertpapiere (Aktien, Fonds, Obligationen) hinzufügen und entfernen  
  - Speicherung des Portfolios in einer JSON-Datei (`portfolio.json`)  

- **Daten aus dem Internet abrufen:**  
  - Aktuelle Kurse über die Tiingo-API laden  
  - Kaufpreise werden mit den aktuellen Kursen verglichen  

- **Rendite berechnen:**  
  - Rendite einzelner Wertpapiere  
  - Gesamt-Rendite des Portfolios  

- **Portfolio anzeigen:**  
  - Übersicht über Anzahl, Kaufpreis, aktuellen Preis, Rendite und Gesamtwert  
  - Einfache Konsolenanzeige der Wertentwicklung  

## Ziel

- Überblick über die Wertentwicklung des eigenen Portfolios behalten  
- Gewinne und Verluste einzelner Wertpapiere nachvollziehen  
- Einfaches, leicht verständliches Konsolenprogramm für die Portfolioverwaltung  

## Benutzung

1. Programm starten  
2. Menüoptionen wählen:  
   - Portfolio anzeigen  
   - Wertpapier hinzufügen  
   - Wertpapier entfernen  
   - Preise aktualisieren  
   - Rendite berechnen  
   - Beenden (speichert automatisch das Portfolio)  
3. Für neue Wertpapiere das Tickersymbol, die Anzahl und das Kaufdatum eingeben  

## Technische Details

- Programmiert in **C# (.NET 6.0)**  
- Daten werden über die **Tiingo-API** abgerufen  
- Portfolio wird lokal als JSON-Datei gespeichert  
- Renditeberechnung: `((aktueller Preis – Kaufpreis) / Kaufpreis) * 100`
