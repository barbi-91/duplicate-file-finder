# duplicate-file-finder
Find duplicate files in folder and allows to delete duplicates

Barbara Erdec Golović

This GUI application consists of WPF application that search for duplicate files in a folder. Compares files in folders according to size and binary using the hash 
that corresponds to the content of the files. Finds unique files and matches,if there is duplicate, show result in app, with possibility to delete that duplicates.

INTRODUCTION  
This article describes how to build a C# application that compares all files in folders according to their size and binary content by using hashes (md5 hash). 
Dictionaries are used so that better performance is achieved. Also, a pre-comparison by file size is used to minimize number of files for which hash comparison needs to be performed. 
Source code might be useful to developers who have interest in file comparison by content.

Application come in Windows - .zip file build which includes .exe file and .dll dependencies which need to be extracted in directory and .exe can be run after that.

BACKGROUND
There is a need for such an application when you implement a comprehensive performance improvement of large export functionality and verify that actual file content of produced 
files hasn't changed (that all bytes in all files are same as before the performance improvement changes to code). 
There is another factor in which the file names have changed each time the content is changed or exist duplicate copies of a unique file with different names, so that it is not 
possible to match files according to their names, but only compare the binary contents of each file and conclude if file is unique or is a copy of an existing one  And such content 
is export in the application according to the hash group with their names.

DEVELOPMENT TOOLS
These were the tools used for development of version 1.0.0.0 of HaschCodeDuplicateFileFinder (in case of later development versions may change):
Windows - Visual Studio 2019 IDE,. Compiler was MSVC. IDE was configured to use Windows edition of Votive2019.vsix installed from downloaded wixWindows installer.

HOW IT WORKS
A user selects the "trial folder" directory. The comparison is performed in a new "worker" thread and it will:

-enumerate files in a trial directory and collect file sizes
-compare whether the files are unique or have the same file sizes
-files that do not have the appropriate file size in the dictionary can be immediately identified as unique and removed from the list
-for files that have repetitive size in the dictionary, hashes will be generated to determine the difference in equality
-create hash files in the "test folder" directory and put them in the hash dictionary
-go through the hashes of files from the "test folder" and discover matches with hashes and sort them into a dictionary according to the same hashes by name and path
-ask file information for each appropriate file size and match hash
-send a pointer to the "List" collection to the main thread through an event that marks the end of processing in the "worker" thread
-display items from the "List" collection in the application list view
-if the a user want, he can delete duplicate files
-if a user delete all duplicate files, the remaining unique file is automatically removed from the duplicate list and cannot be considered 


POINTS OF INTEREST
This software utilises MD5 algorithm implementation by Ronald Rivest (https://en.wikipedia.org/wiki/MD5)
OS versions on which this software was tested are Windows 10, setup toolset Votive2019.vsix installed from downloaded wixWindows installer. 

HISTORY
-	1.0.0.0 - 2021-18-12 - Initial version

LICENSE
This article, along with any associated source code and files, is licensed under GPL v3 License.

Screenshot (viewer application):
![screenshot](./screenshot.png?raw=true)

 