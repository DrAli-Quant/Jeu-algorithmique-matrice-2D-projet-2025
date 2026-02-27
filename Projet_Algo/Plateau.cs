using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Algo
{
    internal class Plateau
    {
        private char[,] matrice;
        private int nbLignes = 8;
        private int nbColonnes = 8;


        public static int[] PoidsLettres = new int[26];
        private static bool PoidsSontCharges = false;

        public Plateau(string nomFichier)
        {
            ChargerPoidsGlobal();

            this.matrice = new char[nbLignes, nbColonnes];

            if (string.IsNullOrWhiteSpace(nomFichier))
            {
                Console.WriteLine("Attention : Nom de fichier invalide/vide pour le plateau.");
                return;
            }

            if (nomFichier.Contains("Lettre.txt"))
            {
                GenererAleatoirement(nomFichier);
            }
            else
            {
                ToRead(nomFichier);
            }
        }

        private void ChargerPoidsGlobal()
        {
            // Si déjà chargé on ne refait pas le travail
            if (PoidsSontCharges) return;

            string fichier = "Lettre.txt";
            if (!File.Exists(fichier)) {
                Console.WriteLine("Le fichier Lettre.txt devant servir a detrminer le poid des lettres est introuvable");
                return; }

            try
            {
                using (StreamReader Lecteur = new StreamReader(fichier))
                {
                    string ligne;
                    while ((ligne = Lecteur.ReadLine()) != null)
                    {
                        string[] infos = ligne.Split(',');
                        if (infos.Length >= 3)
                        {
                            char lettre = char.Parse(infos[0].Trim());
                            int poids = int.Parse(infos[2].Trim());

                            int index = char.ToUpper(lettre) - 'A';

                            // Sécurité pour ne pas sortir du tableau
                            if (index >= 0 && index < 26)
                            {
                                PoidsLettres[index] = poids;
                            }
                        }
                    }
                    PoidsSontCharges = true; // On marque comme fait
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur chargement poids : " + e.Message);
            }
        }

        private void GenererAleatoirement(string cheminFichierLettres)
        {
            List<char> sacDeLettres = new List<char>();
            Random rand = new Random();

            try
            {
                using (StreamReader lecteur = new StreamReader(cheminFichierLettres))
                {
                    string ligne;
                    while ((ligne = lecteur.ReadLine()) != null)
                    {
                        string[] infos = ligne.Split(',');
                        if (infos.Length >= 2)
                        {
                            char lettre = char.Parse(infos[0].Trim());
                            int frequenceMax = int.Parse(infos[1].Trim());

                            for (int i = 0; i < frequenceMax; i++)
                            {
                                sacDeLettres.Add(lettre);
                            }
                        }
                    }
                }

                for (int i = 0; i < nbLignes; i++)
                {
                    for (int j = 0; j < nbColonnes; j++)
                    {
                        if (sacDeLettres.Count > 0)
                        {
                            int indexAleatoire = rand.Next(sacDeLettres.Count);
                            this.matrice[i, j] = sacDeLettres[indexAleatoire];
                            sacDeLettres.RemoveAt(indexAleatoire);
                        }
                        else
                        {
                            this.matrice[i, j] = ' ';
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la génération aléatoire : " + e.Message);
            }
        }

        public void ToRead(string nomfile)
        {
            try
            {
                using (StreamReader Lecteur = new StreamReader(nomfile))
                {
                    string ligne;
                    int i = 0;
                    while ((ligne = Lecteur.ReadLine()) != null && i < nbLignes)
                    {
                        string[] lettres = ligne.Split(';');
                        for (int j = 0; j < nbColonnes && j < lettres.Length; j++)
                        {
                            this.matrice[i, j] = char.Parse(lettres[j]);
                        }
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur de lecture du fichier plateau : " + e.Message);
            }
        }

        public void ToFile(string nomfile)
        {
            if (string.IsNullOrWhiteSpace(nomfile)) return;
            try
            {
                using (StreamWriter sw = new StreamWriter(nomfile))
                {
                    for (int i = 0; i < nbLignes; i++)
                    {
                        string ligne = "";
                        for (int j = 0; j < nbColonnes; j++)
                        {
                            ligne += this.matrice[i, j];
                            if (j < nbColonnes - 1) ligne += ";";
                        }
                        sw.WriteLine(ligne);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur d'écriture du fichier : " + e.Message);
            }
        }

        public string toString()
        {
            string affichage = "   ";
            for (int k = 0; k < nbColonnes; k++) affichage += k + " ";
            affichage += "\n";

            for (int i = 0; i < nbLignes; i++)
            {
                affichage += i + " |";
                for (int j = 0; j < nbColonnes; j++)
                {
                    affichage += this.matrice[i, j] + " ";
                }
                affichage += "|\n";
            }
            return affichage;
        }

        public List<int[]> Recherche_Mot(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot) || mot.Length < 2) return null;

            string motUpper = mot.Trim().ToUpper();
            int ligneBase = nbLignes - 1;

            for (int col = 0; col < nbColonnes; col++)
            {
                if (this.matrice[ligneBase, col] == motUpper[0])
                {
                    List<int[]> chemin = new List<int[]>();
                    chemin.Add(new int[] { ligneBase, col });

                    if (RechercheRecursif(motUpper, 1, ligneBase, col, chemin))
                    {
                        return chemin;
                    }
                }
            }
            return null;
        }

        private bool RechercheRecursif(string mot, int indexLettre, int lig, int col, List<int[]> chemin)
        {
            if (indexLettre == mot.Length) return true;

            int[][] directions = new int[][]
            {
                new int[] {-1, 0},
                new int[] {0, -1},
                new int[] {0, 1},
                new int[] {-1, -1},
                new int[] {-1, 1}
            };

            foreach (int[] d in directions)
            {
                int suivLigne = lig + d[0];
                int suivColonne = col + d[1];

                if (suivLigne >= 0 && suivLigne < nbLignes && suivColonne >= 0 && suivColonne < nbColonnes)
                {
                    if (this.matrice[suivLigne, suivColonne] == mot[indexLettre])
                    {
                        if (!EstDejaDansChemin(suivLigne, suivColonne, chemin))
                        {
                            chemin.Add(new int[] { suivLigne, suivColonne });
                            if (RechercheRecursif(mot, indexLettre + 1, suivLigne, suivColonne, chemin))
                            {
                                return true;
                            }
                            chemin.RemoveAt(chemin.Count - 1);
                        }
                    }
                }
            }
            return false;
        }

        private bool EstDejaDansChemin(int l, int c, List<int[]> chemin)
        {
            foreach (int[] coords in chemin)
            {
                if (coords[0] == l && coords[1] == c) return true;
            }
            return false;
        }

        public void Maj_Plateau(List<int[]> objet)
        {
            if (objet == null) return;
            List<int[]> chemin = (List<int[]>)objet;

            foreach (int[] cases in chemin)
            {
                this.matrice[cases[0], cases[1]] = ' ';
            }

            for (int j = 0; j < nbColonnes; j++)
            {
                AppliquerGraviteColonne(j);
            }
        }

        private void AppliquerGraviteColonne(int col)
        {
            List<char> lettresRestantes = new List<char>();
            for (int i = 0; i < nbLignes; i++) // On établie la liste des lettres a faire tomber
            {
                if (this.matrice[i, col] != ' ')
                {
                    lettresRestantes.Add(this.matrice[i, col]);
                }
            }

            int indexMatrice = nbLignes - 1;
            int indexListe = lettresRestantes.Count - 1;

            while (indexMatrice >= 0)
            {
                if (indexListe >= 0) // <=> Si il reste des lettres a faire tomber
                {
                    this.matrice[indexMatrice, col] = lettresRestantes[indexListe];
                    indexListe--;
                }
                else //sinon on noircie
                {
                    this.matrice[indexMatrice, col] = ' ';
                }
                indexMatrice--;
            }
        }

        public bool EstPlateauVide()
        {
            for (int i = 0; i < nbLignes; i++)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    if (this.matrice[i, j] != ' ') return false;
                }
            }
            return true;
        }
    }
}
