# Lecteur de l'élément de sécurité

Pour lire ce manuel dans différentes langues, veuillez choisir :

-[Serbian(Српски)](README.sr.md)

-[English(English)](README.md)

Si vous voulez télécharger la dernière version de l'application, veuillez cliquer sur les [Releases](https://github.com/Data-Tech-International/Secure-Element-Reader/releases) et sélectionnez la version de l'application selon votre système d'opérations (Windows, Linux ou Mac)

![image](https://user-images.githubusercontent.com/107615589/187879703-46b617f5-5671-43b4-b70b-182f72833afa.png)

# Aperçu

L’application du lecteur de l’élément de sécurité est utilisée pour : 
- lire les données du certificat d’un élément de sécurité de la carte à puce intelligente
- exécuter un audit de l'élément de sécurité
- télécharger et exécuter les commandes en attentes pour l'élément de sécurité
- vérifier le code PIN correct d’une carte à puce intelligente
- vérifier si l’applet de l’ICP et/ou l’applet de l’élément de sécurité sur une carte à puce ont été verrouillés en raison d’un trop grand nombre de mauvaises saisies du code PIN (dans le cas de 5 tentatives erronées consécutives).

## Prérequis pour utiliser l’application

- Un lecteur de carte à puce fonctionnel

- Assurez-vous que le lecteur soit bien connecté au poste de travail avant de lancer l’application

- Assurez-vous que le lecteur de carte à puce soit bien connecté au poste de travail

- Il faut avoir le .Net 6 SDK installé sur le poste de travail

# Comment lire les données des certificats de l'élément de sécurité

1. Afin de lire les données du certificat de l'élément de sécurité, vous devez d'abord insérer la carte à puce dans le lecteur de la carte à puce.
2. Ensuite, vous devez cliquer sur **obtenir le lecteur** pour établir la connexion avec votre lecteur de carte à puce
3. Si la connexion est établie, l'application affichera une fenêtre popup où vous devrez entrez le PIN de la carte à puce intelligente. Si vous ne pouvez pas vous souvenir du code PIN, sélectionnez **Cancel** (la carte à puce intelligente sera verouillée en cas de 5 essais consécutifs erronés).
4. L'application remplira automatiquement tous les champs du formulaire ci-dessous.

## Message d'erreur lors de la lecture des données

Si vous voyez un message d'erreur, essayez de cliquer sur **obtenir un certificat** pour lire les données de la carte à puce intelligente.

# Audit de l'élément de sécurité
La fonction d'audit de l'élément de sécurité est initiée automatiquement lors de l'insertion de la carte à puce intelligente. Si vous entrez le PIN correct de la carte à puce intelligente, l'application exécutera l'audit de l'élément de sécurité et vous verrez le message de résultat en bas de l'écran. L'audit est effectué en tant que proccesus d'arrière plan et ne nécessite pas d'activités supplémentaires de l'utilisateur.

# Exécution des commandes en attentes
Après avoir exécuté l'audit de l'élément de sécurité, l'application exécutera automatiquement toutes commandes en attentes pour l'élément de sécurité (si elles existent dans le système de l'administration fiscale). Vous verrez à nouveau le message de résultat en bas de l'écran.

> REMARQUE : Les fonctionnalités de _l'Audit de l'élément de sécurité_ et de _l'Exécution des commandes en attentes_ sont actuellement disponibles uniquement sur Windows.

# Comment vérifier le code PIN et le statut de l'élément de sécurité

Pour utiliser cette option, suivez les étapes suivantes :

1. Cliquez sur **vérifier le PIN**.
2. Une nouvelle fenêtre s'ouvrira. Saisissez le code PIN de la carte à puce intelligente et cliquez sur **vérifier**.
3. L'application affichera le message approprié qui indiquera si le code PIN est correct.

## Scénarios en cas de mauvaise saisie du code PIN ou de carte verrouillée

Voici les scénarios possibles et les recommandations au cas où vous entrez un PIN erroné ou si une carte à puce intelligente est verrouillée.

### Si vous avez saisi un mauvais code PIN

Si vous entrez un code PIN erroné, l'application affichera alors le message d'erreur approprié avec le nombre d'essais restants. S'il y a plusieurs tentatives, vous pouvez réessayer avec un PIN différent, mais gardez à l'esprit que la carte se verrouille après 5 tentatives erronés consécutives.

Si le contribuable ne se souvient pas du code PIN correct, on doit renvoyer cette carte à puce intelligente à l'autorité fiscale pour la remplacer. L'élément de sécurité sur la carte à puce doit être révoqué.

### Si vous avez saisi le code PIN correct

Si vous saisissez le code PIN correct, vous pourrez alors vérifier le statut actuel de la carte. Les scénarios suivants sont possibles :

- **Les deux applets de l'ES et de l'ICP sont OK**

Si le contribuable avait des problèmes pour se connecter sur le PAC, c'était parce qu'on avait utilisé un mauvais code PIN. Ceci est maintenant résolu puisque le code PIN correct a été vérifié (également, lorsque vous saisissez le code PIN correct, le compteur d'essais du code PIN de l'ICP revient à 5).

Si le contribuable a eu des problèmes pour émettre des factures fiscales avec un CDV-E, le problème n'est pas provoqué par la carte à puce intelligente. Le système de facturation du contribuable et le CDV-E doivent être vérifiés afin de détecter tout dysfonctionnement. 

- **L'applet de l'ICP est verrouillé mais l'applet de l'ES est OK**

Cela signifie que le contribuable a saisi un code PIN erroné 5 fois de suite lors de la connexion au Portail d'administration des contribuables (PAC). L'applet de l'ICP est verrouillé, de sorte que cette carte à puce intelligente ne peut plus être utilisée pour se connecter sur le PAC (bien qu'elle puisse toujours être utilisée pour émettre des factures fiscales).

Il faut retourner la carte à puce intelligente à l'autorité fiscale afin de la remplacer. L'élément de sécurité sur la carte à puce doit être révoqué.

- **L'applet de l'ES est verrouillé mais l'applet de l'ICP est OK**

Cela signifie que le contribuable a saisi un code PIN erroné 5 fois de suite lors de l'émission de factures fiscales avec un CDV-E. L'applet de l'élément de sécurité est verrouillé, de sorte que cette carte à puce ne peut plus être utilisée pour l'émission des factures (bien que le contribuable puisse toujours se connecter sur le PAC avec cette carte).

Il faut retourner la carte à puce intelligente à l'autorité fiscale afin de la remplacer. L'élément de sécurité sur la carte à puce doit être révoqué.

- **Les codes PIN de l'ES et de l'ICP sont bloqués**

Cela signifie que le contribuable a saisi un code PIN erroné 5 fois de suite lors de l'émission de factures fiscales avec un CDV-E **et** lors de la connexion sur le PAC. L'applet de l'ICP et l'applet de l'élément de sécurité sont tous les deux verrouillés, de sorte que cette carte à puce intelligente ne peut plus être utilisée pour l'émission des factures ni pour se connecter sur le PAC.

Il faut retourner la carte à puce intelligente à l'autorité fiscale afin de la remplacer. L'élément de sécurité sur la carte à puce doit être révoqué.

# Contribuer au projet

### Prérequis pour contribuer

- Visual Studio 2022
- L'extension Avalonia

### Comment contribuer

Si vous souhaitez contribuer à un projet et le rendre meilleur, votre aide sera la bienvenue.

Si vous cherchez à faire votre première contribution, veuillez suivre les étapes ci-dessous.

- Forker ce dépôt
- Cloner le référentiel
- Créer une branche
- Apporter les modifications nécessaires et valider ces modifications
- Pousser les modifications vers GitHub
- Soumettre vos modifications pour examen

C'est le dernier point mais non le moindre : écrivez toujours vos messages de validation au présent. Votre message de validation doit décrire ce que c'est la validation, lorsqu'elle est appliquée, fait au code - pas ce que vous avez fait au code.
