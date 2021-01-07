# PMF - Package Management Framework

PMF is a barebones C# library that provides basic support for package management

# Features

  - Install
  - Update
  - Uninstall

# Planned features

  - Abstract Package, Asset and Dependency class
  - Multithreading




# API

## Initialization

These functions need to be called at the beggining of the program and at the end, respectively, they handle the manifest.json file that is saved with information regarding packages installed 

```csharp
PackageManager.Start();

// Do stuff

PackageManager.Stop();
```

### Install

Installs selected version of a package

```csharp
public static PackageState Install(string id, Version version, out Package package)
```

Installs latest version of a package

```csharp
public static PackageState InstallLatest(string id, out Package package)
```

Installs by SDK version provided by software

```csharp
public static PackageState InstallBySdkVersion(string id, out Package package)
```

### Uninstall

Uninstalls a package

```csharp
public static bool Uninstall(string id)
```

### Update

Updates a package to the most recent version, ignoring SDK version

```csharp
public static PackageState UpdateLatest(string id, out Package package)
```

Updates a package to a specified version

```csharp
public static PackageState UpdatePackage(string id, Version version, out Package package)
```

Updates a package to the most recent version provided an SDK version

```csharp
public static PackageState UpdateBySdkVersion(string id, out Package package, bool dontAsk = false)
```

### Configuration

Defines the manifest file name

```csharp
string Config.ManifestFileName = "manifest.json";
```

Defines the folder where packages are to be installed

```csharp
string Config.PackageInstallationFolder
```

The http server where you will be sending information about the packages

```csharp
string Config.RepositoryEndpoint
```

Current SDK version

```csharp
Version Config.CurrentSdkVersion = null;
```

Temporary folder where zip files will be downloaded to, gets deleted at the end of execution

```csharp
string Config.TemporaryFolder = ".pmf-temp";
```

## JSON

This is the basic information a package would have

```json
{
	"ID": "something_cool",
	"Type": "Package",
	"Name": "Im Something cool",
	"Description": "This is a package that makes you cool and awesome",
	"Assets": 
	[
		{
			"Version": "0.0.1",
			"SdkVersion": "0.0.5",
			"Checksum": "somethingCoolWithLettersAndNumbers",
			"FileName": "name.zip",
			"Url": "somewhereElseDoesntNeedToBeYourServer.zip",
			"Dependencies": 
			[
				{
					"ID": "Inner Spirit",
					"Checksum": "somethingHere",
					"Type": "Standalone",
					"FileName": "zipped.zip",
					"Url": "somewhereElseDoesntNeedToBeYourServer.zip"
				},
				{
					"ID": "Other package id",
					"Checksum": "somethingHere",
					"Type": "Package",
					"Version": "0.2.3"
				}
			]
		}
	]
}
```

### Building for source

To build from source, clone or fork the repository and open with Visual Studio.