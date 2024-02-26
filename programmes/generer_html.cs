// Projet créer par : AL Hammuti Sabrina et Tanguy Neu
// But: Génération de la page HTML à partir des données CSV

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

// Création d'une classe 
class projet
{

    // Fonction qui permet de lire le fichier 
    // path:String : chemin du fichier 
    // return : String : contenu du fichier
    static string LireFichier(string path)
    {
        return System.IO.File.ReadAllText(path);
    }

    // Fonction qui permet d'écrire dans le fichier
    // Path: String : chemin du fichier 
    // Content: String:  le contenu du fichier 
    static void EcrireFichier(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    // Fonction qui de compléter une section du site 
    // Source: String : Le code source dans lequel insérer le contenu
    // Key: String : Nom de la section a compléter
    // Value: String : Le contenu à insérer
    // Return : String : Le code complété
    static string PlacerContenu(string source, string key, string value)
    {
        return source.Replace('{' + key + '}', value);
    }


    //Fonction qui décode le CSV 
    // Path: String : chemin du fichier 
    // Return: List<String[]> : données du CSV
    static List<String[]> LireCSV(String path)
    {

        // On crée une nouvelle liste qui sera le resultat (une liste de lignes qui contiennent chaqune une liste de colonne)
        List<String[]> result = new List<String[]>();

        // On récupère le contenu tout entier du CSV
        String contenu = LireFichier(path);

        // On sépare chaque ligne (séparateur : nouvelle ligne (\n))
        String[] lignes = contenu.Split('\n');

        // Pour chaque ligne du csv
        foreach (String l in lignes)
        {

            // L = le contenu de la ligne
            // On sépare les colonnes de la ligne (séparateur des colonnes : ",")
            String[] colonnes = l.Split(',');

            String[] colonnesResult = new String[colonnes.Length];
            int i = 0;
            // On supprime les espaces avant et après
            foreach (String element in colonnes)
            {
                colonnesResult[i] = element.Trim();
                i++;
            }

            // Et on ajoute cette liste de colones, formant donc une ligne, à la liste résultats
            result.Add(colonnesResult);

        }

        return result;
    }



    // Fonction d'interpolation linéaire
    // Min: Float : Valeur minimal
    // Max: Float : Valeur maximal
    // X: Float : c'est le pourcentage souhaitée entre 0 et 1
    // Return: Float : la valeur correspondant entre min et max
    public static float lerp(float min, float max, float x)
    {
        return min * (1 - x) + max * x;
    }

    public static Dictionary<String, int> TrierDictionnaires(Dictionary<String, int> source, int taille) {

        Dictionary<String, int> dictionnary = new Dictionary<string, int>();

        int i = 0;
        foreach(KeyValuePair<String,int> kvp in source.OrderByDescending(v=> v.Value)) {
            
            dictionnary.Add(kvp.Key, kvp.Value);

            i++;
            if (i>taille) {
                break;
            }
        }

        return dictionnary;

    }

    // Fonction qui retourne un degrés aléatoire ( parmi la liste)
    // Return: int : orientation en degrés choisie
    public static int OrientationAleatoire()
    {
        // On créer un random
        Random rng = new Random();
        // Liste des orientations possible
        List<int> orientations = new List<int>() { 0, 45, 90 };
        // Retourne l'orientation choisie au hasard
        return orientations[rng.Next(orientations.Count)];
    }

    // Fonction qui mélange une liste 
    // List: List<string[]> : liste à mélanger 
    public static void Melanger(List<string[]> list)
    {
        // On créer un random
        Random rng = new Random();
        int n = list.Count;
        // Tant que n est inférieur à 1 
        while (n > 1)
        {
            // On décrémente n 
            n--;
            // On choisis le nombre aléatoire
            int k = rng.Next(n + 1);
            string[] value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public static Dictionary<String, int> ListeDictionnaire(List<String[]> source) {
        // Conversion en dictionnaire
        Dictionary<String, int> dictionnaire = new Dictionary<string, int>();
        foreach(String[] item in source) {
            if (item.Length == 2) {
                dictionnaire.Add(item[0], int.Parse(item[1]));
            }
        }
        return dictionnaire;
    }

    public static List<String[]> DictionnaireListe(Dictionary<String, int> source) {

        List<String[]> list = new List<String[]>();
        foreach(KeyValuePair<String,int> kvp in source) {
            String[] array = new String[2];
            array[0] = kvp.Key;
            array[1] = kvp.Value.ToString();
            list.Add(array);
        }
        return list;
    }

    // Fonction qui créer un nuage de mot 
    // Words : List<string[]> : liste des mots
    // Return: String : return le nuage de mot  en html
    static string GenererNuage(List<string[]> words)
    {

        int nbMots = 20;

        // Création du nuage 
        string result = "<div class='word-cloud'>{mots}</div>";

        // Conversion de la liste en dictionnaire 
        Dictionary<String, int> dictionnaire = ListeDictionnaire(words);

        // Tri des éléments
        Dictionary<String, int> tri = TrierDictionnaires(dictionnaire, nbMots);
        
        // Initialisation du minimun et du maximun en fonction de l'importance du mots dans la liste de mots 
        int min = tri.ElementAt(tri.Count-1).Value;
        int max = tri.ElementAt(0).Value;

        // Conversion du dictionnaire en liste
        List<String[]> randomWords = DictionnaireListe(tri);

        // Appel de la fonction qui va mélanger la liste de mots 
        Melanger(randomWords);

        // Initialisation d'un string 
        string mots = "";

        // Pour chaque mots 
        foreach (String[] s in randomWords)
        {
            // Création du mot au nuage avec une taille proportionnelle à sont importance et une orientation aléatoire 
            mots += "<p style='font-size:" + (int)lerp(20, 60, (float.Parse(s[1]) - min) / max) + "px; transform: rotate(" + OrientationAleatoire() + "deg);' >" + s[0] + "</p>";
        }

        // On place le mots dans le nuage
        result = PlacerContenu(result, "mots", mots);
        return result;

    }

    static void Main()
    {

        // Chemin d'accès des fichiers
        string resultFolder = "./../html";
        string sourcesFolder = "./../htmlsources";
        string csvFolder = "./../csv";

        string mainPath = sourcesFolder + "/main.html";
        string footerPath = sourcesFolder + "/footer.html";
        string navigationPath = sourcesFolder + "/navigation.html";
        string periodePath = sourcesFolder + "/periode.html";
        string presidentPath = sourcesFolder + "/president.html";
        string accueilPath = sourcesFolder + "/accueil.html";
        string courantPath = sourcesFolder + "/courant.html";

        string presidentCsv = csvFolder + "/presidents.csv";
        string courantCsv = csvFolder + "/courant.csv";
        string periodeCsv = csvFolder + "/periode.csv";
        string toutCsv = csvFolder + "/Tout.csv";

        // Création d'une liste des sources
        List<string> liste = new List<string>(new string[] { mainPath, footerPath, navigationPath, periodePath, presidentPath, presidentCsv, accueilPath, courantPath, periodePath, periodeCsv, courantCsv, toutCsv });


        try
        {

            // Vérifier l'existence des sources 
            foreach (string element in liste)
            {
                if (!File.Exists(element))
                {
                    Console.WriteLine("La source existe pas " + element);
                    Environment.Exit(0);
                }
            }

            // Supprimer les résultats si ils existent déjà (tout les fichiers contenus dans /resultats/)
            foreach (string file in Directory.GetFiles(resultFolder))
            {
                File.Delete(file);
            }

            // On recrée le dossier résultat
            Directory.CreateDirectory(resultFolder);

            // Lecture des sources
            string main = LireFichier(mainPath);
            string navigation = LireFichier(navigationPath);
            string footer = LireFichier(footerPath);
            string periode = LireFichier(periodePath);
            string president = LireFichier(presidentPath);
            string accueil = LireFichier(accueilPath);
            string courant = LireFichier(courantPath);

            // Créer la page d'accueil 
            string pageAccueil = main;

            // Ajout du contenu de la page d'accueil
            pageAccueil = PlacerContenu(pageAccueil, "contenu", accueil);

            // Ajout du footer
            pageAccueil = PlacerContenu(pageAccueil, "footer", footer);

            List<string[]> presidentListe = LireCSV(presidentCsv);
            List<string[]> courantListe = LireCSV(courantCsv);
            List<string[]> periodeListe = LireCSV(periodeCsv);
            // Pour chaque président :

            // Création de la liste des boutons de présidents sur l'accueil
            string presidentButtons = "";

            foreach (string[] p in presidentListe)
            {

                // Créer sa page

                string pagePresident = main;
                pagePresident = PlacerContenu(pagePresident, "contenu", president);
                pagePresident = PlacerContenu(pagePresident, "footer", footer);
                pagePresident = PlacerContenu(pagePresident, "navigation", navigation);

                // Compléter sa page

                pagePresident = PlacerContenu(pagePresident, "nom", p[0]);
                pagePresident = PlacerContenu(pagePresident, "description", p[1]);
                pagePresident = PlacerContenu(pagePresident, "image", p[2]);

                // Récupérer son fichier CSV

                List<string[]> presidentWords = LireCSV(csvFolder + "/" + p[3] + ".csv");

                // Faire le nuage de mot et l'intégrer

                string presidentCloud = GenererNuage(presidentWords);
                pagePresident = PlacerContenu(pagePresident, "contenu", presidentCloud);

                // Enregistrer sa page
                string fileName = p[3] + ".html";
                EcrireFichier(resultFolder + "/" + fileName, pagePresident);

                // L'ajouter à l'accueil
                presidentButtons += "<li><a href=\"" + fileName + "\" class=\"button\">" + p[0] + "</a></li>" + Environment.NewLine;

            }

            // Ajout des boutons président à l'accueil 
            pageAccueil = PlacerContenu(pageAccueil, "politiciens", presidentButtons);

            // Pour chaque courant

            // Création de la liste des boutons de courant sur l'accueil
            string courantButtons = "";

            foreach (string[] p in courantListe)
            {

                // Créer sa page
                string pageCourant = main;
                pageCourant = PlacerContenu(pageCourant, "contenu", courant);
                pageCourant = PlacerContenu(pageCourant, "navigation", navigation);
                pageCourant = PlacerContenu(pageCourant, "footer", footer);

                // Compléter sa page
                pageCourant = PlacerContenu(pageCourant, "description", p[0]);

                // Récupérer son fichier CSV

                List<string[]> courantWord = LireCSV(csvFolder + "/" + p[1] + ".csv");

                // Faire le nuage de mot et l'intégrer

                string courantCloud = GenererNuage(courantWord);
                pageCourant = PlacerContenu(pageCourant, "contenu", courantCloud);

                // Enregistrer sa page
                string fileName = p[1] + ".html";
                EcrireFichier(resultFolder + "/" + fileName, pageCourant);

                // L'ajouter à l'accueil
                courantButtons += "<li><a href=\"" + fileName + "\" class=\"button\">" + p[0] + "</a></li>" + Environment.NewLine;

            }

            pageAccueil = PlacerContenu(pageAccueil, "courants", courantButtons);

            // Pour chaque période

            // Création de la liste des boutons de periode sur l'accueil
            string periodeButtons = "";

            foreach (string[] p in periodeListe)
            {

                // Créer sa page
                string pagePeriode = main;
                pagePeriode = PlacerContenu(pagePeriode, "contenu", periode);
                pagePeriode = PlacerContenu(pagePeriode, "navigation", navigation);
                pagePeriode = PlacerContenu(pagePeriode, "footer", footer);

                pagePeriode = PlacerContenu(pagePeriode, "description", p[0]);

                // Récupérer son fichier CSV
                List<string[]> periodeWord = LireCSV(csvFolder + "/" + p[1] + ".csv");

                // Faire le nuage de mot et l'intégrer
                string periodeCloud = GenererNuage(periodeWord);
                pagePeriode = PlacerContenu(pagePeriode, "contenu", periodeCloud);

                // Enregistrer sa page
                string fileName = p[1] + ".html";
                EcrireFichier(resultFolder + "/" + fileName, pagePeriode);

                // L'ajouter à l'accueil
                periodeButtons = "<li><a href=\"" + fileName + "\" class=\"button\">" + p[0] + "</a></li>" + Environment.NewLine;

            }
            pageAccueil = PlacerContenu(pageAccueil, "decenies", periodeButtons);

            // Ajout du nuage de tout les discours
            pageAccueil = PlacerContenu(pageAccueil, "tout", GenererNuage(LireCSV(toutCsv)));

            // Ecriture de l'accueil
            EcrireFichier(resultFolder + "/index.html", pageAccueil);

            Console.WriteLine("Pages générés avec succès !");
            Console.WriteLine("Ouvrez le fichier index.html situé dans le dossier /html pour le consulter !");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    static void AfficherDictionnaire(Dictionary<string,int> dictionnaire)
    {
        foreach(KeyValuePair<string,int> kvp in dictionnaire)
        {
            Console.WriteLine(kvp.Key + " : " + kvp.Value);
        }
    }

}