# Szerver
A SmartHome rendszerhez a backend szerverét felelős porgram.
A választott technológiák ASP.NET Core C# nyelven és REST architektúrát követve.
A szerver felépítését tekintve pedig kétirányú lesz, hiszen funkcionál mint REST endpoint és közben maga is egy kliens és indít REST kéréseket a node-k felé.
 
## 1. Üzenetek

A szerver egyrészt fogad kéréseket a node-októl, másrészt pedig a frontend-től.
Továbbá küld kérést a node-oknak.

### 1.1. Fogadás

**Frontend -> szerver**
 - POST: időzített kérések, tartalma, hogy mit és mikor
 - POST: on-demand kérések, tartalma, hogy mit és rögtön végrehajtásra kerül
 
 **Node -> szerver** 
 - POST: New child, amikor egy új node csatlakozott amesh-hálózathoz, tartalma az új node adatai

### 1.2. Küldés

 **Szerver -> node**
 - GET: állapot lekérdezése a node-oknak, tartalma a node adatai

## 2. Adatbázis
A szerverhez tartozni fog egy adatbázis is, amelyben a node-okról tároljuk el az adatokat. Ez az adatbázis két táblából fog állni, az egyikben tároljuk, magának a node-nak az adatait, a másikban pedig a node által szolgáltatott adatokat visszamenőleg mint egy history.

A táblák sémái az alábbiak:

Az esp-k adatait összefogó fő tábla:      
| #ID | IP | Type | Last Seen |  
|---|---|---|---|  
| Kulcs | IP címe a esp-nek  | Milyen típusú szenzor/beavatkozó | Mikor volt utoljára aktív státuszban |  


Az esp adatait historikusan tároló tábla:  
| #ID | TimeStamp | Data |  
|---|---|---|  
|   |   |   |
