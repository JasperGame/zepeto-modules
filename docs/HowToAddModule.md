# How to add a module to the Zepeto Module Importer ?

A **Zepeto module** is a *UnityPackage (*.unitypackage)* that is imported directly to your Zepeto project to help you speed up your project by abstracting some tasks.

In this documentation, you will learn how to make a Zepeto module and add it to the Zepeto Module importer, an Open Source Project. Are you new to Open source projects ? [This guide](https://opensource.guide) is for you.

**Prerequisites:**

- Being able to start a Zepeto Projects
- Being able to Export a Unity Package

## 1. **Create your Unity Zepeto Project:** 
    
You can read more about how to create your Zepeto project by reading the following [guide](https://docs.zepeto.me/studio-world/lang-ko/docs/welcome_zepeto_developers).
    
## 2. **Add the multiplay feature to your world (optional)**
You can read more about **ZepetoMultiplay** [here](https://docs.zepeto.me/studio-world/lang-ko/docs/multiplay_tutorial).

Some module/features might require your world to be multiplayer. You will need to add the multiplay component from the Zepeto Module Importer. Click [here](https://github.com/JasperGame/zepeto-modules) to learn how to do that. For this tutorial you don't need to add the Multiplay Component.

## 3. **Make the required folder for your module**
    
If you are making a module that implement a feature in a sample, make sure to create the required folder such as ***Scenes, Resources > Materials, Resources > Textures, Scripts > ZepetoScript, etc***. Additionally, make sure to add all the required assets for your module to work.
    
## 4. **Writing your script**
    
As this is a Zepeto Project, make sure to create your script the Zepeto-way: **Right click** on your project tab, `Create > Zepeto > Typescript`. This will generate a Typescript code (Yes, Zepeto script is written in Typescript and not C-sharp). If you are just getting started with Typescript, your can check out our collection of Interesting Typescript content [here](https://www.typescriptlang.org/docs/). 

![image](https://user-images.githubusercontent.com/131629767/235897562-11cd3677-af54-422c-a5b2-d05570ec6c75.png)

## 5. **Exporting your Unity Package**
    
If you have never exported a Unity Package, Check out this [article](https://docs.unity3d.com/2018.1/Documentation/Manual/HOWTO-exportpackage.html) and come back later.
    
Already back ? Good! Now make sure that your imported Unity package follows our naming convention v[versionNumber].unitypackage. *(e.g v1.0.1.unitypackage).* If not, don’t worry it;s not too late, you can just rename it. Remember where you save that UnityPackage it will serve us later on this tutorial.
    
## 6. **Download the Zepeto Module importer repo locally**
- Fork [this repository](https://github.com/JasperGame/zepeto-modules) into your Github account.
- Clone the forked repo to your local machine.
- Create a new branch, make sure to name it `add-module-{moduleName}` . ***moduleName***  being the name of the module you are creating. e.g. ***add-module-QuickChat***.
    
## 7. **Add your module to the Zepeto Module Importer locally**
- From the root directory, open the following folder `Project > zepeto-modules > Assets` and create a Folder and name it following this pattern : ***{moduleName} Module***. e.g: **QuickChat Module**
- In the same Assets folder, create another folder named Version and add a `*.cs` file with the following code:

    ```csharp
    public class {moduleName}Version
    {
        public static readonly string VERSION = "1.0.1";
    }
    ```

- From the root directory, modify the `moduleInfo.json` file by adding the following content with the following content:

    ```json
    {
        ...
         "Title": "moduleName",
         "Description": "A brief description of the English version",
         "Description_ko": "A brief description of the Korean version",
         "DocsUrl": "ZEPETO Official Docs URL of the used API, leave blank if not present",
         "LatestVersion": "1.0.0",
         "Dependencies": ["nothing"]
       },
    ```

- Inside the same `Release` folder, create a new folder name after your ***moduleName*** E.g *QuickChat.* Within that same folder, add a **preview image** of your module.
- Within the ***moduleName*** folder, create a **docs** folder where you can add any `README.md` file for the module documentation if there is any. If you have translated version of your `.md` file, make sure to name them as follow: `README_{language}.md`. E.g: README_KR.md.
- Within the **docs** folder, you can also add an **images** folder which could be used for the visual explanation of your application.

## 8. **Finally Adding your unity package to the Zepeto Module Importer**
   
Remember the the Unity Package (**.unitypackage)* you made in step 5 ? Copy and paste it in the `Release > moduleName` folder.
   
## 9. **Push to your github**
 
Before doing the magic push to github, make sure that your module and its elements respect all the naming convention I told you about in this guide. If everything looks, feel free to push to you github. You know how it works right ? `git add .` , `git commit -m "[insert your commit message here]` and finally `git push`.
   
## 10. **Make a Pull Request**
Never heard of pull request ? This github [article](https://docs.github.com/ko/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request) got you covered.   
This is the last thing you have to do so that your module can be part of the Zepeto Module Importer. Create a pull request from your github repo (make sure you are pulling from the branch named `add-module-{moduleName}`). Add a meaningful pull request message to help our team examine your PR faster. 
   
**All good ? Done? You did a great job.**
   
Now give us some time to check the wonderful work you have done for the Zepeto Creators community.
