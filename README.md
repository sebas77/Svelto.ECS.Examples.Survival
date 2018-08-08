# Svelto.ECS.Examples.Survival
Code example for Svelto ECS (https://github.com/sebas77/Svelto-ECS)

Main article: http://www.sebaslab.com/learning-svelto-ecs-by-example-the-survival-example/

Update to svelto 2.5: http://www.sebaslab.com/svelto-ecs-2-5-and-allocation-0-code/

I used the Survival Shooter Unity Demo to show how an ECS framework could work inside Unity. I am not sure about the license of this demo, so use it only for learning purposes.

Most of the source code has been rewritten to work with Svelto.ECS framework. The Survival Demo is tested with unity 5.6 and 2017.3, so I cannot guarantee that it always works, but it should work with all the versions from 5.3 and above.

To know more about Svelto.ECS, please check my blog: http://www.sebaslab.com/

## Why are the Svelto folders under the Assets/Plugins folder?

The Plugins folder is a special foder in Unity. The code put inside it will be compiled in a separate dll (Assembly-CSharp-Editor-firstpass.dll). The main reason I like it this way, is that it enables the real meaning of the _internal_ keyword, which is used with intent inside the framework.

# The project generates a lot of errors:

Please don't download the zip file and unzip it. Unforunately I am using submodules and Github is not smart enough to include them in the zip file. The simplest way is to clone it and follow these instructions https://github.com/sebas77/Svelto.ECS.Vanilla.Example/wiki

