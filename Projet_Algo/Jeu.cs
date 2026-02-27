using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Algo
{
    internal class Jeu
    {
        private Dictionnaire dictionnaire;
        private Plateau plateau;
        private List<Joueur> joueurs;
        private TimeSpan dureePartie;
        private TimeSpan dureeTour;
        // On utilise TimeSpan pour la gestion du temps

        public Jeu()
        {
            this.dictionnaire = new Dictionnaire();
            this.joueurs = new List<Joueur>();
            this.dureePartie = TimeSpan.FromMinutes(2); // Temps par défaut
            this.dureeTour = TimeSpan.FromSeconds(30);
        }

        public void Commencer()
        {
            InitialiserJoueurs();
            ConfigurerTemps();

            bool continuer = true;
            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== MENU MOTS GLISSES ===");
                Console.WriteLine("1. Jouer à partir d'un plateau aléatoire");
                Console.WriteLine("2. Jouer à partir d'un fichier");
                Console.WriteLine("3. Sortir");
                Console.Write("Votre choix : ");

                string choix = Console.ReadLine();

                if (choix == "1")
                {
                    LancerPartie("Lettre.txt");
                }
                else if (choix == "2")
                {
                    Console.WriteLine("Quel est le nom du fichier que vous voulez utiliser comme plateau initial? (Par exemple Save.csv)");
                    string nomFichier = Console.ReadLine();
                    LancerPartie(nomFichier.Trim());
                }
                else if (choix == "3")
                {
                    continuer = false;
                }
                else
                {
                    Console.WriteLine("Choix invalide. Appuyez sur Entrée.");
                    Console.ReadLine();
                }
            }
        }

        private void InitialiserJoueurs()
        {
            int nInt = 0;
            bool saisieValide = false;

            while (!saisieValide) // Demande en boucle tant que saisi non valide
            {
                Console.WriteLine("Nombre de joueurs (Appuyez sur Entrée pour 2 par défaut) :");
                string nString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nString))
                {
                    nInt = 2;
                    saisieValide = true;
                    Console.WriteLine(">> Option par défaut choisie : 2 joueurs.");
                }
                else if (int.TryParse(nString, out int valeur) && valeur > 0)
                {
                    nInt = valeur;
                    saisieValide = true;
                }
                else
                {
                    Console.WriteLine("Erreur : Saisie invalide. Veuillez entrer un nombre entier positif.");
                }
            }

            for (int i = 0; i < nInt; i++) // On constitue le nom des joueurs et on établi les différentes instances de classe joueur
            {
                Console.WriteLine($"Nom du joueur {i + 1} :");
                string nom = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nom)) nom = $"Joueur {i + 1}";
                this.joueurs.Add(new Joueur(nom));
            }
        }

        private void ConfigurerTemps()
        {
            bool saisieValide = false;

            while (!saisieValide)
            {
                Console.WriteLine("Durée de la partie en minutes (Appuyez sur Entrée pour 2 par défaut) :");
                string partieString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(partieString))
                {
                    this.dureePartie = TimeSpan.FromMinutes(2);
                    Console.WriteLine(">> Option par défaut choisie : 2 minutes.");
                    saisieValide = true;
                }
                else if (int.TryParse(partieString, out int min) && min > 0)
                {
                    this.dureePartie = TimeSpan.FromMinutes(min);
                    saisieValide = true;
                }
                else
                {
                    Console.WriteLine("Erreur : Veuillez entrer un nombre entier positif pour les minutes.");
                }
            }

            saisieValide = false;

            while (!saisieValide) // Demande en boucle tant que saisi non valide
            {
                Console.WriteLine("Durée du tour par joueur en secondes (Appuyez sur Entrée pour 30 par défaut) :");
                string tourString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(tourString)) // Cas par défaut
                {
                    this.dureeTour = TimeSpan.FromSeconds(30);
                    Console.WriteLine(">> Option par défaut choisie : 30 secondes.");
                    saisieValide = true;
                }
                else if (int.TryParse(tourString, out int sec) && sec > 0)
                {
                    this.dureeTour = TimeSpan.FromSeconds(sec);
                    saisieValide = true;
                }
                else
                {
                    Console.WriteLine("Erreur : Veuillez entrer un nombre entier positif pour les secondes.");
                }
            }
        }

        private void LancerPartie(string nomFichier)
        {
            if (!nomFichier.Contains("Lettre") && !File.Exists(nomFichier))
            {
                Console.WriteLine($"\nErreur : Le fichier '{nomFichier}' est introuvable.");
                Console.WriteLine("Vérifiez qu'il est bien dans le dossier");
                Console.WriteLine("Appuyez sur entrée pour revenir au menu");
                Console.ReadLine();
                return;
            }

            this.plateau = new Plateau(nomFichier);

            if (this.plateau.EstPlateauVide())
            {
                Console.WriteLine("Erreur : Le plateau généré est vide.");
                return;
            }

            DateTime debutPartie = DateTime.Now;
            bool partieTerminee = false;
            int indexJoueur = 0;

            Console.WriteLine("Le jeu commence !");
            Console.WriteLine(this.plateau.toString());

            while (!partieTerminee)
            {
                if (DateTime.Now - debutPartie > this.dureePartie)
                {
                    Console.WriteLine("Temps de la partie écoulé !");
                    partieTerminee = true;
                    break;
                }

                if (this.plateau.EstPlateauVide())
                {
                    Console.WriteLine("Le plateau est vide !");
                    partieTerminee = true;
                    break;
                }

                JouerTour(this.joueurs[indexJoueur]);

                indexJoueur++;
                if (indexJoueur >= this.joueurs.Count) indexJoueur = 0;
            }

            FinDePartie();
        }

        private void JouerTour(Joueur joueurActuel)
        {
            DateTime debutTour = DateTime.Now;
            bool tourTermine = false;

            Console.WriteLine($"\n--- C'est à {joueurActuel.toString().Split('|')[0]} de jouer ---");

            while (!tourTermine)
            {
                TimeSpan tempsEcoule = DateTime.Now - debutTour;
                if (tempsEcoule > this.dureeTour)
                {
                    Console.WriteLine("Temps du tour écoulé !");
                    tourTermine = true;
                    break;
                }

                Console.WriteLine($"Temps restant pour ce tour : {(int)(this.dureeTour - tempsEcoule).TotalSeconds+1} secondes");
                Console.WriteLine("Entrez un mot (ou appuyez sur Entrée pour passer) :");

                string mot = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(mot))
                {
                    Console.WriteLine("Tour passé.");
                    tourTermine = true;
                    break;
                }

                if (joueurActuel.Contient(mot))
                {
                    Console.WriteLine("Vous avez déjà trouvé ce mot !");
                    continue;
                }

                if (mot.Length < 2)
                {
                    Console.WriteLine("Le mot doit faire au moins 2 lettres.");
                    continue;
                }

                if (!this.dictionnaire.RechDichoRecursif(mot))
                {
                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire.");
                    continue;
                }

                List<int[]> resultatRecherche = this.plateau.Recherche_Mot(mot);

                if (resultatRecherche != null)
                {
                    Console.WriteLine($"Bravo ! Mot '{mot}' trouvé.");

                    this.plateau.Maj_Plateau(resultatRecherche);

                    joueurActuel.Add_Mot(mot);

                    int scoreMot = CalculerScore(mot);
                    joueurActuel.Add_Score(scoreMot);

                    Console.WriteLine(this.plateau.toString());
                }
                else
                {
                    Console.WriteLine("Ce mot n'est pas formable sur le plateau.");
                }
            }
        }

        private int CalculerScore(string mot)
        {
            int sommePoids = 0;
            string motUpper = mot.ToUpper();

            foreach (char c in motUpper)
            {
                int index = c - 'A';

                if (index >= 0 && index < 26)
                {
                    sommePoids += Plateau.PoidsLettres[index];
                }
            }
            
            return sommePoids * mot.Length;
        }

        private void FinDePartie()
        {
            Console.WriteLine("\n=== FIN DE LA PARTIE ===");
            Joueur gagnant = null;
            int maxScore = -1;

            foreach (Joueur j in this.joueurs)
            {
                Console.WriteLine(j.toString());
                string[] parts = j.toString().Split('|');
                int score = 0;

                if (parts.Length > 1)
                {
                    string scorePart = parts[1].Replace("Score :", "").Trim();
                    int.TryParse(scorePart, out score);
                }

                if (score > maxScore)
                {
                    maxScore = score;
                    gagnant = j;
                }
            }

            if (gagnant != null)
            {
                Console.WriteLine($"\nLe gagnant est {gagnant.toString().Split('|')[0]} avec {maxScore} points !");
            }
            Console.WriteLine("Appuyez sur Entrée pour revenir au menu.");
            Console.ReadLine();
        }
    }
}
