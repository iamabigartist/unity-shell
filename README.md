# fork from [marijnz/unity-shell](https://github.com/marijnz/unity-shell)
## This fork:
1. Use UIElement for the frontend.
2. Use the same evaluation code from the fork by Mono.CSharp.

## How to use:

![image](https://user-images.githubusercontent.com/53459343/147764928-977ee5bf-589b-42e2-8bf0-7b64488d9a6e.png)


1. Open the window: 
![image](https://user-images.githubusercontent.com/53459343/147764409-7d6586f1-c517-4766-9d93-ae3cddbf8e8e.png)

2. Input a piece of code and press crtl + enter to execute it.
![image](https://user-images.githubusercontent.com/53459343/147764794-c4d89c8b-7297-418b-ae37-f16b3b2ad193.png)

3. See the completion from here:
![image](https://user-images.githubusercontent.com/53459343/147764904-4d513a83-c9ef-41fe-8273-b262b43ce32b.png)


4. Drag an Unity.Object from the editor to shell to import a Object variable. 
![image](https://user-images.githubusercontent.com/53459343/147764528-63dee6a4-8c29-47e2-b983-a70481fb87ac.png)
![image](https://user-images.githubusercontent.com/53459343/147764607-c81a2748-dc81-4698-b15f-488dff4d96e4.png)



# The original README
# unity-shell
Write and execute code in an intuitive "shell" with autocompletion, for the Unity Editor.
  
Any feedback or suggestions? Just write me, create an Issue or a PR ;)  

_How it looks and works like:_

![Imgur](https://i.imgur.com/fMmHDvH.gif)

## .NET 4.x Runtime support
If your Unity project is using the .NET 4.x runtime (set in the Player Settings), you'll need to enable the Editor platform for Mono.CSharp.4.x.dll and disable it for Mono.CSharp.3.5.dll by selecting them in the Project view and modifying the Include Platforms in the Inspector.

Inspired by [UniShell](https://github.com/rje/unishell)
