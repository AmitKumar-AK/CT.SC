
[![GitHub license](https://img.shields.io/github/license/amitkumar-ak/CT.SC.svg)](https://github.com/amitkumar-ak/CT.SC/blob/master/LICENSE)
[![GitHub contributors](https://img.shields.io/github/contributors/amitkumar-ak/CT.SC.svg)](https://GitHub.com/amitkumar-ak/CT.SC/graphs/contributors/)
[![GitHub issues](https://img.shields.io/github/issues/amitkumar-ak/CT.SC.svg)](https://GitHub.com/amitkumar-ak/CT.SC/issues/)
[![GitHub pull-requests](https://img.shields.io/github/issues-pr/amitkumar-ak/CT.SC.svg)](https://GitHub.com/amitkumar-ak/CT.SC/pulls/)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![GitHub Stars](https://img.shields.io/github/stars/amitkumar-ak/CT.SC?label=GitHub%20Stars)](https://github.com/amitkumar-ak/CT.SC/stargazers)

[![GitHub watchers](https://img.shields.io/github/watchers/amitkumar-ak/CT.SC.svg?style=social&label=Watch&maxAge=2592000)](https://GitHub.com/amitkumar-ak/CT.SC/watchers/)
[![GitHub forks](https://img.shields.io/github/forks/amitkumar-ak/CT.SC.svg?style=social&label=Fork&maxAge=2592000)](https://GitHub.com/amitkumar-ak/CT.SC/network/)
[![GitHub stars](https://img.shields.io/github/stars/amitkumar-ak/CT.SC.svg?style=social&label=Star&maxAge=2592000)](https://GitHub.com/amitkumar-ak/CT.SC/stargazers/)

<img src="https://1.bp.blogspot.com/-8juFM5WwQBU/XQs2FnRiNZI/AAAAAAAAG2g/Q39yZq7QdlE2sz04r62BSZsFXBIJSii7wCLcBGAs/s1600/sitecore-helixbase.png" /><br />
This is Sitecore Helix based repository and provide information about custom developement which includes features, Sitecore extenstions, etc.


#### Features include:

* Sitecore.NET 10.0.0 (rev. 004346) ready
* Use of [PackageReference](https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files) in solution
* Use of [Microsoft.Build.CentralPackageVersions](https://github.com/microsoft/MSBuildSdks/tree/main/src/CentralPackageVersions)
* Account Features
* News Features
* PageContent Features
* Use of [BuildBundlerMinifier](https://github.com/madskristensen/BundlerMinifier) to bundles CSS, JavaScript or HTML files into a single output file
* Use of Sitecore Habitat Dependency Injection/Theming/SitecoreExtensions/Assets features

## Setup Instructions
This solution uses the [PackageReference](https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files) and [Microsoft.Build.CentralPackageVersions](https://github.com/microsoft/MSBuildSdks/tree/main/src/CentralPackageVersions) so good to use the _Visual Studio 2019 or higher_

1. Install [Sitecore Experience Platform 10 Initial Release](https://dev.sitecore.net/Downloads/Sitecore_Experience_Platform/100/Sitecore_Experience_Platform_100.aspx)
2. Clone the repo and restore the Nuget reference
3. Build the solution
4. Deploy Build artifacts to the target IIS folder
