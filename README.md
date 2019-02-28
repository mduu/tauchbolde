| Build | Deployment |
|--|--|
| ![Build status](https://dev.azure.com/tauchbolde-devops/tauchbolde-devops/_apis/build/status/tauchbolde-CI) CI | ![Stage](https://vsrm.dev.azure.com/tauchbolde-devops/_apis/public/Release/badge/12db4506-57a6-40c1-add4-675b966511b0/1/1) STAGE |
| [![Build status](https://dev.azure.com/tauchbolde-devops/tauchbolde-devops/_apis/build/status/tauchbolde-RELEASE)](https://dev.azure.com/tauchbolde-devops/tauchbolde-devops/_build/latest?definitionId=2) RELEASE | ![Prod](https://vsrm.dev.azure.com/tauchbolde-devops/_apis/public/Release/badge/12db4506-57a6-40c1-add4-675b966511b0/1/3) PROD |

# tauchbolde
Next generation website fro http://tauchbolde.ch

This is the code for the http://tauchbolde.ch Website. The  website was a complete rewrite in C# / ASP.Net Core or in other words: a custom development. The previous website was a Drupal site with way to many security patches needed for the Drupal Core as well as for the plugins. Advanved functionallity was tricky to implement und migrate when software updates where needed. So this new site is "back to basics" and as I know how to code I do it myself instead of re-using systems like Drupal.

## Primary goals

* Back to basics regarding features
* Working well on mobile devices and small screens
* Easy maintainance
* No permanent security updates (unlike the current Drupal based website)
* Playing with new tech like .Net Core, ASP.Net Core, Azure and later on Mobile Apps

## Current state

The website using this code is in production.
