# Jeu de Mots Glissés sur matrice 2D

**Note :** Ce dépôt est une archive d'un projet individuel académique réalisé en décembre 2025 dans sa forme pré-final.

## Contexte du projet
Ce projet consiste en la conception et le développement en C# d'un jeu de reconnaissance de mot dans une matrice 2D de lettre généré aléatoirement selon un taux d'apparition prédéfini des différentes lettres pour augmenter les chances d'apparition de mots français dans la matrice.
# Moteur de Résolution Algorithmique 2D (Jeu de Mots Glissés)

> **Note :** Ce dépôt est une archive d'un projet académique réalisé en 2025. Il a été importé dans sa version finale pour documenter mon approche de l'architecture logicielle, de l'optimisation des structures de données et des algorithmes de parcours de graphe.

## 📝 Contexte et Architecture
Ce projet consiste en la conception et le développement en C# d'un jeu de reconnaissance de mot dans une matrice 2D de lettres générées aléatoirement selon un taux d'apparition prédéfini des différentes lettres pour augmenter les chances d'apparition de mots français dans la matrice. L'application simule un système dynamique où la grille se met à jour par un mécanisme de "gravité" lorsque des éléments sont validés et retirés. L'architecture est strictement orientée objet (C#), avec une séparation claire entre la logique de la matrice (`Plateau.cs`), la validation des données massives (`Dictionnaire.cs`), la machine à états (`Jeu.cs`) et les tests d'intégration (`Program.cs`).

## ⚙️ Optimisations Algorithmiques (Dictionnaire)
Le traitement d'un dictionnaire brut de plus de 130 000 mots nécessitait une minimisation drastique de la complexité temporelle et spatiale.

* **Élagage de l'espace de recherche (Pruning) :** Exploitation de l'ordre partiel naturel du dictionnaire (mots regroupés par lettre initiale). Les mots sont indexés via un tableau statique `compteMotDeLettreInitial`.
* **Tri Rapide (QuickSort) "Intra-Ligne" :** Implémentation d'un tri partitionné. Au lieu de trier l'ensemble de la liste, le tri est opéré uniquement sur des intervalles isolés partageant la même lettre initiale. Le choix du QuickSort a été privilégié par rapport au Tri Fusion pour réduire l'empreinte mémoire spatiale due aux duplications de listes lors de la récursion massive.
* **Recherche Dichotomique Ciblée :** La méthode `RechDichoRecursif` calcule dynamiquement l'index de départ grâce à la lettre initiale, permettant une validation instantanée des mots en temps logarithmique O(log (n)) sur un sous-ensemble restreint de données.

## 🧭 Parcours de Graphe et Dynamique Matriciellle (Plateau)
La matrice est traitée comme un graphe orienté où chaque case est un nœud.

* **Recherche Récursive par DFS :** La méthode `RechercheRecursif` explore la matrice à la recherche de mots valides. Contrainte métier : le point de départ se situe strictement sur la ligne inférieure de la matrice, et l'exploration ne s'effectue qu'à travers 5 vecteurs directionnels spécifiques (remontée et translation latérale : `{-1, 0}, {0, -1}, {0, 1}, {-1, -1}, {-1, 1}`).
* **Gestion d'État et Gravité :** Une fois un chemin validé, les coordonnées sont extraites et la méthode `AppliquerGraviteColonne` réorganise la matrice temporelle en $O(L \times C)$, assurant la descente des caractères restants.

## 🛡️ Robustesse et Validation
Le fichier `Program.cs` inclut une routine `TestFonctionnement()` agissant comme une batterie de tests d'intégration (méthodologie proche du TDD). Elle valide de manière autonome :
1. Les cas limites de la recherche dichotomique (Vrai Positif / Faux Positif).
2. Le calcul pondéré des scores.
3. La simulation complète d'une extraction de mot à partir d'un état matriciel prédéfini injecté via un fichier CSV de test.
