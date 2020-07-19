!!! THIS IS A WORK IN PROGRESS AND IT IS NOT CURRENTLY FUNCTIONAL !!!

# Unity Asset Processing Tools [![License](https://img.shields.io/badge/License-MIT-lightgrey.svg?style=flat)](http://mit-license.org)
A set of Unity Asset Processing Tools like: renaming, resizing, type conversion, generation, cropping and analysis.


*  *  *  *  *

## Setup
##### Option A) Clone or download the repository and drop it in your Unity project.
##### Option B) Add the repository to the package manifest (go in YourProject/Packages/ and open the "manifest.json" file and add "com..." line in the depenencies section). If you don't have Git installed, Unity will require you to install it.
```
{
  "dependencies": {
      ...
      "com.razluta.unityassetprocessingtools": "https://github.com/razluta/UnityAssetProcessingTools.git"
      ...
  }
}
```
*  *  *  *  *
## Architecture
Below is a high level explanation of how to tools are architected.

The primary components of the tools are:
- **AssetProcessingToolsEditor** class that inherits from **Unity.EditorWindow** for creating a GUI for the user to interact with
- **AssetProcessingTools** static class that orchestrates the **ActiveFilter** usage (getting, setting).
- **ActiveFilter** class with several properties (path, recursiveness, etc.) that define the instance of the filter as used with the data

- **ProjectSearch** static class for utilities to check if an assets fullfills a filter's conditions and other useful project wide related searches (getting files in a path, etc.)
- **PathUtilities** static class for utilities to interact with the OS level path string searches

- **Renaming** class with several properties (prefix, suffix) that define the renaming instance 
- **RenamingUtilities** static class that uses rules from a **Renaming** instance to perform a safe asset rename
- **Moving** class with several properties (new path) that define the moving instance 
- **MovingUtilities** static class that uses rules from a **Moving** instance to perform a safe asset move

When the user launches the tools, the following steps happen:
- first, ...

