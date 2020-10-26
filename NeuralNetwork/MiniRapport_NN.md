# Mini Rapport NN (Neural Network)

Projet Visual 32 bits.

Ce rapport décrit brièvement mon TP sur les réseaux de neurones en utilisant la librairie FANN (en utilisant un *wrapper* C# : https://github.com/joelself/FannCSharp).

Deux réseaux de neurones sont implémentés :

- L'exemple XOR : pour tester le bon fonctionnement du *wrapper* C# de FANN. Ce dernier est une pure copie de l'exemple vu en cours et dans les slides.

  

- Et mon mini-projet sur les langues. Comme langues j'ai choisi **Français, Allemand, Italien, Espagnol et Anglais**. C'est un petit panel de langues indo-européenne. J'utilise une petite banque de données contenant 6 exemples pour chaque langues. J'ai diversifié les exemples en prenant des citations du livre "Le Petit Prince", l'article 1er de la déclaration des droits de l'Homme et quelques pages choisies aux hasard dans Wikipédia (une énorme source de texte pour l'entrainement !).

  En entré nous avons la fréquence d'apparition des 26 caractères communs de l'alphabet latin, c'est à dire les caractères de base, sans accents et communs à toutes les langues latines.

  En sortie, nous obtenons une probabilité pour chaque langue. Cette probabilité nous indique si le texte est écrit en telle ou telle langue.

  En test, j'ai uniquement pris 2 textes écrit en français.

  Dans la vidéo, vous observerez que le réseau de neurone trouve la bonne langue. Lors de me tests, il est parfois arrivé que le réseau de neurone soit indécis entre 2 langues. Le français était toujours présent avec une probabilité proche de 1 mais l'espagnol était souvent très proche en probabilité ! On peut expliquer cela par un manque de données d'entrainement.

