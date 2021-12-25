# fork from [marijnz/unity-shell](https://github.com/marijnz/unity-shell)
This fork:
1. try to delete the Mono.CSharp.3.5.dll. 
2. and only support the API version for NET_4_6 and NET_STANDARD_2_0.

# The original README
# unity-shell
Write and execute code in an intuitive "shell" with autocompletion, for the Unity Editor.
  
Any feedback or suggestions? Just write me, create an Issue or a PR ;)  

_How it looks and works like:_

![Imgur](https://i.imgur.com/fMmHDvH.gif)

## .NET 4.x Runtime support
If your Unity project is using the .NET 4.x runtime (set in the Player Settings), you'll need to enable the Editor platform for Mono.CSharp.4.x.dll and disable it for Mono.CSharp.3.5.dll by selecting them in the Project view and modifying the Include Platforms in the Inspector.

Inspired by [UniShell](https://github.com/rje/unishell)
