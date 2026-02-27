namespace Projet_Algo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string choix;
            bool c = true;
            while (c)
            {
                Console.Clear();
                Console.WriteLine("Que souhaitez vous faire?");
                Console.WriteLine("     1. Jouer");
                Console.WriteLine("     2. Lancer batterie de test");
                Console.WriteLine("     3. Sortir");
                Console.Write("Votre choix : ");
                choix = Console.ReadLine();

                if (choix == "1")
                {
                    jouer();
                }
                else if (choix == "2")
                {
                    TestFonctionnement();
                }
                else if (choix == "3")
                {
                    c = false;
                }
                else
                {
                    Console.WriteLine("Entrée invalide, appuyez sur Entrée pour réessayer.\n");
                    Console.ReadLine();
                }
            }
        }

        static void TestFonctionnement()
        {
            Console.WriteLine("=== TEST 1 : DICTIONNAIRE ===");
            Dictionnaire dico = new Dictionnaire();

            Console.WriteLine(dico.toString());

            string motExiste = "Maison";
            bool test1 = dico.RechDichoRecursif(motExiste);
            Console.WriteLine($"Recherche '{motExiste}' : " + (test1 ? "OK (Trouvé)" : "ERREUR (Non trouvé)"));

            string motInconnu = "Xywz";
            bool test2 = dico.RechDichoRecursif(motInconnu);
            Console.WriteLine($"Recherche '{motInconnu}' : " + (!test2 ? "OK (Non trouvé)" : "ERREUR (Trouvé)"));

            Console.WriteLine("\nAppuyez sur Entrée pour passer au chargement des Poids...");
            Console.ReadLine();

            Console.WriteLine("=== TEST 2 : CHARGEMENT PLATEAU & POIDS ===");
            Console.WriteLine("Chargement de Lettre.txt pour initialiser les scores...");
            Plateau pAleatoire = new Plateau("Lettre.txt");

            int indexK = 'K' - 'A';

            if (indexK >= 0 && indexK < 26)
            {
                int poidsK = Plateau.PoidsLettres[indexK];
                Console.WriteLine($"Poids de la lettre 'K' chargé : {poidsK} (Attendu : 10)");
            }
            else
            {
                Console.WriteLine("Erreur : Index hors limites.");
            }

            Console.WriteLine(pAleatoire.toString());

            Console.WriteLine("\nAppuyez sur Entrée pour passer au Joueur...");
            Console.ReadLine();


            Console.WriteLine("=== TEST 3 : JOUEUR ===");
            Joueur j1 = new Joueur("Testeur");

            string motTrouve = "KANGOUROU";
            j1.Add_Mot(motTrouve);

            int sommePoids = 0;
            foreach (char c in motTrouve)
            {
                int index = char.ToUpper(c) - 'A';

                if (index >= 0 && index < 26)
                {
                    sommePoids += Plateau.PoidsLettres[index];
                }
            }

            int scoreCalcule;

            if (sommePoids == 0)
            {
                scoreCalcule = motTrouve.Length;
            }
            else
            {
                scoreCalcule = sommePoids * motTrouve.Length;
            }

            j1.Add_Score(scoreCalcule);

            Console.WriteLine($"Simulation : Ajout du mot '{motTrouve}'");
            Console.WriteLine(j1.toString());
            Console.WriteLine($"Score attendu (10+1+1+1+1+1+1+1+1) * 9 = 162. Score affiché ci-dessus doit être 162.");
            Console.WriteLine($"Le joueur a le mot 'kangourou' ? {j1.Contient("kangourou")}");

            Console.WriteLine("\nAppuyez sur Entrée pour passer au Test Fichier CSV...");
            Console.ReadLine();


            // Test de reconnaissance de plateau prédéfini en fichier CSV
            Console.WriteLine("=== TEST 4 : LECTURE FICHIER CSV ===");

            string fichierTest = "TestUnitaire.csv";
            using (StreamWriter textCSVPlateau = new StreamWriter(fichierTest)) // On crée un fichier CSV temporaire avec streamWriter
            {
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("A;B;C;D;E;F;G;H");
                textCSVPlateau.WriteLine("M;A;I;S;O;N;G;H");
            }

            Plateau pFichier = new Plateau(fichierTest);
            Console.WriteLine(pFichier.toString());

            Console.WriteLine("Recherche du mot 'MAISON'...");
            List<int[]> res = pFichier.Recherche_Mot("MAISON");

            if (res != null)
            {
                Console.WriteLine("Mot trouvé ! Mise à jour du plateau...");
                pFichier.Maj_Plateau(res);
                Console.WriteLine(pFichier.toString());
            }
            else
            {
                Console.WriteLine("Erreur : Mot MAISON non trouvé.");
            }

            File.Delete(fichierTest);
            Console.WriteLine("Tests terminés. Appuyez sur Entrée.");
            Console.ReadLine();
        }

        static void jouer()
        {
            Console.Clear();
            Jeu partie = new Jeu();
            partie.Commencer();
        }
    }
}
