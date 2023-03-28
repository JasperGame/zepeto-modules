# How to contribute

[English](./HowToContribute.md) | [Korean](./HowToContribute_KR.md)

## ZEPETO Modules Project contributions are welcome!
Thank you so much for contributing to the creation of ZEPETO Modules that help develop ZEPETO World.

You can contribute to:

- bug report
- Feature suggestions
- Add module


## Report a bug
Please let me know if you find any bugs or problems in the project! You can report bugs by filing an issue in our GitHub repository. When reporting a bug, please provide as much information as possible.

- Clear description of the bug
- Steps to reproduce the bug
- Error messages or console output
- Expected versus actual behavior
- ZEPETO World version and module version in use

## Feature suggestions
Please suggest new features or improvements! You can submit a feature request by opening a new issue in our GitHub repository. When suggesting a feature, please include:

- A clear description of the feature or improvement
- Why the feature or improvement is useful or valuable
- Potential downsides or limitations

## Add module
**You are welcome to add your own modules! If you want to add a module, please do as follows.**

1. Fork the repository into your GitHub account.
2. Clone the forked repository to your local machine.
3. Create a new branch for your changes.
4. Make and commit your changes.
5. Push your changes to your GitHub fork.
6. Open a pull request to merge your changes into our repo.

**Here's how to add a module.**
 
1. Refer to the existing modules and make them according to the format.
2. Inside the Assets folder, add the (Title name with spaces removed) Module folder.
3. Create an Assets/Version folder and add the following inside the folder.
     ``` cs
         public class (Title name with spaces removed)Version
         {
             public static readonly string VERSION = "1.0.0";
         }
     ```
4. Open moduleInfo.json in the Release folder. Add the following below.
      ``` json
         {
           "Title": "your module name",
           "Description": "A brief description of the English version",
           "Description_ko": "A brief description of the Korean version",
           "DocsUrl": "ZEPETO Official Docs URL of the used API, leave blank if not present",
           "LatestVersion": "1.0.0",
           "Dependencies": ["nothing"]
         },
     ```
5. Inside the Release folder, add a Title folder with spaces removed.
6. Add Preview.png image.
7. Export the created module and save it as v[versionnumber].unitypackage. (e.g. v1.0.1.unitypackage)
