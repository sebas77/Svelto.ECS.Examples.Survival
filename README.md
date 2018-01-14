# Svelto-ECS-Example
Code example for Svelto ECS (https://github.com/sebas77/Svelto-ECS)

I used the Survival Shooter Unity Demo to show how an ECS framework could work inside Unity. I am not sure about the license of this demo, so use it only for learning purposes.

Most of the source code has been rewritten to work with the ECS framework. You should disregard the SveltEx folder, which has been added just for convenience. I have used some extra features from my framework, but I won't keep this folder up to date. Check my other repositories to find the most updated version.

The Survival Demo and the Parallelism demo are tested with unity 5.6 and 2017.3, so I cannot guarantee that it always works, but it should work with all the versions from 5.3 and above.

New Parallelism Example is explained here: http://www.sebaslab.com/svelto-ecs-svelto-tasks-new-example-plus-whats-coming-and-optimization-related-thoughts/ the article complements the comments in the code.

You can add Svelto-ECS framework from the original repository or syncing it from this one using the submodule approach. 

**Note: This repository uses submodules. The Submodules are found inside the folder Assets\Plugins\ and they are Svelto.Common, Svelto.Tasks and Svelto.ECS. You must update every single submodule one by one. To read more about submodules, check this link: https://git-scm.com/docs/git-submodule or just google around. You need to update the submodules separately and not recursively. If you have troubles, check this guideline (pay attention, it's a different repository though): https://github.com/sebas77/Svelto.ECS.Vanilla.Example/wiki**
