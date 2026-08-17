# Wertpapierportfolio – Projekt IMS

Dieses Programm ermöglicht es, ein Wertpapierportfolio zu verwalten und die Rendite einzelner Wertpapiere sowie die Gesamtentwicklung des Portfolios zu berechnen. Es handelt sich um ein Konsolenprogramm in C#, das aktuelle Kursdaten aus dem Internet bezieht und die Wertentwicklung übersichtlich anzeigt.

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

 *Beispiel*
  <img width="679" height="1354" alt="image" src="https://github.com/user-attachments/assets/1763453b-91c7-43e5-bc8b-53c164239396" />


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

## Nutzung des Wertpapierportfolio
Um das Wertpapierportfolio verwenden zu können, ist ein Account bei Tiingo erforderlich.

**1. Account erstellen**

Registriere dich auf der folgenden Website:
https://www.tiingo.com/

**2. API Token erhalten**

Nach der Registrierung:

Navigiere im Dashboard zum Bereich Documentation
Dort findest du deinen persönlichen API Token
Kopiere diesen Token

**3. Token im Projekt einfügen**

Öffne im Projekt die Datei:

`Services/ApiService.cs`

Suche folgende Zeile:

`private static readonly string token = "YOUR_TOKEN";`

Ersetze "YOUR_TOKEN" durch deinen persönlichen API Token:

`private static readonly string token = "DEIN_API_TOKEN";`

**4. Programm starten**

Nach dem Einfügen des Tokens kann das Programm gestartet werden.
Du kannst nun:

Aktien zum Portfolio hinzufügen
aktuelle Kurse abrufen
dein Portfolio verwalten
Renditen berechnen

*Hinweis*

Ohne gültigen API Token ist keine Verbindung zur Tiingo-API möglich und das Programm kann keine aktuellen oder historischen Kursdaten laden.
