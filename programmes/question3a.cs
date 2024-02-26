using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class occurences
{
    static void Main()
    {

        Console.WriteLine("Entrez le nom du fichier à analyser");
        string mot = Console.ReadLine();

        // On lit le texte :
        String texte = ReadFile(mot);

        // On récupère la liste des mots :
        String[] mots = ListerMots(texte);

        // On met les mots en minuscule
        List<String> listeMotsMinuscules = Minuscules(mots);

        // On supprime les mots vides
        List<String> listeMotsVides = SupprimerMotsVides(listeMotsMinuscules);

        // On applique les étapes
        List<String> listeMotsEtapes = Traiter(listeMotsVides);

        // On compte les occurences 
        Dictionary<String, int> occurences = CompterOccurences(listeMotsEtapes);

        AfficherDictionnaire(occurences);
        
    }

    static string ReadFile(string path)
    {
        return System.IO.File.ReadAllText(path);
    }

    static String[] ListerMots(String mots) {
        char[] separators = new char[] {',',';',':','/','!','(',')','"',' ','.','?','\'','\t','\n','0','1','2','3','4','5','6','7','8','9'};
        String[] liste = mots.Split(separators);
        return liste;
    }

    static void AfficherListe(List<String> liste) {
        foreach(String element in liste) {
            Console.WriteLine(element);
        }
    }

    static void AfficherDictionnaire(Dictionary<string,int> dictionnaire)
    {
        foreach(KeyValuePair<string,int> kvp in dictionnaire)
        {
            Console.WriteLine(kvp.Key + ":" + kvp.Value);
        }
    }

    static Dictionary<String, int> CompterOccurences(List<String> mots) {

        Dictionary<String, int> dictionnary = new Dictionary<String, int>();

        foreach(String mot in mots) {
            if (dictionnary.ContainsKey(mot)) {
                dictionnary[mot]++;
            } else {
                dictionnary.Add(mot, 1);
            }
        }

        return dictionnary;

    }

    static List<String> SupprimerMotsVides(List<String> mots) {

        // Récupération de la liste des mots vides
        String[] motsVides = ListerMots(ReadFile("assets/stopwords.txt"));

        // Création d'une nouvelle liste
        List<String> nouvelleListe = new List<string>();

        foreach(String mot in mots) {

            Boolean detecte = false;
            for(int i = 0; i<motsVides.Count() && !detecte; i++) {
                if (motsVides[i] == mot) detecte = true;
            }

            if (!detecte) nouvelleListe.Add(mot);

        }

        return nouvelleListe;

    }

    static List<String> Minuscules(String[] mots) {
        List<String> liste = new List<string>();
        foreach(String mot in mots) {
            liste.Add(mot.ToLower());
        }
        return liste;
    }

    static List<String> Traiter(List<String> mots) {

        // Obtention des traitements
        List<String[]> traitement1 = ObtenirTraitement("assets/etape1.txt");
        List<String[]> traitement2 = ObtenirTraitement("assets/etape2.txt");
        List<String[]> traitement3 = ObtenirTraitement("assets/etape3.txt");

        // Création de la nouvelle liste
        List<String> liste = new List<string>();

        // Pour chaque mot
        foreach(String mot in mots) {

            // Si le mot est plus long qu'un caractère
            if (mot.Length > 1) {

                // Mettre le mot en minuscule
                String motTraite = mot;

                // Appliquer les 3 traitement :
                motTraite = TraiterMot(motTraite, traitement1);
                motTraite = TraiterMot(motTraite, traitement2);
                motTraite = TraiterMot(motTraite, traitement3);

                // Ajouter le mot à la liste si il ne fait pas partie des interdits
                liste.Add(motTraite);

            }

        }

        return liste;

    }

    static List<string[]> ObtenirTraitement(string texte)
    {
        List<string[]> lignemots=new List<string[]>();
        string ligne;
        StreamReader sr= new StreamReader(texte);
        while((ligne=sr.ReadLine())!=null)
        {
            lignemots.Add(ligne.Split(' '));
        }
        return lignemots;
    }

    static String TraiterMot(String mot, List<String[]> traitement) {

        Boolean resolu = false;
        for(int i = 0; i<traitement.Count && !resolu; i++) {

            String terminaison = traitement[i][1];
            
            // Si la terminaison est plus grande que le mot, inutile de tester
            if (terminaison.Length < mot.Length) {
                String terminaisonInverse = Inverser(terminaison);
                String motInverse = Inverser(mot);
                
                // Tester la correspondance
                int j = 0;
                while (j < terminaisonInverse.Length && motInverse[j] == terminaisonInverse[j]) {
                    // Console.WriteLine("{0} - {1} : {2}", motInverse[j], terminaisonInverse[j], j);
                    j++;
                }

                // Console.WriteLine("Mot : {0} Terminaison : {1} J:" + j.ToString(), mot, terminaison);

                // Si la terminaison à correspondu jusqu'au bout
                if (j == terminaisonInverse.Length) {
                    // On a trouvé un traitement à effectuer

                    //Console.WriteLine("Traitement : " + mot + " : " + traitement[i][1]);

                    // Si c'est epsilon, on supprime simplement
                    String remplacement = (traitement[i][2] == "epsilon") ? "" : traitement[i][2];

                    // On effectue le remplacement
                    mot = mot.Replace(traitement[i][1], remplacement);

                    resolu = true;
                }
            }

        }

        return mot;

    }

    public static string Inverser(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse( charArray );
        return new string( charArray );
    }



}
