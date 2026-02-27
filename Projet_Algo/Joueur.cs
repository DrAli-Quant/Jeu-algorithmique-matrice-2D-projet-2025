using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Projet_Algo
{
    internal class Joueur
    {
        private string nom;
        private List<string> mots;
        private int score;

        public Joueur(string nom)
        {
            this.nom = nom;
            this.score = 0;
            this.mots = null;
        }

        public void Add_Mot(string mot)
        {
            if (this.mots == null)
            {
                this.mots = new List<string>();
            }
            this.mots.Add(mot);
        }

        public string toString()
        {
            string listeMots;
            if (this.mots == null)
            {
                listeMots = "Aucun mot trouvé pour le moment.";
            }
            else
            {
                listeMots = string.Join(", ", this.mots);
            }

            return "Nom : " + nom + " | Score : " + score + " | Mots : " + listeMots;
        }

        public void Add_Score(int val)
        {
            if (val > 0)
            {
                score += val;
            }
        }


        public bool Contient(string mot)
        {
            if (this.mots == null || string.IsNullOrWhiteSpace(mot)) // Sécurité pour mots invalides
            {
                return false;
            }

            string motCherche = mot.Trim().ToUpper();

            foreach (string m in this.mots)
            {
                if (m.ToUpper() == motCherche)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
