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

        // On compte les occurences 
        Dictionary<String, int> occurences = CompterOccurences(mots);

        AfficherDictionnaire(occurences);
        
    }

    static string ReadFile(string path)
    {
        return System.IO.File.ReadAllText(path);
    }

    static String[] ListerMots(String mots) {
        char[] separators = new char[] {';','.',',',':',' ',':','\n'};
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

    static Dictionary<String, int> CompterOccurences(String[] mots) {

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

}
