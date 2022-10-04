# Secure Element Reader
To read this in other languages please select:
 
 -[Serbian(Српски)](README.sr.md)
 
 -[French(Français)](README.fr.md)
 
 
If you want to download the latest version of the app, go to the [Releases](https://github.com/Data-Tech-International/Secure-Element-Reader/releases) and select the version of the app according to your operating system (Windows, Linux or Mac). 
 
![image](https://user-images.githubusercontent.com/106304412/185734800-48b13d46-52bb-4697-9fa8-b5fcad0a7a7a.PNG)


# Overview

Secure Element Reader application is used for:

- reading certificate data from a smart card secure element 
- perfoming secure element audit 
- downloading and executing pending commands for the secure element
- verifying the correct PIN for a smart card
- checking whether the PKI Applet and/or the Secure Element Applet on a smart card have been locked because of too many wrong PIN inputs (in case of 5 consecutive wrong attempts)

## Prerequisites for using the application

- Functional smart card reader
- Make sure the reader is connected to the workstation before you launch the application
- Make sure the only one smart card reader is connect to workstation
- .Net 6 SDK installed on workstation


# How to read secure element's certificate data

1. To read the certificate data on the secure element you first need to insert the smart card into the smart card reader.

2. Next, click on **Get Reader** to establish connection with your smart card reader.

3. If the connection is established, the application will display a pop-up box where you need to enter the smart card PIN. If you cannot remember the PIN, select **Cancel** (the smart card gets locked in case of 5 consecutive wrong attmpts). 

4. The app will automatically populate all the fields from the form below.

## Error message when reading the data

In case you see an error message, try clicking on **Get Certificate** to read the data from the smart card.  


# Secure element audit

Secure element audit feature is initiated automatically upon inserting the smart card. If you provide the valid smart card PIN, the application will execute audit of the secure element and you will see the result message at the bottom of the screen. The audit is performed as a background process and does not require any further activity from the user.

# Execute pending commands

After performing secure element audit, the app will automatically execute any pending commands for the secure element (if they exist on the Tax Authority system). You will again see the result message at the bottom of the screen.

> **NOTE:** The _Secure element audit_ and _Execute pending commands_ features are currently available only on Windows.


# How to verify secure element PIN and state

To use this option, do the following:

1. Click on **Verify PIN**

2. A new window will open. Enter the smart card PIN and click on **Verify**.

3. The application will display the approprate message whether the is PIN correct.

## Scenarios in case of wrong PIN input or locked card

These are the possible scenarios and recommendations in case you enter a wrong PIN or a smart card is locked.

### If you entered a wrong PIN

If you enter a wrong PIN, the application will display the appropriate error message with the number of attemtps left. If there more attempts, you can try again with a different PIN, but bear in mind that the card gets locked in case of 5 consecutive wrong attempts.

In case the taxpayer can not remember the correct PIN, that smart card must be returned to the tax authority and replaced. The secure element on the smart card must be revoked.

### If you entered the correct PIN

If you enter the correct PIN, you will be able to inspect the current status of the card. The following scenarios are possible:

- **Both SE Applet and PKI Applet are OK**

If the taxpayer had problems logging into the TAP, it was because a wrong PIN was used. This is now resolved since the correct PIN has been verified (also, when you enter the correct PIN, the counter for PKI PIN tries goes back to 5).

If the taxpayer had problems issuing fiscal invoices with an E-SDC, the problem is not caused by the smart card. Taxpayer's invoicing system and E-SDC should be checked for malfunctioning.

- **PKI Applet is locked but SE Applet is OK**

This means that the taxpayer has entered a wrong PIN 5 times in a row when logging into the Taxpayer Administration Portal (TAP). PKI applet is locked so this smart card can no longer be used for logging into TAP (although it can still be used for issuing fiscal invoices).

The smart card must be returned to the tax authority and replaced. The secure element on the smart card must be revoked.

- **SE Applet is locked but PKI Applet is OK**

This means that the taxpayer entered a wrong PIN 5 times in a row when issuing fiscal invoices with an E-SDC. The Secure Element Applet is locked so this smart card can no longer be used for issuing invoices (although the taxpayer is still able to log into the TAP with this card).


The smart card must be returned to the tax authority and replaced. The secure element on the smart card must be revoked.

- **Both SE PIN and PKI PIN are blocked**

This means that the taxpayer entered a wrong PIN 5 times in a row both when issuing fiscal invoices with an E-SDC **and** when logging into the TAP. Both the PKI Applet and the Secure Element Applet are locked so this smart card can no longer be used for issuing invoices nor for logging into the TAP.

The smart card must be returned to the tax authority and replaced. The secure element on the smart card must be revoked.


# Contributing on the project

## Prerequisites for contributing

- Visual studio 2022  
- Avalonia extension

## How to contribute

If you want to contribute to a project and make it better, your help is very welcome. 

If you are looking to make your first contribution, follow the steps below.

- Fork this repository
- Clone the repository
- Create a branch
- Make necessary changes and commit those changes
- Push changes to GitHub
- Submit your changes for review

And last but not least: Always write your commit messages in the present tense. Your commit message should describe what the commit, when applied, does to the code – not what you did to the code.
