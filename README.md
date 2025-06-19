
<h1 align="center">
    <br>
        <a href="https://www.epita.fr">
            <img src="https://upload.wikimedia.org/wikipedia/fr/d/d8/Epita.png" alt="EPITA" width="200">
            </a>
    <br>
    DotNest
    <br>
</h1>

<h4 align="center">Un site Web via lequel des particuliers peuvent proposer leurs biens immobiliers en location à d’autres particuliers pour une durée déterminée.</h4>

<p align="center">
    <a id="badges"></a>
    <a href="https://dotnet.microsoft.com/fr-fr/download/dotnet/9.0">
        <img src="https://img.shields.io/badge/.NET-9.0-green?style=flat" alt=".NET" />
    </a>
    <a href="https://info.microsoft.com/ww-landing-sql-server-2022.html?lcid=fr">
        <img src="https://img.shields.io/badge/SQLServer-2022-46BC99?style=flat" alt="SQLServer" />
    </a>
    <a href="https://www.docker.com/get-started/">
        <img src="https://img.shields.io/badge/Docker-28.1.1-FF69B4?style=flat" alt="Docker" />
    </a>
    <a href="https://visualstudio.microsoft.com/fr/downloads/">
        <img src="https://img.shields.io/badge/Visual Studio-10.0.40219.1-orange?style=flat" alt="VS" />
    </a>
</p>

<p align="center">
    <a href="mailto:abigaelle.panhelleux@epita.fr">abigaelle.panhelleux</a> •
    <a href="mailto:magali.gilbert@epita.fr">magali.gilbert</a> •
    <a href="mailto:pauline.charasson@epita.fr">pauline.charasson</a> •
    <a href="mailto:remi.trotel">remi.trotel</a>
</p>

<p align="center">
    <a href="#fonctionnalités">Fonctionnalités</a> •
    <a href="#cloner-le-projet">Cloner le projet</a> •
    <a href="#connexion-en-remote">Connexion en remote</a> •
    <a href="#tests">Tests</a> •
    <a href="#license">License</a>
</p>

![screenshot](assets/Screenshot%202025-06-18%20201123.png)

## Fonctionnalités

Les fonctionnalités de ce site sont séparées en fonctionnalités publiques (ne nécessitant pas de connexion) et fonctionnalités privées (nécessitant une connexion). 

### Fonctionnalités publiques 

Un utilisateur non-connecté peut regarder la liste des biens proposés sur le site et les filtrer selon des lieux et/ou des dates. Il peut également accéder au détail d’un bien, mais ne peut pas le réserver. 

Demander à réserver un bien redirige l’utilisateur vers la page de connexion/inscription. Un utilisateur non-connecté peut donc se connecter et s’inscrire sur le site. 

### Fonctionnalités privées 

Un utilisateur connecté peut réserver un bien, accéder à la liste de ses réservations et annuler une réservation. 

Il peut également accéder à la liste des biens qu’il a mis en location et leurs réservations. Il peut mettre à jour cette liste en ajoutant ou modifiant l’un de ses biens. 

Enfin, l’utilisateur peut se déconnecter du site.

## Cloner le projet

Pour cloner, aller dans le [projet](https://github.com/Le-WHOOP/DotNest) dans la branche `main` et dans `Code > Copy url to clipboard` copiez l'url. Puis, dans le terminal:

```bash
# Clone le repository
git clone git@github.com:Le-WHOOP/DotNest.git
```

## Connexion en remote

Pour se connecter en [remote](http://vingtdeux.hd.free.fr/), suivre le lien.

## Connexion en local

Pour lancer le serveur en local, il faut avoir au minimum les versions visiblent sur les <a href="#badges">badges</a>. Pour télécharger la bonne version, cliquer sur les badges.

Puis, dans le terminal:
```bash
# Lancer docker depuis la source du projet
make
```
Puis cliquer sur [ce lien](http://localhost:8080/).

> [!WARNING]
> Attention, il faut attendre quelques minutes avant d'aller sur le navigateur. Même si le docker a été lancé, il faut attendre que le server SQL se connecte au projet.

## Tests

Pour lancer les tests, allez dans `Tests > Executer tous les tests` ou `Ctrl R + A`.

> [!NOTE] 
> Pour cause de difficultées, les tests visant à tester directement la DB n'ont pas été fait. Cela inclut des tests pour des fonctions comme `Add`, `Update`et `Delete`. 

## License

Aucun

---

> WHOOP &nbsp;&middot;&nbsp;
> [GitHub](https://github.com/Le-WHOOP) &nbsp;&middot;&nbsp;
> EPITA &nbsp;&middot;&nbsp;
> MTI &nbsp;&middot;&nbsp;
> Promo 2026
