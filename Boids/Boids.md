# Rapport Boids

Version Unity : 2020.1.6f1

Nous avons 3 scripts importants :

- **TargetController** : Un script qui nous permet de contrôler le cube avec les touches ZQSD (ou les flèches) pour avancer, reculer, aller à droite ou aller à gauche, et les touches SHIFT & CTRL pour monter et descendre. La cible ne peut sortir de la zone sombre. (Il arrive parfois que j'effectue CTRL + Z lors du mouvement de la cible, se faisant cela annule certains modifications dans l'éditeur au runtime !)
- **BoidManager** : Instancie et contient les informations de la cible et de l'obstacle. On y définit dans l'ordre :
  - le nombre de boid
  - la distance d'attraction
  - la distance d'alignement
  - la distance de répulsion
  - la vitesse des boids
  - un angle "random" permettant aux boids de tourner un petit peu et de ne pas simplement aller tout droit
  - le gameobject *obstacle* (dans notre cas la sphère rouge)
  - la portée de détection et d'évitement de l'obstacle (doit être plus grand que le radius de l'obstacle lui-même : pour mieux observer l'évitement)
  - et en dernier la *cible*
- **Boid** : Le script qui calcule notre flocking. Le flocking implémenté ici est un flocking vectoriel en 3 dimension. nos boids ne peuvent pas sortir de la zone sombre non plus. Ils sont représentés par des cone dont la pointe du cone représente la queue du boid.
  - Pour l'attraction vers la cible, dans le vecteur d'attraction, je rajoute un vecteur de direction vers la cible. Ce vecteur a le même poids que le vecteur d'attraction de base.
  - Pour l'évitement de l'obstacle, j'joute un vecteur en direction opposée à l'obstacle dans le calcul de la répulsion. Ce vecteur a aussi le même poids que le vecteur de répulsion de base.
  - En plus de tout cela, j'ajoute un vecteur de direction légèrement aléatoire dans la direction du mouvement du dernier tick pour éviter que les boids aillent tout droit et pour rajouter un peu de mouvement naturel.



Dans la vidéo on commence par observer le flocking sans cible ni obstacle.
Puis je rajoute la cible, le cube, je le déplace un peu.
Et enfin je rajoute l'obstacle, la sphère rouge, par dessus la cible.
On observera que les boids se dirigent vers la cible et qu'ils s'en écarte dès que l'obstacle apparait.