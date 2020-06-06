# YingWorld
The Yinglet mod for RimWorld.
This document is a **WORK IN PROGRESS** and should not be considered authoritative or complete.

## Installing
- Install RimWorld on the target system.
- Install .NET Framework 3.5 on the target system.
- Install Visual Studio Community 2019 (or similar) on the target system.
- Clone this repository into the RimWorld mod folder, which is usually at something like `C:/Program Files (x86)/Steam/steamapps/common/RimWorld/Mods`:
````
cd RimWorldRootPath/Mods
git clone https://github.com/EvilDraggie/YingWorld
````
- Clone the AlienRaces repository into the same folder:
````
cd RimWorldRootPath/Mods
git clone https://github.com/RimWorld-CCL-Reborn/AlienRaces
````

## Building
- Open the AlienRaces .sln file in Visual Studio. Check that the references in the Reference tab are not highlighted with a yellow triangle, and are pointing to the appropriate references in the RimWorld folder.
- Build the project.
- Open the YingWorld .sln file in Visual Studio as above. Check that the references are correct. You may need to point the AlienRaces reference to the newly built assembly in the AlienRaces/Assemblies folder.
- Build the project.

### Install and build On linux
- Install RimWorld on the target system.
- Install Mono via your systems package manager.
- Install AlienRaces either though steam workshop or build it yourself.
```
cd ~ /.local/share/Steam/steamapps/common/RimWorld/Mods/
git clone https://github.com/RimWorld-CCL-Reborn/AlienRaces
sed -i 's/RimWorldWin64_Data/RimWorldLinux_Data/g' Source/AlienRace/AlienRace/AlienRace.csproj
cd AlienRace/Source/AlienRace/
msbuild /t:restore
```
- Clone the YingWorld into the RimWorld mod folder, modify the build script for linux, and build.
```
cd ~ /.local/share/Steam/steamapps/common/RimWorld/Mods/
git clone https://github.com/EvilDraggie/YingWorld
sed --in-place 's/RimWorldWin64_Data/RimWorldLinux_Data/g' YingWorld/Source/Yingworld/Yingworld/Yingworld.csproj
cd YingWorld/Source/Yingworld/
msbuild
```
