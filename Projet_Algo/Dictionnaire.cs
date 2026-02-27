using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Algo
{
    internal class Dictionnaire
    {
        public List<string> tousLesMots;
        private int[] compteMotDeLettreInitial = new int[26];
        private string NomFichierDictionnaire = "Mots_Français.txt";
        /* Le tableau compteLettre permettra d'optimiser le tri rapide en tirant profit du fait que le dictionnaire
         est déjà semi-ordonnée : Les mots d'une même ligne ont déjà la même première lettre. Avec les lignes déjà
         alphabétiquement ordonné par la lettre initial qu'ils représentent (ligne 1 : ligne 2 : B ... ligne 26 : Z) */

        public Dictionnaire()
        {
            this.tousLesMots = new List<string>();
            ChargerDictionnaire();
            Tri_Rapide();
        }

        private void ChargerDictionnaire()
        {
            try
            {
                using (StreamReader lecteur = new StreamReader(NomFichierDictionnaire))
                {
                    string ligne;
                    int numLigne = 0;
                    int compteurMot; // Permettra de compter le nombre de mot de la ligne actuelle, et donc le nombre de mot avec cette lettre initial.

                    while ((ligne = lecteur.ReadLine()) != null)
                    {
                        string[] motsDeLaLigne = ligne.Split(' '); // Du fait de la structure du dictionnaire on utilise l'espace comme séparateur.
                        compteurMot = 0;

                        foreach (string mot in motsDeLaLigne)
                        {
                            /* On vérifie que le "mot" n'est pas une abbération qui découle du séparateur ' ' car 
                               une succession de 3 espaces (ou plus) ferait que l'espace du milieu serait considéré comme un "mot" */
                            if (!string.IsNullOrWhiteSpace(mot))
                            {
                                this.tousLesMots.Add(mot.Trim().ToLower()); // On enlève espace résiduel et on met tout en minuscule pour simplifier la relation d'odre du tri
                                compteurMot++;
                            }
                        }
                        compteMotDeLettreInitial[numLigne] = compteurMot; // Pour chaque indice i on injecte le (vrai) nombre de mot de la ligne i+1
                        numLigne++;
                    }
                }
            }
            catch (FileNotFoundException) // On fait un catch a part pour l'erreur la plus probable et la moins grave
            {
                Console.WriteLine($"Erreur : Le fichier du dictionnaire '{NomFichierDictionnaire}' est introuvable.");
            }
            catch // Les autres erreurs seront signe d'un problème plus important
            {
                Console.WriteLine($"Erreur : Fichier trouvé mais lecture impossible");
            }
        }

        public void Tri_Rapide()
        {
            int a = 0;
            /* C'est ici qu'on tire partie de l'ordre partiel du dictionnaire :
               On peut simplement établir le tri par interval de lettre inital.
               De cette manière on évite le tri inutile de mot ayant une lettre initial différente,
               on ne fait donc qu'un tri "intra-ligne" (même lettre initial) resultant en une amélioration significative de performance 
               par économie de calcule inutile.*/
            for (int i = 0; i < 26; i++)
            {
                QuickSort(a, a+compteMotDeLettreInitial[i]-1);
                a += compteMotDeLettreInitial[i];
            }
        }
        private void QuickSort(int a, int b)
        {
            /* C'est ici que se fait le vrai tri intra-ligne. Du fait de la taille du dictionnaire, un tri fusion
               consommerai énormément de mémoire a cause des duplications d'intervals de la liste lors des multiples récursions.
               Le tri rapide, bien que sa compléxité temporelle théorique soit moint bonne dans le pire des cas, n'a pas besoin
               de duplications et représente donc une diminution significative de compléxité spatial dans le cas d'une liste 
               avec plus de 130 000 éléments */
            if (a<b)
            {
                int i = a - 1;
                int j = a;
                string t;
                while (j != b)
                {
                    if (tousLesMots[j].CompareTo(tousLesMots[b]) < 0) // On utilise la méthode .CompareTo() pour établir la relation d'ordre, elle permet de comparer directement deux mots par ordre alphabétique
                    {
                        i++;
                        t = tousLesMots[i];
                        tousLesMots[i] = tousLesMots[j];
                        tousLesMots[j] = t;
                    }
                    j++;
                }
                t = tousLesMots[i + 1];
                tousLesMots[i + 1] = tousLesMots[b];
                tousLesMots[b] = t;
                QuickSort(a, i);
                QuickSort(i + 2, b);
            }
        }

        public bool RechDichoRecursif(string mot)
        {
            bool res = true;
            if (string.IsNullOrWhiteSpace(mot) || this.tousLesMots == null || this.tousLesMots.Count == 0) // Evite les recherches invalides
            {
                res = false;
            }
            else
            {
                string m = mot.Trim().ToLower(); // On "normalise" le mot a trouver
                int l = 0;
                // On utilise ici la même technique de séparation par lettre initial
                for (int i = 0; i < (int)m[0] - 97; i++) // On utilise le code Unicode de la lettre initial dont on soustrait 97 car c'est le code de 'a'
                {
                    l += compteMotDeLettreInitial[i];
                }
                res = RDichotomique(m, l, l + compteMotDeLettreInitial[(int)m[0] - 97] - 1);
            }
             return res;
        }

        private bool RDichotomique(string s, int a, int b)
        {
            // C'est ici que se fait la vrai recherche dichotomique recursive avec son algorithme classique
            if (a > b)
            {
                return false;
            }
            else
            {
                int milieu = a + (b - a)/2;
                // Conformément au tri effectué, on réutilise .CompareTo() comme relation d'ordre pour la recherche
                if (tousLesMots[milieu].CompareTo(s) < 0)
                {
                    return RDichotomique(s, milieu + 1, b);
                }
                else if(tousLesMots[milieu].CompareTo(s) > 0)
                {
                    return RDichotomique(s, a, milieu - 1);
                }
                else
                {
                    return true ;
                }
            }
        }

        public string toString()
        {
            string res = "Langue du dictionnaire : Français\n\nNombre de mots par lettre :\n\n";
            for (int i = 0; i<26; i++)
            {
                res += (char)(65 + i)+" : " + compteMotDeLettreInitial[i]+"\n";
            }
            res += "\nTotal cumulé : "+Convert.ToString(compteMotDeLettreInitial.Sum());
            return res;
        }
        public bool EstTrie()
        {
            //C'est une simple fonction de test permettant de vérifier que le tri est effectif
            if (this.tousLesMots == null || this.tousLesMots.Count <= 1)
            {
                return true;
            }

            for (int i = 0; i < this.tousLesMots.Count - 1; i++)
            {
                if (this.tousLesMots[i].CompareTo(this.tousLesMots[i + 1]) > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
