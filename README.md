# Svelto.ECS.Examples.Survival
Code example for Svelto ECS (https://github.com/sebas77/Svelto-ECS)

Main article: http://www.sebaslab.com/learning-svelto-ecs-by-example-the-survival-example/

Update to svelto 2.5: http://www.sebaslab.com/svelto-ecs-2-5-and-allocation-0-code/

I used the Survival Shooter Unity Demo to show how an ECS framework could work inside Unity. I am not sure about the license of this demo, so use it only for learning purposes.

Most of the source code has been rewritten to work with Svelto.ECS framework. The Survival Demo is tested with unity 5.6 and 2017.3, so I cannot guarantee that it always works, but it should work with all the versions from 5.3 and above.

To know more about Svelto.ECS, please check my blog: http://www.sebaslab.com/

## Why are the Svelto folders under the Assets/Plugins folder?

The Plugins folder is a special foder in Unity. The code put inside it will be compiled in a separate dll (Assembly-CSharp-Editor-firstpass.dll). The main reason I like it this way, is that it enables the real meaning of the _internal_ keyword, which is used with intent inside the framework.

**Note: The folders Svelto.ECS, Svelto.Tasks and Svelto.Common, where present, are submodules pointing to the relative repositories. If you find them empty, you need to initialize the submodules through the submodule command. You must initialize only the first level submodules, so do not run the recursive option. I am sorry if this is confusing, but it's the best solution I found with git. If you have troubles, please check some instructions here: https://github.com/sebas77/Svelto.ECS.Vanilla.Example/wiki**

